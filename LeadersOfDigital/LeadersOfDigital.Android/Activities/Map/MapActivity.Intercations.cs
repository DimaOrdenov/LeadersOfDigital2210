using Definitions.Interactions;
using LeadersOfDigital.Android.Helpers;

namespace LeadersOfDigital.Android.Activities.Map
{
    public partial class MapActivity
    {
        private IExtendedInteraction<BaseInteractionResult> _commonExceptionInteraction;
        private IExtendedInteraction<BaseInteractionResult> _humanReadableExceptionInteraction;

        public IExtendedInteraction<BaseInteractionResult> CommonExceptionInteraction
        {
            get => _commonExceptionInteraction;
            set => this.SetInteraction(ref _commonExceptionInteraction, value);
        }

        public IExtendedInteraction<BaseInteractionResult> HumanReadableExceptionInteraction
        {
            get => _humanReadableExceptionInteraction;
            set => this.SetInteraction(ref _humanReadableExceptionInteraction, value);
        }
    }
}
