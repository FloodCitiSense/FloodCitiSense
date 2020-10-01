using Abp.Extensions;
using IIASA.FloodCitiSense.Mobile.Core.Core.Threading;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IIASA.FloodCitiSense.Mobile.Core.Commands
{
    public class HttpRequestCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly ICommand _command;

        protected HttpRequestCommand(ICommand command)
        {
            _command = command;
            command.CanExecuteChanged += (sender, args) => CanExecuteChanged.InvokeSafely(sender, args);
        }

        public bool CanExecute(object parameter)
        {
            return _command.CanExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _command.Execute(parameter);
        }

        public static HttpRequestCommand Create(Func<Task> func)
        {
            return new HttpRequestCommand(AsyncCommand.Create(() => WebRequestExecuter.Execute(func)));
        }
    }
}