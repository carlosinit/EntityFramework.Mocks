# EntityFramework Mocks

## Introduction
This project was built in order to help developers to unit test code that depends on Entity Framework, without the need for any database, be it in-memory or "real".

> **Personal story**: Since there was already 3 occasions that I implemented this bunch of classes in different projects in order to test my data access, I decided to create a nuget that I could reuse in all of those projects and in the next to come.

## Nuget package
https://www.nuget.org/packages/CarlosInIt.EntityFramework.Mocks
``` 
Install-Package CarlosInIt.EntityFramework.Mocks
```
## Features
This project proposes an implementation of a mock of DbContext. That mock proposes several features:
* Register empty DbSet
* Register a pre-filled DbSet
* Count the number of times SaveChanges or SaveChangesAsync was called
* Define the result that SaveChanges or SaveChangesAsync should return
* Support for async method calls on the DbSet
* Support for Find anf FindAsync method when a find predicate is provided

What is not supported at the moment:
* Entity state tracking
* Include method (for now calling it makes no difference)
* Identity column support
* Advanced scenarios like logical delete
* Simulate database exceptions

## Quick start
You can create a DbContext mock by instantiating the Generic DbContextMock class while passing your real DbContext Type as the generic parameter
``` cs
var dbContextMock = new DbContextMock<FleetContext>();
```
Then you will need to tell which DbSet you will use
``` cs
dbContextMock.WithDbSet(c => c.Cars);
```
If you need the DbSet to be preloaded with data, you can use this overload instead
``` cs
dbContextMock.WithDbSet(c => c.Cars, new[] { ... });
```
If you need the DbSet to be able to perform a Find or FindAsync, you need to specify how the id is resolved
``` cs
dbContextMock.WithDbSet(c => c.Cars, new[] { ... }, (entity, keys)=>entity.Id == (int)keys[0]);
```
Now you can use the configured DbContext mock
``` cs
var sut = new CarService(dbContextMock.Object);
// Use the service now...
```
If you need to check if SaveChanges or SaveChangesAsync was called (which you do if you are saving data to the database)
``` cs
Assert.Equal(1, dbContextMock.SaveChangesCalls);
// or in case of async
Assert.Equal(1, dbContextMock.SaveChangesAsyncCalls);
```
You can also specify what SaveChanges and SaveChangesAsync should return
``` cs
dbContextMock.WithCallToSaveChanges(numberYouWantToBeReturned);
// or in case of async
dbContextMock.WithCallToSaveChangesAsync(numberYouWantToBeReturned);
```
Here goes a full example of an update
``` cs
public void SaveCarTest()
{
    // Arrange
    var existingCars = new[]
    {
        new Car { Id = 7, Brand = "VW", Model = "Polo", PlateNumber="AZERTY7" },
        new Car { Id = 8, Brand = "VW", Model = "Golf", PlateNumber="AZERTY8" },
        new Car { Id = 9, Brand = "VW", Model = "Passat", PlateNumber="AZERTY9" },
        new Car { Id = 10, Brand = "VW", Model = "Jetta", PlateNumber="AZERTY10" }
    };
    var dbContextMock = new DbContextMock<FleetContext>();
    dbContextMock.WithDbSet(c => c.Cars, existingCars, (car, keys) => car.Id == (int)keys[0]);
    var expectedCar = existingCars[new Random().Next(0, existingCars.Length - 1)];
    var updatedCar = new Car { Id = expectedCar.Id, Brand = "Skoda", Model = "Superb" };
    dbContextMock.WithCallToSaveChanges(1);

    // Act
    var sut = new CarService(dbContextMock.Object);
    sut.SaveCar(expectedCar.Id, updatedCar);

    // Assert
    Assert.Equal(updatedCar.Brand, expectedCar.Brand);
    Assert.Equal(updatedCar.Model, expectedCar.Model);
    Assert.Equal(1, dbContextMock.SaveChangesCalls);
}
```


## More examples
A complete example of a service (depending on DbContext) being tested can be found in this repository:
https://github.com/carlosinit/EntityFramework.Mocks/tree/master/CarlosInIt.EntityFramework.Mocks.Examples

*Note:* This examples use FluentAssertions and XUnit

## What's next?
All the unsupported features described above

## Remarks
* The implementation of this mock is based on the following Microsoft documentation:
https://msdn.microsoft.com/en-us/library/dn314431(v=vs.113).aspx
* The package has a dependency to Castle.Core in order to allow more specific method call interception in the future.
