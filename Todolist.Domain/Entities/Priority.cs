namespace Todolist.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Priority")]
    public partial class Priority
    {
        public Priority()
        {
            Goals = new HashSet<Goals>();
        }

        [Key]
        public int PriorityId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public virtual ICollection<Goals> Goals { get; set; }
    }
}
