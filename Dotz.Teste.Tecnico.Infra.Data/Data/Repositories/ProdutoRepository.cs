using Dapper;
using Dotz.Teste.Tecnico.Domain.Interfaces;
using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Dotz.Teste.Tecnico.Infra.Data.Data.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string TableName = "produto";
        public ProdutoRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool Insert(ProdutoRequest produtoRequest)
        {
            var query = $@"INSERT INTO {TableName}
                            (Nome,
                            Descricao,
                            Preco,
                            Estoque,
                            DataCriacao,
                            DataAlteracao,
                            Ativo)
                            VALUES
                            (@Nome,
                            @Descricao,
                            @Preco,
                            @Estoque,
                            now(),
                            now(),
                            @Ativo)";

            var parameters = new DynamicParameters();
            parameters.Add("@Nome", produtoRequest.Nome);
            parameters.Add("@Descricao", produtoRequest.Descricao);
            parameters.Add("@Preco", produtoRequest.Preco);
            parameters.Add("@Estoque", produtoRequest.Estoque);
            parameters.Add("@Ativo", produtoRequest.Ativo);

            //var result = _unitOfWork.Connection.Execute(query, parameters, commandType: CommandType.Text);

            var result = _unitOfWork.Connection.Execute(query, parameters, commandType: CommandType.Text);

            return result > 0;
        }
        public ProdutoQueryResult ObterProdutoPorId(int id)
        {
            var query = $@"Select 
                            idProduto as Id,
                            Nome,
                            Descricao,
                            Preco,
                            Estoque,
                            DataCriacao,
                            DataAlteracao,
                            Ativo
                            from {TableName} where IdProduto = {id}";

            var result = _unitOfWork.Connection.QueryFirstOrDefault<ProdutoQueryResult>(query);
            return result;
        }
        public IEnumerable<ProdutoQueryResult> ObterProdutoPorNome(string Nome)
        {
            var query = $@"Select 
                            idProduto as Id,
                            Nome,
                            Descricao,
                            Preco,
                            Estoque,
                            DataCriacao,
                            DataAlteracao
                            from {TableName} where Nome like '%{Nome}%'";

            var result = _unitOfWork.Connection.Query<ProdutoQueryResult>(query);
            return result;
        }
        public bool Atualizar(PutProdutoRequest putUsuarioRequest)
        {
            try
            {
                var query = $@"UPDATE {TableName} SET 
                                                    Nome = @Nome,
                                                    Descricao = @Descricao,
                                                    Preco =  @Preco,
                                                    Estoque = @Estoque,
                                                    DataAlteracao = now(),
                                                    Ativo  = @Ativo where idProduto = @idProduto";

                var parameters = new DynamicParameters();
                parameters.Add("@Nome", putUsuarioRequest.Nome);
                parameters.Add("@Descricao", putUsuarioRequest.Descricao);
                parameters.Add("@Preco", putUsuarioRequest.Preco);
                parameters.Add("@Estoque", putUsuarioRequest.Estoque);
                parameters.Add("@Ativo", putUsuarioRequest.Ativo);
                parameters.Add("@idProduto", putUsuarioRequest.Id);

                var result = _unitOfWork.Connection.Execute(query, parameters, commandType: CommandType.Text);

                return result > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool VerificarProdutoExistente(int id, string nome)
        {
            try
            {
                var query = $@"SELECT idProduto FROM {TableName} WHERE ";
                if (id > 0)
                {
                    query += $@" idProduto =  {id}";
                }
                else
                {
                    query += $@" nome like  '%{nome}%'";
                }

                var result = _unitOfWork.Connection.Query<int>(query);

                return result.Count() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ExcluirProduto(int id)
        {
            try
            {
                var query = $@"DELETE FROM {TableName} WHERE idProduto = {id}";
                var result = _unitOfWork.Connection.Execute(query);

                return result > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<ProdutoQueryResult> ListagemProdutos(ListagemProdutoRequest listagemProduto)
        {
            var query = $@"Select 
                            idProduto as Id,
                            Nome,
                            Descricao,
                            Preco,
                            Estoque,
                            Ativo
                            from {TableName} where  Ativo = 1
                             ORDER BY 1 asc ";

            if (!string.IsNullOrWhiteSpace(listagemProduto.Descricao))
            {
                query += $@" and Nome like '%{listagemProduto.Descricao}%'";
            }

            var take = listagemProduto.limitePagina <= 0 ? 1 : listagemProduto.limitePagina;
            var skip = listagemProduto.Pagina <= 1 ? 0 : (listagemProduto.Pagina - 1) * take;

            query += $"  LIMIT {take} OFFSET {skip}";


            var result = _unitOfWork.Connection.Query<ProdutoQueryResult>(query);
            return result;
        }
        public bool AtualizarEstoque(int IdProduto)
        {
            var query = $@"update {TableName} set Estoque = (select estoque - 1 from produto where idproduto = {IdProduto}) where  idproduto = {IdProduto} ";

            var result = _unitOfWork.Connection.Execute(query);

            return result > 0;
        }
        public decimal BuscarPrecoProduto(int idProduto)
        {
            var query = $@"select Preco from {TableName} where idproduto = {idProduto}";

            var result = _unitOfWork.Connection.Query<decimal>(query);
            return result.FirstOrDefault();
        }
        public bool ExisteProduto(int idProduto)
        {
            var query = $@"select idproduto from {TableName} where idproduto = {idProduto} ";
            var result = _unitOfWork.Connection.Query<int>(query);
            return result.FirstOrDefault() > 0;
        }

    }
}
