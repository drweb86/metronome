using System.Windows;
using Caliburn.Micro;
using Metronome.Windows;

namespace Metronome
{
    public class MetronomeBootstrapper : BootstrapperBase
    {
        public MetronomeBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}
