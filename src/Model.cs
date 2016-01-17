using System.Collections.Generic;
using Metronome.Services;

namespace Metronome
{
    internal class Model
    {
        public IEnumerable<string> TickSoundFiles { get; set; }
        public string SelectedTickSoundFile { get; set; }

        public IEnumerable<string> MultimediaDevicesFriendlyNames { get; set; }
        public string SelectedMultimediaDeviceFriendlyName { get; set; }

        public float Volume { get; set; } = 1;
        public int DelayMseconds { get; set; } = 1000;
        public int LatencyMseconds { get; set; } = 20;

        public IEnumerable<Copyright> Copyrights { get; set; }

        public MetronomeSettings ToSettings()
        {
            return new MetronomeSettings(
                SelectedTickSoundFile,
                SelectedMultimediaDeviceFriendlyName,
                Volume,
                DelayMseconds,
                LatencyMseconds);
        }

        public void FromSettings(MetronomeSettings settings)
        {
            SelectedTickSoundFile = settings.SelectedTickSoundFile;
            SelectedMultimediaDeviceFriendlyName = settings.SelectedMultimediaDeviceFriendlyName;
            Volume = settings.Volume;
            DelayMseconds = settings.DelayMseconds;
            LatencyMseconds = settings.LatencyMseconds;
        }
    }
}
