using AutoMapper;
using DTO;
using Entities;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Store.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        IOrderService _iorderService;
        IMapper _imapper;
        public OrdersController(IOrderService iorderService, IMapper imapper, ILogger<OrdersController> logger)
        {
            _iorderService = iorderService;
            _imapper = imapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderDTO>> GetById(int id)
        {
            Order order=await _iorderService.GetById(id);
            GetOrderDTO orderDTO=_imapper.Map<Order,GetOrderDTO>(order);
            if (orderDTO == null)
                return  NoContent();
            else return Ok(orderDTO);
        }
             

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<ActionResult<GetOrderDTO>> Post([FromBody] OrderDTO order)
        {   
            if(order.OrderItems.Count==0)
                return NoContent();
            
            Order orderF = _imapper.Map<OrderDTO, Order>(order);
        
            Order order1=await _iorderService.Post(orderF);
            if (order1 == null) {
                _logger.LogInformation($"");
                return Unauthorized();
            }
            GetOrderDTO getOrder = _imapper.Map<Order, GetOrderDTO>(order1);
            return Ok(getOrder);
        }

   
    }
}
