using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.API.Data;
using Task.API.Models;
using Task.API.Dtos;
using Task.API.Enums;

namespace Task.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> GetAllTasks()
        {
            return await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        // GET: api/tasks/1
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            return task;
        }

        // UPDATE STATUS: api/tasks/1/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> EditTask(int id, [FromBody] TaskDtos.UpdateTaskStatusDto dto)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            task.Status = dto.Status;
            task.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(task);
        }

        // POST: api/tasks
        [HttpPost]
        public async Task<ActionResult<TaskItem>> AddTask(TaskItem newTask)
        {
            newTask.CreatedAt = DateTime.UtcNow;

            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = newTask.Id }, newTask);
        }

        // DELETE: api/tasks/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}