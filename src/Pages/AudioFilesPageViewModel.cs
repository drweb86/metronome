using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using Metronome.Annotations;

namespace Metronome.Pages
{
    class AudioFilesPageViewModel : DependencyObject, INotifyPropertyChanged
    {
        readonly Controller Controller = Controller.Instance;

        private bool _initialized;
        public AudioFilesPageViewModel()
        {
            try
            {
                TickSoundFiles = Controller.Model.TickSoundFiles;
                BeatSound = Controller.Model.BeatSound;
                AccentedBeatSound = Controller.Model.AccentedBeatSound;
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


        #region Tick Sound Files

        public static readonly DependencyProperty TickSoundFilesProperty = DependencyProperty.Register(
            "TickSoundFiles", typeof(IEnumerable<string>), typeof(AudioFilesPageViewModel), new PropertyMetadata(default(IEnumerable<string>)));

        public IEnumerable<string> TickSoundFiles
        {
            get { return (IEnumerable<string>)GetValue(TickSoundFilesProperty); }
            set
            {
                SetValue(TickSoundFilesProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Selected Tick Sound File

        public static readonly DependencyProperty BeatSoundProperty = DependencyProperty.Register(
            "BeatSound", typeof(string), typeof(AudioFilesPageViewModel),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeBeatSound));

        private static void OnChangeBeatSound(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((AudioFilesPageViewModel)d);
            if (viewModel._initialized)
            {
                viewModel.Controller.ChangeBeatSound((string)e.NewValue);
                Task.Run(() => Controller.Instance.TestSound(false));
            }
        }

        public string BeatSound
        {
            get { return (string)GetValue(BeatSoundProperty); }
            set
            {
                SetValue(BeatSoundProperty, value);
                OnPropertyChanged();
            }
        }


        public static readonly DependencyProperty AccentedBeatSoundProperty = DependencyProperty.Register(
            "AccentedBeatSound", typeof(string), typeof(AudioFilesPageViewModel),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnChangeAccentedBeatSound));

        private static void OnChangeAccentedBeatSound(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            var viewModel = ((AudioFilesPageViewModel)d);
            if (viewModel._initialized)
            {
                viewModel.Controller.ChangeAccentedBeatSound((string)e.NewValue);
                Task.Run(() => Controller.Instance.TestSound(true));
            }
        }

        public string AccentedBeatSound
        {
            get { return (string)GetValue(AccentedBeatSoundProperty); }
            set
            {
                SetValue(AccentedBeatSoundProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
