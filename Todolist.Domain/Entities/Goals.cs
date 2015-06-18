namespace Todolist.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Goals
    {
        [Key]
        public int GoalId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public int PriorityId { get; set; }

        public bool Completed { get; set; }

        public bool Deleted { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public virtual Priority Priority { get; set; }
    }
}
