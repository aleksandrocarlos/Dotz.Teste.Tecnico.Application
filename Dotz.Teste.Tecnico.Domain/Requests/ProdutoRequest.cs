using System;

namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class ProdutoRequest
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }

    }
}
