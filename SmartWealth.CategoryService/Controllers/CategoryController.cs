using Microsoft.AspNetCore.Mvc;
using SmartWealth.CategoryService.Services;
using SmartWealth.CategoryService.ViewModels;
using SmartWealth.CategoryService.Utilities.Exeptions;

namespace SmartWealth.CategoryService.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController(ICategoryService service) : Controller
{
    private readonly ICategoryService _service = service;

    [HttpGet("all")]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            return Ok(await _service.GetCategoriesAsync());
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        try
        {
            return Ok(await _service.GetCategoryAsync(id));
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryViewModel createdCategory)
    {
        try
        {
            await _service.CreateCategoryAsync(createdCategory);
            return Created();
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpPut("edit/{id:guid}")]
    public async Task<IActionResult> EditCategory(Guid id, [FromBody] CategoryViewModel editedCategory)
    {
        try
        {
            await _service.EditCategoryAsync(id, editedCategory);
            return Created();
        }
        catch (NotValidException notValidException)
        {
            return BadRequest(notValidException.Message);
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpDelete("delete/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            await _service.DeleteCategoryAsync(id);
            return NoContent();
        }
        catch (NotFoundException notFoundException)
        {
            return NotFound(notFoundException.Message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}