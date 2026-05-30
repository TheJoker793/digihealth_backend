using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.DTOs.Responses;
using Document_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentService _documentService;
        public DocumentController(DocumentService documentService)
        {
            _documentService = documentService;
        }

        /// <summary>
        /// Upload d'un nouveau document
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<DocumentResponse>> Upload(
            [FromForm] UploadDocumentRequest request,
            CancellationToken cancellationToken)
        {
            var result = await _documentService.UploadAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
        }
        /// <summary>
        /// Récupérer un document par Id
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DocumentResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _documentService.GetByIdAsync(id, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Télécharger un document
        /// </summary>
        [HttpGet("{id:guid}/download")]
        public async Task<ActionResult<UrlPresigneeResponse>> Download(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _documentService.GetDownloadUrlAsync(id, cancellationToken);

            return Ok(result);
        }
        /// <summary>
        /// Archiver un document
        /// </summary>
        [HttpPut("{id:guid}/archiver")]
        public async Task<IActionResult> Archiver(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _documentService.ArchiverAsync(id, cancellationToken);

            return NoContent();
        }

        /// <summary>
        /// Supprimer un document
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            await _documentService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }



    }
}
