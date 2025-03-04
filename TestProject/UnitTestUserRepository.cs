using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;

namespace TestProject
{
    public class UnitTestUserRepository
    {
        [Fact]
        public async Task GetUserLogin_validate_returnUser()
            {
            var user = new User { UserName = "shani", Password = "Aa12**30Ss#" };
            var mockContext = new Mock<ManagerApiContext>();
            var users = new List<User> { user };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            var result = await userRepository.PostLoginR(user.UserName, user.Password);

            Assert.Equal(user, result);
            }



        [Fact]
        public async Task GetById_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, UserName = "testuser@example.com" }; // הנחית כאן את פרטיך של המשתמש

            var mockContext = new Mock<ManagerApiContext>();
            var users = new List<User> { expectedUser };
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.GetById(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Id, result.Id);
            Assert.Equal(expectedUser.UserName, result.UserName);
        }

        [Fact]
        public async Task GetById_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var mockContext = new Mock<ManagerApiContext>();
            var users = new List<User>(); // אין משתמשים
            mockContext.Setup(x => x.Users).ReturnsDbSet(users);

            var userRepository = new UserRepository(mockContext.Object);

            // Act
            var result = await userRepository.GetById(userId);

            // Assert
            Assert.Null(result);
        }

    }
}