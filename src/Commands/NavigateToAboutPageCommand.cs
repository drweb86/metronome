using Metronome.Pages;
using Metronome.Windows;

namespace Metronome.Commands
{
    class NavigateToAboutPageCommand : ViewModelCommand<MainWindowViewModel>
    {
        protected override void OnExecute(MainWindowViewModel model)
        {
            model.PageUri = PagesHelper.GetAboutPageUri();
        }

        protected override bool OnCanExecute(MainWindowViewModel model)
        {
            return true;
        }
    }
}
