using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICatigoryService _categoryService;

        public CategoryController(ICatigoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryService.GetCategories()
                .Select(cat => new categoryRES
                {
                    ID = cat.Id,
                    Name = cat.Name,
                    Description = cat.Description,
                    State = cat.State
                });

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategory(c => c.Id == id);
            if (category == null)
                return NotFound();

            var response = new categoryRES
            {
                ID = category.Id,
                Name = category.Name,
                Description = category.Description,
                State = category.State
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CatigoryDTO categoryDto)
        {
            var file = categoryDto.mainImg;
            if (file == null && file.Length > 0)
            {
                var fileNmae = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgs", fileNmae);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
            }
            if (categoryDto == null)
                return BadRequest();

            var newCategory = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                State = categoryDto.State
            };

            var created = _categoryService.Add(newCategory);

            var response = new categoryRES
            {
                ID = created.Id,
                Name = created.Name,
                Description = created.Description,
                State = created.State
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CatigoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest();

            var updatedCategory = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                State = categoryDto.State
            };

            var result = _categoryService.Update(id, updatedCategory);
            return result ? Ok(categoryDto) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var result = _categoryService.Delete(id);
            return result ? NoContent() : NotFound();
        }
    }
}
