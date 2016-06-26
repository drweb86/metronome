using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using HDE.Platform.Wpf.Commands;
using Metronome.Annotations;
using Metronome.Commands;

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
                SelectedTickSoundFile = Controller.Model.SelectedTickSoundFile;
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

        public static readonly DependencyProperty SelectedTickSoundFileProperty = DependencyProperty.Register(
            "SelectedTickSoundFile", typeof(string), typeof(AudioFilesPageViewModel),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectTickSound));

        private static void OnSelectTickSound(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = ((AudioFilesPageViewModel)d);
            if (viewModel._initialized)
            {
                viewModel.Controller.ChangeSelectedTickSoundFile((string)e.NewValue);
                if (viewModel.TestSoundCommand.CanExecute(viewModel))
                {
                    viewModel.TestSoundCommand.Execute(viewModel);
                }
            }
        }

        public string SelectedTickSoundFile
        {
            get { return (string)GetValue(SelectedTickSoundFileProperty); }
            set
            {
                SetValue(SelectedTickSoundFileProperty, value);
                OnPropertyChanged();
            }
        }

        #endregion


        #region Commands

        public ICommand TestSoundCommand { get; } = new ViewModelActionCommand<AudioFilesPageViewModel>(
            vm => Task.Run(() => Controller.Instance.TestSound()),
            vm => true);

        #endregion
    }
}
