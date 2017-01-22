using CarlosInIt.EntityFramework.Mocks.Examples.Application;
using CarlosInIt.EntityFramework.Mocks.Examples.Model;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace CarlosInIt.EntityFramework.Mocks.Examples.Tests
{
    public class CarServiceTests
    {
        #region Private Fields

        private Car[] audiCars;
        private DbContextMock<FleetContext> dbContextMock;
        private Car[] existingCars;
        private CarService sut;
        private Car[] vwCars;

        #endregion Private Fields

        #region Public Constructors

        public CarServiceTests()
        {
            audiCars = GenerateAudiCars();
            vwCars = GenerateVwCars();
            existingCars = audiCars.Union(vwCars).ToArray();
            dbContextMock = new DbContextMock<FleetContext>();
            dbContextMock.WithDbSet(c => c.Cars, existingCars, (car, keys) => car.Id == (int)keys[0]);
            sut = new CarService(dbContextMock.Object);
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public void CreateNewCarTest_WithOneRowChanged()
        {
            // Arrange
            dbContextMock.WithCallToSaveChanges(1);
            var newCar = new Car { Brand = "Porche", Model = "911", PlateNumber = "AZERTY0" };

            // Act
            sut.CreateNewCar(newCar);

            // Assert
            var createdCar = dbContextMock.Object.Cars.Where(c => c.Id == 0).Single();
            createdCar.Should().Be(newCar);
            dbContextMock.SaveChangesCalls.Should().Be(1);
        }

        [Fact]
        public void CreateNewCarTest_WithoutRowChanges()
        {
            // Arrang
            dbContextMock.WithCallToSaveChanges();
            var newCar = new Car { Brand = "Porche", Model = "911", PlateNumber = "AZERTY0" };

            // Act
            Action action = () => sut.CreateNewCar(newCar);

            // Assert
            action.ShouldThrow<InvalidOperationException>();
            dbContextMock.SaveChangesCalls.Should().Be(1);
        }

        [Fact]
        public void DeleteCar_WithOneRowChanged()
        {
            // Arrange
            var carToDelete = existingCars[new Random().Next(0, existingCars.Length - 1)];
            dbContextMock.WithCallToSaveChanges(1);

            // Act
            sut.DeleteCar(carToDelete.Id);

            // Assert
            dbContextMock.Object.Cars.Should().NotContain(carToDelete);
        }

        [Fact]
        public void DeleteCar_WithoutRowChanges()
        {
            // Arrange
            var expectedCar = existingCars[new Random().Next(0, existingCars.Length - 1)];
            dbContextMock.WithCallToSaveChanges();

            // Act
            Action action = () => sut.DeleteCar(expectedCar.Id);

            // Assert
            action.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void GetAllCarsTest()
        {
            // Arrange

            // Act
            var result = sut.GetAllCars();

            // Assert
            result.Should().Equal(existingCars);
        }

        [Fact]
        public void GetByIdTest()
        {
            // Arrange
            var expectedCar = existingCars[new Random().Next(0, existingCars.Length - 1)];

            // Act
            var result = sut.GetById(expectedCar.Id);

            // Assert
            result.Should().Be(expectedCar);
        }

        [Fact]
        public void GetCarsByBrandTest()
        {
            // Arrange

            // Act
            var result = sut.GetCarsByBrand("VW").ToList();

            // Assert
            result.Should().Equal(vwCars);
        }

        [Fact]
        public void SaveCar_WithOneRowChanged()
        {
            // Arrange
            var expectedCar = existingCars[new Random().Next(0, existingCars.Length - 1)];
            var updatedCar = new Car { Id = expectedCar.Id, Brand = "Skoda", Model = "Superb" };
            dbContextMock.WithCallToSaveChanges(1);

            // Act
            sut.SaveCar(expectedCar.Id, updatedCar);

            // Assert
            expectedCar.Brand.Should().Be(updatedCar.Brand);
            expectedCar.Model.Should().Be(updatedCar.Model);
        }

        [Fact]
        public void SaveCar_WithoutRowChanges()
        {
            // Arrange
            var expectedCar = existingCars[new Random().Next(0, existingCars.Length - 1)];
            var updatedCar = new Car { Id = expectedCar.Id, Brand = "Skoda", Model = "Superb" };
            dbContextMock.WithCallToSaveChanges();

            // Act
            Action action = () => sut.SaveCar(expectedCar.Id, updatedCar);

            // Assert
            action.ShouldThrow<InvalidOperationException>();
        }

        private static Car[] GenerateAudiCars()
        {
            return new[]
            {
                new Car { Id = 1, Brand = "Audi", Model = "A1", PlateNumber="AZERTY1" },
                new Car { Id = 2, Brand = "Audi", Model = "A3", PlateNumber="AZERTY2" },
                new Car { Id = 3, Brand = "Audi", Model = "A5", PlateNumber="AZERTY3" },
                new Car { Id = 4, Brand = "Audi", Model = "A7", PlateNumber="AZERTY4" },
                new Car { Id = 5, Brand = "Audi", Model = "A8", PlateNumber="AZERTY5" },
                new Car { Id = 6, Brand = "Audi", Model = "Q3", PlateNumber="AZERTY6" }
            };
        }

        private static Car[] GenerateVwCars()
        {
            return new[]
            {
                new Car { Id = 7, Brand = "VW", Model = "Polo", PlateNumber="AZERTY7" },
                new Car { Id = 8, Brand = "VW", Model = "Golf", PlateNumber="AZERTY8" },
                new Car { Id = 9, Brand = "VW", Model = "Passat", PlateNumber="AZERTY9" },
                new Car { Id = 10, Brand = "VW", Model = "Jetta", PlateNumber="AZERTY10" }
            };
        }

        #endregion Public Methods
    }
}