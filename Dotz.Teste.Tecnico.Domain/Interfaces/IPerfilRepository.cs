using Dotz.Teste.Tecnico.Domain.Queries;
using System.Collections.Generic;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface IPerfilRepository
    {
        public bool VerificarPerfilExistente(int idPerfil);
        public List<PerfilQueryResult> ObterPerfil();
    }
}
