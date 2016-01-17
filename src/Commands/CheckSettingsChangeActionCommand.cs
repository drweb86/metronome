using System.Threading.Tasks;
using Metronome.Windows;

namespace Metronome.Commands
{
    class CheckSettingsChangeActionCommand: ViewModelActionCommand<MainWindowViewModel>
    {
        public CheckSettingsChangeActionCommand() : base(CheckSettingsChange, CanCheck)
        {
        }

        private static bool CanCheck(MainWindowViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.SelectedTickSoundFile))
                return false;

            if (string.IsNullOrWhiteSpace(viewModel.SelectedMultimediaDeviceFriendlyName))
                return false;

            if (viewModel.ExecuteMetronomeAsyncCommand.IsRunning)
                return false;

            return true;
        }

        private static void CheckSettingsChange(MainWindowViewModel viewModel)
        {
            // because of execution in other thread.
            string selectedMultimediaDeviceFriendlyName = viewModel.SelectedMultimediaDeviceFriendlyName;
            string selectedTickSoundFile = viewModel.SelectedTickSoundFile;

            Task.Run(() => viewModel.Controller.TestSound(
                selectedMultimediaDeviceFriendlyName,
                selectedTickSoundFile));
        }
    }
}
