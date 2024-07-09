using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange( IEnumerable<T> entity);

        T Get(Expression<Func<T,bool>> filter, string? includeProperty = null);

        IEnumerable<T> GetAll(string? includeProperty = null);

    }


}
