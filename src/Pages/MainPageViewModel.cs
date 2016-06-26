using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Metronome.Annotations;
using Metronome.Commands;
using Metronome.Pictures;

namespace Metronome.Pages
{
    class MainPageViewModel : DependencyObject, INotifyPropertyChanged
    {
        private readonly bool _initialized;
        public MainPageViewModel()
        {
            try
            {
                Controller = Controller.Instance;

                DelayMseconds = Controller.Model.DelayMseconds;
                Volume = Controller.Model.Volume;
                StartMetronomeButtonImageUri = Controller.MetronomeService.IsRunning ?
                    PicturesHelper.GetStop() :
                    PicturesHelper.GetStart();
            }
            finally
            {
                _initialized = true;
            }
        }

        internal Controller Controller { get; }
        
        #region Volume

        public static readonly DependencyProperty VolumeProperty = DependencyProperty.Register(
            "Volume", typeof(double), typeof(MainPageViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeVolume));

        private static void OnChangeVolume(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainPageViewModel)d);
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
            "DelayMseconds", typeof(double), typeof(MainPageViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeDelayMseconds));

        private static void OnChangeDelayMseconds(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainPageViewModel)d);
            viewModel.Controller.ChangeDelay((int)(double)e.NewValue);
        }

        public double DelayMseconds
        {
            get { return (double)GetValue(DelayMsecondsProperty); }
            set
            {
                SetValue(DelayMsecondsProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Start Metronome Button Enabled

        public static readonly DependencyProperty StartMetronomeButtonEnabledProperty = DependencyProperty.Register(
            "StartMetronomeButtonEnabled", typeof(bool), typeof(MainPageViewModel), new PropertyMetadata(true));

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
            "StartMetronomeButtonStartMetronomeButtonImageUri", typeof(string), typeof(MainPageViewModel));

        public string StartMetronomeButtonImageUri
        {
            get { return (string)GetValue(StartMetronomeButtonImageUriProperty); }
            set
            {
                SetValue(StartMetronomeButtonImageUriProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion


        #region Commands

        public ICommand CheckSettingsChangeCommand { get; } = new ViewModelActionCommand<MainPageViewModel>(
            vm => Task.Run(() => vm.Controller.TestSound()));

        public ICommand ExecuteMetronomeAsyncCommand { get; } = new ViewModelActionCommand<MainPageViewModel>(vm => vm.StartOrStopMetronome());

        private void StartOrStopMetronome()
        {
            if (string.IsNullOrWhiteSpace(Controller.Model.SelectedTickSoundFile))
                return;

            StartMetronomeButtonEnabled = false;
            if (Controller.MetronomeService.IsRunning)
            {
                Controller.StopMetronomeSounds();
                StartMetronomeButtonImageUri = PicturesHelper.GetStart(); 
            }
            else
            {
                Controller.ProduceMetronomeSounds();
                StartMetronomeButtonImageUri = PicturesHelper.GetStop();
            }
            StartMetronomeButtonEnabled = true;
        }

        public ICommand DestroyPageCommand { get; } = new ViewModelActionCommand<MainPageViewModel>(
            vm => { vm.Controller.MetronomeService.Stop(); });

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
