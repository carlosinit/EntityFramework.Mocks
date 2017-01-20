using CarlosInIt.EntityFramework.Mocks.Stubs.Tests;
using CarlosInIt.EntityFramework.Mocks.Tests.Stubs;
using FluentAssertions;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarlosInIt.EntityFramework.Mocks.Tests
{
    public class FakeDbSetTests
    {
        #region Public Methods

        [Fact]
        public void Add_WhenEntityAdded_ThenEntityIsInTheDbSet()
        {
            // Arrange
            var entity = new TestEntity();
            var context = PrepareContext();

            // Act
            context.Entities.Add(entity);

            // Assert
            var entities = context.Entities.ToList();
            entities.Count.Should().Be(1);
            entities.First().Should().Be(entity);
        }

        [Fact]
        public void Attach_WhenEntityAttached_ThenEntityIsAdded()
        {
            // Arrange
            var entity = new TestEntity();
            var context = PrepareContext();

            // Act
            context.Entities.Attach(entity);

            // Assert
            var entities = context.Entities.ToList();
            entities.Count.Should().Be(1);
            entities.First().Should().Be(entity);
        }

        [Fact]
        public void Create_ThenNewEntityIsConstructed()
        {
            // Arrange
            var context = PrepareContext();

            // Act
            var result = context.Entities.Create();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TestEntity>();
        }

        [Fact]
        public void CreateDerived_ThenNewDerivedEntityIsConstructed()
        {
            // Arrange
            var context = PrepareContext();

            // Act
            var result = context.Entities.Create<TestDerivedEntity>();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TestDerivedEntity>();
        }

        [Fact]
        public void Find_WhenFindPredicateIsNotProvided_ThenNotSupportedExceptionIsThrown()
        {
            // Arrange
            var context = PrepareContext();

            // Act
            Action action = () => context.Entities.Find(10);

            // Arrange
            action.ShouldThrow<NotSupportedException>();
        }

        [Fact]
        public void Find_WhenFindPredicateIsProvidedAndEntityFound_FoundEntityIsReturned()
        {
            // Arrange
            var entity = new TestEntity();
            var context = PrepareContext((e, kv) => e.Id == (int)kv[0], entity);

            // Act
            var result = context.Entities.Find(entity.Id);

            // Arrange
            result.Should().Be(entity);
        }

        [Fact]
        public void Find_WhenFindPredicateIsProvidedAndEntityNotFound_NullIsReturned()
        {
            // Arrange
            var context = PrepareContext((e, kv) => e.Id == (int)kv[0]);

            // Act
            var result = context.Entities.Find(10);

            // Arrange
            result.Should().BeNull();
        }

        [Fact]
        public void Include_WhenCalled_NoError()
        {
            // Arrange
            var entities = new[] { new TestEntity(), new TestEntity(), new TestEntity() };
            var context = PrepareContext(entities);

            // Act
            var result = context.Entities.Include(e => e.SubEntity).First();

            // Assert
            result.SubEntity.Should().NotBeNull();
        }

        [Fact]
        public void Remove_ThenEntityIsNoMoreInDbSet()
        {
            // Arrange
            var entityToBeDeleted = new TestEntity();
            var context = PrepareContext(entityToBeDeleted, new TestEntity());

            // Act
            var deleted = context.Entities.Remove(entityToBeDeleted);

            // Assert
            deleted.Should().Be(entityToBeDeleted);
            context.Entities.ToList().Count.Should().Be(1);
        }

        [Fact]
        public async Task ToListAsync_ThenAllTheEntitiesAreReturned()
        {
            // Arrange
            var entities = new[] { new TestEntity(), new TestEntity(), new TestEntity() };
            var context = PrepareContext(entities);

            // Act
            var result = await context.Entities.ToListAsync();

            // Assert
            result.Should().Equal(entities);
        }

        #endregion Public Methods

        #region Private Methods

        private static TestDbContext PrepareContext(
            Func<TestEntity, object[], bool> findPredicate,
            params TestEntity[] entities)
        {
            var context = new DbContextMock<TestDbContext>();
            context.WithDbSet(c => c.Entities, entities, findPredicate);
            return context.Object;
        }

        private static TestDbContext PrepareContext(params TestEntity[] entities)
        {
            var context = new DbContextMock<TestDbContext>();
            context.WithDbSet(c => c.Entities, entities);
            return context.Object;
        }

        #endregion Private Methods
    }
}