﻿using Microsoft.AspNetCore.Mvc;
using Sarashop.DTO;
using Sarashop.Models;
using Sarashop.service;

namespace Sarashop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdactController : ControllerBase
    {
        private readonly IProdact _prodactService;

        public ProdactController(IProdact prodactService)
        {
            _prodactService = prodactService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? query, [FromQuery] int page, [FromQuery] int limit = 10)
        {

            var baseUrl = $"{Request.Scheme}://{Request.Host}/imgs/Prodact/";
            var products = _prodactService.GetAll(query, page, limit)
               .Select(p => new ProdactDTO
               {
                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   Type = p.Type,
                   Price = p.Price,
                   Discount = p.Discount,
                   Quntity = p.Quntity,
                   State = p.State,
                   Rate = p.Rate,
                   CategoryId = p.CategoryId,
                   BrandID = p.BrandID,
                   mainImg = baseUrl + p.mainImg,
               });
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _prodactService.GetProdact(p => p.Id == id);
            if (product == null)
                return NotFound();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/imgs/Prodact/";
            var res = new ProdactDTO
            {

                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Type = product.Type,
                Price = product.Price,
                Discount = product.Discount,
                Quntity = product.Quntity,
                State = product.State,
                Rate = product.Rate,
                CategoryId = product.CategoryId,
                BrandID = product.BrandID,
                mainImg = baseUrl + product.mainImg,
            };

            return Ok(res);
        }

        [HttpPost]
        public IActionResult Create([FromForm] ProdactRES dto)
        {
            var file = dto.mainImg;
            string img = "";
            if (file != null && file.Length > 0)
            {
                var fileNmae = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgs/Prodact/", fileNmae);
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyToAsync(stream);
                }
                img = fileNmae;
            }
            if (dto == null)
                return BadRequest();

            var newProduct = new Prodact
            {
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type,
                Price = dto.Price,
                Discount = dto.Discount,
                Quntity = dto.Quntity,
                State = dto.State,
                Rate = dto.Rate,
                CategoryId = dto.CategoryId,
                BrandID = dto.BrandID,
                mainImg = img,

            };

            var created = _prodactService.Add(newProduct);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new ProdactDTO
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description,
                Type = created.Type,
                Price = created.Price,
                Discount = created.Discount,
                Quntity = created.Quntity,
                State = created.State,
                Rate = created.Rate,
                CategoryId = created.CategoryId,
                BrandID = created.BrandID,
                mainImg = created.mainImg
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromForm] ProdactRES dto)
        {
            if (dto == null)
                return BadRequest();

            var updated = new Prodact
            {
                Name = dto.Name,
                Description = dto.Description,
                Type = dto.Type,
                Price = dto.Price,
                Discount = dto.Discount,
                Quntity = dto.Quntity,
                State = dto.State,
                Rate = dto.Rate,
                CategoryId = dto.CategoryId,
                BrandID = dto.BrandID
            };

            var result = _prodactService.Update(id, updated);
            return result != null ? Ok(dto) : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _prodactService.Delete(id);
            return result ? Ok() : NotFound();
        }
    }
}
