﻿using System;
using System.Windows.Input;

namespace Metronome.Commands
{
    public class ViewModelActionCommand<TViewModel> : ViewModelCommand<TViewModel>
    {
        #region Constructors

        public ViewModelActionCommand(Action<TViewModel> execute, Predicate<TViewModel> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            if (canExecute == null)
                throw new ArgumentNullException("canExecute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        protected override void OnExecute(TViewModel model)
        {
            _execute(model);
        }

        protected override bool OnCanExecute(TViewModel model)
        {
            return _canExecute(model);
        }

        #endregion

        private readonly Action<TViewModel> _execute;
        private readonly Predicate<TViewModel> _canExecute;
    }
}
