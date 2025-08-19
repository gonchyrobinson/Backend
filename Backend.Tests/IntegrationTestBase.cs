using AutoMapper;
using Backend.Contexts;
using Backend.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tests
{
    public abstract class IntegrationTestBase
    {
        protected ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        protected IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}
