using Autofac.Configuration.Elements;

namespace Autofac
{
    public class CmdLineModulesCollection : ModuleElementCollection
    {
        public void Add(LoadArg l)
        {
            var e = new CmdLineModuleElement(l);
            BaseAdd(e);
        }
    }
}