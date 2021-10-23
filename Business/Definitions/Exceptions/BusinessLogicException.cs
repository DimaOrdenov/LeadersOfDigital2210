using System;
using Business.Definitions.Types;

namespace Business.Definitions.Exceptions
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(BusinessExceptionType type)
            : base(type.ToString())
        {
            Type = type;
        }

        public BusinessLogicException(BusinessExceptionType type, string message)
            : base(message)
        {
            Type = type;
        }

        public BusinessLogicException(BusinessExceptionType type, Exception exception)
            : base(type.ToString(), exception)
        {
            Type = type;
        }

        public BusinessLogicException(BusinessExceptionType type, string message, Exception exception)
            : base(message, exception)
        {
            Type = type;
        }

        public BusinessExceptionType Type { get; }
    }
}
