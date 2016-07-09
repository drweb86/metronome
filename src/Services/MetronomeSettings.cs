using System;

namespace Metronome.Services
{
    [Serializable]
    public class MetronomeSettings: IEquatable<MetronomeSettings>
    {
        public static string DefaultBeatTextColor => "White";
        public static string DefaultJustBeatedSquareColor => "#1fa198";
        public static string DefaultPassedBeatSquareColor => "#073642";
        public static string DefaultBeatSquareColor => "#3182a4";


        public static int DefaultBitsSequenceLength => 2;
        public static int MinimumBitsSequenceLength => 1;
        public static int MaximumBitsSequenceLength => 8;

        public static int DefaultBitsPerMinute => 60;
        public static int MinimumBitsPerMinute => 20;
        public static int MaximumBitsPerMinute => 300;

        public static double MinimumVolume => 0.01;
        public static double MaximumVolume => 1;
        public static double DefaultVolume => 1;

        public static bool IsBitsSequenceLength(int bitsSequenceLength)
        {
            return bitsSequenceLength >= MinimumBitsSequenceLength &&
                    bitsSequenceLength <= MaximumBitsSequenceLength;
        }

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

        public static bool IsColorValid(string color)
        {
            if (string.IsNullOrWhiteSpace(color))
                return false;


            return true; //TODO:
        }

        public MetronomeSettings(
            string selectedTickSoundFile,
            string selectedMultimediaDeviceFriendlyName,
            double volume,
            int bitsPerMinute,
            int latencyMseconds,
            int bitsSequenceLength,
            string beatTextColor,
            string justBeatedSquareColor,
            string passedBeatSquareColor,
            string beatSquareColor)
        {
            if (!IsBitsPerMinuteValid(bitsPerMinute))
                throw new ArgumentOutOfRangeException(nameof(bitsPerMinute));
            if (!IsVolumeValid(volume))
                throw new ArgumentOutOfRangeException(nameof(volume));
            if (!IsBitsSequenceLength(bitsSequenceLength))
                throw new ArgumentOutOfRangeException(nameof(bitsSequenceLength));

            if (!IsColorValid(beatTextColor))
                throw new ArgumentOutOfRangeException(nameof(beatTextColor));
            if (!IsColorValid(justBeatedSquareColor))
                throw new ArgumentOutOfRangeException(nameof(justBeatedSquareColor));
            if (!IsColorValid(passedBeatSquareColor))
                throw new ArgumentOutOfRangeException(nameof(passedBeatSquareColor));
            if (!IsColorValid(beatSquareColor))
                throw new ArgumentOutOfRangeException(nameof(beatSquareColor));

            SelectedTickSoundFile = selectedTickSoundFile;
            SelectedMultimediaDeviceFriendlyName = selectedMultimediaDeviceFriendlyName;
            Volume = volume;
            BitsPerMinute = bitsPerMinute;
            LatencyMseconds = latencyMseconds;
            BitsSequenceLength = bitsSequenceLength;

            BeatTextColor = beatTextColor;
            JustBeatedSquareColor = justBeatedSquareColor;
            PassedBeatSquareColor = passedBeatSquareColor;
            BeatSquareColor = beatSquareColor;
        }

        public MetronomeSettings()
        {
        }

        public string BeatTextColor { get; set; }
        public string JustBeatedSquareColor { get; set; }
        public string PassedBeatSquareColor { get; set; }
        public string BeatSquareColor { get; set; }

        public string SelectedTickSoundFile { get; set; }
        public string SelectedMultimediaDeviceFriendlyName { get; set; }
        public double Volume { get; set; }
        public int BitsPerMinute { get; set; }
        public int LatencyMseconds { get; set; }
        public int BitsSequenceLength { get; set; }

        public bool Equals(MetronomeSettings other)
        {
            return 
                SelectedTickSoundFile == other.SelectedTickSoundFile &&
                SelectedMultimediaDeviceFriendlyName == other.SelectedMultimediaDeviceFriendlyName &&
                Math.Abs(Volume - other.Volume) < 0.001 &&
                BitsPerMinute == other.BitsPerMinute &&
                LatencyMseconds == other.LatencyMseconds &&
                BitsSequenceLength == other.BitsSequenceLength &&

                BeatTextColor == other.BeatTextColor &&
                JustBeatedSquareColor == other.JustBeatedSquareColor &&
                PassedBeatSquareColor == other.PassedBeatSquareColor &&
                BeatSquareColor == other.BeatSquareColor;
        }
    }
}
