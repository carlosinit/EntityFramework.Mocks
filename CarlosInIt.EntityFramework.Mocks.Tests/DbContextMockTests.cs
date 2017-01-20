using FluentAssertions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CarlosInIt.EntityFramework.Mocks.Tests
{
    public class DbContextMockTests
    {
        [Fact]
        public async Task VerifySaveChangesAsyncCalled_CalledOnce_NoException()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act
            await contextMock.Object.SaveChangesAsync();
            Action action = () => contextMock.VerifySaveChangesAsyncCalled();

            // Assert
            action.ShouldNotThrow();
        }

        [Fact]
        public void VerifySaveChangesAsyncCalled_NeverCalled_MockExceptionThrown()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act
            Action action = () => contextMock.VerifySaveChangesAsyncCalled();

            // Assert
            action.ShouldThrow<MockException>().And.Message.Contains("No invocations performed");
        }

        [Fact]
        public void VerifySaveChangesCalled_CalledOnce_NoException()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act
            contextMock.Object.SaveChanges();
            Action action = () => contextMock.VerifySaveChangesCalled();

            // Assert
            action.ShouldNotThrow();
        }

        [Fact]
        public void VerifySaveChangesCalled_NeverCalled_MockExceptionThrown()
        {
            // Arrange
            var contextMock = new DbContextMock<TestDbContext>();

            // Act
            Action action = () => contextMock.VerifySaveChangesCalled();

            // Assert
            action.ShouldThrow<MockException>().And.Message.Contains("No invocations performed");
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
                new TestEntity {City = "C1", Name = "N1" },
                new TestEntity {City = "C2", Name = "N2" }
            };
            // Act
            var contextMock = new DbContextMock<TestDbContext>();
            contextMock.WithDbSet(c => c.Entities, entities);

            // Assert
            contextMock.Object.Entities.Should().NotBeNull();
            contextMock.Object.Entities.Should().BeOfType<FakeDbSet<TestEntity>>();
            contextMock.Object.Entities.Should().Equal(entities);
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
            contextMock.Object.Entities.Should().BeOfType<FakeDbSet<TestEntity>>();
            contextMock.Object.Entities.ToList().Count.Should().Be(0);
        }
    }
}