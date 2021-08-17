namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class PedidoRequest
    {
        public string Email { get; set; }
        public int IdProduto { get; set; }
        public int IdEndereco { get; set; }
        public string Descricao { get; set; }
    }
}
