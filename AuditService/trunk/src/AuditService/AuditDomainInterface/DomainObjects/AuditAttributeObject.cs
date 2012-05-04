using System;
using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.Domain
{
    [DataContract]
    [KnownType(typeof(BinaryAuditAttributeObject))]
    [KnownType(typeof(FileAuditAttributeObject))]
    [KnownType(typeof(StringAuditAttributeObject))]
    public abstract class AuditAttributeObject
    {
        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public DateTime? DateUpdated { get; set; }

        [DataMember]
        public DateTime? DateDeleted { get; set; }
    }
}