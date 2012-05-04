using System;
using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.BusinessRules
{
    public class InvalidLevelException : Exception, ISerializable
    {
        public InvalidLevelException() { }

        public InvalidLevelException(string message) { }

        public InvalidLevelException(string message, Exception inner) { }

        public InvalidLevelException(SerializationInfo info, StreamingContext ctx) { }
    }
}