using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todolist.Domain.Entities;

namespace Todolist.Domain.Abstract
{
    public interface IDatabaseFactory
    {
        TodoEntities Get();
    }
}
