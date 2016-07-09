using System;
using System.IO;
using System.Linq;
using HDE.Platform.AspectOrientedFramework;
using HDE.Platform.Logging;
using HDE.Platform.Services;
using Metronome.Services;
using NAudio.CoreAudioApi;

namespace Metronome
{
    class Controller: BaseController<Model>
    {
        public static Controller Instance { get; } = new Controller();

        public MetronomeService MetronomeService { get; }
        public ColorIndicatorService ColorIndicatorService { get; }

        protected override ILog CreateOpenLog()
        {
            var log = new SimpleFileLog(Path.Combine(Path.GetTempPath(), "Metronome#"));
            log.Open();
            return log;
        }

        private Controller()
        {
            ColorIndicatorService = new ColorIndicatorService(Model.BitsSequenceLength);
            MetronomeService = new MetronomeService(Log, Model.ToSettings, ColorIndicatorService);
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

        public void ChangeBitsPerMinute(int bitsPerMinute)
        {
            Model.BitsPerMinute = bitsPerMinute;
        }

        public void ChangeBitsSequenceLength(int value)
        {
            Model.BitsSequenceLength = value;
        }

        public void ChangeBeatSound(string soundFile)
        {
            Model.BeatSound = soundFile;
        }

        public void ChangeAccentedBeatSound(string soundFile)
        {
            Model.AccentedBeatSound = soundFile;
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

            Model.BeatSound = Model.TickSoundFiles.First();
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

        public void ProduceMetronomeSounds()
        {
            MetronomeService
                .Start();
        }

        public void StopMetronomeSounds()
        {
            MetronomeService
                .Stop();
        }

        public void TestSound(bool accented)
        {
            MetronomeService
                .Test(accented);
        }

        private SettingsService<MetronomeSettings> GetSettingsService()
        {
            return new SettingsService<MetronomeSettings>(
                Log,
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Metronome#"));
        }

        public void SaveSettings()
        {
            GetSettingsService()
                .Save(Model.ToSettings());
        }

        public void LoadSettings()
        {
            var settings = GetSettingsService()
                .Load();

            if (!settings.Equals(new MetronomeSettings())) // first run or settings are corrupted
                Model.FromSettings(settings);
        }

        public void ChangeDefaultBeatSquareColor(string newColor)
        {
            Model.DefaultBeatSquareColor = newColor;
        }

        public void ChangePassedBeatSquareColor(string newColor)
        {
            Model.PassedBeatSquareColor = newColor;
        }

        public void ChangeJustBeatedSquareColor(string newColor)
        {
            Model.JustBeatedSquareColor = newColor;
        }

        public void ChangeBeatTextColor(string newColor)
        {
            Model.BeatTextColor = newColor;
        }
    }
}
