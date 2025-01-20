# ApiVersioningLib v1.1.3

**ApiVersioningLib** is a library designed to simplify API versioning in your .NET project.

## Usage

To utilize the API versioning features, you can access the version management methods defined in the library. Here’s how
to implement it in your code:

Instantiation:

```csharp
var instance = new ApiVersioning();
```

Register your services and their versions:

```csharp
instance.AddService("FirstService", "1.0"); // Add FirstService with version 1.0
instance.AddService("SecondService", "1.2"); // Add SecondService with version 1.2
instance.AddService("ThirdService", "2.0"); // Add ThirdService with version 2.0
```

Get the version of a specific service:

```csharp
var secondServiceVersion = instance.GetVersion("SecondService"); // which gives "1.2"
```

Get all services and their versions:

```csharp
var allVersions = instancer.GetAll(); // which
gives ["FirstService": "1.0", "SecondService": "1.2", "ThirdService": "2.0"]
```

## Exceptions

1. **InvalidVersionFormatException**: Thrown when the version format is invalid.
2. **ServiceVersionAlreadySetException**: Thrown if the version for a service already exists and is different.
3. **ServiceVersionNotFoundException**: Thrown when the service version is not found.

## Versions

Last updated: 20. january 2025.

[Support & Change Log on GitHub](https://github.com/perisicnikola37/microservices-dotnet-bank-system/blob/develop/SECURITY.md)

## Contributors

[ApiVersioningLib repository](https://github.com/perisicnikola37/microservices-dotnet-bank-system)