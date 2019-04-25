using Microsoft.Build.Framework;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MSBuildTasks
{
    public class OrasPush : Microsoft.Build.Utilities.Task
    {
        [Required]
        public string OrasExe { get; set; }

        public override bool Execute()
        {
            var psi = new ProcessStartInfo(fileName: OrasExe,
                                           arguments: "--help");
            psi.RedirectStandardOutput = true;
            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
                Log.LogMessage(MessageImportance.High, proc.StandardOutput.ReadToEnd());
            }
            return true;
        }
    }
}
