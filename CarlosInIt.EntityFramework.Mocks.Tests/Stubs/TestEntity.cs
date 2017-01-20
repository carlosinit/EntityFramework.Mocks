using System;

namespace CarlosInIt.EntityFramework.Mocks.Tests.Stubs
{
    internal class TestEntity
    {
        #region Public Properties

        public string City { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public TestSubEntity SubEntity { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public TestEntity()
        {
            Id = new Random().Next();
            City = $"{nameof(City)}_{Guid.NewGuid():N}";
            Name = $"{nameof(Name)}_{Guid.NewGuid():N}";
            SubEntity = new TestSubEntity();
        }

        #endregion Public Constructors
    }
}