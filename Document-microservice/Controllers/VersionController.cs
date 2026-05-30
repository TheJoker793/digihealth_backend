using Document_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private readonly VersionService _versionService;
        public VersionController(VersionService versionService)
        {
            _versionService = versionService;
        }

        /// <summary>
        /// Liste des versions d'un document
        /// </summary>
        [HttpGet("{documentId:guid}")]
        public async Task<IActionResult> GetVersions(
            Guid documentId,
            CancellationToken cancellationToken)
        {
            var result = await _versionService.GetHistoriqueAsync(
                documentId,
                cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Restaurer une ancienne version
        /// </summary>
        [HttpPost("{documentId:guid}/restaurer/{numeroVersion:int}")]
        public async Task<IActionResult> RestaurerVersion(
            Guid documentId,
            int numeroVersion,
            CancellationToken cancellationToken)
        {
            await _versionService.RestaurerVersionAsync(
                documentId,
                numeroVersion,
                cancellationToken);

            return NoContent();
        }

    }
}
