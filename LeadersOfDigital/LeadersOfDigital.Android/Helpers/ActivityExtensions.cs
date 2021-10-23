using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Converters;
using Definitions.Interactions;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.ViewModels;
using MvvmCross.Base;
using MvvmCross.Platforms.Android.Binding;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Platforms.Android.Views.Fragments;

namespace LeadersOfDigital.Android.Helpers
{
    public static class ActivityExtensions
    {
        public static TView AddLoadingStateActivityBindings<TView, TViewModel>(this TView view)
            where TView : MvxActivity<TViewModel>
            where TViewModel : PageViewModel
        {
            var progressBar = view.FindViewById<ProgressBar>(Resource.Id.loading_state_progress_bar) ??
                              throw new NullReferenceException($"There is no progress bar in {view.GetType()}. Include LoadingStateView.axml in your layout");

            var set = view.CreateBindingSet();

            set.Bind(progressBar)
                .For(x => x.BindVisible())
                .To(vm => vm.State)
                .WithConversion<IfEqualToParameterConverter>(PageStateType.Loading);

            set.Apply();

            return view;
        }

        public static TView AddLoadingStateDialogFragmentBindings<TView, TViewModel>(this TView dialogFragment, View dialogView)
            where TView : MvxDialogFragment<TViewModel>
            where TViewModel : PageViewModel
        {
            var progressBar = dialogView.FindViewById<ProgressBar>(Resource.Id.loading_state_progress_bar) ??
                              throw new NullReferenceException($"There is no progress bar in {dialogView.GetType()}. Include LoadingStateView.axml in your layout");

            var set = dialogFragment.CreateBindingSet();

            set.Bind(progressBar)
                .For(x => x.BindVisible())
                .To(vm => vm.State)
                .WithConversion<IfEqualToParameterConverter>(PageStateType.Loading);

            set.Apply();

            return dialogFragment;
        }

        public static void ShowToast(this Activity activity, string message, ToastLength length) =>
            Toast.MakeText(activity, message, length).Show();

        public static void SetInteraction<TResult>(
            this Activity activity,
            ref IExtendedInteraction<TResult> backingField,
            IExtendedInteraction<TResult> newValue,
            EventHandler<MvxValueEventArgs<TResult>> onRequestedSuccess = null,
            EventHandler<MvxValueEventArgs<TResult>> onRequestedFailure = null,
            EventHandler<MvxValueEventArgs<TResult>> onRequested = null)
            where TResult : BaseInteractionResult
        {
            if (backingField != null)
            {
                backingField.Requested -= onRequested;
                backingField.RequestedSuccess -= onRequestedSuccess;
                backingField.RequestedFailure -= onRequestedFailure;
                backingField.RequestedFailure -= OnRequestedFailure;
            }

            backingField = newValue;

            if (onRequested != null)
            {
                backingField.Requested += onRequested;
            }

            if (onRequestedSuccess != null)
            {
                backingField.RequestedSuccess += onRequestedSuccess;
            }

            if (onRequestedFailure != null)
            {
                backingField.RequestedFailure += onRequestedFailure;
            }
            else
            {
                backingField.RequestedFailure += OnRequestedFailure;
            }

            void OnRequestedFailure(object sender, MvxValueEventArgs<TResult> e) =>
                activity.ShowToast(e.Value.ErrorMessage, ToastLength.Short);
        }
    }
}
