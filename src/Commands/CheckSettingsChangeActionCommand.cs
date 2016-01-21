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

            if (viewModel.ExecuteMetronomeAsyncCommand.IsRunning)
                return false;

            return true;
        }

        private static void CheckSettingsChange(MainWindowViewModel viewModel)
        {
            Task.Run(() => viewModel.Controller.TestSound());
        }
    }
}
