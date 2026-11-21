
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using System.Reflection;
using System.Text.Json;
using HubServer.Services;

namespace HubServer.Controllers
{
    [ApiController]

    //[Route("confidentvotesapi/[controller]")]
    [Route("smartinventoryhubapi/products")]
    // the class must be public
    public class ProductsController : ControllerBase
    {   
     
        public required IProductsHandler  _MyProductsHandler ;

        // when the controller is create the only one instance of 
        // IProductsHandler will be used or reused
        public ProductsController(IProductsHandler ProductsHandler)
        {
            _MyProductsHandler = ProductsHandler;
        }
        /* ############### GET ############*/
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDto>>> GetAll()
        {
            var products = await _MyProductsHandler.GetProducts();
            return Ok(products);
        }
         [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ProductResponseDto>> GetbyId(int id)
        {
            var product = await _MyProductsHandler.GetProductById(id);
            
            return product != null ? Ok(product) : NotFound($"{id} is not related to a product, enter valid id");
            
        }
        [HttpPost] 
         /* ############### POST ############*/
        public async Task<ActionResult<ProductResponseDto>> Create( [FromBody] ProductRequestDto newProduct)
        {
            if (!ModelState.IsValid)
            {   
                return ValidationProblem(ModelState);
            }
            var isCreated = await _MyProductsHandler.addProduct(newProduct);
            return isCreated? CreatedAtAction(
                            actionName: nameof(GetbyId), 
                            routeValues: new {id = newProduct.ProductID}, 
                            value: newProduct
                        ) : BadRequest("Check your product entries like categoryID if they are valid");
        }
        /* ############### PUT ############*/
        [HttpPut("{id:int:min(1)}")] 
        public async Task<ActionResult<ProductResponseDto>> Update(int id, ProductRequestDto updatedProduct)
        {
           if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
           var isUpdated = await _MyProductsHandler.UpdateProduct(id, updatedProduct);
            return isUpdated ? AcceptedAtAction(actionName:nameof(GetbyId), 
                       routeValues: new {id = updatedProduct.ProductID}, 
                       value: updatedProduct): BadRequest("Check your product entries like category id if they are valid and the product id");;
        }

        /* ############### DELETE ############*/
        [HttpDelete("{id:int:min(1)}")] 
        public async Task<ActionResult<ProductResponseDto>> Delete(int id)
        {
            var isRemoved = await _MyProductsHandler.RemoveProduct(id);
            return isRemoved ? NoContent() : BadRequest("check your id if it is valid");
        }
    }
}