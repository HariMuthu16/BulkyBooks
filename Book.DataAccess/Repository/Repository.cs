using Book.DataAccess.Repository.IRepository;
using BookWeb.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Book.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();    // _db.Category. etc   _db.Product. etc
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperty = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var include in includeProperty.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }

            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProperty = null)
        {
            IQueryable<T> query = dbSet;

            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach(var include  in includeProperty.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }


            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }



    }
}
