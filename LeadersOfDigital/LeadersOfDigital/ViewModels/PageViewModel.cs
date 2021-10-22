using System;
using System.Threading;
using System.Threading.Tasks;
using LeadersOfDigital.Definitions;
using LeadersOfDigital.Definitions.Exceptions;
using Microsoft.Extensions.Logging;
using MvvmCross.Commands;
using MvvmCross.Localization;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace LeadersOfDigital.ViewModels
{
    public abstract class PageViewModel : MvxViewModel
    {
        private readonly CancellationTokenSource _cancellationTokenSource;

        private string _pageError;
        private PageStateType _state;

        protected PageViewModel(IMvxNavigationService navigationService, ILogger logger)
        {
            NavigationService = navigationService;
            Logger = logger;

            NavigateBackCommand = new MvxCommand(() => navigationService.Close(this));

            _cancellationTokenSource = new CancellationTokenSource();
            // CommonExceptionInteractionLocal = new ExtendedInteraction<BaseInteractionResult>();
            // HumanReadableExceptionInteractionLocal = new ExtendedInteraction<BaseInteractionResult>();
        }

        protected IMvxNavigationService NavigationService { get; }

        public ILogger Logger { get; }

        protected IMvxTextProvider TextProvider { get; }

        public IMvxCommand NavigateBackCommand { get; }

        public PageStateType State
        {
            get => _state;
            protected set
            {
                if (!SetProperty(ref _state, value))
                {
                    return;
                }

                if (_state is PageStateType.Loading or PageStateType.Content)
                {
                    PageError = null;
                }
            }
        }

        public string PageError
        {
            get => _pageError;
            set => SetProperty(ref _pageError, value);
        }

        public CancellationToken CancellationToken => _cancellationTokenSource?.Token ?? CancellationToken.None;

        // public IExtendedInteraction<BaseInteractionResult> CommonExceptionInteraction => CommonExceptionInteractionLocal;
        //
        // public IExtendedInteraction<BaseInteractionResult> HumanReadableExceptionInteraction => HumanReadableExceptionInteractionLocal;
        //
        // protected ExtendedInteraction<BaseInteractionResult> CommonExceptionInteractionLocal { get; }
        //
        // protected ExtendedInteraction<BaseInteractionResult> HumanReadableExceptionInteractionLocal { get; }

        public virtual void OnHardwareBackPressed()
        {
            NavigationService.Close(this);
        }

        public override void ViewDestroy(bool viewFinishing = true)
        {
            base.ViewDestroy(viewFinishing);

            _cancellationTokenSource?.Cancel();
        }

        protected async Task PerformAsync(ViewModelHandledAction action)
        {
            try
            {
                await action.Action();
            }
            catch (Exception ex) when (action.ExceptionHandlers.ContainsKey(ex.GetType()))
            {
                Logger.LogError(ex, ex.Message);

                var handler = action.ExceptionHandlers[ex.GetType()];

                await handler.Invoke(ex);
            }
            catch (HumanReadableException ex)
            {
                Logger.LogError(ex, ex.HumanReadableMessage);

                var error = ex.HumanReadableMessage;

                if (action.IsWithError)
                {
                    PageError = error;
                    State = PageStateType.Error;
                }

                // if (action.IsWithInteraction)
                // {
                //     HumanReadableExceptionInteractionLocal.Raise(new BaseInteractionResult(false) { ErrorMessage = error });
                // }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);

                var error = action.CommonErrorMessage ?? TextProvider.GetText(string.Empty, "Common", "UnknownError");

                if (action.IsWithError)
                {
                    PageError = error;
                    State = PageStateType.Error;
                }

                // if (action.IsWithInteraction)
                // {
                //     CommonExceptionInteractionLocal.Raise(new BaseInteractionResult(false) { ErrorMessage = error });
                // }
            }
            finally
            {
                if (!action.IsWithError)
                {
                    State = PageStateType.Content;
                }
            }
        }
    }

    public abstract class PageViewModel<TParameter> : PageViewModel, IMvxViewModel<TParameter>
        where TParameter : class
    {
        protected PageViewModel(IMvxNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        {
        }

        public abstract void Prepare(TParameter parameter);
    }

    public abstract class PageViewModel<TParameter, TResult> : PageViewModel<TParameter>, IMvxViewModel<TParameter, TResult>
        where TParameter : class
        where TResult : class
    {
        protected PageViewModel(IMvxNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        {
        }

        public TaskCompletionSource<object> CloseCompletionSource { get; set; }

        public new abstract IMvxCommand NavigateBackCommand { get; }

        public new abstract void OnHardwareBackPressed();
    }
}
