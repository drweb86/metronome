using System;

namespace Metronome.Services
{
    [Serializable]
    class MetronomeSettings: IEquatable<MetronomeSettings>
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

        public string SelectedTickSoundFile { get; }
        public string SelectedMultimediaDeviceFriendlyName { get; }
        public float Volume { get; }
        public int DelayMseconds { get; }
        public int LatencyMseconds { get; set; }

        public bool Equals(MetronomeSettings other)
        {
            return SelectedTickSoundFile == other.SelectedTickSoundFile &&
                SelectedMultimediaDeviceFriendlyName == other.SelectedMultimediaDeviceFriendlyName &&
                Volume == other.Volume &&
                DelayMseconds == other.DelayMseconds &&
                LatencyMseconds == other.LatencyMseconds;
        }
    }
}
