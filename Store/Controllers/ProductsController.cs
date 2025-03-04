using AutoMapper;
using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _iproductService;
        IMapper _imapper;
      
        public ProductsController(IProductService iproductService,  IMapper imapper)
        {
            _iproductService = iproductService;
            _imapper = imapper;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<ActionResult <IEnumerable<ProductDTO>>> Get([FromQuery]int position, [FromQuery] int skip, [FromQuery] string? desc, [FromQuery] int? minPrice, [FromQuery] int? maxPrice, [FromQuery] int?[] categoryIds)
        {
            IEnumerable<Product>products= await _iproductService.Get( position,  skip, desc, minPrice,  maxPrice, categoryIds);
            IEnumerable<ProductDTO>productsDTO=_imapper.Map<IEnumerable<Product>,IEnumerable<ProductDTO>>(products);
            if (productsDTO == null) 
                return NoContent();
            else
                return Ok(productsDTO);         
        }

    }
}
