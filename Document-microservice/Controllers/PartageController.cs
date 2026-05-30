using Document_microservice.Application.DTOs.Requests;
using Document_microservice.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Document_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartageController : ControllerBase
    {
        private readonly PartageService _partageService;

        public PartageController(PartageService partageService)
        {
            _partageService = partageService;
        }

        /// <summary>
        /// Générer un lien de partage
        /// </summary>
        [HttpPost("partager")]
            public async Task<IActionResult> Partager(
             [FromBody] CreerPartageRequest request,
              CancellationToken cancellationToken)
            {
            var result = await _partageService.CreerPartageAsync(request, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Accéder via token
        /// </summary>
        [HttpGet("acces/{token}")]
        public async Task<IActionResult> AccesParToken(
            string token,
            CancellationToken cancellationToken)
        {
            var result = await _partageService.AccederParTokenAsync(token, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Révoquer un partage
        /// </summary>
        [HttpDelete("{partageId:guid}")]
        public async Task<IActionResult> Revoquer(
            Guid partageId,
            CancellationToken cancellationToken)
        {
            await _partageService.RevoquerAsync(
                partageId,
                cancellationToken);

            return NoContent();
        }
    }
}
