using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;
using Todolist.Domain.Entities;
using Todolist.Web.ViewModels.Goals;
using Todolist.Web.ViewModels.Priority;
using TodoList.Service;

namespace Todolist.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IGoalsService _goalsService;
        private readonly IPriorityService _priorityService;
        private readonly int pageSize = 3;

        public HomeController(IGoalsService goalsService, IPriorityService priorityService)
        {
            _goalsService = goalsService;
            _priorityService = priorityService;
        }

        public ViewResult Index()
        {
            return View();
        }

        // GET: Goals/List
        /// <summary>
        /// Страница с активными (не выполненными) задачами
        /// </summary>
        /// <param name="page">Номер страницы </param>
        /// <param name="viewModel">Модель представления списка задач</param>
        /// <returns></returns> 
        public ActionResult List(int? page, UserGoalsViewModel viewModel)
        {
            if (page == null) page = 1;
            
            //if (viewModel == null)
            //{
            //    viewModel = new UserGoalsViewModel();
            //}

            var userId = HttpContext.User.Identity.GetUserId();

            ViewBag.Title = "Активные задачи";
            ViewBag.PartialName = "_GoalsList";
            ViewBag.ActionName = "List";

            DateTime startDate;
            DateTime endDate;

            DateTime.TryParse(viewModel.StartDateStr, out startDate);
            DateTime.TryParse(viewModel.EndDateStr, out endDate);

            var userGoals = _goalsService.GetActualGoals(userId);

            if (startDate > DateTime.MinValue)
            {
                userGoals = userGoals.Where(p => p.CreationDate.Date >= startDate);
            }

            if (endDate > DateTime.MinValue)
            {
                userGoals = userGoals.Where(p => p.CreationDate.Date <= endDate);
            }

            if (viewModel.Priority != null)
            {
                userGoals = userGoals.Where(p => p.PriorityId == viewModel.Priority);
            }

            if (!viewModel.SearchStr.IsNullOrWhiteSpace())
            {
                userGoals =
                    userGoals.Where(
                        p =>
                            p.Name.ToLower().Contains(viewModel.SearchStr.ToLower()) ||
                            p.Description.ToLower().Contains(viewModel.SearchStr.ToLower()));
            }

            viewModel.PagedGoals = new PagedList<Goals>(userGoals, page.Value, pageSize);

            var priorityList = _priorityService.GetAllForUser(userId);
            viewModel.Priorities = new SelectList(priorityList, "PriorityId", "Name");
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_GoalsList", viewModel.PagedGoals);
            }

            return View(viewModel);
        }

        // GET: Goals/CompletedGoals
        public ActionResult CompletedGoals(int? page, UserGoalsViewModel viewModel)
        {
            if (page == null) page = 1;
            var userId = HttpContext.User.Identity.GetUserId();

            //if (viewModel == null)
            //{
            //    viewModel = new UserGoalsViewModel();
            //}

            ViewBag.Title = "Выполненные задачи";
            ViewBag.PartialName = "_CompletedGoalsList";
            ViewBag.ActionName = "CompletedGoals";

            DateTime startDate;
            DateTime endDate;

            DateTime.TryParse(viewModel.StartDateStr, out startDate);
            DateTime.TryParse(viewModel.EndDateStr, out endDate);

            var userGoals = _goalsService.GetCompletedGoals(userId);

            if (startDate > DateTime.MinValue)
            {
                userGoals = userGoals.Where(p => p.CreationDate.Date >= startDate);
            }

            if (endDate > DateTime.MinValue)
            {
                userGoals = userGoals.Where(p => p.CreationDate.Date <= endDate);
            }

            if (viewModel.Priority != null)
            {
                userGoals = userGoals.Where(p => p.PriorityId == viewModel.Priority);
            }

            if (!viewModel.SearchStr.IsNullOrWhiteSpace())
            {
                userGoals =
                    userGoals.Where(
                        p =>
                            p.Name.ToLower().Contains(viewModel.SearchStr.ToLower()) ||
                            p.Description.ToLower().Contains(viewModel.SearchStr.ToLower()));
            }

            var priorityList = _priorityService.GetAllForUser(userId);
            viewModel.Priorities = new SelectList(priorityList, "PriorityId", "Name");

            viewModel.PagedGoals = new PagedList<Goals>(userGoals, page.Value, pageSize);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_CompletedGoalsList", viewModel.PagedGoals);
            }

            return View("List", viewModel);
        }

        public ActionResult RemoveGoal(int id)
        {
            string userId = HttpContext.User.Identity.GetUserId();
            _goalsService.RemoveGoal(id, userId);
            return RedirectToAction("List");
        }

        public ActionResult CompleteGoal(int id)
        {
            string userId = HttpContext.User.Identity.GetUserId();
            _goalsService.CompleteGoal(id, userId);
            return RedirectToAction("List");
        }

        /// <summary>
        /// Редактирование задачи - GET
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult NewGoal()
        {
            string userId = HttpContext.User.Identity.GetUserId();

            var newGoal = new NewGoalViewModel();
            var priority = _priorityService.GetAllForUser(userId);
            newGoal.Priorities = new SelectList(priority, "PriorityId", "Name");

            return View(newGoal);
        }

        [HttpPost]
        public ActionResult NewGoal(NewGoalViewModel newGoal)
        {
            string userId = HttpContext.User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                var priority = _priorityService.GetAllForUser(userId);
                newGoal.Priorities = new SelectList(priority, "PriorityId", "Name");
                return View("NewGoal", newGoal);
            }

            var goal = new Goals
            {
                Description = newGoal.Description,
                Name = newGoal.Name,
                PriorityId = newGoal.Priority.Value,
                UserId = userId,
                CreationDate = DateTime.Now
            };
            _goalsService.CreateGoal(goal);
            return RedirectToAction("List");
        }

        /// <summary>
        /// Редактирование задачи - GET
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditGoal(int? id)
        {
            string userId = HttpContext.User.Identity.GetUserId();

            if (id == null)
            {
                return RedirectToAction("List");
            }
            var editedGoal = _goalsService.GetById(id.Value);
            if (editedGoal == null || editedGoal.UserId != userId)
            {
                return RedirectToAction("List");
            }

            var editedGoalVm = new EditGoalViewModel
            {
                Name = editedGoal.Name,
                Description = editedGoal.Description,
                GoalId = editedGoal.GoalId,
                Priority = editedGoal.PriorityId
            };

            var priority = _priorityService.GetAllForUser(userId);
            editedGoalVm.Priorities = new SelectList(priority, "PriorityId", "Name");

            return View(editedGoalVm);
        }

        [HttpPost]
        public ActionResult EditGoal(EditGoalViewModel model)
        {
            string userId = HttpContext.User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {

                var priority = _priorityService.GetAllForUser(userId);
                model.Priorities = new SelectList(priority, "PriorityId", "Name");
                return View("EditGoal", model);
            }

            var editedGoal = _goalsService.GetById(model.GoalId);

            editedGoal.Name = model.Name;
            editedGoal.Description = model.Description;
            editedGoal.PriorityId = model.Priority;
            _goalsService.UpdateGoal(editedGoal, userId);

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult NewPriority()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewPriority(NewPriorityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("PriorityList");
            }
            
            string userId = HttpContext.User.Identity.GetUserId();

            var newPriority = new Priority();
            newPriority.Name = viewModel.Name;
            newPriority.UserId = userId;

            _priorityService.NewPriority(newPriority);

            return RedirectToAction("PriorityList");
        }

        [HttpGet]
        public ActionResult EditPriority(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PriorityList");
            }

            string userId = HttpContext.User.Identity.GetUserId();

            var editedPriority = _priorityService.GetById(id.Value);

            if (editedPriority.UserId.ToLower() != userId.ToLower())
            {
                return RedirectToAction("PriorityList");
            }

            var editedPriorityViewModel = new EditPriorityViewModel();

            editedPriorityViewModel.Name = editedPriority.Name;
            editedPriorityViewModel.PriorityId = editedPriority.PriorityId;

            return View(editedPriorityViewModel);
        }

        [HttpPost]
        public ActionResult EditPriority(EditPriorityViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("EditPriority", viewModel);                
            }

            string userId = HttpContext.User.Identity.GetUserId();

            var dbPriority = _priorityService.GetById(viewModel.PriorityId);

            dbPriority.Name = viewModel.Name;
            _priorityService.UpdatePriority(dbPriority,userId);

            return RedirectToAction("PriorityList");
        }


        public ActionResult PriorityList()
        {
            string userId = HttpContext.User.Identity.GetUserId();

            var userPriorities = _priorityService.GetAllForUser(userId);
            
            return View(userPriorities);
        }
    }
}