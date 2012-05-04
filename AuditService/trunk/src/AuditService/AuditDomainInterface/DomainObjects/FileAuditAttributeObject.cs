using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.Domain
{
    [DataContract]
    public class FileAuditAttributeObject : BinaryAuditAttributeObject
    {
        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}