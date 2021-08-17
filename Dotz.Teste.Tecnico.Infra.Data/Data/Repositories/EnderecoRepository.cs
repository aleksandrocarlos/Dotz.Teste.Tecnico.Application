using Dapper;
using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dotz.Teste.Tecnico.Infra.Data.Data.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string TableName = "endereco";
        public EnderecoRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool Insert(EnderecoRequest enderecoRequest)
        {
            var query = $@"
                            INSERT INTO {TableName}
                            (
                             Endereco,
                             Numero,
                             Cidade,
                             Estado,
                             CEP,
                             Complemento,
                             DataCadastro,
                             DataAlteracao,
                             IdUsuario)
                            VALUES
                            (
                            '{enderecoRequest.Endereco}'
                            ,'{enderecoRequest.Numero}'
                            ,'{enderecoRequest.Cidade}'
                            ,'{enderecoRequest.Estado}'
                            ,'{enderecoRequest.CEP}'
                            ,'{enderecoRequest.Complemento}'
                            ,now()
                            ,now()
                            ,{enderecoRequest.IdUsuario})";
            var result = _unitOfWork.Connection.Execute(query);

            return result > 0;
        }

        public IEnumerable<EnderecoQueryResult> ObterEndereco(int id)
        {
            var query = $@"SELECT 
                            idEndereco AS Id,
                            Endereco,
                            Numero,
                            Cidade,
                            Estado,
                            CEP,
                            Complemento,
                            DataCadastro,
                            DataAlteracao,
                            IdUsuario            
                            FROM  endereco where  idEndereco = {id}";

            var result = _unitOfWork.Connection.Query<EnderecoQueryResult>(query);
            return result;
        }

        public bool Atualizar(PutEnderecoRequest putUsuarioRequest)
        {
            try
            {
                var query = $@"Update {TableName} SET  
                            Endereco = '{putUsuarioRequest.Endereco}',
                            Numero = '{putUsuarioRequest.Numero}',
                            Cidade = '{putUsuarioRequest.Cidade}',
                            Estado = '{putUsuarioRequest.Estado}',
                            CEP = '{putUsuarioRequest.CEP}',
                            Complemento = '{putUsuarioRequest.Complemento}',
                            DataAlteracao = now() where idEndereco = {putUsuarioRequest.Id} and  IdUsuario = {putUsuarioRequest.IdUsuario}";
                var result = _unitOfWork.Connection.Execute(query);

                return result > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool VerificarEnderecoExistente(int id)
        {
            try
            {
                var query = $@"SELECT idEndereco FROM {TableName} WHERE idEndereco =  {id}";

                var result = _unitOfWork.Connection.Query<int>(query);

                return result.Count() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ExcluirEndereco(int id)
        {
            try
            {
                var query = $@"DELETE FROM {TableName} WHERE idEndereco = {id}";
                var result = _unitOfWork.Connection.Execute(query);

                return result > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
