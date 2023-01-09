using System;
using System.Runtime.Serialization;

namespace TDL.Infrastructure.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public NotFoundException() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
