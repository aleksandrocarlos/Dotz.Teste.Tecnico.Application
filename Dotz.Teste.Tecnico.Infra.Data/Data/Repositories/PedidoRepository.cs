using Dapper;
using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Requests;
using System.Collections.Generic;
using System.Data;

namespace Dotz.Teste.Tecnico.Infra.Data.Data.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string TableName = "pedido";
        public PedidoRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Insert(PedidoRequest resgateProdutoRequest, int idCliente, decimal valor)
        {
            var query = $@" INSERT INTO {TableName}
                                (
                                IdProduto,
                                IdUsuario,
                                DataCriacao,
                                Valor,
                                IdEndereco,
                                IdStatusEntrega)
                                VALUES
                                (
                                @IdProduto,
                                @IdUsuario,
                                now(),
                                @Valor,
                                @IdEndereco,1)";

            var parameters = new DynamicParameters();
            parameters.Add("@IdProduto", resgateProdutoRequest.IdProduto);
            parameters.Add("@IdUsuario", idCliente);
            parameters.Add("@Valor", valor);
            parameters.Add("@IdEndereco", resgateProdutoRequest.IdEndereco);

            var result = _unitOfWork.Connection.Execute(query, parameters, commandType: CommandType.Text);

            return result > 0;



        }
        public IEnumerable<PedidosRequest> ListaPedidos(int idUsuario)
        {
            var query = $@"
                        select
                         concat(endereco.endereco ,' / ', endereco.numero,' ', endereco.cidade) as endereco
                         ,pedido.valor
                         ,statusentrega.Descricao as status
                         ,usuario.Nome
                         ,usuario.Email
                         from pedido
                        inner join endereco
                        on pedido.IdEndereco = endereco.idEndereco
                        inner join usuario
                        on pedido.IdUsuario = usuario.idUsuario
                        inner join statusentrega
                        on pedido.IdStatusEntrega = statusentrega.idStatusEntrega
                        where usuario.IdUsuario =  {idUsuario}";
            var result = _unitOfWork.Connection.Query<PedidosRequest>(query);
            return result;
        }
        public bool AtualizarStatus(StatusPedidoRequest statusPedidoRequest)
        {

            var query = $@"Update {TableName} set IdStatusEntrega = {statusPedidoRequest.IdStatusEntrega}  where idRescateProduto = {statusPedidoRequest.IdPedido}";
            var result = _unitOfWork.Connection.Execute(query);

            return result > 0;
        }

    }
}
