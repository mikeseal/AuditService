using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Silverbear.Enterprise.Audit.Domain
{
    [DataContract]
    public class AuditObject
    {
        [DataMember]
        public DateTime DateCreated { get; set; }

        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public List<string> Tags { get; set; }

        [DataMember]
        public Dictionary<string, AuditAttributeObject> AuditAttributes { get; set; }

        [DataMember]
        public OrganisationObject Organisation { get; set; }

        [DataMember]
        public string Application;

        public AuditObject()
        {
            Tags = new List<string>();
            AuditAttributes = new Dictionary<string, AuditAttributeObject>();
        }
    }
}