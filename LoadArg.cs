using System;
using System.Linq;

namespace Autofac
{
    public class LoadArg
    {
        public string AssemblyName;
        public string ModuleTypeName;

        public LoadArg(string arg)
        {
            string[] parts = arg.Split(',').Select(s => s.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToArray();

            switch (parts.Length)
            {
                case 2:
                    // Type and AssemblyName given
                    ModuleTypeName = parts[0];
                    AssemblyName = parts[1];
                    break;
                case 1:
                    // only TypeName given
                    ModuleTypeName = parts[0];
                    break;
                default:
                    throw new ArgumentException("Assembly/Type names in wrong format: " + arg);
            }
        }

        public override string ToString()
        {
            string result = ModuleTypeName;
            if (!String.IsNullOrWhiteSpace(AssemblyName))
            {
                result += ", " + AssemblyName;
            }

            return result;
        }
    }
}