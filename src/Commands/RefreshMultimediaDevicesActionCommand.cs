using Metronome.Windows;

namespace Metronome.Commands
{
    class RefreshMultimediaDevicesActionCommand : ViewModelActionCommand<MainWindowViewModel>
    {
        public RefreshMultimediaDevicesActionCommand() : base(RefreshOutputDevices, vm=>true)
        {
        }

        private static void RefreshOutputDevices(MainWindowViewModel viewModel)
        {
            viewModel.Controller.LoadAvailableSoundDevices();

            viewModel.MultimediaDevicesFriendlyNames = viewModel.Controller.Model.MultimediaDevicesFriendlyNames;
            viewModel.SelectedMultimediaDeviceFriendlyName = viewModel.Controller.Model.SelectedMultimediaDeviceFriendlyName;
        }
    }
}
