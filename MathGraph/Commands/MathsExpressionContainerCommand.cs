using MathGraph.Interfaces;
using System.Windows.Input;
using System;

namespace MathGraph.Commands
{
    public abstract class MathsExpressionContainerCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected readonly IMathsExpressionContainer container;
        private readonly bool allowNullParameter;

        private bool couldExecute = true;

        public MathsExpressionContainerCommand(IMathsExpressionContainer container, bool allowNullParameter)
        {
            this.allowNullParameter = allowNullParameter;
            this.container = container;
        }

        public bool CanExecute(object parameter)
        {
            bool canExecute = this.GetCanExecute(parameter);
            if (canExecute != this.couldExecute)
            {
                this.couldExecute = canExecute;
                this.CanExecuteChanged?.Invoke(this, new EventArgs());
            }
            return canExecute;
        }

        protected virtual bool GetCanExecute(object parameter)
        {
            return (allowNullParameter || parameter != null) && this.container != null;
        }

        public abstract void Execute(object parameter);
    }
}
