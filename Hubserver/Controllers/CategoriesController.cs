
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System.Reflection;
using System.Text.Json;
using HubServer.Services;

namespace ConfidentVotesAPI.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("smartinventoryhubapi/categories")]
    // the class must be public
    public class CategoriesController : ControllerBase
    {   
     
        public required ICategoriesHandler  _MyCategoriesHandler ;

        // when the controller is create the only one instance of 
        // ICategoriesHandler will be used or reused
        public CategoriesController(ICategoriesHandler CategoriesHandler)
        {
            _MyCategoriesHandler = CategoriesHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
        {
            var Categories = await _MyCategoriesHandler.GetCategories();
            return Ok(Categories);
        }
         [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<CategoryResponseDto>> GetbyId(int id)
        {
            var category = await _MyCategoriesHandler.GetCategoryById(id);
            return Ok(category);
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<CategoryResponseDto>> Create( [FromBody] CategoryRequestDto newCategory)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var isAdded = await _MyCategoriesHandler.addCategory(newCategory);
            return isAdded ? CreatedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = newCategory.CategoryID}, 
                       value:newCategory): BadRequest("Check your category entries if they are valid");
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(1)}")] 
        public async Task<ActionResult<CategoryResponseDto>> Update(int id, CategoryRequestDto updatedCategory)
        {
           if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var isUpdated = await _MyCategoriesHandler.UpdateCategory(id, updatedCategory);
            return isUpdated ? AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedCategory.CategoryID}, 
                       value:updatedCategory) : BadRequest("Check your category entries like id if they are valid");
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<CategoryResponseDto>> Delete(int id)
        {
            var isRemoved = await _MyCategoriesHandler.RemoveCategory(id);
            return  isRemoved ? NoContent():BadRequest("Check your category entries like id if they are valid");
        }
    }
}