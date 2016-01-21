using System;
using System.Windows.Controls;

namespace Metronome.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Dispatcher.ShutdownStarted += OnShutdown;
        }

        private void OnShutdown(object sender, EventArgs e)
        {
            var vm = (MainPageViewModel) DataContext;
            if (vm.DestroyPageCommand.CanExecute(vm))
                vm.DestroyPageCommand.Execute(vm);
        }
    }
}
