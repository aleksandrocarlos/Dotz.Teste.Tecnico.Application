namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class PutUsuarioRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int IdPerfil { get; set; }
    }
}
