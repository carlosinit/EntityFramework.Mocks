using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace CarlosInIt.EntityFramework.Mocks
{
    internal class FakeDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        #region Private Fields

        private readonly ObservableCollection<TEntity> _data;
        private readonly IQueryable _query;

        #endregion Private Fields

        #region Public Properties

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return _data; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new FakeDbAsyncQueryProvider<TEntity>(_query.Provider); }
        }

        #endregion Public Properties

        #region Public Constructors

        public FakeDbSet()
            :this(new ObservableCollection<TEntity>())
        {
        }

        public FakeDbSet(ObservableCollection<TEntity> memoryCollection)
        {
            _data = memoryCollection;
            _query = _data.AsQueryable();
        }

        #endregion Public Constructors

        #region Public Methods

        public override TEntity Add(TEntity entity)
        {
            _data.Add(entity);
            return entity;
        }

        public override TEntity Attach(TEntity entity)
        {
            _data.Add(entity);
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

        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new FakeDbAsyncEnumerator<TEntity>(_data.GetEnumerator());
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public override TEntity Remove(TEntity entity)
        {
            _data.Remove(entity);
            return entity;
        }

        #endregion Public Methods
    }
}