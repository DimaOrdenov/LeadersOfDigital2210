using LeadersOfDigital.Definitions.Attributes;

namespace LeadersOfDigital.ViewModels.Map
{
    public enum PlaceType
    {
        [Image("ic_pin_underline")]
        Default,

        [Image("ic_airplane_in_flight")]
        Airport,
        
        [Image("ic_train")]
        Train,
    }
}
