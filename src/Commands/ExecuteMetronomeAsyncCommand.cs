using System.ComponentModel;
using Metronome.Pages;
using Metronome.Pictures;

namespace Metronome.Commands
{
    //TODO: do not use background worker. switch to thread or task.
    class ExecuteMetronomeAsyncCommand: ViewModelCommand<MainPageViewModel>, IStoppableInfiniteCommand
    {
        public bool IsRunning => _backgroundWorker != null && _backgroundWorker.IsBusy;

        public void Stop()
        {
            _backgroundWorker.CancelAsync();
        }

        //static because MainPageViewModel can be destroyed several times upon progress.
        private static readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        public ExecuteMetronomeAsyncCommand()
        {
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += DoWork;
            _backgroundWorker.RunWorkerCompleted += WorkCompleted;

        }

        protected override void OnExecute(MainPageViewModel model)
        {
            _cachedViewModel.StartMetronomeButtonImageUri = PicturesHelper.GetStop();

            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();
                _cachedViewModel.StartMetronomeButtonEnabled = false;
            }
            else
            {
                _backgroundWorker.RunWorkerAsync();
            }
        }

        private MainPageViewModel _cachedViewModel;
        protected override bool OnCanExecute(MainPageViewModel viewModel)
        {
            if (_backgroundWorker.CancellationPending)
                return false;
            if (string.IsNullOrWhiteSpace(viewModel.Controller.Model.SelectedTickSoundFile))
                return false;

            _cachedViewModel = viewModel;
            return true;
        }

        private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _cachedViewModel.StartMetronomeButtonImageUri = PicturesHelper.GetStart();
            _cachedViewModel.StartMetronomeButtonEnabled = true;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            _cachedViewModel.Controller.ProduceMetronomeSounds(
                () => _backgroundWorker.CancellationPending);
        }
    }
}
