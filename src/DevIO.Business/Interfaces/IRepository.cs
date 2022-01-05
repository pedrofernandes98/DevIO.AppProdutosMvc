using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task Add(TEntity entity);

        Task<TEntity> GetById(Guid id);

        Task<List<TEntity>> GetAll();

        Task Edit(TEntity entity);

        Task Remove(Guid id);

        Task<IEnumerable<TEntity>> Search(Expression<Func<TEntity, bool>> predicate); //Permite enviar uma Expressão Lambda que retorna true ou false para verificar na lista e retornar apenas os registros cujo retorno seja true;

        Task<int> SaveChanges(); //Retorna o número de linhas afetadas
    }
}
