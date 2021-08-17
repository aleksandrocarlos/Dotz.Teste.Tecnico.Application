using System;

namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class CreditoUsuarioRequest
    {
        public string Email { get; set; }
        public Decimal Valor { get; set; }
        public string Descricao { get; set; }
    }
}
