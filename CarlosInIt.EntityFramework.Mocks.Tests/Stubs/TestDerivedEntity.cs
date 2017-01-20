using System;

namespace CarlosInIt.EntityFramework.Mocks.Tests.Stubs
{
    internal class TestDerivedEntity : TestEntity
    {
        #region Public Properties

        public string DerivedProperty { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public TestDerivedEntity()
        {
            DerivedProperty = $"{nameof(DerivedProperty)}_{Guid.NewGuid():N}";
        }

        #endregion Public Constructors
    }
}