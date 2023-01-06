using Microsoft.AspNetCore.Mvc;
using Tarefas.Data;
using Tarefas.models;

namespace Tarefas.Controllers
{
    // CONTROLE DE API, IREMOS RETORNAR APENAS JSON

    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public IActionResult Get([FromServices] DataContext context)
            => Ok(context.Tarefas?.ToList());
        

        [HttpGet("/{id:int}")]
        public IActionResult GetById(
            [FromServices] DataContext context,
            [FromRoute] int id
        )
        {
            var tarefa =  context.Tarefas?.FirstOrDefault(x => x.Id == id);

            if(tarefa is not null)
                return Ok(tarefa);
            else
                return NotFound("Tarefa não encontrada");
        }

        [HttpPost("/")]
        public IActionResult Post(
            [FromServices] DataContext context,
            [FromBody] TarefaModel tarefa
        )
        {
            if (tarefa is not null)
            {
                context.Tarefas?.Add(tarefa);
                context.SaveChanges();
                 return Created($"/{tarefa.Id}", tarefa);
            }
            else
                return NotFound();

           
        }


        [HttpPut("/{id:int}")]
        public IActionResult Update(
            [FromServices] DataContext context,
            [FromBody] TarefaModel tarefa,
            [FromRoute] int id
        )
        {
            var  model = context.Tarefas?.FirstOrDefault(x => x.Id == id);

            if(model is not null)
            {   
                model.Title = tarefa.Title;
                model.Done = tarefa.Done;

                context.Tarefas?.Update(model);
                context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        [HttpDelete("/{id:int}")]
        public IActionResult Delete(
            [FromServices] DataContext context,
            [FromRoute] int id
        )
        {
            var  model = context.Tarefas?.FirstOrDefault(x => x.Id == id);

            if(model is not null)
            {   
                context.Tarefas?.Remove(model);
                context.SaveChanges();
                return Ok();
            }

            return NotFound();
        }
        
    }
}