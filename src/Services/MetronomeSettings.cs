using System;

namespace Metronome.Services
{
    [Serializable]
    public class MetronomeSettings: IEquatable<MetronomeSettings>
    {
        public static int DefaultBitsPerMinute => 60;
        public static int MinimumBitsPerMinute => 20;
        public static int MaximumBitsPerMinute => 300;

        public static double MinimumVolume => 0.01;
        public static double MaximumVolume => 1;
        public static double DefaultVolume => 1;

        public static bool IsBitsPerMinuteValid(int bitsPerMinute)
        {
            return  bitsPerMinute >= MinimumBitsPerMinute &&
                    bitsPerMinute <= MaximumBitsPerMinute;
        }

        public static bool IsVolumeValid(double volume)
        {
            return  volume >= (MinimumVolume - 0.00001) &&
                    volume <= (MaximumVolume + 0.000001);
        }

        public MetronomeSettings(
            string selectedTickSoundFile,
            string selectedMultimediaDeviceFriendlyName,
            double volume,
            int bitsPerMinute,
            int latencyMseconds)
        {
            if (!IsBitsPerMinuteValid(bitsPerMinute))
                throw new ArgumentOutOfRangeException(nameof(bitsPerMinute));
            if (!IsVolumeValid(volume))
                throw new ArgumentOutOfRangeException(nameof(volume));

            SelectedTickSoundFile = selectedTickSoundFile;
            SelectedMultimediaDeviceFriendlyName = selectedMultimediaDeviceFriendlyName;
            Volume = volume;
            BitsPerMinute = bitsPerMinute;
            LatencyMseconds = latencyMseconds;
        }

        public MetronomeSettings()
        {
        }

        public string SelectedTickSoundFile { get; set; }
        public string SelectedMultimediaDeviceFriendlyName { get; set; }
        public double Volume { get; set; }
        public int BitsPerMinute { get; set; }
        public int LatencyMseconds { get; set; }

        public bool Equals(MetronomeSettings other)
        {
            return SelectedTickSoundFile == other.SelectedTickSoundFile &&
                SelectedMultimediaDeviceFriendlyName == other.SelectedMultimediaDeviceFriendlyName &&
                Math.Abs(Volume - other.Volume) < 0.001 &&
                BitsPerMinute == other.BitsPerMinute &&
                LatencyMseconds == other.LatencyMseconds;
        }
    }
}
