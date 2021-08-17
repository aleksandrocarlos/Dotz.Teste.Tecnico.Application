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
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpPost]
        [Route("CadastrarProduto")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm")]
        public IActionResult CadastrarProduto([FromQuery] ProdutoRequest ProdutoRequest)
        {
            try
            {

                var result = _produtoRepository.Insert(ProdutoRequest);

                if (!result)
                {
                    return new UnprocessableEntityObjectResult("Ocorreu um problema na gravação do produto!");
                }

                else
                {
                    return new OkObjectResult("produto Cadastrado com Sucesso!");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("BuscarProdutoPorId")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm")]
        public IActionResult BuscarProdutoPorId([FromQuery] int id)
        {
            try
            {

                if (_produtoRepository.VerificarProdutoExistente(id))
                {
                    var result = _produtoRepository.ObterProdutoPorId(id);


                    if (result == null)
                    {
                        return new OkObjectResult("Nenhum Produto foi encontrado");
                    }

                    return new OkObjectResult(result);
                }
                else
                {
                    return new NotFoundObjectResult($"Produto não cadastrados!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("BuscarProdutoPorNome")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm")]
        public IActionResult BuscarProdutoPorNome([FromQuery] string nome)
        {
            try
            {

                if (_produtoRepository.VerificarProdutoExistente(0, nome))
                {
                    var result = _produtoRepository.ObterProdutoPorNome(nome);


                    if (result == null)
                    {
                        return new OkObjectResult("Nenhum Produto foi encontrado");
                    }

                    return new OkObjectResult(result.ToList());
                }
                else
                {
                    return new NotFoundObjectResult($"Produto não cadastrados!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut]
        [Route("AtualizarProduto")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm")]
        public IActionResult AtualizarProduto([FromBody] PutProdutoRequest putProdutoRequest)
        {
            try
            {

                if (_produtoRepository.VerificarProdutoExistente(putProdutoRequest.Id))
                {
                    var result = _produtoRepository.Atualizar(putProdutoRequest);

                    if (!result)
                        return new UnprocessableEntityObjectResult("Ocorreu um problema na atualização do Produto");
                    else
                        return new OkObjectResult("Produto atualizado com sucesso!");
                }
                else
                {
                    return new NotFoundObjectResult($"Produto não cadastrados!");
                }



            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("ExcluirProduto")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm")]
        public IActionResult ExcluirProduto([FromQuery] int id)
        {
            try
            {

                if (_produtoRepository.VerificarProdutoExistente(id))
                {

                    var result = _produtoRepository.ExcluirProduto(id);

                    if (!result)
                        return new UnprocessableEntityObjectResult("Ocorreu um problema ao Excluir o Produto");
                    else
                        return new OkObjectResult("Produto Excluido com sucesso!");

                }
                else
                {
                    return new NotFoundObjectResult($"Produto não cadastrados!");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
