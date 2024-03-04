using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Data;
using Todo.Model;

namespace Todo.Controller
{
	[ApiController]
	public class HomeController : ControllerBase
	{
		[HttpGet("/todos")]
		public IActionResult Get([FromServices] AppDbContext context)
		{
			return Ok(context.Todos.ToList());
		}
		
		[HttpGet("/todos/{id:int}")]
		public IActionResult GetById(
			[FromRoute] int id,
			[FromServices] AppDbContext context)
		{
			var todo = context.Todos.FirstOrDefault(x => x.Id == id);
			
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