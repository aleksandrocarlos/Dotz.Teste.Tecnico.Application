using Dapper;
using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Dotz.Teste.Tecnico.Infra.Data.Data.Repositories
{
    public class CreditoUsuarioRepository : ICreditoUsuarioRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string TableName = "credito";
        public CreditoUsuarioRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool InsertCredito(CreditoUsuarioRequest creditoUsuarioRequest, int idCliente)
        {
            var query = $@"
                            INSERT INTO {TableName}
                                (
                                IdUsuarioCredito,
                                Valor,
                                Descricao,
                                DataCriacao)
                                VALUES
                                (
                                @IdUsuarioCredito,
                                @Valor,
                                @DataCriacao,
                                now())";

            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuarioCredito", idCliente);
            parameters.Add("@Valor", creditoUsuarioRequest.Valor);
            parameters.Add("@DataCriacao", creditoUsuarioRequest.Descricao);

            var result = _unitOfWork.Connection.Execute(query, parameters, commandType: CommandType.Text);

            return result > 0;
        }

        public decimal SaldoPontos(string email)
        {
            var query = $@"
                            SELECT
                             sum(credito.valor) 
                            FROM credito
                            inner join usuario on
                            credito.IdUsuarioCredito = usuario.IdUsuario
                            where usuario.email = '{email}'";
            var result = _unitOfWork.Connection.Query<decimal>(query);

            return result.FirstOrDefault();
        }

        public IEnumerable<ExtratoClienteQueryResult> Extrato(string email)
        {
            var query = $@"
                            SELECT
                            credito.valor
                            ,credito.descricao
                            ,credito.DataCriacao
                            FROM credito
                            inner join usuario on
                            credito.IdUsuarioCredito = usuario.IdUsuario
                            where usuario.email = '{email}'";

            var result = _unitOfWork.Connection.Query<ExtratoClienteQueryResult>(query);
            return result;
        }
        public bool InsertCreditoResgate(string descricao, decimal preco, int idCliente)
        {

            var query = $@"
                            INSERT INTO {TableName}
                                (
                                IdUsuarioCredito,
                                Valor,
                                Descricao,
                                DataCriacao)
                                VALUES
                                (
                                @IdUsuarioCredito,
                                @Valor,
                                @Descricao,
                                now())";

            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuarioCredito", idCliente);
            parameters.Add("@Valor", decimal.Parse($"-{preco}"));
            parameters.Add("@Descricao", descricao);

            var result = _unitOfWork.Connection.Execute(query, parameters, commandType: CommandType.Text);

            return result > 0;
        }

    }
}
