using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadersOfDigital.Definitions
{
    public class ViewModelHandledAction
    {
        public ViewModelHandledAction(Func<Task> action)
        {
            Action = action;

            ExceptionHandlers = new Dictionary<Type, Func<Exception, Task>>();
        }

        public Func<Task> Action { get; }

        public IDictionary<Type, Func<Exception, Task>> ExceptionHandlers { get; }

        public bool IsWithError { get; private set; }

        public bool IsWithInteraction { get; private set; }

        public string CommonErrorMessage { get; private set; }

        public ViewModelHandledAction WithExceptionHandle<TException>(Func<TException, Task> handler)
            where TException : Exception
        {
            ExceptionHandlers.TryAdd(typeof(TException), ex => handler(ex as TException));

            return this;
        }

        public ViewModelHandledAction SetIsWithError(bool value)
        {
            IsWithError = value;

            return this;
        }

        public ViewModelHandledAction SetIsWithInteraction(bool value)
        {
            IsWithInteraction = value;

            return this;
        }

        public ViewModelHandledAction SetCommonErrorMessage(string message)
        {
            CommonErrorMessage = message;

            return this;
        }
    }
}
