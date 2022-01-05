using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        /// <summary>
        /// Retorna o endereço passando o id fornecedor por parâmetro
        /// </summary>
        /// <param name="fornecedorId">Id do fornecedor</param>
        /// <returns></returns>
        Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId);
    }
}
