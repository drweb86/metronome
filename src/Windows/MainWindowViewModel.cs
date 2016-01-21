using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Metronome.Annotations;
using Metronome.Commands;
using Metronome.Pages;

namespace Metronome.Windows
{
    public class MainWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        private readonly bool _initialized;
        public MainWindowViewModel()
        {
            try
            {
                Controller = Controller.Instance;

                PageUri = PagesHelper.GetMainPageUri();
            }
            finally
            {
                _initialized = true;
            }
        }

        internal Controller Controller { get; }

        #region Page Navigation

        public static readonly DependencyProperty PageUriProperty = DependencyProperty.Register(
            "PageUri", typeof(string), typeof(MainWindowViewModel));

        public string PageUri
        {
            get { return (string)GetValue(PageUriProperty); }
            set
            {
                SetValue(PageUriProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand CloseApplicationCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.Controller.SaveSettings());
        public ICommand NavigateToAboutPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAboutPageUri());
        public ICommand NavigateToAudioDevicePageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAudioDevicePageUri());
        public ICommand NavigateToAudioFilesPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAudioFilesPageUri());
        public ICommand NavigateToMainPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetMainPageUri());

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
