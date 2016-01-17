using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Metronome.Services
{
    class CopyrightService
    {
        public IEnumerable<Copyright> Collect()
        {
            var licenseFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "License.csv");
            return File
                .ReadAllLines(licenseFile)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(item =>
                {
                    var parts = item.Split(';');
                    return new Copyright(parts[0], parts[1], parts[2], parts[3]);
                })
                .ToArray();
        }
    }
}
