using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Metronome
{
    static class Directories
    {
        public static string ApplicationPath { get; }
        public static string TicksPath { get; }

        static Directories()
        {
            ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            TicksPath = Path.Combine(ApplicationPath, "ticks");
        }
    }
}
