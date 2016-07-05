using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HDE.Platform.Wpf.Commands;
using Metronome.Annotations;
using Metronome.Commands;
using Metronome.Pages;

namespace Metronome.Windows
{
    public class MainWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            Controller = Controller.Instance;

            PageUri = PagesHelper.MainPageUri;
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

        public ICommand CloseMainWindowCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
           vm => Application.Current.MainWindow.Close());

        public ICommand NavigateToAboutPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.AboutPageUri);
        public ICommand NavigateToAudioDevicePageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.AudioDevicePageUri);
        public ICommand NavigateToAudioFilesPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.AudioFilesPageUri);
        public ICommand NavigateToMainPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.MainPageUri);

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
