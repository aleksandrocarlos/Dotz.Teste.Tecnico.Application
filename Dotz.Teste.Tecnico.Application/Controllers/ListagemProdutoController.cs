using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dotz.Teste.Tecnico.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListagemProdutoController : ControllerBase
    {

        private readonly IProdutoRepository _produtoRepository;
        public ListagemProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }


        [HttpGet]
        [Route("ListagemProduto")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult ListagemProduto([FromQuery] ListagemProdutoRequest listagemProduto)
        {
            try
            {

                var result = _produtoRepository.ListagemProdutos(listagemProduto);


                if (result == null)
                {
                    return new OkObjectResult("Nenhum Produto foi encontrado");
                }

                return new OkObjectResult(result);


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
