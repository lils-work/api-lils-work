using LilsWorkApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TaskPlanEntityConfig : IEntityTypeConfiguration<TaskPlan>
{
    public void Configure(EntityTypeBuilder<TaskPlan> builder)
    {
        builder.ToTable(nameof(TaskPlan).ToLower());
    }
}