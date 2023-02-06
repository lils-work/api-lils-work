namespace LilsWorkApi.Models
{
    public class TaskPlan
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public PlanCycle Cycle { get; set; }  
    }
}