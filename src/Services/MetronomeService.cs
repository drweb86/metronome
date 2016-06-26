using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Metronome.Collections;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Metronome.Services
{
    class MetronomeService: IMetronomeService
    {
        private Thread _thread;
        private readonly Func<MetronomeSettings> _getSettings;

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

        public MetronomeService(Func<MetronomeSettings> getSettings)
        {
            _getSettings = getSettings;
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

                var device = GetDevice(settings);
                var tickTockFiles = GetTickTockFiles(settings);

                var playingContents = new CircleCollection<Tuple<WasapiOut, WaveStream, WaveChannel32>>();
                foreach (var audioFile in tickTockFiles)
                {
                    AddPlayingContent(device, settings, audioFile, playingContents);
                }

                while (!_isCancellationRequested &&
                       _getSettings().Equals(settings))
                {
                    var playingContent = playingContents.GetNext();
                    playingContent.Item2.Seek(0, SeekOrigin.Begin);
                    playingContent.Item1.Play();

                    Thread.Sleep(settings.DelayMseconds);
                }

                DisposePlayingContents(playingContents);
            } while (!_isCancellationRequested);
        }

        private static void AddPlayingContent(MMDevice device, MetronomeSettings activeSettings, string audioFile,
            CircleCollection<Tuple<WasapiOut, WaveStream, WaveChannel32>> devices)
        {
            var player = new WasapiOut(device, AudioClientShareMode.Shared, false, activeSettings.LatencyMseconds);
            var mainOutputStream = FileReaderFactory.Create(audioFile);
            var volumeStream = new WaveChannel32(mainOutputStream);
            player.Init(volumeStream);
            volumeStream.Volume = activeSettings.Volume;

            devices.Add(new Tuple<WasapiOut, WaveStream, WaveChannel32>(
                player,
                mainOutputStream,
                volumeStream));
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

        private static void DisposePlayingContents(CircleCollection<Tuple<WasapiOut, WaveStream, WaveChannel32>> playingContents)
        {
            foreach (var item in playingContents.GetItems())
            {
                item.Item3.Dispose();
                item.Item2.Dispose();
                item.Item1.Dispose();
            }
        }

        private static MMDevice GetDevice(MetronomeSettings settings)
        {
            //this code is executed in different thread. thats why searching for devices again.
            var enumerator = new MMDeviceEnumerator();
            enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);

            var device = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                .First(item => item.FriendlyName == settings.SelectedMultimediaDeviceFriendlyName);
            return device;
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
                volumeStream.Volume = settings.Volume;
                player.Play();

                Thread.Sleep(1000);
            }
        }
    }
}
