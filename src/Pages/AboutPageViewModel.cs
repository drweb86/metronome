using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Metronome.Annotations;
using Metronome.Commands;
using Metronome.Services;

namespace Metronome.Pages
{
    class AboutPageViewModel : DependencyObject, INotifyPropertyChanged
    {
        public AboutPageViewModel()
        {
            Copyrights = Controller.Instance.Model.Copyrights;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Copyrights

        public static readonly DependencyProperty CopyrightsProperty = DependencyProperty.Register(
            "Copyrights", typeof (IEnumerable<Copyright>), typeof (AboutPageViewModel), 
            new PropertyMetadata(default(IEnumerable<Copyright>)));

        public IEnumerable<Copyright> Copyrights
        {
            get { return (IEnumerable<Copyright>) GetValue(CopyrightsProperty); }
            set { SetValue(CopyrightsProperty, value); }
        }

        #endregion

        #region Commands

        public ICommand GoToProjectWebsiteCommand { get; } = new ViewModelActionCommand<AboutPageViewModel>(
            vm=> Process.Start("https://metronomesharp.codeplex.com"), 
            vm=>true);

        #endregion
    }
}
