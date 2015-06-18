using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PagedList;

namespace Todolist.Web.ViewModels.Goals
{
    public class UserGoalsViewModel
    {
        public string StartDateStr { get; set; }

        public string EndDateStr { get; set; }

        public string SearchStr { get; set; }

        
        public virtual IEnumerable<SelectListItem> Priorities { get; set; }

        [Display(Name = "Важность")]
        public int? Priority { get; set; }

        public PagedList<Domain.Entities.Goals> PagedGoals { get; set; }
    }

    public class NewGoalViewModel
    {
        [Required]
        [Display(Name = "Название")]
        [StringLength(128, ErrorMessage = "Название не должно быть пустым", MinimumLength = 1)]
        public string Name { get; set; }
        
        [Required]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(512, ErrorMessage = "Описание не должно быть пустым", MinimumLength = 1)]
        public string Description { get; set; }

        public virtual IEnumerable<SelectListItem> Priorities { get; set; }

        [Required]
        [Display(Name = "Важность")]
        public int? Priority { get; set; }
    }

    public class EditGoalViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int GoalId { get; set; }

        [Required]
        [Display(Name = "Название")]
        [StringLength(128, ErrorMessage = "Название не должно быть пустым", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(512, ErrorMessage = "Описание не должно быть пустым", MinimumLength = 1)]
        public string Description { get; set; }

        public virtual IEnumerable<SelectListItem> Priorities { get; set; }

        [Required]
        [Display(Name = "Важность")]
        public int Priority { get; set; }
    }
}