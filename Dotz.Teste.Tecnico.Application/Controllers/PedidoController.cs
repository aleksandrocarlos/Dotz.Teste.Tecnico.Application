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
    public class PedidoController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICreditoUsuarioRepository _creditoUsuarioRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IProdutoRepository _produtoRepository;
        public PedidoController(IUsuarioRepository usuarioRepository, ICreditoUsuarioRepository creditoUsuarioRepository, IPedidoRepository pedidoRepository, IProdutoRepository produtoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _creditoUsuarioRepository = creditoUsuarioRepository;
            _pedidoRepository = pedidoRepository;
            _produtoRepository = produtoRepository;
        }


        [HttpPost]
        [Route("ResgateProduto")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult ResgateProduto([FromQuery] PedidoRequest PedidoRequest)
        {
            try
            {
                if (ValidateEmailService.ValidaEmail(PedidoRequest.Email))
                {
                    var idCliente = _usuarioRepository.VerificarUsuarioExistenteEmail(PedidoRequest.Email);
                    if (idCliente > 0)
                    {
                        if (_produtoRepository.ExisteProduto(PedidoRequest.IdProduto))
                        {
                            var saldo = _creditoUsuarioRepository.SaldoPontos(PedidoRequest.Email);

                            var preco = _produtoRepository.BuscarPrecoProduto(PedidoRequest.IdProduto);
                            if (saldo < preco)
                            {
                                return new UnprocessableEntityObjectResult("saldo insuficiente!");
                            }
                            else
                            {
                                var result = _creditoUsuarioRepository.InsertCreditoResgate(PedidoRequest.Descricao, preco, idCliente);
                                if (result)
                                {
                                    var insert = _pedidoRepository.Insert(PedidoRequest, idCliente, preco);
                                    if (!insert)
                                    {
                                        return new UnprocessableEntityObjectResult("Ocorreu um problema no Resgate do Produto.");
                                    }
                                    else
                                    {

                                        return new OkObjectResult("Resgate Realisado com sucesso!");
                                    }
                                }
                                else
                                {
                                    return new UnprocessableEntityObjectResult("Ocorreu um problema no Resgate do Produto.");
                                }
                            }
                        }
                        else
                        {
                            return new UnprocessableEntityObjectResult("Produto não cadastrado.");
                        }
                    }
                    else
                    {
                        return new UnprocessableEntityObjectResult("Email não cadastrado.");
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
        [Route("ListagemPedidos")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult ListagemPedidos([FromQuery] int idUsuario)
        {
            try
            {
                if (_usuarioRepository.VerificarUsuarioExistente(idUsuario))
                {
                    var result = _pedidoRepository.ListaPedidos(idUsuario);
                    if (result == null)
                    {
                        return new OkObjectResult("Nenhum Produto foi encontrado");
                    }

                    return new OkObjectResult(result);
                }
                else
                {
                    return new UnprocessableEntityObjectResult("Usuário não cadastrado.");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("AtualizarStatusPedido")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult AtualizarStatusPedido([FromBody] StatusPedidoRequest statusPedidoRequest)
        {
            try
            {
                if (_usuarioRepository.VerificarUsuarioExistente(statusPedidoRequest.IdPedido))
                {
                    var result = _pedidoRepository.AtualizarStatus(statusPedidoRequest);
                    if (!result)
                        return new UnprocessableEntityObjectResult("Ocorreu um problema na atualização do Status");
                    else
                        return new OkObjectResult("Status atualizado com sucesso!");
                }
                else
                {
                    return new UnprocessableEntityObjectResult("Usuário não cadastrado.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
