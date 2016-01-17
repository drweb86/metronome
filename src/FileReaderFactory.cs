using System;
using NAudio.Wave;
using NAudio.WindowsMediaFormat;
using NVorbis.NAudioSupport;

namespace Metronome
{
    static class FileReaderFactory
    {
        public static WaveStream Create(string file)
        {
            if (file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
                return new WaveFileReader(file);

            if (file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                return new Mp3FileReader(file);

            if (file.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                return new VorbisWaveReader(file);

            if (file.EndsWith(".wma", StringComparison.OrdinalIgnoreCase))
                return new WMAFileReader(file);

            return new MediaFoundationReader(file);
        }
    }
}
