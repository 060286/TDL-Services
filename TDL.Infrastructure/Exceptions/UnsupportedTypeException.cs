using System;
using System.Runtime.Serialization;

namespace TDL.Infrastructure.Exceptions
{
    [Serializable]
    public class UnsupportedTypeException : Exception
    {
        protected UnsupportedTypeException(SerializationInfo info, StreamingContext context) : base(info, context) 
        {
        }

        public UnsupportedTypeException()
        {
        }

        public UnsupportedTypeException(string message) : base (message)
        {
        }

        public UnsupportedTypeException(string message, Exception ex) : base(message, ex) { } 
    }
}
