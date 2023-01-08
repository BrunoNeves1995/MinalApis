using System.Reflection;
using Blog.Externsions;
using Blog.ViewModels;
using Blog.ViewModels.Result;
using introducao.Data;
using introducao.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] DataContext context
        )
        {
            try
            {
                var categories = await context.Categories!.ToListAsync();

                if (categories.Count is 0 || categories is null)
                    return NotFound("Não existe categorias cadastradas");

                return Ok(new ResultViewModel<List<Category>>(data: categories));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>(erro: "01XE1 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] DataContext context,
            [FromRoute] int id
        )
        {
            try
            {
                var categorie = await context.Categories!.FirstOrDefaultAsync(x => x.Id == id);

                if (categorie is null)
                    return NotFound(new ResultViewModel<Category>(erro: "Categoria não encontrada"));

                return Ok(new ResultViewModel<Category>(data: categorie));
            }
            catch (Exception)
            {

                return StatusCode(500, new ResultViewModel<Category>(erro: "01XE2 - Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromServices] DataContext context,
            [FromBody] EditorCategoryViewModel model
        )
        {   
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(errors: ModelState.GetErrors()));

            try
            {   
                var category = new Category{
                    Id = 0,
                    Name = model.Name.Replace("-", ""),
                    Slug = model.Slug.ToLower(),
                };

                await context.Categories!.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}",new ResultViewModel<Category>(data: category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>(erro: "01XE3 - Não foi possivel incluir uma nova categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>(erro:"01XE4 - Falha interna no servidor"));
            }
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] DataContext context,
            [FromBody] EditorCategoryViewModel model,
            [FromRoute] int id
        )
        {
            try
            {
                var categorie = await context.Categories!.FirstOrDefaultAsync(x => x.Id == id);

                if (categorie is null)
                    return NotFound(new ResultViewModel<Category>(erro: $"Não foi possivel atualizar a categoria, que tem o id igual a ({id})"));

                categorie.Name = model.Name;
                categorie.Slug = model.Slug;

                context.Categories!.Update(categorie);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{categorie.Id}", new ResultViewModel<Category>(data: categorie));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>(erro: "01XE5 - Não foi possivel aleterar a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, "01XE6 - Falha interna no servidor");
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] DataContext context,
            [FromRoute] int id
        )
        {
            try
            {
                var category = await context.Categories!.FirstOrDefaultAsync(x => x.Id == id);

                if (category is null)
                    return NotFound(new ResultViewModel<Category>(erro: "Categoria não encontrada"));

                context.Categories!.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(data: category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>(erro: "01XE7 - Não foi possivel excluir a categoria"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ResultViewModel<Category>(erro: "01XE8 - Falha interna no servidor"));
            }
        }
    }
}
