using LilsWorkApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LilsWorkApi.Controllers
{
    [ApiController]
    [EnableCors("allowpaired")]
    [Route("[controller]")]
    public class TaskPlanController : ControllerBase
    {
        private readonly TaskDbContext dbContext;

        public TaskPlanController(TaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<TaskPlan>> Get()
        {
            var taskPlans = await dbContext.TaskPlans.ToListAsync();
            return taskPlans;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<TaskPlan>>> Post(IEnumerable<TaskPlan> taskPlans)
        {
            dbContext.TaskPlans.AddRange(taskPlans);
            await dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), taskPlans);
        }

        [HttpPut]
        public async Task<ActionResult<TaskPlan>> Put(TaskPlan taskPlan)
        {
            dbContext.Entry(taskPlan).State = EntityState.Modified;

            try
            {
                var found = await dbContext.TaskPlans.FindAsync(taskPlan.Id);
                if (found == null)
                    return NotFound();

                found.Title = taskPlan.Title;
                found.Cycle = taskPlan.Cycle;

                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (dbContext.TaskPlans.All(p => p.Id != taskPlan.Id))
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
                var found = await dbContext.TaskPlans.FindAsync(id);
                if (found == null)
                    return NotFound();

                dbContext.Remove(found);

                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (dbContext.TaskPlans.All(p => p.Id != id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}