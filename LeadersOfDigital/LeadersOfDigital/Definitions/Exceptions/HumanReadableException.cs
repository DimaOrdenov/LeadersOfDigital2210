using System;

namespace LeadersOfDigital.Definitions.Exceptions
{
    public class HumanReadableException : Exception
    {
        public HumanReadableException(string humanReadableMessage)
            : base(humanReadableMessage)
        {
            HumanReadableMessage = humanReadableMessage;
        }

        public HumanReadableException(string humanReadableMessage, Exception innerException)
            : base(humanReadableMessage, innerException)
        {
            HumanReadableMessage = humanReadableMessage;
        }

        public string HumanReadableMessage { get; }
    }
}
