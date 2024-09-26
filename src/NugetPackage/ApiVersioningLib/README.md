# ApiVersioningLib

**ApiVersioningLib** is a lightweight library designed to simplify API versioning for your services. It helps manage and maintain different versions of APIs, allowing for smooth transitions and backward compatibility.

## Features

- **Centralized Versioning**: Define and manage API versions in a single place.
- **Easy Integration**: Quick installation and straightforward usage in your applications.

## Installation

You can install the **ApiVersioningLib** library via the NuGet Package Manager. Run the following command in your Package Manager Console:

```bash
Install-Package ApiVersioningLib
```

or

```bash
dotnet add package ApiVersioningLib --version 1.0.0
```

## Usage

To utilize the API versioning features, you can access the version constants defined in the library. Here’s how to implement it in your code:

```c#
using ApiVersioningLib;

public class ApiService
{
    public void PrintApiVersions()
    {
        Console.WriteLine($"Account API Version: {ApiVersioningExtensions.AccountApiVersion}");
        Console.WriteLine($"Customer API Version: {ApiVersioningExtensions.CustomerApiVersion}");
        Console.WriteLine($"Transaction API Version: {ApiVersioningExtensions.TransactionApiVersion}");
    }
}
```

### Example

Here's an example of how to incorporate the API versioning in a service:

```c#
public class AccountService
{
    public void GetAccountDetails()
    {
        // Use the defined API version for account-related functionality
        string version = ApiVersioningExtensions.AccountApiVersion;

        // Implementation for fetching account details...
        Console.WriteLine($"Fetching account details for API version: {version}");
    }
}
```

### API Versioning Constants

The library provides the following version constants:

- AccountApiVersion: The version for account-related APIs (currently set to "1.0")
- CustomerApiVersion: The version for customer-related APIs (currently set to "1.0")
- TransactionApiVersion: The version for transaction-related APIs (currently set to "1.0")

Feel free to modify the version constants as your APIs evolve over time!