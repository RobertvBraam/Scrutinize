using Domain;
using Domain.Dependencies;
using FluentAssertions;
using Integration.Tests.Utils;
using Scanning.Npm.Licenses;
using Scanning.Npm.Vulnerabilities;
using Snapshooter.Xunit;
using Npm = Scanning.Npm;
using Nuget = Scanning.Nuget;

namespace Integration.Tests;

public class DependencyScanningTests
{
    public DependencyScanningTests()
    {
        TestAppHelper.InitializeTestTools();
    }
    
    [Fact]
    public void WhenNugetScanningDependencies_ThenReturnListOfDependencies()
    {
        //Arrange
        var licensesScanning = new Nuget.Licenses.LicenseScanning(TestAppHelper.IsWindows);
        var vulnerabilityScanning = new Nuget.Vulnerabilities.VulnerabilityScanning(TestAppHelper.IsWindows);
        var sut = new ScanDependenciesUseCase(licensesScanning, vulnerabilityScanning);

        //Act
        var actual = sut.Execute(TestAppHelper.TestAppDirectory);

        //Assert
        actual.HasSucceeded.Should().BeTrue();
        Snapshot.Match(actual.Value, "e112d6e0-06a8-4a3b-95ca-46728964197f");
    }
    
    [Fact]
    public void WhenNpmScanningDependencies_ThenReturnListOfDependencies()
    {
        //Arrange
        var licensesScanning = new Npm.Licenses.LicenseScanning(TestAppHelper.IsWindows, new Logger<LicenseScanning>());
        var vulnerabilityScanning = new Npm.Vulnerabilities.VulnerabilityScanning(TestAppHelper.IsWindows, new Logger<VulnerabilityScanning>());
        var sut = new ScanDependenciesUseCase(licensesScanning, vulnerabilityScanning);

        //Act
        var actual = sut.Execute(TestAppHelper.TestAppDirectory);

        //Assert
        actual.HasSucceeded.Should().BeTrue();
        Snapshot.Match(actual.Value, "ce35020b-5e4a-470c-96d1-b3036ea44b75");
    }
}