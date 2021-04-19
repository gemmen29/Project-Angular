using BL.AppServices;
using BL.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        ProductAppService _productAppService;

        public ProductController(ProductAppService productAppService)
        {
            this._productAppService = productAppService;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(_productAppService.GetAllProduct());
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            return Ok(_productAppService.GetPoduct(id));
        }

        [HttpPost]
        public IActionResult Create(ProductViewModel productViewModel)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _productAppService.SaveNewProduct(productViewModel);
                
                return Created("CreateProduct", productViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut("{id}")]
        public IActionResult Edit(int id, ProductViewModel productViewModel)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _productAppService.UpdateProduct(productViewModel);
                return Ok(productViewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _productAppService.DeleteProduct(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("count")]
        public IActionResult CategoriesCount()
        {
            return Ok(_productAppService.CountEntity());
        }
        [HttpGet("{pageSize}/{pageNumber}")]
        public IActionResult GetCategoriesByPage(int pageSize, int pageNumber)
        {
            return Ok(_productAppService.GetPageRecords(pageSize, pageNumber));
        }
    }
}
