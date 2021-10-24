namespace LeadersOfDigital.Definitions
{
    public class ViewModelResult
    {
        public ViewModelResult(bool closeRequested = false)
        {
            CloseRequested = closeRequested;
        }

        public bool CloseRequested { get; }
    }
}
