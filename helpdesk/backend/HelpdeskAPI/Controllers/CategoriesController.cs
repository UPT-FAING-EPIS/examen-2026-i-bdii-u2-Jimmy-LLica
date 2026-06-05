using Microsoft.AspNetCore.Mvc;
using HelpdeskAPI.Entities;
using HelpdeskAPI.Repositories;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IMongoRepository<Category> _categoryRepo;
    public CategoriesController(IMongoRepository<Category> categoryRepo) => _categoryRepo = categoryRepo;

    [HttpGet]
    public async Task<IActionResult> GetCategories() => Ok(await _categoryRepo.GetAllAsync());

    [HttpPost] // solo admin
    public async Task<IActionResult> CreateCategory([FromBody] Category category)
    {
        category.CreatedAt = DateTime.UtcNow;
        var created = await _categoryRepo.InsertAsync(category);
        return CreatedAtAction(nameof(GetCategories), new { id = created.Id }, created);
    }
}