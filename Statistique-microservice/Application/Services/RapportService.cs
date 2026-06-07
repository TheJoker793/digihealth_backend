using System.Text.Json;
using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.DTOs.Responses;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Enums;
using Statistique_microservice.Domain.Exceptions;
using Statistique_microservice.Domain.Interfaces;
using Statistique_microservice.Domain.ValueObjects;

namespace Statistique_microservice.Application.Services;

public class RapportService
{
    private readonly IUnitOfWork _uow;
    private readonly ICalculateurKPI _calculateur;
    private readonly IExporteurRapport _exporteur;
    private readonly INumeroRapportGenerator _numeroGen;
    private readonly IEventPublisher _publisher;
    private readonly IRapportCache _cache;

    public RapportService(
        IUnitOfWork uow,
        ICalculateurKPI calculateur,
        IExporteurRapport exporteur,
        INumeroRapportGenerator numeroGen,
        IEventPublisher publisher,
        IRapportCache cache)
    {
        _uow = uow;
        _calculateur = calculateur;
        _exporteur = exporteur;
        _numeroGen = numeroGen;
        _publisher = publisher;
        _cache = cache;
    }

    // ═══════════════════════════════════════════
    // GÉNÉRER UN RAPPORT
    // ═══════════════════════════════════════════
    public async Task<RapportResponse> GenererAsync(
        GenererRapportRequest request,
        CancellationToken ct = default)
    {
        var periode = ResoudrePeriode(request);

        var numero = await _numeroGen.GenererAsync(
            request.CabinetId, request.TypeRapport, ct);

        var rapport = RapportStatistique.Create(
            numero,
            request.TypeRapport,
            request.CabinetId,
            periode,
            request.MedecinId,
            request.DatePlanifiee);

        await _uow.Rapports.AddAsync(rapport, ct);
        await _uow.SaveChangesAsync(ct);

        // Si planifié → on persiste et on revient
        if (request.DatePlanifiee.HasValue)
            return ToResponse(rapport);

        // Génération immédiate
        await ExecuterGenerationAsync(rapport, ct);

        return ToResponse(rapport);
    }

    // ═══════════════════════════════════════════
    // EXÉCUTER LA GÉNÉRATION (appelé aussi par le job)
    // ═══════════════════════════════════════════
    public async Task ExecuterGenerationAsync(
        RapportStatistique rapport,
        CancellationToken ct = default)
    {
        try
        {
            rapport.DemarrerGeneration();
            _uow.Rapports.Update(rapport);
            await _uow.SaveChangesAsync(ct);

            // Calculer les KPIs
            var kpis = await _calculateur.CalculerAsync(
                rapport.CabinetId, rapport.Periode, ct);

            // Ajouter les indicateurs au rapport
            foreach (var kpi in kpis)
            {
                // Persister les KPIs individuellement
                await _uow.KPIs.AddAsync(kpi, ct);
                rapport.AjouterIndicateur(kpi);
            }

            // Sérialiser les données
            var donnees = new
            {
                TypeRapport = rapport.TypeRapport.ToString(),
                Periode = rapport.Periode.ToString(),
                GenereLe = DateTimeOffset.UtcNow,
                KPIs = kpis.Select(k => new
                {
                    k.Code,
                    k.TypeKPI,
                    k.Valeur,
                    k.Unite,
                    k.ValeurPrecedente,
                    k.VariationPourcentage
                })
            };

            rapport.MarquerGenere(JsonSerializer.Serialize(donnees));

            _uow.Rapports.Update(rapport);
            await _uow.SaveChangesAsync(ct);

            // Invalider le cache KPIs
            await _cache.InvaliderAsync(rapport.CabinetId);

            // Publier domain events
            foreach (var evt in rapport.DomainEvents)
                await _publisher.PublishAsync(evt, ct);
            rapport.ClearDomainEvents();
        }
        catch (Exception ex)
        {
            rapport.MarquerEchec(ex.Message);
            _uow.Rapports.Update(rapport);
            await _uow.SaveChangesAsync(ct);
            throw;
        }
    }

    // ═══════════════════════════════════════════
    // EXPORTER
    // ═══════════════════════════════════════════
    public async Task<(byte[] Contenu, string NomFichier, string ContentType)> ExporterAsync(
        Guid rapportId,
        string format,
        CancellationToken ct = default)
    {
        var rapport = await _uow.Rapports.GetByIdAsync(rapportId, ct)
            ?? throw new RapportIntrouvableException(rapportId);

        if (!rapport.EstPret())
            throw new InvalidOperationException("Le rapport n'est pas encore généré.");

        if (format.ToLower() == "pdf")
        {
            var pdf = await _exporteur.ExporterPdfAsync(rapport, ct);
            rapport.EnregistrerExportPdf($"rapports/{rapport.Numero}.pdf");
            _uow.Rapports.Update(rapport);
            await _uow.SaveChangesAsync(ct);
            return (pdf, $"{rapport.Numero}.pdf", "application/pdf");
        }
        else
        {
            var xlsx = await _exporteur.ExporterExcelAsync(rapport, ct);
            rapport.EnregistrerExportExcel($"rapports/{rapport.Numero}.xlsx");
            _uow.Rapports.Update(rapport);
            await _uow.SaveChangesAsync(ct);
            return (xlsx,
                $"{rapport.Numero}.xlsx",
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
    }

    // ═══════════════════════════════════════════
    // ANNULER
    // ═══════════════════════════════════════════
    public async Task AnnulerAsync(
        Guid rapportId, string motif, CancellationToken ct = default)
    {
        var rapport = await _uow.Rapports.GetByIdAsync(rapportId, ct)
            ?? throw new RapportIntrouvableException(rapportId);

        rapport.Annuler(motif);
        _uow.Rapports.Update(rapport);
        await _uow.SaveChangesAsync(ct);
    }

    // ═══════════════════════════════════════════
    // LECTURE
    // ═══════════════════════════════════════════
    public async Task<RapportResponse> GetByIdAsync(
        Guid id, CancellationToken ct = default)
    {
        var rapport = await _uow.Rapports.GetByIdAsync(id, ct)
            ?? throw new RapportIntrouvableException(id);
        return ToResponse(rapport);
    }

    public async Task<IEnumerable<RapportResponse>> GetByCabinetAsync(
        Guid cabinetId,
        TypeRapport? type = null,
        CancellationToken ct = default)
    {
        var rapports = await _uow.Rapports.GetByCabinetAsync(cabinetId, type, ct);
        return rapports.Select(ToResponse);
    }

    // ═══════════════════════════════════════════
    // HELPERS
    // ═══════════════════════════════════════════
    private static PeriodeAnalyse ResoudrePeriode(GenererRapportRequest req)
        => req.TypePeriode switch
        {
            TypePeriode.Journee => PeriodeAnalyse.Journee(
                DateOnly.FromDateTime(DateTime.UtcNow)),
            TypePeriode.Hebdomadaire => PeriodeAnalyse.SemaineCourante(),
            TypePeriode.Mensuel => PeriodeAnalyse.MoisCourant(),
            TypePeriode.Annuel => PeriodeAnalyse.AnneeCourante(),
            TypePeriode.Personnalise => PeriodeAnalyse.Personnalisee(
                DateOnly.Parse(req.DateDebut!),
                DateOnly.Parse(req.DateFin!)),
            _ => PeriodeAnalyse.MoisCourant()
        };

    private static RapportResponse ToResponse(RapportStatistique r)
        => new(
            r.Id, r.Numero, r.TypeRapport, r.CabinetId, r.MedecinId,
            ToPeriodeResponse(r.Periode),
            r.Statut, r.DateGeneration, r.DatePlanifiee,
            r.MessageErreur,
            r.AExportPdf(), r.AExportExcel(),
            r.Indicateurs.Select(ToKPIResponse),
            r.CreatedAt);

    internal static PeriodeResponse ToPeriodeResponse(PeriodeAnalyse p)
        => new(p.DateDebut.ToString("yyyy-MM-dd"),
               p.DateFin.ToString("yyyy-MM-dd"),
               p.TypePeriode, p.NbJours());

    internal static KPIResponse ToKPIResponse(IndicateurKPI k)
        => new(k.Id, k.TypeKPI, k.Code, k.Valeur, k.Unite,
               k.FormaterValeur(),
               k.ValeurPrecedente, k.VariationPourcentage,
               k.EstEnHausse(), ToPeriodeResponse(k.Periode));
}