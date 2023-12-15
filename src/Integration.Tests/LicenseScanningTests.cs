using Domain.UserCases;
using FluentAssertions;
using Integration.Tests.Utils;
using Npm = Scanning.Licenses.Npm;
using Nuget = Scanning.Licenses.Nuget;

namespace Integration.Tests;

public class LicenseScanningTests
{
    [Fact]
    public void WhenNugetScanningLicenses_ThenReturnListOfLicenses()
    {
        //Arrange
        var licensesScanning = new Nuget.LicenseScanning(true);
        var sut = new ScanLicensesUserCase(licensesScanning);

        //Act
        var actual = sut.Execute(TestAppHelper.TestAppDirectory, false);

        //Assert
        actual.HasSucceeded.Should().BeTrue();
        actual.Value.Should().HaveCount(20);
        //Contain specific license
        actual.Value.Should().Contain(x => x.DependencyName == "AutoMapper" &&
                                             x.Version == "12.0.1" &&
                                             x.Type == "MIT");
    }
    
    [Fact]
    public void WhenNpmScanningLicenses_ThenReturnListOfLicenses()
    {
        //Arrange
        var licensesScanning = new Npm.LicenseScanning(true);
        var sut = new ScanLicensesUserCase(licensesScanning);

        //Act
        var actual = sut.Execute(TestAppHelper.TestAppDirectory, false);

        //Assert
        actual.HasSucceeded.Should().BeTrue();
        actual.Value.Should().HaveCount(751);
        //Two packages have the same name but different license
        actual.Value.Where(x => x.DependencyName == "atob").Should().HaveCount(2).And
            .OnlyHaveUniqueItems(x => x.Type);
        //Two packages have the same name but different version
        actual.Value.Where(x => x.DependencyName == "escape-string-regexp").Should().HaveCount(2).And
            .OnlyHaveUniqueItems(x => x.Version);
    }
}