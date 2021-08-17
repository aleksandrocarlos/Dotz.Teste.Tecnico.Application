using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dotz.Teste.Tecnico.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoRepository _EnderecoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public EnderecoController(IEnderecoRepository EnderecoRepository, IUsuarioRepository usuarioRepository)
        {
            _EnderecoRepository = EnderecoRepository;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        [Route("CadastrarEndereco")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult CadastrarEndereco([FromQuery] EnderecoRequest EnderecoRequest)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(EnderecoRequest.Endereco) ||
                    string.IsNullOrWhiteSpace(EnderecoRequest.Numero) ||
                    string.IsNullOrWhiteSpace(EnderecoRequest.Cidade) ||
                    string.IsNullOrWhiteSpace(EnderecoRequest.Estado) ||
                    string.IsNullOrWhiteSpace(EnderecoRequest.CEP) ||
                    EnderecoRequest.IdUsuario == 0)
                {

                    if (string.IsNullOrWhiteSpace(EnderecoRequest.Endereco))
                    {
                        return new UnprocessableEntityObjectResult("Informe o Endereco.");
                    }

                    if (string.IsNullOrWhiteSpace(EnderecoRequest.Numero))
                    {
                        return new UnprocessableEntityObjectResult("Informe o Numero.");
                    }

                    if (string.IsNullOrWhiteSpace(EnderecoRequest.Cidade))
                    {
                        return new UnprocessableEntityObjectResult("Informe a Cidade.");
                    }


                    if (string.IsNullOrWhiteSpace(EnderecoRequest.Estado))
                    {
                        return new UnprocessableEntityObjectResult("Informe o Estado.");
                    }


                    if (string.IsNullOrWhiteSpace(EnderecoRequest.CEP))
                    {
                        return new UnprocessableEntityObjectResult("Informe o CEP.");
                    }

                    if (EnderecoRequest.IdUsuario == 0)
                    {
                        return new UnprocessableEntityObjectResult("Informe do IdUsuario.");

                    }

                }

                var result = _EnderecoRepository.Insert(EnderecoRequest);

                if (!result)
                {
                    return new UnprocessableEntityObjectResult("Ocorreu um problema na gravação do Endereço!");
                }

                else
                {
                    return new OkObjectResult("Endereco Cadastrado com Sucesso!");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("BuscarEndereco")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult BuscarEndereco([FromQuery] int id)
        {
            try
            {

                if (_EnderecoRepository.VerificarEnderecoExistente(id))
                {
                    var result = _EnderecoRepository.ObterEndereco(id);


                    if (result == null)
                    {
                        return new OkObjectResult("Nenhum Endereco foi encontrado");
                    }

                    return new OkObjectResult(result.ToList());
                }
                else
                {
                    return new NotFoundObjectResult($"Endereco não cadastrados!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [Route("AtualizarEndereco")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult AtualizarEndereco([FromBody] PutEnderecoRequest putEnderecoRequest)
        {
            try
            {
                if (putEnderecoRequest.Id == 0 || putEnderecoRequest.IdUsuario == 0)
                {
                    if (putEnderecoRequest.Id == 0)
                    {
                        return new UnprocessableEntityObjectResult("Informe o Id");
                    }

                    if (putEnderecoRequest.IdUsuario == 0)
                    {
                        return new UnprocessableEntityObjectResult("Informe o IdUsuario");

                    }
                }


                var result = _EnderecoRepository.Atualizar(putEnderecoRequest);

                if (!result)
                    return new UnprocessableEntityObjectResult("Ocorreu um problema na atualização do Endereço");
                else
                    return new OkObjectResult("Endereço atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("ExcluirEndereco")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult ExcluirEndereco([FromQuery] int id)
        {
            try
            {
                if (_EnderecoRepository.VerificarEnderecoExistente(id))
                {

                    var result = _EnderecoRepository.ExcluirEndereco(id);

                    if (!result)
                        return new UnprocessableEntityObjectResult("Ocorreu um problema na Excluir o Endereço");
                    else
                        return new OkObjectResult("Endereço Excluido com sucesso!");

                }
                else
                {
                    return new NotFoundObjectResult($"Endereço não cadastrados!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
