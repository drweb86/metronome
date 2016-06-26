namespace Metronome.Services
{
    interface IMetronomeService
    {
        bool IsRunning { get; }
        
        /// <summary>
        /// Starts metronome is different thread and waits till its started.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops and waits till service is started.
        /// </summary>
        void Stop();

        void Test();
    }
}