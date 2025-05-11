using Mapster;
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

        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAsync();
            return Ok(categories.Adapt<IEnumerable<Category>>());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetOne(c => c.Id == id);
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
        public async Task<IActionResult> CreateAsync([FromForm] CatigoryDTO categoryDto, CancellationToken cancellationToken)
        {

            if (categoryDto == null)
                return BadRequest();

            var catigory = categoryDto.Adapt<Category>();

            var file = categoryDto.mainImg;
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgs", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }
                catigory.mainImg = fileName;
            }

            var created = await _categoryService.AddAsync(catigory, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] CatigoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest();

            var updatedCategory = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                State = categoryDto.State
            };

            var result = await _categoryService.Update(id, updatedCategory);
            return result ? Ok(categoryDto) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _categoryService.RemoveAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}
