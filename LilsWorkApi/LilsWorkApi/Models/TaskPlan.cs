using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace LilsWorkApi.Models
{
    public class TaskPlan
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        [Column(TypeName = "nvarchar(12)")]
        public PlanCycle Cycle { get; set; }  
    }
}