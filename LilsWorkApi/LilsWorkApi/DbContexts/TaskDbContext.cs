using LilsWorkApi.Models;
using Microsoft.EntityFrameworkCore;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
    public DbSet<TaskPlan> TaskPlans { get; set; }
    public DbSet<LilsWorkApi.Models.Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<TaskPlan>()
            .Property(e => e.Cycle)
            .HasConversion<string>();
    }
}