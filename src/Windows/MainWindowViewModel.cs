using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Metronome.Annotations;
using Metronome.Commands;
using Metronome.Pages;
using Metronome.Pictures;

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

                DelayMseconds = Controller.Model.DelayMseconds;
                Volume = Controller.Model.Volume;
                PageUri = PagesHelper.GetAboutPageUri();

                StartMetronomeButtonImageUri = PicturesHelper.GetStart();
            }
            finally
            {
                _initialized = true;
            }
        }

        internal Controller Controller { get; }

        #region Volume

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(
            "Volume", typeof(double), typeof(MainWindowViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeVolume));

        private static void OnChangeVolume(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainWindowViewModel)d);
            viewModel.Controller.ChangeVolume((float)(double)e.NewValue);
        }

        public double Volume
        {
            get { return (double)GetValue(VolumeProperty); }
            set
            {
                SetValue(VolumeProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Delay Mseconds

        public static readonly DependencyProperty DelayMsecondsProperty = DependencyProperty.Register(
            "DelayMseconds", typeof (double), typeof (MainWindowViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeDelayMseconds));

        private static void OnChangeDelayMseconds(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainWindowViewModel)d);
            viewModel.Controller.ChangeDelay((int)(double)e.NewValue);
        }

        public double DelayMseconds
        {
            get { return (double) GetValue(DelayMsecondsProperty); }
            set
            {
                SetValue(DelayMsecondsProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Start Metronome Button Enabled

        public static readonly DependencyProperty StartMetronomeButtonEnabledProperty = DependencyProperty.Register(
            "StartMetronomeButtonEnabled", typeof(bool), typeof(MainWindowViewModel), new PropertyMetadata(true));

        public bool StartMetronomeButtonEnabled
        {
            get { return (bool)GetValue(StartMetronomeButtonEnabledProperty); }
            set
            {
                SetValue(StartMetronomeButtonEnabledProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Start Metronome Button Text

        public static readonly DependencyProperty StartMetronomeButtonImageUriProperty = DependencyProperty.Register(
            "StartMetronomeButtonStartMetronomeButtonImageUri", typeof (string), typeof (MainWindowViewModel));

        public string StartMetronomeButtonImageUri
        {
            get { return (string) GetValue(StartMetronomeButtonImageUriProperty); }
            set
            {
                SetValue(StartMetronomeButtonImageUriProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

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

        public ICommand CheckSettingsChangeCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => Task.Run(() => vm.Controller.TestSound()));
        public IStoppableInfiniteCommand ExecuteMetronomeAsyncCommand { get; } = new ExecuteMetronomeAsyncCommand();
        public ICommand CloseApplicationCommand { get; } = new CloseApplicationCommand();
        public ICommand NavigateToAboutPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAboutPageUri());
        public ICommand NavigateToAudioDevicePageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAudioDevicePageUri());
        public ICommand NavigateToAudioFilesPageCommand { get; } = new ViewModelActionCommand<MainWindowViewModel>(
            vm => vm.PageUri = PagesHelper.GetAudioFilesPageUri());

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
