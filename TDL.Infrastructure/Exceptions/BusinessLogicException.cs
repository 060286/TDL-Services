using System;
using System.Runtime.Serialization;

namespace TDL.Infrastructure.Exceptions
{
    [Serializable]
    public class BusinessLogicException : Exception
    {
        protected BusinessLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public BusinessLogicException()
        {
        }

        public BusinessLogicException(string message) : base(message)
        {
        } 

        public BusinessLogicException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}
