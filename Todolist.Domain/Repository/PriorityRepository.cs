using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist.Domain.Abstract;
using Todolist.Domain.Entities;

namespace Todolist.Domain.Repository
{
    public class PriorityRepository : RepositoryBase<Priority>, IPriorityRepository
    {
        public PriorityRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
