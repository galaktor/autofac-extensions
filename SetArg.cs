using System.Linq;
using System.Collections.Generic;

namespace Autofac
{
    public class SetArg
    {
        public string ModuleTypeName;
        public Dictionary<string, string> Args = new Dictionary<string, string>();

        public SetArg(string rawblob)
        {
            // TODO: checks

            var parts = rawblob.Split('{', '}', ',').Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();
            ModuleTypeName = parts[0];

            for (var i = 1; i < parts.Length; i++)
            {
                var arg = parts[i];
                var keyValueParts = arg.Split('=');
                Args.Add(keyValueParts[0], keyValueParts.Length > 1 ? keyValueParts[1] : null);
            }
        }
    }
}