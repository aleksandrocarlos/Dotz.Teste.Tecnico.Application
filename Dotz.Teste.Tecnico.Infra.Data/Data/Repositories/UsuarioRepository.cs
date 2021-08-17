using Dapper;
using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotz.Teste.Tecnico.Infra.Data.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string TableName = "usuario";
        public UsuarioRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool VerificarEmailExistente(string Email, int id = 0)
        {
            try
            {
                var query = $@"SELECT idUsuario FROM {TableName} WHERE  email = '{Email}'";
                if (id > 0)
                {
                    query += $@" and idUsuario = {id}";
                }

                var entity = _unitOfWork.Connection.Query<int>(query);

                return entity.Count() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool VerificarUsuarioExistente(int id)
        {
            try
            {
                var query = $@"SELECT idUsuario FROM {TableName} WHERE  idUsuario = {id}";

                var result = _unitOfWork.Connection.Query<int>(query);

                return result.Count() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Insert(UsuarioRequest usuarioRequest)
        {
            try
            {
                var query = $@"
                                INSERT INTO {TableName}
                               (Nome,
                                Email,
                                Senha,
                                IdPerfil,
                                DataCriacao,
                                DataAlteracao)
                                VALUES
                                (
                                '{usuarioRequest.Nome}',
                                '{usuarioRequest.Email}',
                                '{usuarioRequest.Senha}',
                                {usuarioRequest.IdPerfil},
                                now(),
                                now())";

                _unitOfWork.Connection.Query(query);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public UsuarioQueryResult DadosUsuario(UsuarioAutenticacaoRequest usuarioAutenticacaoRequest)
        {
            try
            {
                var query = $@"
                            SELECT 
	                            usuario.idUsuario
                                ,usuario.Nome
                                ,usuario.Email
                                ,usuario.IdPerfil
                                ,usuario.DataCriacao
                                ,usuario.DataAlteracao
                                ,perfil.idPerfil
                                ,perfil.Nome as  Perfil
                             FROM usuario 
                            inner join perfil  
                            on usuario.idPerfil = perfil.idPerfil
                            where usuario.Email = '{usuarioAutenticacaoRequest.Email}'
                            AND  Senha = '{usuarioAutenticacaoRequest.Senha}'";

                var result = _unitOfWork.Connection.QueryFirstOrDefault<UsuarioQueryResult>(query);
                return result;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool Atualizar(PutUsuarioRequest putUsuarioRequest)
        {
            try
            {
                var query = $@"Update {TableName} SET DataAlteracao = now(),
                                                      Nome = '{putUsuarioRequest.Nome}',
                                                      Email = '{putUsuarioRequest.Email}',
                                                      Senha = '{putUsuarioRequest.Senha}',
                                                      IdPerfil = {putUsuarioRequest.IdPerfil}
                                                        where idUsuario = {putUsuarioRequest.Id} ";
                var result = _unitOfWork.Connection.Execute(query);

                return result > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool ExcluirUsuario(int id)
        {
            try
            {
                var query = $@"Delete from {TableName} where  idUsuario = {id}";
                var result = _unitOfWork.Connection.Execute(query);

                return result > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UsuarioQueryResult ObterUsuario(GetUsuarioRequest getUsuarioRequest)
        {
            try
            {

                List<string> listCampo = new List<string>();

                if (!string.IsNullOrWhiteSpace(getUsuarioRequest.Nome))
                    listCampo.Add($@"usuario.Nome = '{getUsuarioRequest.Nome}' ");

                if (!string.IsNullOrWhiteSpace(getUsuarioRequest.Email))
                    listCampo.Add($@"usuario.Email = '{getUsuarioRequest.Email}' ");

                if (getUsuarioRequest.IdPerfil > 0)
                    listCampo.Add($@"usuario.IdPerfil = {getUsuarioRequest.IdPerfil}");

                var where = string.Join("AND ", listCampo);

                var query = $@"
                            SELECT 
	                            usuario.idUsuario
                                ,usuario.Nome
                                ,usuario.Email
                                ,usuario.Senha
                                ,usuario.IdPerfil
                                ,usuario.DataCriacao
                                ,usuario.DataAlteracao
                                ,perfil.idPerfil
                                ,perfil.Nome as  Perfil
                             FROM usuario 
                            inner join perfil  
                            on usuario.idPerfil = perfil.idPerfil
                            where {where}";

                var result = _unitOfWork.Connection.QueryFirstOrDefault<UsuarioQueryResult>(query);
                return result;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public int VerificarUsuarioExistenteEmail(string email)
        {
            try
            {
                var query = $@"SELECT idUsuario FROM {TableName} WHERE  email = '{email}'";

                var result = _unitOfWork.Connection.QueryFirstOrDefault<int>(query);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
