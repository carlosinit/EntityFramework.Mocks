using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace CarlosInIt.EntityFramework.Mocks
{
    internal class FakeDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        #region Private Fields

        private readonly IEnumerator<T> _inner;

        #endregion Private Fields

        #region Public Properties

        public T Current
        {
            get { return _inner.Current; }
        }

        object IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }

        #endregion Public Properties

        #region Public Constructors

        public FakeDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            _inner.Dispose();
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        #endregion Public Methods
    }
}