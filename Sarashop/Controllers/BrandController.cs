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
        public async Task<IActionResult> GetAll()
        {

            var baseUrl = $"{Request.Scheme}://{Request.Host}/imgs/Brand/";
            var brands = await _brandService.GetAsync();
            var mapBrand = brands.ToList().Select(b =>
            {
                var dto = b.Adapt<BrandDto>();
                dto.mainImg = baseUrl + b.mainImg;
                return dto;
            });

            return Ok(mapBrand);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brand = await _brandService.GetOne(b => b.Id == id);
            if (brand == null)
                return NotFound();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/imgs/Brand/";
            var brandmap = brand.Adapt<BrandDto>();
            brandmap.mainImg = baseUrl + brand.mainImg;
            return Ok(brandmap);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] BrandREQ brandDto, CancellationToken cancellationToken)
        {

            if (brandDto == null)
                return BadRequest();

            var brand = brandDto.Adapt<Brand>();
            var file = brandDto.mainImg;
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgs/Brand", fileName);

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                // Save the file name to the database
                brand.mainImg = fileName;
            }

            var created = await _brandService.AddAsync(brand, cancellationToken);

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
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _brandService.RemoveAsync(id);
            return result ? Ok() : NotFound();
        }
    }
}
