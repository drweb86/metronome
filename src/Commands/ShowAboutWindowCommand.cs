using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metronome.Windows;

namespace Metronome.Commands
{
    class ShowAboutWindowCommand : ViewModelCommand<MainWindowViewModel>
    {
        protected override void OnExecute(MainWindowViewModel model)
        {
            var window = new AboutWindow(model.Controller.Model.Copyrights);
            window.ShowDialog();
        }

        protected override bool OnCanExecute(MainWindowViewModel model)
        {
            return true;
        }
    }
}
