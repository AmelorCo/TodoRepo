using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Todolist.Domain.Entities;

namespace Todolist.Domain.Abstract
{
    public abstract class RepositoryBase<T> where T: class
    {
        private TodoEntities _dbContext;
        private readonly IDbSet<T> _dbSet;
        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbSet = DbContext.Set<T>();
        }

        protected TodoEntities DbContext
        {
            get { return _dbContext ?? (_dbContext = DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public virtual void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbSet.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbSet.Remove(obj);
        }
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public virtual T GetById(string id)
        {
            return _dbSet.Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbSet.FirstOrDefault<T>(where);
        }

        public virtual IEnumerable<T> GetWhere(Expression<Func<T, bool>> where)
        {
            return _dbSet.Where(where).ToList();
        }
    }
}
