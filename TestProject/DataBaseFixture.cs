using Microsoft.EntityFrameworkCore;
using Repositories;

namespace TestProject
{
    public class DataBaseFixture
    {
        public ManagerApiContext Context { get; private set; }
        public DataBaseFixture()
        {
            var options = new DbContextOptionsBuilder<ManagerApiContext>()
            .UseSqlServer("Server=srv2\\pupils;Database=Test_shani;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
            Context = new ManagerApiContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

    }
}
