namespace ECommerce.Architecture.Tests;

public class ArchitectureTests
{
    private const string DomainNamespace = "ECommerce.Domain";
    private const string ApplicationNamespace = "ECommerce.Application";
    private const string InfrastructureNamespace = "ECommerce.Infrastructure";
    private const string WebNamespace = "ECommerce.API";

    [Fact]
    public void Domain_ShouldNot_HAveDependencyOnOtherProjects()
    {
        // Arrange
        //System.Reflection.Assembly assembly = typeof(ECommerce.Domain.AssemblyReference).Assembly;
    }
}
