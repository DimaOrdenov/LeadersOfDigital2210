using System;
using MvvmCross.Base;
using MvvmCross.ViewModels;

namespace Definitions.Interactions
{
    public interface IExtendedInteraction<TResult> : IMvxInteraction<TResult>
        where TResult : BaseInteractionResult
    {
        event EventHandler<MvxValueEventArgs<TResult>> RequestedSuccess;

        event EventHandler<MvxValueEventArgs<TResult>> RequestedFailure;
    }
}
