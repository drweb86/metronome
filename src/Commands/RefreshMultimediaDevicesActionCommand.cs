using HDE.Platform.Wpf.Commands;
using Metronome.Pages;

namespace Metronome.Commands
{
    class RefreshMultimediaDevicesActionCommand : ViewModelActionCommand<AudioDevicePageViewModel>
    {
        public RefreshMultimediaDevicesActionCommand() : base(RefreshOutputDevices, vm=>true)
        {
        }

        private static void RefreshOutputDevices(AudioDevicePageViewModel viewModel)
        {
            var controller = Controller.Instance;

            controller.LoadAvailableSoundDevices();

            viewModel.MultimediaDevicesFriendlyNames = controller.Model.MultimediaDevicesFriendlyNames;
            viewModel.SelectedMultimediaDeviceFriendlyName = controller.Model.SelectedMultimediaDeviceFriendlyName;
        }
    }
}
