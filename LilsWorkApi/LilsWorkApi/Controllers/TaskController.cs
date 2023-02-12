using LilsWorkApi.Helpers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LilsWorkApi.Controllers
{
    [ApiController]
    [EnableCors("allowpaired")]
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
            var dayafter31 = DateTimeOffset.Now.AddDays(31);
            // 除了检查到期日是否超过，还判断到期日是否在 31 日内
            var tasks = await dbContext.Tasks
                .Where(t => t.DueTo >= DateTimeOffset.Now && t.DueTo < dayafter31)
                .ToListAsync();
            tasks.ForEach(t => t.DueTo = t.DueTo.ToZone(TimeZoneHelper.CurrentTimeZone));
            return tasks;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Models.Task>>> Post(IEnumerable<Models.Task> tasks)
        {
            dbContext.Tasks.AddRange(tasks);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), tasks);
        }

        [HttpPut]
        public async Task<ActionResult<Models.Task>> Put(Models.Task task)
        {
            dbContext.Entry(task).State = EntityState.Modified;

            try
            {
                var found = await dbContext.Tasks.FindAsync(task.Id);
                if (found == null)
                    return NotFound();

                found.Title = task.Title;
                found.IsCompleted = task.IsCompleted;
                found.IsCancel= task.IsCancel;

                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (dbContext.Tasks.All(t => t.Id != task.Id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var found = await dbContext.Tasks.FindAsync(id);
                if (found == null)
                    return NotFound();

                dbContext.Remove(found);
                
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (dbContext.Tasks.All(t => t.Id != id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}