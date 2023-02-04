using Microsoft.EntityFrameworkCore;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }
    public DbSet<LilsWorkApi.Models.Task> Tasks { get; set; }
}