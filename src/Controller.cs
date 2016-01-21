using System;
using System.IO;
using System.Linq;
using System.Threading;
using Metronome.Services;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Metronome
{
    class Controller
    {
        public static Controller Instance { get; } = new Controller();

        public Model Model { get; }

        private Controller()
        {
            Model = new Model();

            LoadAvailableMusic();
            LoadAvailableSoundDevices();
            LoadSettings();
            LoadCopyrights();
        }

        public void ChangeVolume(float volume)
        {
            Model.Volume = volume;
        }

        public void ChangeLatency(int latencyMseconds)
        {
            Model.LatencyMseconds = latencyMseconds;
        }

        public void ChangeDelay(int delay)
        {
            Model.DelayMseconds = delay;
        }

        public void ChangeSelectedTickSoundFile(string selectedTickSoundFile)
        {
            Model.SelectedTickSoundFile = selectedTickSoundFile;
        }

        public void LoadAvailableMusic()
        {
            Model.TickSoundFiles = Directory
                .GetFiles(Directories.TicksPath, "*.*", SearchOption.AllDirectories)
                .Where(item=>
                    !item.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (!Model.TickSoundFiles.Any())
                throw new InvalidDataException("Can't find any tick track");

            Model.SelectedTickSoundFile = Model.TickSoundFiles.First();
        }

        public void LoadAvailableSoundDevices()
        {
            var enumerator = new MMDeviceEnumerator();
            Model.MultimediaDevicesFriendlyNames = enumerator
                .EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                .Select(item=>item.FriendlyName)
                .ToArray();

            if (!Model.MultimediaDevicesFriendlyNames.Any())
                throw new InvalidDataException("Sound devices are missing");

            var defaultOutputDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            if (defaultOutputDevice != null)
                Model.SelectedMultimediaDeviceFriendlyName = defaultOutputDevice.FriendlyName;
            else
                Model.SelectedMultimediaDeviceFriendlyName = Model.MultimediaDevicesFriendlyNames.First();
        }

        public void UpdateSelectedMultimediaDeviceFriendlyName(string selectedMultimediaDeviceFriendlyName)
        {
            Model.SelectedMultimediaDeviceFriendlyName = selectedMultimediaDeviceFriendlyName;
        }

        private void LoadCopyrights()
        {
            Model.Copyrights = new CopyrightService().Collect();
        }

        public void ProduceMetronomeSounds(Func<bool> cancel)
        {
            new MetronomeService()
                .ProduceSounds(cancel, Model.ToSettings);
        }

        public void TestSound(string deviceFriendlyName, string soundFile)
        {
            if (string.IsNullOrWhiteSpace(deviceFriendlyName))
                throw new ArgumentException(deviceFriendlyName);
            if (string.IsNullOrWhiteSpace(deviceFriendlyName))
                throw new ArgumentException(soundFile);

            //this code is executed in different thread. thats why searching for devices again.
            var enumerator = new MMDeviceEnumerator();
            
            var device = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)
                .First(item => item.FriendlyName == deviceFriendlyName);

            using (WasapiOut player = new WasapiOut(device, AudioClientShareMode.Shared, false, Model.LatencyMseconds))
            using (WaveStream mainOutputStream = FileReaderFactory.Create(soundFile))
            using (WaveChannel32 volumeStream = new WaveChannel32(mainOutputStream))
            { 
                player.Init(volumeStream);
                volumeStream.Volume = Model.Volume;
                player.Play();

                Thread.Sleep(1000);
            }
        }

        public void SaveSettings()
        {
            new MetronomeSettingsService()
                .Save(Model.ToSettings());
        }

        public void LoadSettings()
        {
            var settings = new MetronomeSettingsService()
                .Load();

            if (settings == null)
                return;

            Model.FromSettings(settings);
        }
    }
}
