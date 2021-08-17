using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System.Collections.Generic;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface ICreditoUsuarioRepository
    {
        public bool InsertCredito(CreditoUsuarioRequest creditoUsuarioRequest, int idCliente);
        public decimal SaldoPontos(string email);
        public IEnumerable<ExtratoClienteQueryResult> Extrato(string email);
        public bool InsertCreditoResgate(string descricao, decimal preco, int idCliente);
    }
}
