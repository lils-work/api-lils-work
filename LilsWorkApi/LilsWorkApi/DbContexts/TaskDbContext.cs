using LilsWorkApi.Models;
using Microsoft.EntityFrameworkCore;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
    public DbSet<TaskPlan> TaskPlans { get; set; }
    public DbSet<LilsWorkApi.Models.Task> Tasks { get; set; }
}