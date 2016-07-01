using System;
using System.Linq;

namespace Metronome.Services
{
    class ColorIndicatorService
    {
        private bool[] _colorIndicatorState;

        public ColorIndicatorService(int bitsSequenceLength)
        {
            Reset(bitsSequenceLength);
        }

        public void Reset(int bitsSequenceLength)
        {
            if (bitsSequenceLength < 1)
                throw new ArgumentOutOfRangeException(nameof(bitsSequenceLength));

            _colorIndicatorState = new bool[bitsSequenceLength];
            InvokeColorIndicatorChanged();
        }

        public void Bit(out bool isLast)
        {
            if (_colorIndicatorState.Length == 1)
            {
                _colorIndicatorState[0] = !_colorIndicatorState[0];
                isLast = false;
            }
            else
            {
                if (_colorIndicatorState.All(item => item))
                    for (int i = 0; i < _colorIndicatorState.Length; i++)
                        _colorIndicatorState[i] = false;

                for (int i = 0; i < _colorIndicatorState.Length; i++)
                    if (!_colorIndicatorState[i])
                    {
                        _colorIndicatorState[i] = true;
                        break;
                    }

                isLast = _colorIndicatorState.Length > 1 && _colorIndicatorState[_colorIndicatorState.Length - 1];
            }
            InvokeColorIndicatorChanged();
        }

        private void InvokeColorIndicatorChanged()
        {
            ColorIndicatorChanged?.Invoke(this, new ColorIndicatorEventArgs(_colorIndicatorState));
        }

        public EventHandler<ColorIndicatorEventArgs> ColorIndicatorChanged;
    }
}
