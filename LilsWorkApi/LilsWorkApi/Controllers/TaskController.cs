using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LilsWorkApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskDbContext dbContext;
        public TaskController(TaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<Models.Task>> Get()
        {
            var tasks = await dbContext.Tasks.ToListAsync();
            return tasks;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Models.Task>>> Post(IEnumerable<Models.Task> tasks)
        {
            dbContext.Tasks.AddRange(tasks);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), tasks);
        }
    }
}