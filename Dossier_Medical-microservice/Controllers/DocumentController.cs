using Dossier_Medical_microservice.Application.DTOs.Requests;
using Dossier_Medical_microservice.Application.Services;
using Dossier_Medical_microservice.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class DocumentController : ControllerBase
{
    private readonly DocumentService _documentService;

    public DocumentController(DocumentService service)
    {
        _documentService = service;
    }

    [HttpPost("dossiers/{id:guid}/documents")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
    Guid id,
    [FromForm] UploadDocumentForm form,
    CancellationToken ct)
    {
        using var stream = form.Fichier.OpenReadStream();

        var req = new UploadDocumentRequest(
            id,
            (TypeDocument)form.TypeDocument,
            form.Titre,
            stream,
            form.Fichier.FileName,
            form.Fichier.ContentType);

        var result = await _documentService.UploadAsync(req, ct);

        return CreatedAtAction(nameof(GetByDossier), new { id }, result);
    }



    [HttpGet("dossiers/{id:guid}/documents")]
    public async Task<IActionResult> GetByDossier(
        Guid id,
        [FromQuery] TypeDocument? type,
        CancellationToken ct)
    {
        var result = await _documentService.GetByDossierAsync(id, type, ct);
        return Ok(result);
    }

    [HttpGet("documents/{did:guid}/download")]
    public async Task<IActionResult> Download(Guid did, CancellationToken ct)
    {
        var url = await _documentService.GetDownloadUrlAsync(did, ct);
        return Ok(new { downloadUrl = url });
    }

    [HttpDelete("documents/{did:guid}")]
    public async Task<IActionResult> Delete(Guid did, CancellationToken ct)
    {
        await _documentService.DeleteAsync(did, ct);
        return NoContent();
    }
}