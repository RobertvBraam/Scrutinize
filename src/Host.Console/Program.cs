using System.Runtime.InteropServices;
using CommandDotNet;
using Domain.UserCases;
using Npm = Scanning.Licenses.Npm;
using Nuget = Scanning.Licenses.Nuget;

namespace Host.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            var exitcode = new AppRunner<Program>().Run(args);
            
            return exitcode;
        }
        
        public void Test(string outLoud)
        {
            System.Console.WriteLine(outLoud);
        }
        
        public void TestMore(string outLoud)
        {
            System.Console.WriteLine(outLoud);
        }
    }
}