using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository.Interfaces;

namespace TrainingCenter.Lib.Repository
{
    //internal class GenericRepository<T> : IGenericRepository<T> where T : class
    //{

    //    private readonly TrainingCenterLibDbContext _Dbcontext;
    //    private readonly DbSet<T> _dbSet;


    //    public GenericRepository(TrainingCenterLibDbContext context)
    //    {
    //        _Dbcontext = context;
    //        _dbSet = _Dbcontext.Set<T>();
    //    }

    //    public void Add(T entity)
    //    {
            
    //        _dbSet.Add(entity);
    //    }

    //    public void Delete(int id)
    //    {
    //        T entity = _dbSet.Find(id);
    //        _dbSet.Remove(entity);
    //    }

    //    public IEnumerable<T> GetAll()
    //    {
    //        return _dbSet.ToList();
    //    }

    //    public T GetById(int id)
    //    {
    //        return _dbSet.Find(id);
    //    }

    //    public void SoftDelete(int id)
    //    {
    //        var entity = _dbSet.Find(id);
    //        if (entity != null)
    //        {
    //            var property = entity.GetType().GetProperty("IsDeleted");
    //            if (property != null && property.PropertyType == typeof(bool))
    //            {
    //                property.SetValue(entity, true);
    //                Update(entity);
    //            }
    //        }
    //    }

    //    public void Update(T entity)
    //    {
    //        _dbSet.Attach(entity);
    //        _Dbcontext.Entry(entity).State = EntityState.Modified;
    //    }

    //    public void Save()
    //    {
    //        _Dbcontext.SaveChanges();
    //    }
    //}
}
