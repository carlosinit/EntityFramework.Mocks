using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CarlosInIt.EntityFramework.Mocks
{
    internal class FakeDbAsyncEnumerable<T> :
        EnumerableQuery<T>,
        IDbAsyncEnumerable<T>,
        IQueryable<T>
    {
        #region Public Properties

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<T>(this); }
        }

        #endregion Public Properties

        #region Public Constructors

        public FakeDbAsyncEnumerable(IEnumerable<T> enumerable)
                    : base(enumerable)
        { }

        public FakeDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        #endregion Public Constructors

        #region Public Methods

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new FakeDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        #endregion Public Methods
    }
}