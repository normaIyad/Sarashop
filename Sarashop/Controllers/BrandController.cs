using Mapster;
using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;

namespace Sarashop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly IBrand _brandService;

        public BrandController(IBrand brandService)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var brands = _brandService.GetBrands(); // Rename method
            return Ok(brands.Select(b => b.Adapt<BrandREQ>())); // or map manually
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var brand = _brandService.GetBrand(b => b.Id == id);
            if (brand == null)
                return NotFound();

            return Ok(brand.Adapt<BrandREQ>());
        }

        [HttpPost]
        public IActionResult Create([FromBody] BrandDto brandDto)
        {
            var file = brandDto.mainImg;
            if (file == null && file.Length > 0)
            {
                var fileNmae = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgs", fileNmae);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
            }
            if (brandDto == null)
                return BadRequest();

            var brand = brandDto.Adapt<Brand>();
            var created = _brandService.Add(brand);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] BrandDto brandDto)
        {
            if (brandDto == null)
                return BadRequest();

            var brand = brandDto.Adapt<Brand>();
            var result = _brandService.Update(id, brand);

            return result ? Ok(brandDto) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _brandService.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}
