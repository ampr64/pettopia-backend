# About Pettopia
Pettopia is a web application where members - be it shelters, rescue groups or just persons - can post animals up for adoption as well as help owners find their missing pets.
Originally, this was the final year project for my degree, but I decided to share it here as more of a sample project.

# The solution
REST API with DDD and CQRS implementation with a vertical slice architecture.

# Frameworks and libraries
* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0) with [C# 10](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-10)
* SQL Server
* [EF Core 6](https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0/whatsnew) as the ORM for write and read operations
* [SmartEnum](https://github.com/ardalis/SmartEnum) for strongly typed, feature rich enumerations
* [MediatR](https://github.com/jbogard/MediatR) for CQRS implementation
* [FluentValidation](https://github.com/FluentValidation/FluentValidation) for requests (commands and queries) validations
* [MailKit](https://github.com/jstedfast/MailKit) for emailing
* [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) for Swagger API documentation
* [Swashbuckle.FluentValidation](https://github.com/micro-elements/MicroElements.Swashbuckle.FluentValidation) for FluentValidation rules integration in Swagger
* [XUnit](https://github.com/xunit/xunit) for unit testing
* [FluentAssertions](https://github.com/fluentassertions/fluentassertions) for more readable test assertions
* [Moq](https://github.com/moq/moq4) for mocking
* [AutoFixture](https://github.com/AutoFixture/AutoFixture) for easier test data generation

# References
* [eShopOnWeb by Microsoft](https://github.com/dotnet-architecture/eShopOnWeb)
* [Clean Architecture by Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)
* [Sample DotNet Core CQRS API by Kamil Grzybek](https://github.com/kgrzybek/sample-dotnet-core-cqrs-api)
