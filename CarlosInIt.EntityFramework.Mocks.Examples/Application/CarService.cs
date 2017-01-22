using CarlosInIt.EntityFramework.Mocks.Examples.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarlosInIt.EntityFramework.Mocks.Examples.Application
{
    public class CarService
    {
        #region Private Fields

        private readonly FleetContext fleetContext;

        #endregion Private Fields

        #region Public Constructors

        public CarService(FleetContext fleetContext)
        {
            this.fleetContext = fleetContext;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateNewCar(Car car)
        {
            fleetContext.Cars.Add(car);
            var result = fleetContext.SaveChanges();
            if (result != 1)
            {
                throw new InvalidOperationException("Car was not created");
            }
        }

        public void DeleteCar(int id)
        {
            var car = GetById(id);
            fleetContext.Cars.Remove(car);
            var result = fleetContext.SaveChanges();
            if (result != 1)
            {
                throw new InvalidOperationException("Car was not deleted");
            }
        }

        public IEnumerable<Car> GetAllCars()
        {
            return fleetContext.Cars.ToList();
        }

        public Car GetById(int id)
        {
            return fleetContext.Cars.Find(id);
        }

        public IEnumerable<Car> GetCarsByBrand(string brand)
        {
            return fleetContext.Cars.Where(c => c.Brand == brand).ToList();
        }

        public void SaveCar(int id, Car car)
        {
            var existingCar = GetById(id);
            existingCar.Brand = car.Brand;
            existingCar.Model = car.Model;
            existingCar.PlateNumber = car.PlateNumber;
            var result = fleetContext.SaveChanges();
            if (result != 1)
            {
                throw new InvalidOperationException("Car was not saved");
            }
        }

        #endregion Public Methods
    }
}