using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        /// <summary>
        /// Retorna uma coleção de Produtos de um Fornecedor
        /// </summary>
        /// <param name="fornecedorId">Id do fornecedor</param>
        /// <returns></returns>
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);

        /// <summary>
        /// Retorna uma coleção de produtos com os seus respectivos fornecedores
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Produto>> ObterProdutosFornecedores();

        /// <summary>
        /// Retorna o Produto e seu respectivo fornecedor
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns></returns>
        Task<Produto> ObterProdutoFornecedor(Guid id);
    }
}
