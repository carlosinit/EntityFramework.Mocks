using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarlosInIt.EntityFramework.Mocks
{
    internal class FakeDbSet<TDbContext, TEntity> :
        DbSet<TEntity>,
        IQueryable,
        IEnumerable<TEntity>,
        IDbAsyncEnumerable<TEntity>
        where TEntity : class
        where TDbContext : DbContext
    {
        #region Private Fields

        private readonly ObservableCollection<TEntity> data;
        private readonly IQueryable queryable;
        private TDbContext context;
        private Func<TEntity, object[], bool> findPredicate;

        #endregion Private Fields

        #region Public Properties

        Type IQueryable.ElementType
        {
            get { return queryable.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return queryable.Expression; }
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return data; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<TEntity>(queryable.Provider); }
        }

        #endregion Public Properties

        #region Public Constructors

        public FakeDbSet(
            TDbContext context,
            ObservableCollection<TEntity> data,
            Func<TEntity, object[], bool> findPredicate = null)
        {
            this.findPredicate = findPredicate;
            this.context = context;
            this.data = data;
            queryable = this.data.AsQueryable();
        }

        #endregion Public Constructors

        #region Public Methods

        public override TEntity Add(TEntity entity)
        {
            data.Add(entity);
            return entity;
        }

        public override TEntity Attach(TEntity entity)
        {
            data.Add(entity);
            return entity;
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }

        public override TEntity Find(params object[] keyValues)
        {
            if (findPredicate == null) throw new NotSupportedException(
                "Find or FindAsync cannot be called if the find predicate is not provided");

            return data.SingleOrDefault(d => findPredicate(d, keyValues));
        }

        public override Task<TEntity> FindAsync(params object[] keyValues)
        {
            return Task.FromResult(Find(keyValues));
        }

        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new FakeDbAsyncEnumerator<TEntity>(data.GetEnumerator());
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public override TEntity Remove(TEntity entity)
        {
            data.Remove(entity);
            return entity;
        }

        #endregion Public Methods
    }
}