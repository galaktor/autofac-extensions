using Autofac.Configuration.Elements;

namespace Autofac
{
    public class CmdLineModuleElement : ModuleElement
    {
        public CmdLineModuleElement(LoadArg l)
        {
            base["type"] = l.ToString();
        }
    }
}