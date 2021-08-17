using Dotz.Teste.Tecnico.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dotz.Teste.Tecnico.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly IPerfilRepository _perfilRepository;
        public PerfilController(IPerfilRepository perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }

        [HttpGet]
        [Route("GetPerfil")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        public IActionResult GetPerfil()
        {
            var perfil = _perfilRepository.ObterPerfil();

            return new OkObjectResult(perfil);
        }
    }
}
