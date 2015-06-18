using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist.Domain.Entities;

namespace Todolist.Domain.Abstract
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory _databaseFactory;
        private TodoEntities _dbContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public TodoEntities DbContext
        {
            get { return _dbContext ?? (_dbContext = _databaseFactory.Get()); }
        }

        public void Commit()
        {
            DbContext.Commit();
        }
    }
}
