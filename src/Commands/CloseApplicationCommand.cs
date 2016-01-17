using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metronome.Windows;

namespace Metronome.Commands
{
    class CloseApplicationCommand : ViewModelCommand<MainWindowViewModel>
    {
        protected override void OnExecute(MainWindowViewModel model)
        {
            model.ExecuteMetronomeAsyncCommand.Stop();
            model.Controller.SaveSettings();
        }

        protected override bool OnCanExecute(MainWindowViewModel model)
        {
            return true;
        }
    }
}
