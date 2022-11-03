using API_Dapper.Models;
using API_Dapper.Rules;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Dapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public List<Product> GetAllProducts()
        {
            var rule = new ProductRule();
            return rule.GetAllProducts();
        }
        [HttpGet("/api/test/{id}")]
        public Product GetProduct(int id)
        {
            var rule = new ProductRule();
            return rule.GetProductById(id);
        }
        [HttpGet("/api/orders/")]
        public List<Order> GetOrders()
        {
            var rule = new OrderRule();
            return rule.GetAllOrders();
        }
        [HttpDelete("/api/orders/")]
        public RespuestaDelete DeleteOrderById(int orderId)
        {
            var rule = new OrderRule();
            return rule.DeleteOrderById(orderId);
        }

    }
}
