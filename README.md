# EntityFramework.Mocks (work in progress)
A mock of DbContext allowing the user to test it's entity framework calls against a memory collection.

This repository aims at becoming a nuget package that will be uploaded once finished.

The package will allow the developper using it to use entity framework with in memory collections instead of a real database for unit (or integration) testing. This is not a new concept and probably many already have their own implementations but I keep needing it every now and then and have trouble finding the appropriate documentation.

The documentation I am talking about and that is implemented in this package can be found here: https://msdn.microsoft.com/en-us/library/dn314431(v=vs.113).aspx
