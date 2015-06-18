using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist.Domain.Entities;

namespace Todolist.Domain.Abstract
{
    public class DatabaseFactory : IDatabaseFactory
    {
        private TodoEntities _dbContext;

        public TodoEntities Get()
        {
            return _dbContext ?? (_dbContext = new TodoEntities());
        }
    }
}
