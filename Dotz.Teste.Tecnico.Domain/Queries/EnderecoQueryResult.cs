using System;

namespace Dotz.Teste.Tecnico.Domain.Queries
{
    public class EnderecoQueryResult
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Endereco { get; set; }
        public int Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }

    }
}
