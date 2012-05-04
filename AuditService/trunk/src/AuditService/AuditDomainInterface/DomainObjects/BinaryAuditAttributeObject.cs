using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.Domain
{
    [DataContract]
    public class BinaryAuditAttributeObject : GenericAuditAttributeObject<byte[]>
    {
    }
}