using System;

namespace Metronome.Services
{
    [Serializable]
    public class MetronomeSettings: IEquatable<MetronomeSettings>
    {
        public MetronomeSettings(
            string selectedTickSoundFile,
            string selectedMultimediaDeviceFriendlyName,
            float volume,
            int delayMseconds,
            int latencyMseconds)
        {
            SelectedTickSoundFile = selectedTickSoundFile;
            SelectedMultimediaDeviceFriendlyName = selectedMultimediaDeviceFriendlyName;
            Volume = volume;
            DelayMseconds = delayMseconds;
            LatencyMseconds = latencyMseconds;
        }

        public MetronomeSettings()
        {
        }

        public string SelectedTickSoundFile { get; set; }
        public string SelectedMultimediaDeviceFriendlyName { get; set; }
        public float Volume { get; set; }
        public int DelayMseconds { get; set; }
        public int LatencyMseconds { get; set; }

        public bool Equals(MetronomeSettings other)
        {
            return SelectedTickSoundFile == other.SelectedTickSoundFile &&
                SelectedMultimediaDeviceFriendlyName == other.SelectedMultimediaDeviceFriendlyName &&
                Math.Abs(Volume - other.Volume) < 0.001 &&
                DelayMseconds == other.DelayMseconds &&
                LatencyMseconds == other.LatencyMseconds;
        }
    }
}
