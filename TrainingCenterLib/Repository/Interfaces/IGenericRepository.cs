using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingCenterLib.Repository.Interfaces
{
   
    public interface IGenericRepository<TEntity, TContext> 
    where TEntity : class
    where TContext : DbContext
    {
        void Add(TEntity entity);
        void Delete(int id);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        void SoftDelete(int id);
        void Update(TEntity entity);
        void Save();
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task SaveAsync();
    }


}
