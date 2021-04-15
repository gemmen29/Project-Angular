using BL.AppServices;
using BL.StaticClasses;
using BL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoles.Admin)]
    public class CategoryController : ControllerBase
    {
        CategoryAppService _categoryAppService;

        public CategoryController(CategoryAppService categoryAppService)
        {
            this._categoryAppService = categoryAppService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_categoryAppService.GetAllCateogries());
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel categoryViewModel)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _categoryAppService.SaveNewCategory(categoryViewModel);

                //string urlDetails = Url.Link("DefaultApi", new { id = categoryViewModel.ID });
                //return Created(urlDetails, "Added Sucess");
                return Created("CreateCategory" , categoryViewModel.Name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpPut]
        public IActionResult Edit(int id, CategoryViewModel categoryViewModel)
        {

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            try
            {
                _categoryAppService.UpdateCategory(categoryViewModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int catID)
        {
            try
            {
                _categoryAppService.DeleteCategory(catID);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
