namespace Account.Test
{
    public class ArchitectureTests : BaseTest
    {
        [Fact]
        public void ShouldHaveRequiredDependencies()
        {
            var assemblies = LoadAssemblies();

            var apiAssembly = assemblies.First(a => a.GetName().Name == ApiNamespace);
            var applicationAssembly = assemblies.First(a => a.GetName().Name == ApplicationNamespace);
            var domainAssembly = assemblies.First(a => a.GetName().Name == DomainNamespace);
            var infrastructureAssembly = assemblies.First(a => a.GetName().Name == InfrastructureNamespace);

            // Ensure API references Application
            Assert.Contains(applicationAssembly, apiAssembly.GetReferencedAssemblies().Select(a => Assembly.Load(a.Name!)));

            // Ensure Application references Domain
            Assert.Contains(domainAssembly, applicationAssembly.GetReferencedAssemblies().Select(a => Assembly.Load(a.Name!)));

            // Ensure Infrastructure references Domain
            Assert.Contains(domainAssembly, infrastructureAssembly.GetReferencedAssemblies().Select(a => Assembly.Load(a.Name!)));
        }
        
        [Fact]
        public void ShouldNotHaveUnwantedDependencies()
        {
            var assemblies = LoadAssemblies();

            var apiAssembly = assemblies.First(a => a.GetName().Name == ApiNamespace);
            var infrastructureAssembly = assemblies.First(a => a.GetName().Name == InfrastructureNamespace);

            // Ensure API does not reference Domain directly
            Assert.DoesNotContain(
                assemblies.First(a => a.GetName().Name == DomainNamespace),
                apiAssembly.GetReferencedAssemblies().Select(a => Assembly.Load(a.Name!))
            );

            // Ensure Infrastructure does not reference API
            Assert.DoesNotContain(
                apiAssembly,
                infrastructureAssembly.GetReferencedAssemblies().Select(a => Assembly.Load(a.Name!))
            );
        }
    }
}
