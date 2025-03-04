using Entities;
using Repositories;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class IntegrationTestOrderService: IClassFixture<DataBaseFixture>
    {
            private readonly DataBaseFixture _dbFixture;
            private readonly OrderService _orderService;
            public IntegrationTestOrderService()
            {
                _dbFixture = new DataBaseFixture();
                var orderRepository = new OrderRepository(_dbFixture.Context);
                var productRepository = new ProductRpository(_dbFixture.Context);
                _orderService = new OrderService(orderRepository, productRepository);
            }

            [Fact]
            public async Task Post_Should_Return_Null_If_Sum_Is_Incorrect()
            {
                // Arrange
                var category = new Category { CategoryName = "TestCategory" };
                await _dbFixture.Context.Categories.AddAsync(category);
                await _dbFixture.Context.SaveChangesAsync();

                var product1 = new Product { ProductName = "Product1", Price = 30, CategoryId = category.Id };
                var product2 = new Product { ProductName = "Product2", Price = 40, CategoryId = category.Id };
                await _dbFixture.Context.Products.AddRangeAsync(product1, product2);
                await _dbFixture.Context.SaveChangesAsync();

                var order = new Order
                {
                    OrderSum = 50,
                    OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = product1.Id },
                    new OrderItem { ProductId = product2.Id }
                }
                };

                // Act
                var result = await _orderService.Post(order);

                // Assert
                Assert.Null(result);
            }
            [Fact]
            public async Task Post_Should_Return_Order_If_Sum_Is_Correct()
            {
                // Arrange
                var user = new User { UserName = "johndoe", FirstName = "John", LastName = "Doe", Password = "password123" };
                await _dbFixture.Context.Users.AddAsync(user);
                await _dbFixture.Context.SaveChangesAsync();

                var category = new Category { CategoryName = "TestCategory" };
                await _dbFixture.Context.Categories.AddAsync(category);
                await _dbFixture.Context.SaveChangesAsync();

                var product1 = new Product { ProductName = "Product1", Price = 30, CategoryId = category.Id };
                var product2 = new Product { ProductName = "Product2", Price = 40, CategoryId = category.Id };
                await _dbFixture.Context.Products.AddRangeAsync(product1, product2);
                await _dbFixture.Context.SaveChangesAsync();

                var order = new Order
                {
                    UserId = user.Id,
                    OrderDate = DateTime.Now,
                    OrderSum = 70,
                    OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = product1.Id },
                    new OrderItem { ProductId = product2.Id }
                }
                };

                // Act
                var result = await _orderService.Post(order);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(order.OrderSum, result.OrderSum);
            }
        }
    }

