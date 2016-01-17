using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Metronome.Services
{
    class MetronomeSettingsService
    {
        private string GetSettingsFile()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Metronome#",
                "Settings.data");
        }

        public MetronomeSettings Load()
        {
            var settingsFile = GetSettingsFile();
            if (!File.Exists(settingsFile))
                return null;

            using (var stream = File.OpenRead(settingsFile))
                return (MetronomeSettings)new BinaryFormatter()
                    .Deserialize(stream);
        }

        public void Save(MetronomeSettings settings)
        {
            var settingsFile = GetSettingsFile();
            var settingsFolder = Path.GetDirectoryName(settingsFile);
            if (!Directory.Exists(settingsFolder))
                Directory.CreateDirectory(settingsFolder);

            if (File.Exists(settingsFile))
                File.Delete(settingsFile);

            using (var stream = File.OpenWrite(settingsFile))
                new BinaryFormatter().Serialize(stream, settings);
        }
    }
}
