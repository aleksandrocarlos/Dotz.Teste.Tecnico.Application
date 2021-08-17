using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System.Collections.Generic;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        public bool Insert(EnderecoRequest enderecoRequest);
        public IEnumerable<EnderecoQueryResult> ObterEndereco(int id);
        public bool Atualizar(PutEnderecoRequest putUsuarioRequest);
        public bool VerificarEnderecoExistente(int id);
        public bool ExcluirEndereco(int id);
    }
}
