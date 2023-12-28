using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Integration.Tests.Utils;

public static class TestAppHelper
{
    public static string IntegrationTestDirectory => Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.ToString();
    public static string TestAppDirectory => IntegrationTestDirectory + "/TestApp/";
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    
    private static readonly object LockObject = new();
    private static bool _isInitialized;

    public static void InitializeTestTools()
    {
        lock (LockObject)
        {
            if (!_isInitialized)
            {
                var installToolsProcess = StartCmdProcess(TestAppDirectory);
                installToolsProcess.StandardInput.WriteLine("dotnet new tool-manifest --force");
                installToolsProcess.StandardInput.WriteLine("dotnet tool install dotnet-project-licenses");
                installToolsProcess.StandardInput.WriteLine("npm install license-checker");
                installToolsProcess.StandardInput.WriteLine("exit");
                installToolsProcess.WaitForExit();

                _isInitialized = true;
            }
        }
    }
    
    public static Process StartCmdProcess(string sourceDirectory)
    {
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = IsWindows ? "cmd" : "/bin/bash",
            WorkingDirectory = sourceDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        var process = Process.Start(processStartInfo)!;
        return process;
    }
}