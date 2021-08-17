using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Requests;
using Dotz.Teste.Tecnico.Infra.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dotz.Teste.Tecnico.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public AutenticacaoController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        [Route("Autenticacao")]
        [AllowAnonymous]
        public ActionResult<dynamic> Autenticacao([FromBody] UsuarioAutenticacaoRequest usuarioAutenticacaoRequest)
        {
            if (string.IsNullOrWhiteSpace(usuarioAutenticacaoRequest.Email) || string.IsNullOrWhiteSpace(usuarioAutenticacaoRequest.Senha) )
            {
                if (string.IsNullOrWhiteSpace(usuarioAutenticacaoRequest.Email))
                    return new UnprocessableEntityObjectResult("Informe o Email.");

                else
                    return new UnprocessableEntityObjectResult("Informe a Senha.");
            }

            if (ValidateEmailService.ValidaEmail(usuarioAutenticacaoRequest.Email))
            {
                var usuario = _usuarioRepository.DadosUsuario(usuarioAutenticacaoRequest);
                if (usuario == null)
                    return NotFound(new { message = "Usuário ou senha inválidos" });
                var token = TokenService.GenerateToken(usuario);
                return Ok(new { usuario = usuario, token = token });
            }
            else
            {
                return new NotFoundObjectResult($"O Email informado não é valido!");
            }

        }
    }
}
