using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using HDE.Platform.Logging;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Metronome.Services
{
    class MetronomeService: IMetronomeService
    {
        private readonly ILog _log;
        private Thread _thread;
        private readonly Func<MetronomeSettings> _getSettings;
        private readonly ColorIndicatorService _colorIndicatorService;

        public bool IsRunning
        {
            get
            {
                if (_thread == null)
                    return false;
                return _thread.IsAlive;
            }
        }

        private volatile bool _isCancellationRequested;

        public MetronomeService(
            ILog log,
            Func<MetronomeSettings> getSettings,
            ColorIndicatorService colorIndicatorService)
        {
            _log = log;
            _getSettings = getSettings;
            _colorIndicatorService = colorIndicatorService;
            _thread = new Thread(Run);
        }

        public void Start()
        {
            if (_thread != null && _thread.IsAlive)
                throw new InvalidOperationException("MetronomeService is already running.");

            _isCancellationRequested = false;
            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop()
        {
            if (IsRunning)
            {
                _isCancellationRequested = true;
                _thread.Join();
                _thread = null;
            }
        }

        private void Run()
        {
            do
            {
                var settings = _getSettings(); // support for changing on the fly.

                // Output audio device.
                var device = GetDevice(settings);
                if (device == null)
                {
                    _log.Error("Unable to get output audio device.");
                    return;
                }

                // Color indicator setup
                _colorIndicatorService.Reset(settings.BitsSequenceLength);

                // Tick tock files
                var tickTockFiles = GetTickTockFiles(settings);
                if (tickTockFiles.Count == 0)
                {
                    _log.Error("List of audio files is empty.");
                    return;
                }
                if (tickTockFiles.Count > 2)
                {
                    _log.Warning("Only first and last files will be used.");
                }

                var firstPlayingContent = ToPlayingContent(device, settings, tickTockFiles.First());
                var lastPlayingContent = ToPlayingContent(device, settings, tickTockFiles.Last());

                while (!_isCancellationRequested &&
                       _getSettings().Equals(settings))
                {
                    bool isLast;
                    _colorIndicatorService.Bit(out isLast);

                    var playingContent = isLast ? firstPlayingContent : lastPlayingContent;

                    playingContent.Item2.Seek(0, SeekOrigin.Begin);
                    playingContent.Item1.Play();

                    Thread.Sleep(60000 / settings.BitsPerMinute);
                }

                DisposePlayingContent(firstPlayingContent);
                DisposePlayingContent(lastPlayingContent);
            } while (!_isCancellationRequested);
        }

        private Tuple<WasapiOut, WaveStream, WaveChannel32> ToPlayingContent(MMDevice device, MetronomeSettings activeSettings, string audioFile)
        {
            _log.Write(LoggingEvent.Debug, $"Creating playing content for {audioFile}");
            var player = new WasapiOut(device, AudioClientShareMode.Shared, false, activeSettings.LatencyMseconds);
            var mainOutputStream = FileReaderFactory.Create(audioFile);
            var volumeStream = new WaveChannel32(mainOutputStream);
            player.Init(volumeStream);
            volumeStream.Volume = (float)activeSettings.Volume;

            return new Tuple<WasapiOut, WaveStream, WaveChannel32>(
                player,
                mainOutputStream,
                volumeStream);
        }

        private static List<string> GetTickTockFiles(MetronomeSettings settings) // we support tick-tocks which contains of 2 files
        {
            var result = new List<string>();
            result.Add(settings.SelectedTickSoundFile);
            var pairFile = settings.SelectedTickSoundFile.Contains("-TICK")
               ? settings.SelectedTickSoundFile.Replace("-TICK", "-TOCK")
               : settings.SelectedTickSoundFile.Replace("-TOCK", "-TICK");

            if (File.Exists(pairFile))
            {
                result.Add(pairFile);
            }
            return result;
        }

        private static void DisposePlayingContent(Tuple<WasapiOut, WaveStream, WaveChannel32> content)
        {
            content.Item3.Dispose();
            content.Item2.Dispose();
            content.Item1.Dispose();
        }

        private static MMDevice GetDevice(MetronomeSettings settings)
        {
            //this code is executed in different thread. thats why searching for devices again.
            var enumerator = new MMDeviceEnumerator();

            return enumerator
                .EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                .FirstOrDefault(item => item.FriendlyName == settings.SelectedMultimediaDeviceFriendlyName) ??
                enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        }

        public void Test()
        {
            // we should not run neither produce test sounds while metronome is active.
            if (IsRunning)
                return;

            var settings = _getSettings();

            var device = GetDevice(settings);

            using (WasapiOut player = new WasapiOut(device, AudioClientShareMode.Shared, false, settings.LatencyMseconds))
            using (WaveStream mainOutputStream = FileReaderFactory.Create(settings.SelectedTickSoundFile))
            using (WaveChannel32 volumeStream = new WaveChannel32(mainOutputStream))
            {
                player.Init(volumeStream);
                volumeStream.Volume = (float)settings.Volume;
                player.Play();

                Thread.Sleep(1000);
            }
        }
    }
}
