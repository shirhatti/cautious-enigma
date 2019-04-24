using Microsoft.Build.Framework;

namespace MSBuildTasks
{
    public class OrasPush : Microsoft.Build.Utilities.Task
    {
        public override bool Execute()
        {
            Log.LogMessage(MessageImportance.High, "Aloha");
            return true;
        }
    }
}
