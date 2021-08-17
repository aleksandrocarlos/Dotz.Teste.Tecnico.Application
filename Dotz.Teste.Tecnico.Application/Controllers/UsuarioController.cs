using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Requests;
using Dotz.Teste.Tecnico.Infra.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dotz.Teste.Tecnico.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPerfilRepository _perfilRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository, IPerfilRepository perfilRepository)
        {
            _usuarioRepository = usuarioRepository;
            _perfilRepository = perfilRepository;
        }

        [HttpPost]
        [Route("CadastrarUsuario")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        public IActionResult CadastrarUsuario([FromQuery] UsuarioRequest usuarioRequest)
        {
            try
            {
                if (ValidateEmailService.ValidaEmail(usuarioRequest.Email))
                {
                    if (!_usuarioRepository.VerificarEmailExistente(usuarioRequest.Email))
                    {
                        if (_perfilRepository.VerificarPerfilExistente(usuarioRequest.IdPerfil))
                        {
                            var result = _usuarioRepository.Insert(usuarioRequest);

                            if (!result)
                            {
                                return new UnprocessableEntityObjectResult("Ocorreu um problema na gravação do usuário");
                            }

                            else
                            {
                                return new OkObjectResult("Usuário Cadastrado com Sucesso!");

                            }
                        }
                        else
                        {
                            return new NotFoundObjectResult($"O Perfil informado não foi encontrado");
                        }
                    }
                    else
                    {
                        return new UnprocessableEntityObjectResult("Email já cadastrado.");
                    }

                }
                else
                {
                    return new NotFoundObjectResult($"O Email informado não é valido!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("BuscarUsuario")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult BuscarUsuario([FromQuery] GetUsuarioRequest getUsuarioRequest)
        {
            try
            {
                var result = _usuarioRepository.ObterUsuario(getUsuarioRequest);


                if (result == null)
                {
                    return new OkObjectResult("Nenhum Usuário foi encontrado");
                }

                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
   

        [HttpPut]
        [Route("AtualizarUsuario")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult AtualizarUsuario([FromBody] PutUsuarioRequest putUsuarioRequest)
        {
            try
            {

                if (ValidateEmailService.ValidaEmail(putUsuarioRequest.Email))
                {

                        var result = _usuarioRepository.Atualizar(putUsuarioRequest);

                        if (!result)
                            return new UnprocessableEntityObjectResult("Ocorreu um problema na atualização do usuário");
                        else
                            return new OkObjectResult("Usuário atualizado com sucesso!");
                    
                   
                }
                else
                {
                    return new NotFoundObjectResult($"O Email informado não é valido!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("ExcluirUsuario")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult ExcluirUsuario([FromQuery] int id)
        {
            try
            {
                if (_usuarioRepository.VerificarUsuarioExistente(id))
                {

                    var result = _usuarioRepository.ExcluirUsuario(id);

                    if (!result)
                        return new UnprocessableEntityObjectResult("Ocorreu um problema na Excluir o usuário");
                    else
                        return new OkObjectResult("Usuário Excluido com sucesso!");

                }
                else
                {
                    return new NotFoundObjectResult($"Usuário não cadastrados!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
