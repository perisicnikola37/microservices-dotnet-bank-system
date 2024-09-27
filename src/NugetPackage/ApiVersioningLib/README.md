# ApiVersioningLib

**ApiVersioningLib** is a library designed to simplify API versioning for your services. It helps manage API versions for your services.

## Usage

To utilize the API versioning features, you can access the version management methods defined in the library. Here’s how to implement it in your code:

```csharp
// Instantiation
var apiVersioning = new ApiVersioning();

// Register your services and their versions
apiVersioning.AddService("FirstService", "1.0");  // Add FirstService with version 1.0
apiVersioning.AddService("SecondService", "1.2"); // Add SecondService with version 1.2
apiVersioning.AddService("ThirdService", "2.0");  // Add ThirdService with version 2.0

// Get the version of a specific service
var secondServiceVersion = apiVersioning.GetServiceVersion("SecondService"); // which gives "1.2"

// Get all services and their versions
var allVersions = apiVersioning.GetAllServiceVersions(); // which gives ["FirstService": "1.0", "SecondService": "1.2", "ThirdService": "2.0"]
