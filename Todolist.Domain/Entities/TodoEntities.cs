namespace Todolist.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TodoEntities : DbContext
    {
        public TodoEntities()
            : base("name=TodoEntities")
        {
        }

        public virtual DbSet<Goals> Goals { get; set; }
        public virtual DbSet<Priority> Priority { get; set; }

        public void Commit()
        {
            this.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Priority>()
                .HasMany(e => e.Goals)
                .WithRequired(e => e.Priority)
                .WillCascadeOnDelete(false);
        }
    }
}
