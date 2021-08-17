namespace Dotz.Teste.Tecnico.Domain.Requests
{
    public class PutEnderecoRequest
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
    }
}
