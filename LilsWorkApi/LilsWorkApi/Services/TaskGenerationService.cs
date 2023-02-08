namespace LilsWorkApi.Services
{
    /// <summary>
    /// �������ɷ���
    /// ����ο� https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-7.0&tabs=visual-studio#backgroundservice-base-class
    /// </summary>
    public class TaskGenerationService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory scopeFactory;
        private Timer? timer;

        public TaskGenerationService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(CheckGeneration, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private async void CheckGeneration(object? state)
        {
            using var scope = scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

            // �������Ϊ�յ�����
            var today = DateTime.Today;
            var dueToday = today.AddDays(1).AddSeconds(-1);
            var dailyTaskPlansToAdd = dbContext.TaskPlans
                .Where(tp => tp.Cycle == Models.PlanCycle.Daily)
                .Where(tp => !dbContext.Tasks.Any(t => t.DueTo > today && t.Title == tp.Title));

            dbContext.Tasks.AddRange(dailyTaskPlansToAdd.Select(tp => new Models.Task
            {
                Title = tp.Title,
                State = TaskState.Todo,
                CreatedAt = DateTime.Now,
                DueTo = dueToday,
            }));

            // ��ǹ��ڵ�����
            var nextMinute = DateTime.Now.AddMinutes(1);
            var incompleteTasks = dbContext.Tasks.Where(t => t.State == TaskState.Todo && t.DueTo != null && t.DueTo < nextMinute);
            foreach (var task in incompleteTasks)
            {
                task.State = TaskState.Expired;
            }

            await dbContext.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}