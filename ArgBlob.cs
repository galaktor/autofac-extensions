using System.Collections.Generic;

namespace Autofac
{
    public class ArgBlob
    {
        public string Target;
        public Dictionary<string, string> Args = new Dictionary<string, string>();

        public ArgBlob(string rawblob)
        {
            // TODO: checks

            var parts = rawblob.Split(':');
            Target = parts[0];
            var argParts = parts[1].Split(';');
            foreach (var arg in argParts)
            {
                var keyValueParts = arg.Split('=');
                Args.Add(keyValueParts[0],keyValueParts[1]);
            }
        }
    }
}