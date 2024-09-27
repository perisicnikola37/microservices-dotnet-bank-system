# ApiVersioningLib

**ApiVersioningLib** is a library designed to simplify API versioning for your services.

## Usage 

### Registering versions in one service

```js
// Access the singleton instance of 'ApiVersioning'
var globalStore = ApiVersioning.Instance;

// Register your services and their versions
globalStore.AddService("FirstService", "1.0");  // Add FirstService with version 1.0
globalStore.AddService("SecondService", "1.2"); // Add SecondService with version 1.2
globalStore.AddService("ThirdService", "2.0");  // Add ThirdService with version 2.0
```

### Accessing versions in another service

In your second service, you can access the registered versions from global store like this:

```js
// Access the singleton instance of 'ApiVersioning'
var globalStore = ApiVersioning.Instance;

// Get the version of a specific service
var firstServiceVersion = globalStore.GetServiceVersion("FirstService"); // returns "1.0"
var thirdServiceVersion = globalStore.GetServiceVersion("ThirdService"); // returns "2.0"

// Optionally, get all services and their versions
var allVersions = globalStore.GetAllServiceVersions(); // returns ["FirstService": "1.0", "SecondService": "1.2", "ThirdService": "2.0"]
```

