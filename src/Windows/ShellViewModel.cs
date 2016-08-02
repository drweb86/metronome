using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using HDE.Platform.Wpf.Commands;
using Metronome.Pages;

namespace Metronome.Windows
{
    public class ShellViewModel : PropertyChangedBase
    {
        public ShellViewModel()
        {
            Controller = Controller.Instance;

            PageUri = PagesHelper.MainPageUri;
        }

        internal Controller Controller { get; }

        #region Page Navigation

        private string _pageUri;

        public string PageUri
        {
            get { return _pageUri; }
            set
            {
                _pageUri = value;
                NotifyOfPropertyChange(() => PageUri);
            }
        }

        #endregion

        #region Commands

        public ICommand CloseApplicationCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
            vm => vm.Controller.SaveSettings());

        public ICommand CloseMainWindowCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
           vm => Application.Current.MainWindow.Close());

        public ICommand NavigateToAboutPageCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
            vm => vm.PageUri = PagesHelper.AboutPageUri);
        public ICommand NavigateToAudioDevicePageCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
            vm => vm.PageUri = PagesHelper.AudioDevicePageUri);
        public ICommand NavigateToColorsPageCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
            vm => vm.PageUri = PagesHelper.ColorsPageUri);
        public ICommand NavigateToAudioFilesPageCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
            vm => vm.PageUri = PagesHelper.AudioFilesPageUri);
        public ICommand NavigateToMainPageCommand { get; } = new ViewModelActionCommand<ShellViewModel>(
            vm => vm.PageUri = PagesHelper.MainPageUri);

        #endregion
    }
}
