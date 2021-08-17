using System;

namespace Dotz.Teste.Tecnico.Domain.Queries
{
    public class UsuarioQueryResult
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public int IdPerfil { get; set; }
        public string Perfil { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
