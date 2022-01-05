using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorRepository : IRepository<Fornecedor>
    {
        /// <summary>
        /// Retorna o Fornecedor e o seu respectivo Endereço
        /// </summary>
        /// <param name="id">Id do fornecedor</param>
        /// <returns></returns>
        Task<Fornecedor> ObterFornecedorEndereco(Guid id);

        /// <summary>
        /// Retorna o Fornecedor, o Endereço deste fornecedor e a lista de produtos deste fornecedor
        /// </summary>
        /// <param name="id">Id do fornecedor</param>
        /// <returns></returns>
        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);
    }
}
