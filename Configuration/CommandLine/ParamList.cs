using System.Collections.Generic;
using System.Linq;

namespace Autofac.CommandLine
{
    public class ParamList
    {
        public IEnumerable<string> Args;

        public static readonly ParamList Empty = new ParamList();

        public ParamList(IEnumerable<string> args = null)
        {
            Args = args ?? Enumerable.Empty<string>();
        }
    }
}