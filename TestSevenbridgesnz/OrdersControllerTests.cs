using Microsoft.AspNetCore.Mvc;
using Sevenbridgesnz.Controllers;
using Sevenbridgesnz.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Sevenbridgesnz.SevenbridgesnzTests
{
    public class OrdersControllerTests
    {
        private OrdersController _controller;

        public OrdersControllerTests()
        {
            _controller = new OrdersController();
        }

        [Fact]
        public void PostOrder_ValidOrder_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var order = new Order
            {
                FirstName = "John",
                LastName = "Doe",
                Description = "Test Order Description",
                Quantity = 5
            };

            // Act
            var result = _controller.PostOrder(order);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Order>(createdAtActionResult.Value);
            Assert.Equal(order.FirstName, returnValue.FirstName);
            Assert.Equal(order.LastName, returnValue.LastName);
            Assert.Equal(order.Description, returnValue.Description);
            Assert.Equal(order.Quantity, returnValue.Quantity);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void PostOrder_InvalidOrder_ReturnsBadRequest()
        {
            // Arrange
            var order = new Order
            {
                FirstName = "John",
                // Missing LastName
                Description = "Test Order Description",
                Quantity = 5
            };
            _controller.ModelState.AddModelError("LastName", "Required");

            // Act
            var result = _controller.PostOrder(order);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void GetOrders_ReturnsAllOrders()
        {
            // Arrange
            var order1 = new Order
            {
                FirstName = "John",
                LastName = "Doe",
                Description = "Order 1 Description",
                Quantity = 5
            };
            var order2 = new Order
            {
                FirstName = "Jane",
                LastName = "Doe",
                Description = "Order 2 Description",
                Quantity = 10
            };
            _controller.PostOrder(order1);
            _controller.PostOrder(order2);

            // Act
            var result = _controller.GetOrders();

            // Assert
            var okResult = Assert.IsType<ActionResult<IEnumerable<Order>>>(result);
            var returnValue = Assert.IsType<List<Order>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetOrder_ExistingId_ReturnsOrder()
        {
            // Arrange
            var order = new Order
            {
                FirstName = "John",
                LastName = "Doe",
                Description = "Order 1 Description",
                Quantity = 5
            };
            _controller.PostOrder(order);

            // Act
            var result = _controller.GetOrder(order.Id);

            // Assert
            var okResult = Assert.IsType<ActionResult<Order>>(result);
            var returnValue = Assert.IsType<Order>(okResult.Value);
            Assert.Equal(order.Id, returnValue.Id);
            Assert.Equal(order.FirstName, returnValue.FirstName);
            Assert.Equal(order.LastName, returnValue.LastName);
            Assert.Equal(order.Description, returnValue.Description);
            Assert.Equal(order.Quantity, returnValue.Quantity);
        }

        [Fact]
        public void GetOrder_NonExistingId_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetOrder(99);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteOrder_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var order = new Order
            {
                FirstName = "John",
                LastName = "Doe",
                Description = "Order 1 Description",
                Quantity = 5
            };
            _controller.PostOrder(order);

            // Act
            var result = _controller.DeleteOrder(order.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteOrder_NonExistingId_ReturnsNotFound()
        {
            // Act
            var result = _controller.DeleteOrder(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
