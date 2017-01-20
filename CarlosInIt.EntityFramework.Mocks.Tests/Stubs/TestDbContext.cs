using CarlosInIt.EntityFramework.Mocks.Tests.Stubs;
using System.Data.Entity;

namespace CarlosInIt.EntityFramework.Mocks.Stubs.Tests
{
    internal class TestDbContext : DbContext
    {
        #region Public Properties

        public virtual DbSet<TestEntity> Entities { get; set; }

        #endregion Public Properties
    }
}