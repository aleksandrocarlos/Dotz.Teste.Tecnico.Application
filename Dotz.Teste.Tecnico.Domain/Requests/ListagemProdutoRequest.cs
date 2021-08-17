namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public  class ListagemProdutoRequest
    {
        public int Pagina { get; set; }
        public int limitePagina { get; set; }
        public string Descricao { get; set; }
    }
}
