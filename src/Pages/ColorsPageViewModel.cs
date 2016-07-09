using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using HDE.Platform.Wpf.Commands;
using Metronome.Annotations;
using Metronome.Services;

namespace Metronome.Pages
{
    internal class ColorsPageViewModel : DependencyObject, INotifyPropertyChanged
    {
        public ColorsPageViewModel()
        {
            DefaultBeatSquareColor = Controller.Instance.Model.DefaultBeatSquareColor;
            PassedBeatSquareColor = Controller.Instance.Model.PassedBeatSquareColor;
            JustBeatedSquareColor = Controller.Instance.Model.JustBeatedSquareColor;
            BeatTextColor = Controller.Instance.Model.BeatTextColor;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty DefaultBeatSquareColorProperty = DependencyProperty.Register(
            "DefaultBeatSquareColor", typeof (string), typeof (ColorsPageViewModel), new PropertyMetadata(default(string), OnDefaultBeatSquareColorChanged));

        private static void OnDefaultBeatSquareColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (string)e.NewValue;
            Controller.Instance.ChangeDefaultBeatSquareColor(newColor);
        }

        public string DefaultBeatSquareColor
        {
            get { return (string) GetValue(DefaultBeatSquareColorProperty); }
            set
            {
                SetValue(DefaultBeatSquareColorProperty, value);
                OnPropertyChanged();
            }
        }


        public static readonly DependencyProperty PassedBeatSquareColorProperty = DependencyProperty.Register(
            "PassedBeatSquareColor", typeof(string), typeof(ColorsPageViewModel), new PropertyMetadata(default(string), OnPassedBeatSquareColorChanged));

        private static void OnPassedBeatSquareColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (string)e.NewValue;
            Controller.Instance.ChangePassedBeatSquareColor(newColor);
        }

        public string PassedBeatSquareColor
        {
            get { return (string)GetValue(PassedBeatSquareColorProperty); }
            set
            {
                SetValue(PassedBeatSquareColorProperty, value);
                OnPropertyChanged();
            }
        }





        public static readonly DependencyProperty JustBeatedSquareColorProperty = DependencyProperty.Register(
            "JustBeatedSquareColor", typeof(string), typeof(ColorsPageViewModel), new PropertyMetadata(default(string), OnJustBeatedSquareColorChanged));

        private static void OnJustBeatedSquareColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (string)e.NewValue;
            Controller.Instance.ChangeJustBeatedSquareColor(newColor);
        }

        public string JustBeatedSquareColor
        {
            get { return (string)GetValue(JustBeatedSquareColorProperty); }
            set
            {
                SetValue(JustBeatedSquareColorProperty, value);
                OnPropertyChanged();
            }
        }




        public static readonly DependencyProperty BeatTextColorProperty = DependencyProperty.Register(
            "BeatTextColor", typeof(string), typeof(ColorsPageViewModel), new PropertyMetadata(default(string), OnBeatTextColorChanged));

        private static void OnBeatTextColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newColor = (string)e.NewValue;
            Controller.Instance.ChangeBeatTextColor(newColor);
        }

        public string BeatTextColor
        {
            get { return (string)GetValue(BeatTextColorProperty); }
            set
            {
                SetValue(BeatTextColorProperty, value);
                OnPropertyChanged();
            }
        }

        #region Commands
        
        public ICommand ResetColorsCommand { get; } = new ViewModelActionCommand<ColorsPageViewModel>(
            vm => vm.ResetColors());

        #endregion

        private void ResetColors()
        {
            DefaultBeatSquareColor = MetronomeSettings.DefaultBeatSquareColor;
            PassedBeatSquareColor = MetronomeSettings.DefaultPassedBeatSquareColor;
            JustBeatedSquareColor = MetronomeSettings.DefaultJustBeatedSquareColor;
            BeatTextColor = MetronomeSettings.DefaultBeatTextColor;
        }
    }
}
