using System.Windows.Controls;
using System.Windows.Input;

namespace Metronome.Pages
{
    /// <summary>
    /// Interaction logic for AudioDevicePage.xaml
    /// </summary>
    public partial class AudioDevicePage : Page
    {
        public AudioDevicePage()
        {
            InitializeComponent();
        }

        private void OnLatencySliderMouseUp(object sender, MouseButtonEventArgs e)
        {
            var vm = (AudioDevicePageViewModel) DataContext;
            if (vm.TestSoundCommand.CanExecute(vm))
            {
                vm.TestSoundCommand.Execute(vm);
            }
        }
    }
}
