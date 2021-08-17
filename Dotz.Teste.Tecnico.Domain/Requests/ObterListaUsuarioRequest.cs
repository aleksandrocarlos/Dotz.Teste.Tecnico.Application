namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class ObterListaUsuarioRequest
    {
        public string Filtro { get; set; }
        public int Pagina { get; set; } = 0;
        public int Limite { get; set; } = 10;
        public string Ordenacao { get; set; } = "1 asc";
    }
}
