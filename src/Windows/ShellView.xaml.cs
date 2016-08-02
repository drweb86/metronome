using System;
using System.Windows;
using System.Windows.Input;

namespace Metronome.Windows
{
    public partial class ShellView
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            var viewModel = (ShellViewModel) DataContext;
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

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
                e.Handled = true;
            }
            //else if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.Alt)
            //{
            //    OnMaximizeMinimize(sender, e);
            //    e.Handled = true;
            //}
        }
    }
}
