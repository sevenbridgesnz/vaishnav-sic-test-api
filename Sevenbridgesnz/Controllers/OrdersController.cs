using Microsoft.AspNetCore.Mvc;
using Sevenbridgesnz.Models;

namespace Sevenbridgesnz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private static List<Order> _orders = new List<Order>();
        private static int _nextId = 1;

        [HttpPost]
        public ActionResult<Order> PostOrder(Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            order.Id = _nextId++;
            _orders.Add(order);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return _orders.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            _orders.Remove(order);

            return NoContent();
        }
    }
}
