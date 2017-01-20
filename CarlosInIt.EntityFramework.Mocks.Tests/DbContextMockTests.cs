using CarlosInIt.EntityFramework.Mocks.Stubs.Tests;
using CarlosInIt.EntityFramework.Mocks.Tests.Stubs;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarlosInIt.EntityFramework.Mocks.Tests
{
    public class DbContextMockTests
    {
        #region Public Methods

        [Fact]
        public async Task SaveChangesAsyncCalls_CalledOnce_SaveChangesAsyncCallsIsOne()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act
            await contextMock.Object.SaveChangesAsync();

            // Assert
            contextMock.SaveChangesAsyncCalls.Should().Be(1);
        }

        [Fact]
        public void SaveChangesAsyncCalls_NeverCalled_SaveChangesAsyncCallsIsZero()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act

            // Assert
            contextMock.SaveChangesAsyncCalls.Should().Be(0);
        }

        [Fact]
        public void SaveChangesCalls_CalledOnce_SaveChangesCallsIsOne()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act
            contextMock.Object.SaveChanges();

            // Assert
            contextMock.SaveChangesCalls.Should().Be(1);
        }

        [Fact]
        public void SaveChangesCalls_NeverCalled_MockExceptionThrown()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act

            // Assert
            contextMock.SaveChangesCalls.Should().Be(0);
        }

        [Fact]
        public void WithCallToSaveChanges_WithoutValue_ResultShouldBeZero()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithCallToSaveChanges();

            // Act
            var result = contextMock.Object.SaveChanges();

            // Assert
            result.Should().Be(0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public void WithCallToSaveChanges_WithValue_ResultShouldBeAsExpected(int expectedValue)
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithCallToSaveChanges(expectedValue);

            // Act
            var result = contextMock.Object.SaveChanges();

            // Assert
            result.Should().Be(expectedValue);
        }

        [Fact]
        public async Task WithCallToSaveChangesAsync_WithoutValue_ResultShouldBeZero()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithCallToSaveChangesAsync();

            // Act
            var result = await contextMock.Object.SaveChangesAsync();

            // Assert
            result.Should().Be(0);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(5)]
        public async Task WithCallToSaveChangesAsync_WithValue_ResultShouldBeAsExpected(int expectedValue)
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithCallToSaveChangesAsync(expectedValue);

            // Act
            var result = await contextMock.Object.SaveChangesAsync();

            // Assert
            result.Should().Be(expectedValue);
        }

        [Fact]
        public void WithDbSetTest_WithEntities_EntitiesAreFound()
        {
            // Arrange
            var entities = new[]
            {
                new TestEntity(),
                new TestEntity()
            };
            // Act
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithDbSet(c => c.Entities, entities);

            // Assert
            contextMock.Object.Entities.Should().BeOfType<FakeDbSet<TestDbContext, TestEntity>>();
            contextMock.Object.Entities.Should().Equal(entities);
            contextMock.Object.Entities.Should().NotBeNull();
        }

        [Fact]
        public void WithDbSetTest_WithoutEntities_NoEntitiesFound()
        {
            // Arrange
            // Act
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithDbSet(c => c.Entities);

            // Assert
            contextMock.Object.Entities.Should().NotBeNull();
            contextMock.Object.Entities.Should().BeOfType<FakeDbSet<TestDbContext, TestEntity>>();
            contextMock.Object.Entities.ToList().Count.Should().Be(0);
        }

        #endregion Public Methods
    }
}