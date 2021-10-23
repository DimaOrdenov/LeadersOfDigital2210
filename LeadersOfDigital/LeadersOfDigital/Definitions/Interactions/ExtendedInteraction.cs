using System;
using MvvmCross.Base;
using MvvmCross.ViewModels;

namespace Definitions.Interactions
{
    public class ExtendedInteraction<TResult> : MvxInteraction<TResult>, IExtendedInteraction<TResult>
        where TResult : BaseInteractionResult
    {
        public event EventHandler<MvxValueEventArgs<TResult>> RequestedSuccess;

        public event EventHandler<MvxValueEventArgs<TResult>> RequestedFailure;

        public new event EventHandler<MvxValueEventArgs<TResult>> Requested;

        public new void Raise(TResult request)
        {
            Requested?.Raise(this, request);

            if (request.IsSuccess)
            {
                RequestedSuccess?.Raise(this, request);
            }
            else
            {
                RequestedFailure?.Raise(this, request);
            }
        }
    }
}
