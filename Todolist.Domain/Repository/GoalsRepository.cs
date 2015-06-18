using Todolist.Domain.Abstract;
using Todolist.Domain.Entities;

namespace Todolist.Domain.Repository
{
    public class GoalsRepository : RepositoryBase<Goals>, IGoalsRepository
    {
        public GoalsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}