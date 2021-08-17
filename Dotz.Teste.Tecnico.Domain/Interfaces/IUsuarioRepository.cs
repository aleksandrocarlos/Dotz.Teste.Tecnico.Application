using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        public bool VerificarEmailExistente(string Email, int id = 0);
        public UsuarioQueryResult ObterUsuario(GetUsuarioRequest getUsuarioRequest);
        bool Insert(UsuarioRequest userAccountDTO);
        public bool Atualizar(PutUsuarioRequest putUsuarioRequest);
        public bool ExcluirUsuario(int id);
        public bool VerificarUsuarioExistente(int id);
        public UsuarioQueryResult DadosUsuario(UsuarioAutenticacaoRequest usuarioAutenticacaoRequest);
        public int VerificarUsuarioExistenteEmail(string email);
    }
}
