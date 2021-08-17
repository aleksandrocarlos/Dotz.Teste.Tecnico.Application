using System;

namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class PedidosRequest
    {
        public string endereco { get; set; }
        public Decimal valor { get; set; }
        public string status { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
