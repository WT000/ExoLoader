using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterLoader
{
    internal static class ModInstance
    {
        public static Injector instance;

        public static void log(string message)
        {
            instance.Log(message);
        }
    }
}
