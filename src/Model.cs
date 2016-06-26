using System;
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

        public int LatencyMseconds { get; set; } = 20;

        public IEnumerable<Copyright> Copyrights { get; set; }

        public MetronomeSettings ToSettings()
        {
            return new MetronomeSettings(
                SelectedTickSoundFile,
                SelectedMultimediaDeviceFriendlyName,
                Volume,
                BitsPerMinute,
                LatencyMseconds);
        }

        public void FromSettings(MetronomeSettings settings)
        {
            SelectedTickSoundFile = settings.SelectedTickSoundFile;
            SelectedMultimediaDeviceFriendlyName = settings.SelectedMultimediaDeviceFriendlyName;

            Volume = MetronomeSettings.IsVolumeValid(settings.Volume) ? settings.Volume: MetronomeSettings.DefaultVolume;
            BitsPerMinute = MetronomeSettings.IsBitsPerMinuteValid(settings.BitsPerMinute) ? settings.BitsPerMinute : MetronomeSettings.DefaultBitsPerMinute;

            LatencyMseconds = settings.LatencyMseconds;
        }
    }
}
