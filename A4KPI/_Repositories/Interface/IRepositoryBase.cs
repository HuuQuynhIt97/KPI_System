using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace A4KPI._Repositories.Interface
{
    public interface IRepositoryBase<T> where T : class
    {
        T FindById(object id);
        Task<T> FindByIdAsync(object id);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> FindAllAsEnumerable(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "");
        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);
        void SaveChangeAsync(T entity);
        void SaveChange(T entity);
        void Update(T entity);
        void UpdateRange(List<T> entities);
        Task<bool> SaveAll();
        void Save();
        void Remove(T entity);

        void Remove(int id);

        void RemoveMultiple(List<T> entities);

        IQueryable<T> GetAll();

        //Task<bool> SaveAll();
        //void Save();
        void AddRange(List<T> entity);
    }
    
}
