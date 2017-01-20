using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CarlosInIt.EntityFramework.Mocks
{
    internal class FakeDbAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
    {
        #region Private Fields

        private readonly IQueryProvider _inner;

        #endregion Private Fields

        #region Internal Constructors

        internal FakeDbAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IQueryable CreateQuery(Expression expression)
        {
            return new FakeDbAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new FakeDbAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute(expression));
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute<TResult>(expression));
        }

        #endregion Public Methods
    }
}