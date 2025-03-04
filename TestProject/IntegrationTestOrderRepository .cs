using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class IntegrationTestOrderRepository : IClassFixture<DataBaseFixture>
    {
        private readonly DataBaseFixture _dbFixture;
        public IntegrationTestOrderRepository()
        {
            _dbFixture = new();
        }

        [Fact]
        public async Task CreateOrder_Should_Add_Order_To_Database()
        {
            // Arrange
            var _userRepository = new UserRepository(_dbFixture.Context);
            var _orderRepository = new OrderRepository(_dbFixture.Context);

            // Act
            var user = new User { FirstName = "John", LastName = "Doe", UserName = "john.doe@example.com", Password = "pas@@!@ASsword123" };
            var dbUser = await _userRepository.Post(user);

            // Ensure the user is saved before creating the order
            await _dbFixture.Context.SaveChangesAsync();

            var order = new Order { UserId = dbUser.Id, OrderDate = DateTime.Now, OrderSum = 100 };
            var dbOrder = await _orderRepository.Post(order);

            // Assert
            Assert.NotNull(dbOrder);
            Assert.NotEqual(0, dbOrder.Id);
            Assert.Equal(dbUser.Id, dbOrder.UserId);

            _dbFixture.Dispose();
        }

        [Fact]
        public async Task CreateOrder_Should_Fail_If_UserId_Is_Invalid()
        {
            // Arrange
            var _orderRepository = new OrderRepository(_dbFixture.Context);

            var order = new Order { UserId = 999, OrderDate = DateTime.Now, OrderSum = 100 };

            // Act
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _orderRepository.Post(order));
            _dbFixture.Dispose();
        }
    }
}