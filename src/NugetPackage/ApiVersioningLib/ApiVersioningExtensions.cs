namespace ApiVersioningLib
    {
    public class ApiVersioning
        {
        // Dictionary to store service names and their versions
        private readonly Dictionary<string, string> _serviceVersions = new();

        public void AddService(string serviceName, string version)
        {
            // Update the version if it already exists or add a new service
            _serviceVersions[serviceName] = version;
        }

        // Method to get the version of a specific service
        public string GetServiceVersion(string serviceName)
        {
            return (_serviceVersions.TryGetValue(serviceName, out var version) ? version : null)!; 
        }

        // Optional method to get all services and their versions
        public Dictionary<string, string> GetAllServiceVersions()
        {
            return new Dictionary<string, string>(_serviceVersions); // Returns a copy of the dictionary
        }
        }
    }