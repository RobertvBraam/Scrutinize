using System.Runtime.InteropServices;

namespace Integration.Tests.Utils;

public static class TestAppHelper
{
    public static string CurrentDirectory => Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.ToString();
    public static string TestAppDirectory => CurrentDirectory + @"/TestApp/";
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
}