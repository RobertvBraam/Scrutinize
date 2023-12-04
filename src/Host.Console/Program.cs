using System.Runtime.InteropServices;
using Domain.UserCases;
using Npm = Scanning.Licenses.Npm;
using Nuget = Scanning.Licenses.Nuget;

Console.WriteLine("Hello, World!");
var testNpmAppPath = "";
var testNugetAppPath = "";
var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
var npmlicenseScanning = new Npm.LicenseScanning(isWindows);
var usecase = new ScanLicensesUserCase(npmlicenseScanning);
var licenses = usecase.Execute(testNpmAppPath).Value;

var nugetLicenseScanning = new Nuget.LicenseScanning(isWindows);
usecase = new ScanLicensesUserCase(nugetLicenseScanning);
licenses.AddRange(usecase.Execute(testNugetAppPath).Value);

foreach (var license in licenses)
{
    Console.WriteLine($"Name: {license.DependencyName}");
    Console.WriteLine($"Version: {license.Version}");
    Console.WriteLine($"License: {license.Type}");
    Console.WriteLine($"Parent: {string.Join(", ", license.Parents)}");
}
Console.WriteLine("BYE BYE");