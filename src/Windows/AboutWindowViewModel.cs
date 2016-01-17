using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Metronome.Annotations;
using Metronome.Services;

namespace Metronome.Windows
{
    class AboutWindowViewModel : DependencyObject, INotifyPropertyChanged
    {
        public AboutWindowViewModel(IEnumerable<Copyright> copyrights)
        {
            Copyrights = copyrights;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty CopyrightsProperty = DependencyProperty.Register(
            "Copyrights", typeof (IEnumerable<Copyright>), typeof (AboutWindowViewModel), 
            new PropertyMetadata(default(IEnumerable<Copyright>)));

        public IEnumerable<Copyright> Copyrights
        {
            get { return (IEnumerable<Copyright>) GetValue(CopyrightsProperty); }
            set { SetValue(CopyrightsProperty, value); }
        }
    }
}
