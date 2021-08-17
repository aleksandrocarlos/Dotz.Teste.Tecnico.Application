using Dotz.Teste.Tecnico.Domain.Queries;
using Dotz.Teste.Tecnico.Domain.Requests;
using System.Collections.Generic;

namespace Dotz.Teste.Tecnico.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        public bool Insert(ProdutoRequest ProdutoRequest);
        public ProdutoQueryResult ObterProdutoPorId(int idUsuario);
        public IEnumerable<ProdutoQueryResult> ObterProdutoPorNome(string Nome);
        public bool Atualizar(PutProdutoRequest putUsuarioRequest);
        public bool ExcluirProduto(int id);
        public bool VerificarProdutoExistente(int id, string nome= "");
        public IEnumerable<ProdutoQueryResult> ListagemProdutos(ListagemProdutoRequest listagemProduto);
        public bool AtualizarEstoque(int idProduto);
        public decimal BuscarPrecoProduto(int idProduto);
        public bool ExisteProduto(int idProduto);
    }
}
