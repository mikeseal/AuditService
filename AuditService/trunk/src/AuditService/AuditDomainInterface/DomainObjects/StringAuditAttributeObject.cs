using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.Domain
{
    [DataContract]
    public class StringAuditAttributeObject : GenericAuditAttributeObject<string>
    {
    }
}