# CarlosInIt.EntityFramework.Mocks
---
## Introduction
This project was built in order to help developers to unit test code that depends on Entity Framework without the need for any database being it in-memory or traditional.

## Nuget package
https://www.nuget.org/packages/CarlosInIt.EntityFramework.Mocks/1.0.0
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
* Entity tracking

## Example
``` cs
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
```
**Notice the real DbContext type needs to be passed as a generic type of the mock** 

## What's next?
In the near future, a more exhaustive how-to documentation will be added on this page.

Otherwise, right now it does what I need from it. But if you have a great idea, well I am open for suggestions and pull requests!

## Remarks
* The implementation of this mock is based on the following Microsoft documentation:
https://msdn.microsoft.com/en-us/library/dn314431(v=vs.113).aspx
* The package has a dependency to Castle.Core in order to allow more specific method call interception in the future.
