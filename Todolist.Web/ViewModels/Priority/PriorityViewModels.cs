using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Todolist.Web.ViewModels.Priority
{
    public class NewPriorityViewModel
    {
        [Required]
        [Display(Name = "Название")]
        [StringLength(128, ErrorMessage = "Название не должно быть пустым", MinimumLength = 1)]
        public string Name { get; set; }
    }

    public class EditPriorityViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int PriorityId { get; set; }

        [Required]
        [Display(Name = "Название")]
        [StringLength(128, ErrorMessage = "Название не должно быть пустым", MinimumLength = 1)]
        public string Name { get; set; }
    }
}