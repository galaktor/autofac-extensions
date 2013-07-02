using Autofac.Configuration;

namespace Autofac
{
    public class CmdLineSectionHandler : SectionHandler
    {
        private readonly CmdLineModulesCollection modules = new CmdLineModulesCollection();

        public CmdLineSectionHandler()
        {
            base["modules"] = modules;
        }

        public void Add(LoadArg l)
        {
            modules.Add(l);
        }
    }
}