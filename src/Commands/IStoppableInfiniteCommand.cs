using System.Windows.Input;

namespace Metronome.Commands
{
    public interface IStoppableInfiniteCommand: ICommand
    {
        void Stop();
    }
}
