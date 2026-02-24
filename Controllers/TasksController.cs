using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task.API.Data;
using Task.API.Models;

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

        // Get all Tasks
        [HttpGet]
        public async Task<ActionResult<IList<TaskItem>>> GetAllTasks()
        {
            return await _context.Tasks.OrderByDescending(t => t.CreatedAt).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> AddTask(TaskItem newTask)
        {
            _context.Add(newTask);

            await _context.SaveChangesAsync();

            return Ok(newTask);
        }
    }
}
