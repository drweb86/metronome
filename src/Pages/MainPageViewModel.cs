using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using HDE.Platform.Wpf.Commands;
using Metronome.Annotations;
using Metronome.Pictures;
using Metronome.Services;

namespace Metronome.Pages
{
    class MainPageViewModel : DependencyObject, INotifyPropertyChanged
    {
        public int MinimumBitsSequenceLength => MetronomeSettings.MinimumBitsSequenceLength;
        public int MaximumBitsSequenceLength => MetronomeSettings.MaximumBitsSequenceLength;

        public int MinimumBitsPerMinute => MetronomeSettings.MinimumBitsPerMinute;
        public int MaximumBitsPerMinute => MetronomeSettings.MaximumBitsPerMinute;

        public double MinimumVolume => MetronomeSettings.MinimumVolume;
        public double MaximumVolume => MetronomeSettings.MaximumVolume;

        public MainPageViewModel()
        {
            Controller = Controller.Instance;

            BitsPerMinute = Controller.Model.BitsPerMinute;
            Volume = Controller.Model.Volume;
            StartMetronomeButtonText = Controller.MetronomeService.IsRunning
                ? "Stop metronome"
                : "Start metronome";
            BitsSequenceLength = Controller.Model.BitsSequenceLength;

            //TODO: there will be a memory leak, because VM is created twice for some reason.
            Controller.ColorIndicatorService.ColorIndicatorChanged += (sender, args) => Dispatcher.BeginInvoke((Action)(() =>
                {
                    OnColorIndicatorChanged(sender, args);
                }));
        }

        private readonly object _syncObject = new object();
        private void OnColorIndicatorChanged(object sender, ColorIndicatorEventArgs e)
        {
            lock (_syncObject)
            {
                int lastSelectedBit = -1;
                for (int i = e.Mask.Length - 1; i >= 0; i--)
                    if (e.Mask[i])
                    {
                        lastSelectedBit = i;
                        break;
                    }

                if (ColorIndicators == null || e.Mask.Length != ColorIndicators.Count)
                {
                    var result = new List<Tuple<int, bool, bool>>();
                    for (int i = 0; i < e.Mask.Length; i++)
                        result.Add(new Tuple<int, bool, bool>(i + 1, e.Mask[i], lastSelectedBit == i));

                    ColorIndicators = new ObservableCollection<Tuple<int, bool, bool>>(result);
                    return;
                }
                for (int i = 0; i < e.Mask.Length; i++)
                    ColorIndicators[i] = new Tuple<int, bool, bool>(i + 1, e.Mask[i], lastSelectedBit == i);
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

        #region BitsSequenceLength

        public static readonly DependencyProperty BitsSequenceLengthProperty = DependencyProperty.Register(
            "BitsSequenceLength", typeof (int), typeof (MainPageViewModel),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBitsSequenceLength));

        public int BitsSequenceLength
        {
            get { return (int) GetValue(BitsSequenceLengthProperty); }
            set
            {
                SetValue(BitsSequenceLengthProperty, value);
                OnPropertyChanged();
            }
        }

        private static void OnBitsSequenceLength(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainPageViewModel)d);
            viewModel.Controller.ChangeBitsSequenceLength((int)e.NewValue);
            viewModel.OnColorIndicatorChanged(null, new ColorIndicatorEventArgs(new bool[(int)e.NewValue]));
        }

        #endregion

        #region Color Indicators

        public static readonly DependencyProperty ColorIndicatorsProperty = DependencyProperty.Register(
            "ColorIndicators", typeof (ObservableCollection<Tuple<int, bool, bool>>), typeof (MainPageViewModel), new PropertyMetadata(default(ObservableCollection<Tuple<int, bool, bool>>)));

        public ObservableCollection<Tuple<int, bool, bool>> ColorIndicators
        {
            get { return (ObservableCollection<Tuple<int, bool, bool>>) GetValue(ColorIndicatorsProperty); }
            set
            {
                SetValue(ColorIndicatorsProperty, value); 
                OnPropertyChanged();
            }
        }

        #endregion

        #region Bits Per Minute

        public static readonly DependencyProperty BitsPerMinuteProperty = DependencyProperty.Register(
            "BitsPerMinute", typeof(int), typeof(MainPageViewModel),
            new FrameworkPropertyMetadata(default(int), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeBitsPerMinute));

        private static void OnChangeBitsPerMinute(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((MainPageViewModel)d);
            viewModel.Controller.ChangeBitsPerMinute((int)e.NewValue);
        }

        public int BitsPerMinute
        {
            get { return (int)GetValue(BitsPerMinuteProperty); }
            set
            {
                SetValue(BitsPerMinuteProperty, value);
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

        public static readonly DependencyProperty StartMetronomeButtonTextProperty = DependencyProperty.Register(
            "StartMetronomeButtonText", typeof(string), typeof(MainPageViewModel));

        public string StartMetronomeButtonText
        {
            get { return (string)GetValue(StartMetronomeButtonTextProperty); }
            set
            {
                SetValue(StartMetronomeButtonTextProperty, value);
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
                StartMetronomeButtonText = "Start metronome"; 
            }
            else
            {
                Controller.ProduceMetronomeSounds();
                StartMetronomeButtonText = "Stop metronome";
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
