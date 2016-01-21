using System.ComponentModel;
using Metronome.Pages;
using Metronome.Pictures;

namespace Metronome.Commands
{
    //TODO: do not use background worker.
    class ExecuteMetronomeAsyncCommand: ViewModelCommand<MainPageViewModel>, IStoppableInfiniteCommand
    {
        public void Stop()
        {
            _backgroundWorker.CancelAsync();
        }

        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        public ExecuteMetronomeAsyncCommand()
        {
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += DoWork;
            _backgroundWorker.RunWorkerCompleted += WorkCompleted;

        }

        protected override void OnExecute(MainPageViewModel model)
        {
            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();
                _cachedViewModel.StartMetronomeButtonImageUri = PicturesHelper.GetStop();
                _cachedViewModel.StartMetronomeButtonEnabled = false;
            }
            else
            {
                _cachedViewModel.StartMetronomeButtonImageUri = PicturesHelper.GetStop();
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
