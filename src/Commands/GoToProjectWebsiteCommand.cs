using System.Diagnostics;
using Metronome.Windows;

namespace Metronome.Commands
{
    class GoToProjectWebsiteCommand : ViewModelCommand<MainWindowViewModel>
    {
        protected override void OnExecute(MainWindowViewModel model)
        {
            Process.Start("https://metronomesharp.codeplex.com");
        }

        protected override bool OnCanExecute(MainWindowViewModel model)
        {
            return true;
        }
    }
}
