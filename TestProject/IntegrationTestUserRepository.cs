using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class IntegrationTestUserRepository : IClassFixture<DataBaseFixture>
    {
      

        private readonly DataBaseFixture _dbFixture;
        public IntegrationTestUserRepository()
        {
            _dbFixture = new();
        }


        [Fact]
            public async Task CreateUser_Should_Add_User_To_Database()
            {
                // Arrange
                var _repository = new UserRepository(_dbFixture.Context);

                // Act
                var user = new User { FirstName = "Shani", LastName = "Shulman", UserName = "mmm@gmail.com", Password = "21436@dfgAS@@!" };
                var dbUser = await _repository.Post(user);

                // Assert
                Assert.NotNull(dbUser);
                Assert.NotEqual(0, dbUser.Id);
                Assert.Equal("mmm@gmail.com", dbUser.UserName);
                _dbFixture.Dispose();
            }

        [Fact]
        public async Task UpdateUser_Should_Modify_User_In_Database()
        {
            // Arrange
            var _repository = new UserRepository(_dbFixture.Context);
            var user = new User { FirstName = "Shani", LastName = "Shulman", UserName = "mmm@gmail.com", Password = "21436@dfgAS@@!" };
            var dbUser = await _repository.Post(user);
            await _dbFixture.Context.SaveChangesAsync();

            // Act
            dbUser.FirstName = "UpdatedName";
            var updatedUser = await _repository.Put(dbUser.Id, dbUser);

            // Assert
            Assert.NotNull(updatedUser);
            Assert.Equal("UpdatedName", updatedUser.FirstName);
            Assert.Equal(dbUser.Id, updatedUser.Id);
            Assert.Equal("mmm@gmail.com", updatedUser.UserName);
            _dbFixture.Dispose();
        }

        [Fact]
        public async Task UpdateUser_Should_Return_Null_If_User_Not_Found()
        {
            // Arrange
            var _repository = new UserRepository(_dbFixture.Context);
            var user = new User { Id = 999, FirstName = "NonExistent", LastName = "User", UserName = "nonexistent@gmail.com", Password = "password123" };

            // Act & Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _repository.Put(user.Id, user));
            _dbFixture.Dispose();
        }

    }
}

