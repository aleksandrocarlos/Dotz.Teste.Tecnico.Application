using System;

namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class PutProdutoRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }
    }
}
