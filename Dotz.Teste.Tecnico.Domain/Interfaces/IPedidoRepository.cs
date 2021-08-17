using Dotz.Teste.Tecnico.Domain.Requests;
using System.Collections.Generic;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        public bool Insert(PedidoRequest resgateProdutoRequest, int idCliente, decimal valor);
        public IEnumerable<PedidosRequest> ListaPedidos(int usuario);
        public bool AtualizarStatus(StatusPedidoRequest statusPedidoRequest);
    }
}
