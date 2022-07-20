using Microsoft.EntityFrameworkCore;
using TaskAPI.Interfaces;

namespace TaskAPI.Context
{
    public class TaskContext : DbContext
    {
        public TaskContext(DbContextOptions options) : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            //optionsBuilder.UseSqlServer(@"Server=.;Database=Task;MultipleActiveResultSets=True;Trusted_Connection=True;");
        }
        public DbSet<Models.TaskModel> Tasks { get; set; }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changeSet = ChangeTracker.Entries<ITask>();

            if (changeSet == null) return await base.SaveChangesAsync();

            foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
            {
                entry.Entity.DueDate = entry.Entity.DueDate.Date;
                entry.Entity.EndDate = entry.Entity.DueDate.Date;
                entry.Entity.StartDate = entry.Entity.DueDate.Date;

            }

            return await base.SaveChangesAsync();
        }
    }
}
