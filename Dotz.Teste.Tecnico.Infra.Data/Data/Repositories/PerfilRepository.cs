using Dapper;
using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotz.Teste.Tecnico.Infra.Data.Data.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string TableName = "Perfil";
        public PerfilRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool VerificarPerfilExistente(int idPerfil)
        {
            try
            {
                var query = $@"SELECT idPerfil FROM {TableName} where idperfil = {idPerfil}";

                var entity = _unitOfWork.Connection.Query<int>(query);

                return entity.Count() > 0;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<PerfilQueryResult> ObterPerfil()
        {
            try
            {
                var query = $@"SELECT  idPerfil as Id, Nome FROM {TableName}";

                var entity = _unitOfWork.Connection.Query<PerfilQueryResult>(query);

                return entity.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
