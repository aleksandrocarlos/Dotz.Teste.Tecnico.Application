using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using Dotz.Teste.Tecnico.Infra.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Dotz.Teste.Tecnico.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditoUsuarioController : ControllerBase
    {

        private readonly ICreditoUsuarioRepository _creditoUsuarioRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public CreditoUsuarioController(ICreditoUsuarioRepository creditoUsuarioRepository, IUsuarioRepository usuarioRepository)
        {
            _creditoUsuarioRepository = creditoUsuarioRepository;
            _usuarioRepository = usuarioRepository;
        }


        [HttpPost]
        [Route("CadastrarCreditoUsuario")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm")]
        public IActionResult CadastrarCreditoUsuario([FromQuery] CreditoUsuarioRequest creditoUsuarioRequest)
        {
            try
            {
                if (ValidateEmailService.ValidaEmail(creditoUsuarioRequest.Email))
                {
                    var idCliente = _usuarioRepository.VerificarUsuarioExistenteEmail(creditoUsuarioRequest.Email);
                    if (idCliente > 0)
                    {
                        var result = _creditoUsuarioRepository.InsertCredito(creditoUsuarioRequest, idCliente);

                        if (!result)
                            return new UnprocessableEntityObjectResult("Ocorreu um problema no cadastro de moeda.");
                        else
                            return new OkObjectResult("Moeda Cadastrada com sucesso!");
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
        [Route("ConsultaSaldoExtrato")]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), (int)StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(bool), (int)StatusCodes.Status200OK)]
        [Authorize(Roles = "adm,cliente")]
        public IActionResult ConsultaSaldoExtrato([FromQuery] string email)
        {
            try
            {
                if (ValidateEmailService.ValidaEmail(email))
                {
                    if (_usuarioRepository.VerificarEmailExistente(email))
                    {
                        SaldoExtratoQueryResult saldoExtratoQueryResult = new SaldoExtratoQueryResult();

                        saldoExtratoQueryResult.Saldo = _creditoUsuarioRepository.SaldoPontos(email);
                        saldoExtratoQueryResult.Extrato = _creditoUsuarioRepository.Extrato(email).ToList();

                        return new OkObjectResult(saldoExtratoQueryResult);
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
    }
}
