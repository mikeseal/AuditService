using System;
using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.BusinessRules
{
    public class InvalidOrganisationException : Exception, ISerializable
    {
        public InvalidOrganisationException() { }

        public InvalidOrganisationException(string message) { }

        public InvalidOrganisationException(string message, Exception inner) { }

        public InvalidOrganisationException(SerializationInfo info, StreamingContext ctx) { }
    }
}