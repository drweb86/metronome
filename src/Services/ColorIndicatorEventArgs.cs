using System;

namespace Metronome.Services
{
    internal class ColorIndicatorEventArgs : EventArgs
    {
        public bool[] Mask { get; private set; }

        public ColorIndicatorEventArgs(bool[] mask)
        {
            Mask = mask;
        }
    }
}