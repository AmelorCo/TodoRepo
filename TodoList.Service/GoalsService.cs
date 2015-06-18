using System.Collections.Generic;
using Todolist.Domain.Abstract;
using Todolist.Domain.Entities;

namespace TodoList.Service
{
    public interface IGoalsService
    {
        IEnumerable<Goals> GetUserGoals(string userId);
        IEnumerable<Goals> GetCompletedGoals(string userId);
        IEnumerable<Goals> GetActualGoals(string userId);
        Goals GetById(int id);
        void CreateGoal(Goals goal);
        void RemoveGoal(int goalId, string userId);
        void UpdateGoal(Goals goal, string userId);
        void CompleteGoal(int goalId, string userId);
        void Save();
    }

    public class GoalsService : IGoalsService
    {
        private readonly IGoalsRepository _goalsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GoalsService(IGoalsRepository goalsRepository, IUnitOfWork unitOfWork)
        {
            _goalsRepository = goalsRepository;
            _unitOfWork = unitOfWork;
        }

        public void CreateGoal(Goals goal)
        {
            _goalsRepository.Add(goal);
            Save();
        }

        public Goals GetById(int id)
        {
            return _goalsRepository.GetById(id);
        }

        public void RemoveGoal(int goalId, string userId)
        {
            var goal = _goalsRepository.GetById(goalId);
            if (goal == null || goal.UserId != userId) return;

            goal.Deleted = true;
            _goalsRepository.Update(goal);
            Save();
        }

        public void UpdateGoal(Goals goal, string userId)
        {
            if(goal.UserId != userId) return;
            _goalsRepository.Update(goal);
            Save();
        }

        public void CompleteGoal(int goalId, string userId)
        {
            var goal = _goalsRepository.GetById(goalId);
            if (goal == null || goal.UserId != userId) return;
            goal.Completed = true;
            _goalsRepository.Update(goal);
            Save();
        }

        public IEnumerable<Goals> GetUserGoals(string userId)
        {
            var userGoals = _goalsRepository.GetWhere(p => p.UserId == userId);
            return userGoals;
        }

        public IEnumerable<Goals> GetActualGoals(string userId)
        {
            var actualGoals =
                _goalsRepository.GetWhere(p => p.UserId == userId && p.Deleted == false && p.Completed == false);
            return actualGoals;
        }

        public IEnumerable<Goals> GetCompletedGoals(string userId)
        {
            var completedGoals = _goalsRepository.GetWhere(p => p.UserId == userId && p.Completed && p.Deleted == false);
            return completedGoals;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}