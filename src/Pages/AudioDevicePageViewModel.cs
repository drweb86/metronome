using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HDE.Platform.Wpf.Commands;
using Metronome.Annotations;
using Metronome.Commands;

namespace Metronome.Pages
{
    class AudioDevicePageViewModel : DependencyObject, INotifyPropertyChanged
    {
        readonly Controller Controller = Controller.Instance;

        private bool _initialized;
        public AudioDevicePageViewModel()
        {
            try
            {
                MultimediaDevicesFriendlyNames = Controller.Model.MultimediaDevicesFriendlyNames;
                SelectedMultimediaDeviceFriendlyName = Controller.Model.SelectedMultimediaDeviceFriendlyName;
                LatencyMiliseconds = Controller.Model.LatencyMseconds;
            }
            finally
            {
                _initialized = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Selected Multimedia Device Friendly Name

        public static readonly DependencyProperty SelectedMultimediaDeviceFriendlyNameProperty = DependencyProperty.Register(
            "SelectedMultimediaDeviceFriendlyName", typeof(string), typeof(AudioDevicePageViewModel),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectMultimediaDevice));

        private static void OnSelectMultimediaDevice(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((AudioDevicePageViewModel)d);
            if (viewModel._initialized)
            {
                Controller.Instance.Model.SelectedMultimediaDeviceFriendlyName = (string)e.NewValue;
                Task.Run(() => Controller.Instance.TestSound(false));
            }
        }

        public string SelectedMultimediaDeviceFriendlyName
        {
            get { return (string)GetValue(SelectedMultimediaDeviceFriendlyNameProperty); }
            set
            {
                SetValue(SelectedMultimediaDeviceFriendlyNameProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Multimedia Devices Friendly Names

        public static readonly DependencyProperty MultimediaDevicesFriendlyNamesProperty = DependencyProperty.Register(
            "MultimediaDevicesFriendlyNames", typeof(IEnumerable<string>), typeof(AudioDevicePageViewModel), new PropertyMetadata(default(IEnumerable<string>)));

        public IEnumerable<string> MultimediaDevicesFriendlyNames
        {
            get { return (IEnumerable<string>)GetValue(MultimediaDevicesFriendlyNamesProperty); }
            set
            {
                SetValue(MultimediaDevicesFriendlyNamesProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Latency

        public static readonly DependencyProperty LatencyMilisecondsProperty = DependencyProperty.Register(
            "LatencyMiliseconds", typeof(double), typeof(AudioDevicePageViewModel),
            new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeLatency));

        private static void OnChangeLatency(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((AudioDevicePageViewModel)d);
            if (viewModel._initialized)
            {
                viewModel.Controller.ChangeLatency((int) (double) e.NewValue);
            }
        }

        public double LatencyMiliseconds
        {
            get { return (double)GetValue(LatencyMilisecondsProperty); }
            set
            {
                SetValue(LatencyMilisecondsProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand RefreshMultimediaDevicesCommand { get; } = new RefreshMultimediaDevicesActionCommand();
        public ICommand TestSoundCommand { get; } = new ViewModelActionCommand<AudioDevicePageViewModel>(
            vm => Task.Run(() => Controller.Instance.TestSound(false)),
            vm => true);

        #endregion
    }
}
