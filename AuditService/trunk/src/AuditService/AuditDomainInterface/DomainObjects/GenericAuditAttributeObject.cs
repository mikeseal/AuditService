using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.Domain
{
    [DataContract]
    public abstract class GenericAuditAttributeObject<T> : AuditAttributeObject
    {
        [DataMember]
        public T Value { get; set; }
    }
}