using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Enigma
{
    public class Program
    {
        static void Main()
        {
            var finder = new MsBuildProjectFinder(Environment.CurrentDirectory);
            var projectFile = finder.FindMsBuildProject(null);
            var targetsFile = FindTargetsFile();
            var args = new[]
            {
                    "msbuild",
                    projectFile,
                    "/nologo",
                    "/t:Restore;Build;Publish;DisplayMessages", // defined in Oras.targets
                    "/p:CustomAfterMicrosoftCommonTargets=" + targetsFile,
                    "/p:CustomAfterMicrosoftCommonCrossTargetingTargets=" + targetsFile,
                    "/bl"
            };
            var psi = new ProcessStartInfo
            {
                FileName = DotNetMuxer.MuxerPathOrDefault(),
                Arguments = ArgumentEscaper.EscapeAndConcatenate(args),
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var process = Process.Start(psi);
            process.WaitForExit();
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            string FindTargetsFile()
            {
                var assemblyDir = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var searchPaths = new[]
                {
                    Path.Combine(AppContext.BaseDirectory, "assets"),
                    Path.Combine(assemblyDir, "assets"),
                    AppContext.BaseDirectory,
                    assemblyDir,
                };

                var targetPath = searchPaths.Select(p => Path.Combine(p, "Oras.targets")).FirstOrDefault(File.Exists);
                if (targetPath == null)
                {
                    Console.WriteLine("Fatal error: could not find Oras.targets");
                }
                return targetPath;
            }
        }
    }
}
