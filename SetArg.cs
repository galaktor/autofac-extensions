using System.Collections.Generic;
using System.Linq;

namespace Autofac
{
    public class SetArg
    {
        public Dictionary<string, string> Args = new Dictionary<string, string>();
        public string TargetName;

        public SetArg(string rawblob)
        {
            // TODO: checks

            string[] parts =
                rawblob.Split('{', '}', ',').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            TargetName = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                string arg = parts[i];
                string[] keyValueParts = arg.Split('=');
                Args.Add(keyValueParts[0], keyValueParts.Length > 1 ? keyValueParts[1] : null);
            }
        }
    }
}