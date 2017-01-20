using System.Data.Entity;

namespace CarlosInIt.EntityFramework.Mocks.Tests
{
    public class TestDbContext : DbContext
    {
        public TestDbContext()
        {
        }

        public virtual DbSet<TestEntity> Entities { get; set; }
    }
}