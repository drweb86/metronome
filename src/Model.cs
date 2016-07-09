using System;
using System.Collections.Generic;
using System.Linq;
using Metronome.Services;

namespace Metronome
{
    internal class Model
    {
        public IEnumerable<string> TickSoundFiles { get; set; }
        public string BeatSound { get; set; }
        public string AccentedBeatSound { get; set; }

        public IEnumerable<string> MultimediaDevicesFriendlyNames { get; set; }
        public string SelectedMultimediaDeviceFriendlyName { get; set; }

        private double _volume { get; set; }
        public double Volume
        {
            get { return _volume; }
            set
            {
                if (!MetronomeSettings.IsVolumeValid((value)))
                    throw new ArgumentOutOfRangeException(nameof(Volume));

                _volume = value;
            }
        }

        private int _bitsPerMinute;
        public int BitsPerMinute
        {
            get { return _bitsPerMinute; }
            set
            {
                if (!MetronomeSettings.IsBitsPerMinuteValid(value))
                    throw new ArgumentOutOfRangeException(nameof(BitsPerMinute));

                _bitsPerMinute = value;
            }
        }

        private int _bitsSequenceLength = MetronomeSettings.DefaultBitsSequenceLength;
        public int BitsSequenceLength
        {
            get { return _bitsSequenceLength; }
            set {
                if (!MetronomeSettings.IsBitsSequenceLength(value))
                    throw new ArgumentOutOfRangeException(nameof(BitsSequenceLength));

                _bitsSequenceLength = value;
            }
        }

        private string _beatTextColor;
        public string BeatTextColor
        {
            get { return _beatTextColor; }
            set
            {
                if (!MetronomeSettings.IsColorValid(value))
                    throw new ArgumentOutOfRangeException(nameof(BeatTextColor));

                _beatTextColor = value;
            }
        }

        private string _justBeatedSquareColor;
        public string JustBeatedSquareColor
        {
            get { return _justBeatedSquareColor; }
            set {
                if (!MetronomeSettings.IsColorValid(value))
                    throw new ArgumentOutOfRangeException(nameof(JustBeatedSquareColor));

                _justBeatedSquareColor = value;
            }
        }

        private string _passedBeatSquareColor;
        public string PassedBeatSquareColor
        {
            get { return _passedBeatSquareColor; }
            set {
                if (!MetronomeSettings.IsColorValid(value))
                    throw new ArgumentOutOfRangeException(nameof(PassedBeatSquareColor));

                _passedBeatSquareColor = value;
            }
        }

        private string _defaultBeatSquareColor;
        public string DefaultBeatSquareColor
        {
            get { return _defaultBeatSquareColor; }
            set {
                if (!MetronomeSettings.IsColorValid(value))
                    throw new ArgumentOutOfRangeException(nameof(DefaultBeatSquareColor));

                _defaultBeatSquareColor = value;
            }
        }

        public int LatencyMseconds { get; set; } = 20;

        public IEnumerable<Copyright> Copyrights { get; set; }

        public MetronomeSettings ToSettings()
        {
            return new MetronomeSettings(
                BeatSound,
                AccentedBeatSound,

                SelectedMultimediaDeviceFriendlyName,
                Volume,
                BitsPerMinute,
                LatencyMseconds,
                BitsSequenceLength,
                
                BeatTextColor,
                JustBeatedSquareColor,
                PassedBeatSquareColor,
                DefaultBeatSquareColor);
        }

        public void FromSettings(MetronomeSettings settings)
        {
            SelectedMultimediaDeviceFriendlyName = settings.SelectedMultimediaDeviceFriendlyName;
            Volume = MetronomeSettings.IsVolumeValid(settings.Volume) ? settings.Volume: MetronomeSettings.DefaultVolume;
            LatencyMseconds = settings.LatencyMseconds;

            // Support legacy configs of v.1.1
            BitsPerMinute = MetronomeSettings.IsBitsPerMinuteValid(settings.BitsPerMinute) ? settings.BitsPerMinute : MetronomeSettings.DefaultBitsPerMinute;
            BitsSequenceLength = MetronomeSettings.IsBitsSequenceLength(settings.BitsSequenceLength) ? settings.BitsSequenceLength : MetronomeSettings.DefaultBitsSequenceLength;
            // Support legacy configs of v.1.2
            BeatTextColor = MetronomeSettings.IsColorValid(settings.BeatTextColor) ? settings.BeatTextColor : MetronomeSettings.DefaultBeatTextColor;
            JustBeatedSquareColor = MetronomeSettings.IsColorValid(settings.JustBeatedSquareColor) ? settings.JustBeatedSquareColor : MetronomeSettings.DefaultJustBeatedSquareColor;
            PassedBeatSquareColor = MetronomeSettings.IsColorValid(settings.PassedBeatSquareColor) ? settings.PassedBeatSquareColor : MetronomeSettings.DefaultPassedBeatSquareColor;
            DefaultBeatSquareColor = MetronomeSettings.IsColorValid(settings.BeatSquareColor) ? settings.BeatSquareColor : MetronomeSettings.DefaultBeatSquareColor;
            BeatSound = !string.IsNullOrWhiteSpace(settings.BeatSound) ? settings.BeatSound : TickSoundFiles.FirstOrDefault();
            AccentedBeatSound = !string.IsNullOrWhiteSpace(settings.AccentedBeatSound) ? settings.AccentedBeatSound : TickSoundFiles.FirstOrDefault();
        }
    }
}
