namespace Definitions.Interactions
{
    public class BaseInteractionResult
    {
        public BaseInteractionResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }

        public string ErrorMessage { get; set; }
        
        public string SuccessMessage { get; set; }
    }
}
