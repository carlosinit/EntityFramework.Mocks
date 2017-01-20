using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarlosInIt.EntityFramework.Mocks
{
    public class DbContextMock<TDbContext> : IInterceptor
        where TDbContext : DbContext, new()
    {
        #region Public Properties

        public TDbContext Object { get; private set; }
        public int SaveChangesAsyncCalls { get; private set; }
        public int SaveChangesCalls { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public DbContextMock(params object[] constructorParameters)
        {
            var generator = new ProxyGenerator();
            var options = new ProxyGenerationOptions();
            Object = (TDbContext)generator.CreateClassProxy(
                typeof(TDbContext),
                constructorParameters,
                this);
        }

        #endregion Public Constructors

        #region Private Fields

        private int saveChangesResult;

        #endregion Private Fields

        #region Public Methods

        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case nameof(DbContext.SaveChanges):
                    SaveChangesCalls++;
                    invocation.ReturnValue = saveChangesResult;
                    break;

                case nameof(DbContext.SaveChangesAsync):
                    SaveChangesAsyncCalls++;
                    invocation.ReturnValue = Task.FromResult(saveChangesResult);
                    break;

                default:
                    invocation.Proceed();
                    break;
            }
        }

        public DbContextMock<TDbContext> WithCallToSaveChanges(int returnValue = 0)
        {
            saveChangesResult = returnValue;
            return this;
        }

        public DbContextMock<TDbContext> WithCallToSaveChangesAsync(int returnValue = 0)
        {
            saveChangesResult = returnValue;
            return this;
        }

        public DbContextMock<TDbContext> WithDbSet<TEntity>(
            Expression<Func<TDbContext, DbSet<TEntity>>> dbSetExpression,
            IEnumerable<TEntity> entities,
            Func<TEntity, object[], bool> findPredicate = null)
            where TEntity : class
        {
            var fakeDbSet = new FakeDbSet<TDbContext, TEntity>(
                Object,
                new ObservableCollection<TEntity>(entities),
                findPredicate);

            var propertyName = (dbSetExpression.Body as MemberExpression).Member.Name;
            typeof(TDbContext).GetProperty(propertyName).SetValue(Object, fakeDbSet);

            return this;
        }

        public DbContextMock<TDbContext> WithDbSet<TEntity>(
            Expression<Func<TDbContext, DbSet<TEntity>>> dbSetExpression,
            TEntity entity = null,
            Func<TEntity, object[], bool> findPredicate = null)
            where TEntity : class
        {
            var entities = new List<TEntity>();
            if (entity != null) entities.Add(entity);
            return WithDbSet(dbSetExpression, entities, findPredicate);
        }

        #endregion Public Methods
    }
}