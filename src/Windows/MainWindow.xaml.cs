using System;
using System.Windows;
using System.Windows.Controls;
using Metronome.Pages;
using Metronome.Windows;

namespace Metronome.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            var viewModel = (MainWindowViewModel) DataContext;
            if (viewModel.CloseApplicationCommand.CanExecute(viewModel))
                viewModel.CloseApplicationCommand.Execute(viewModel);
        }
    }
}
