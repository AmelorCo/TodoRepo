using System.Collections.Generic;
using System.Xml.Linq;
using Todolist.Domain.Abstract;
using Todolist.Domain.Entities;

namespace TodoList.Service
{
    public interface IPriorityService
    {
        IEnumerable<Priority> GetAll();
        IEnumerable<Priority> GetAllForUser(string userId);
        Priority GetById(int id);
        void NewPriority(Priority newPriority);
        void UpdatePriority(Priority priority, string userId);
        void Save();
    }

    public class PriorityService : IPriorityService
    {
        private readonly IPriorityRepository _priorityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PriorityService(IPriorityRepository priorityRepository, IUnitOfWork unitOfWork)
        {
            _priorityRepository = priorityRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Priority> GetAll()
        {
            return _priorityRepository.GetAll();
        }

        public IEnumerable<Priority> GetAllForUser(string userId)
        {
            return _priorityRepository.GetWhere(p => p.UserId == userId);
        }

        public Priority GetById(int id)
        {
            return _priorityRepository.GetById(id);
        }

        public void NewPriority(Priority newPriority)
        {
            _priorityRepository.Add(newPriority);
            Save();
        }

        public void UpdatePriority(Priority priority, string userId)
        {
            if (priority.UserId != userId) return;
            _priorityRepository.Update(priority);
            Save();
        }
        
        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}