using System.Windows.Input;

namespace Metronome.Commands
{
    public interface IStoppableInfiniteCommand: ICommand
    {
        bool IsRunning { get; }
        void Stop();
    }
}
