using System;

namespace CarlosInIt.EntityFramework.Mocks.Tests.Stubs
{
    internal class TestSubEntity
    {
        #region Public Properties

        public int Id { get; set; }
        public string Name { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public TestSubEntity()
        {
            Id = new Random().Next();
            Name = $"{nameof(Name)}_{Guid.NewGuid():N}";
        }

        #endregion Public Constructors
    }
}