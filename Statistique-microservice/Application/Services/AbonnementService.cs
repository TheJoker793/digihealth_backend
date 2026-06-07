using Statistique_microservice.Application.DTOs.Requests;
using Statistique_microservice.Application.DTOs.Responses;
using Statistique_microservice.Domain.Entities;
using Statistique_microservice.Domain.Exceptions;
using Statistique_microservice.Domain.Interfaces;

namespace Statistique_microservice.Application.Services
{
    public class AbonnementService
    {
            private readonly IUnitOfWork _uow;
            private readonly RapportService _rapportService;

            public AbonnementService(IUnitOfWork uow, RapportService rapportService)
            {
                _uow = uow;
                _rapportService = rapportService;
            }

            public async Task<AbonnementResponse> CreerAsync(
                CreerAbonnementRequest request,
                CancellationToken ct = default)
            {
                var abonnement = AbonnementRapport.Create(
                    request.CabinetId,
                    request.CreePar,
                    request.TypeRapport,
                    request.Frequence,
                    request.Destinataires);

                await _uow.Abonnements.AddAsync(abonnement, ct);
                await _uow.SaveChangesAsync(ct);
                return ToResponse(abonnement);
            }

            public async Task<AbonnementResponse> MettreAJourAsync(
                Guid id,
                MettreAJourAbonnementRequest request,
                CancellationToken ct = default)
            {
                var abonnement = await _uow.Abonnements.GetByIdAsync(id, ct)
                    ?? throw new AbonnementIntrouvableException(id);

                if (request.Frequence.HasValue)
                {
                    // Recréer l'abonnement avec la nouvelle fréquence (immutabilité partielle)
                    var nouveau = AbonnementRapport.Create(
                        abonnement.CabinetId, abonnement.CreePar,
                        abonnement.TypeRapport, request.Frequence.Value,
                        request.Destinataires ?? abonnement.Destinataires);
                    _uow.Abonnements.Remove(abonnement);
                    await _uow.Abonnements.AddAsync(nouveau, ct);
                    await _uow.SaveChangesAsync(ct);
                    return ToResponse(nouveau);
                }

                if (request.Destinataires is not null)
                    abonnement.MettreAJourDestinataires(request.Destinataires);

                _uow.Abonnements.Update(abonnement);
                await _uow.SaveChangesAsync(ct);
                return ToResponse(abonnement);
            }

            public async Task ActiverAsync(Guid id, CancellationToken ct = default)
            {
                var abonnement = await _uow.Abonnements.GetByIdAsync(id, ct)
                    ?? throw new AbonnementIntrouvableException(id);
                abonnement.Activer();
                _uow.Abonnements.Update(abonnement);
                await _uow.SaveChangesAsync(ct);
            }

            public async Task DesactiverAsync(Guid id, CancellationToken ct = default)
            {
                var abonnement = await _uow.Abonnements.GetByIdAsync(id, ct)
                    ?? throw new AbonnementIntrouvableException(id);
                abonnement.Desactiver();
                _uow.Abonnements.Update(abonnement);
                await _uow.SaveChangesAsync(ct);
            }

            /// <summary>
            /// Traite tous les abonnements échus — génère les rapports et marque envoyé.
            /// Appelé par EnvoyerRapportsPlanifiesJob (toutes les heures).
            /// </summary>
            public async Task TraiterAbonnementsEchusAsync(CancellationToken ct = default)
            {
                var echus = await _uow.Abonnements.GetEchusAsync(ct);

                foreach (var abonnement in echus)
                {
                    try
                    {
                        // Calculer la période correspondant à la fréquence
                        var periode = abonnement.Frequence switch
                        {
                            Domain.Enums.FrequenceRapport.Quotidien =>
                                Domain.ValueObjects.PeriodeAnalyse.Journee(
                                    DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))),
                            Domain.Enums.FrequenceRapport.Hebdomadaire =>
                                Domain.ValueObjects.PeriodeAnalyse.SemaineCourante(),
                            Domain.Enums.FrequenceRapport.Mensuel =>
                                Domain.ValueObjects.PeriodeAnalyse.MoisPrecedent(),
                            _ => Domain.ValueObjects.PeriodeAnalyse.MoisPrecedent()
                        };

                        // Générer le rapport
                        await _rapportService.GenererAsync(new GenererRapportRequest(
                            abonnement.TypeRapport,
                            abonnement.CabinetId,
                            periode.TypePeriode,
                            periode.DateDebut.ToString("yyyy-MM-dd"),
                            periode.DateFin.ToString("yyyy-MM-dd")), ct);

                        abonnement.MarquerEnvoye();
                        _uow.Abonnements.Update(abonnement);
                        await _uow.SaveChangesAsync(ct);
                    }
                    catch (Exception)
                    {
                        // On continue les autres abonnements même si un échoue
                    }
                }
            }

            public async Task<IEnumerable<AbonnementResponse>> GetByCabinetAsync(
                Guid cabinetId, CancellationToken ct = default)
            {
                var abonnements = await _uow.Abonnements.GetByCabinetAsync(cabinetId, ct);
                return abonnements.Select(ToResponse);
            }

            private static AbonnementResponse ToResponse(AbonnementRapport a)
                => new(a.Id, a.CabinetId, a.TypeRapport, a.Frequence,
                       a.Destinataires, a.EstActif,
                       a.DernierEnvoi, a.ProchainEnvoi, a.CreatedAt);

        }
    }
