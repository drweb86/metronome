using System;
using System.Windows;
using System.Windows.Input;

namespace Metronome.Windows
{
    public partial class MainWindow
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

        private void OnHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
                e.Handled = true;
            }
        }

        private void OnWindowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void OnMaximizeMinimize(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }
    }
}
