using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TodoWebApi.Models;
using TodoWebApp.Data;
using TodoWebApp.Model;

namespace TodoWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : Controller
    {
        private ApplicationDbContext dbContext;
        public ToDosController(ApplicationDbContext dbTodo)
        {
            dbContext = dbTodo;
        }
        // /api/todo/getall
        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] Paginator page)
        {
            Paginator paginator = new Paginator(page.PerPage, page.CurrentPage);
            return Ok(await dbContext.ToDos
                .Skip((page.CurrentPage - 1) * page.PerPage)
                .Take(page.PerPage)
                .ToListAsync());
        }

        [HttpGet]
        [Route("Id")]
        public async Task<IActionResult> GetSingleTodo([FromQuery] int Id)
        {
            var todo = await dbContext.ToDos.FirstOrDefaultAsync(x => x.ToDoId == Id);
            if (todo != null)
            {
                return Ok(todo);
            }
            return BadRequest("Not Found");
        }
        // /api/todo/add
        [HttpPost]
        public async Task<IActionResult> AddTodo(AddTodoModel addTodoModel)
        {
            var todo = new ToDo()
            {
                Description = addTodoModel.Description,
                Date = addTodoModel.Date,
            };
            await dbContext.ToDos.AddAsync(todo);
            await dbContext.SaveChangesAsync();

            return Ok(todo);
        }
        // /api/todo/update
        [HttpPut]
        [Route("Id")]
        public async Task<IActionResult> UpdateTodo([FromQuery] int Id, AddTodoModel updateTodoRequest)
        {
            var todo = await dbContext.ToDos.FirstOrDefaultAsync(x => x.ToDoId == Id);

            if (todo != null)
            {
                todo.Description = updateTodoRequest.Description;
                todo.Date = updateTodoRequest.Date;
                todo.IsCompleted = updateTodoRequest.IsCompleted;

                await dbContext.SaveChangesAsync();
                return Ok(todo);
            }

            return NotFound();
        }
        // /api/todo/delete
        [HttpDelete]
        [Route("Id")]
        public async Task<IActionResult> DeleteTodo([FromQuery] int Id)
        {
            var todo = await dbContext.ToDos.FirstOrDefaultAsync(x => x.ToDoId == Id);

            if (todo != null)
            {
                dbContext.Remove(todo);
                await dbContext.SaveChangesAsync();
                return Ok(todo);
            }

            return NotFound();
        }
    }
}
