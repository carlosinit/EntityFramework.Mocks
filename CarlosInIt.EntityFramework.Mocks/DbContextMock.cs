using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq.Expressions;

namespace CarlosInIt.EntityFramework.Mocks
{
    /// <summary>
    /// Implementation follows https://msdn.microsoft.com/en-us/library/dn314431(v=vs.113).aspx
    /// </summary>
    public class DbContextMock<TDbContext> : Mock<TDbContext>
        where TDbContext : DbContext
    {
        #region Public Methods

        public DbContextMock<TDbContext> VerifySaveChangesAsyncCalled(int numberOfTimes = 1)
        {
            Verify(c => c.SaveChangesAsync(), Times.Exactly(numberOfTimes));
            return this;
        }

        public DbContextMock<TDbContext> VerifySaveChangesCalled(int numberOfTimes = 1)
        {
            Verify(c => c.SaveChanges(), Times.Exactly(numberOfTimes));
            return this;
        }

        public DbContextMock<TDbContext> WithCallToSaveChanges(int returnValue = 0)
        {
            Setup(c => c.SaveChanges()).Returns(returnValue);
            return this;
        }

        public DbContextMock<TDbContext> WithCallToSaveChangesAsync(int returnValue = 0)
        {
            Setup(c => c.SaveChangesAsync()).ReturnsAsync(returnValue);
            return this;
        }

        public DbContextMock<TDbContext> WithDbSet<TEntity>(
            Expression<Func<TDbContext, DbSet<TEntity>>> dbSetExpression,
            IEnumerable<TEntity> entities) where TEntity : class
        {
            var fakeDbSet = new FakeDbSet<TEntity>(new ObservableCollection<TEntity>(entities));
            Setup(dbSetExpression).Returns(fakeDbSet);
            return this;
        }

        public DbContextMock<TDbContext> WithDbSet<TEntity>(
            Expression<Func<TDbContext, DbSet<TEntity>>> dbSetExpression,
            TEntity entity = null)
            where TEntity : class
        {
            if (entity != null) return WithDbSet(dbSetExpression, new[] { entity });

            var fakeDbSet = new FakeDbSet<TEntity>();
            Setup(dbSetExpression).Returns(fakeDbSet);
            return this;
        }

        #endregion Public Methods
    }
}