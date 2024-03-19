using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Model;

namespace Todo.Controller
{
	[ApiController]
	public class HomeController : ControllerBase
	{
        [EnableQuery(PageSize = 3)]
        [HttpGet("/todos")]
        public IQueryable<TodoModel> Get([FromServices] AppDbContext context)
		{
			return context.Todos.ToList().AsQueryable();
		}

        [EnableQuery]
        [HttpGet("{id}")]
        public IActionResult GetById(
			[FromRoute] int id,
			[FromServices] AppDbContext context)
        {
			var todo = context.Todos.Where(x => x.Id == id).AsQueryable();

            if (todo == null) return NotFound();

            return Ok(todo);
        }

        [HttpPost("/todos")]
		public IActionResult Post(
			[FromBody] TodoModel todo,
			[FromServices] AppDbContext context)
		{
			context.Todos.Add(todo);
			context.SaveChanges();
			return Created($"/todo/{todo.Id}", todo);
		}
		
		[HttpPut("/todos/{id:int}")]
		public IActionResult Put(
			[FromRoute] int id,
			[FromBody] TodoModel todo,
			[FromServices] AppDbContext context)
		{
			var model = context.Todos.FirstOrDefault(x => x.Id == id);
			
			if (model == null) return NotFound();
			
			model.Title = todo.Title;
			model.Done = todo.Done;
			
			context.Todos.Update(model);
			context.SaveChanges();
			
			return Ok(model);
		}
		
		[HttpDelete("/todos/{id:int}")]
		public IActionResult Delete(
			[FromRoute] int id,
			[FromServices] AppDbContext context)
		{
			var model = context.Todos.FirstOrDefault(x => x.Id == id);
			
			if (model == null) return NotFound();
			context.Todos.Remove(model);
			context.SaveChanges();
			
			return Ok(model);
		}
	}
}