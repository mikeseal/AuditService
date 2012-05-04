using System;
using System.Collections.Generic;
using Silverbear.Enterprise.Audit.BusinessRules.DataAccess;
using Silverbear.Enterprise.Audit.Domain;

namespace Silverbear.Enterprise.Audit.BusinessRules
{
    public class AuditBr
    {
        public static int Add(AuditObject AuditRecord)
        {
            return AuditDataAccess.Add(AuditRecord);
        }

        public static void Delete(int Id)
        {
            AuditDataAccess.Delete(Id);
        }

        public static AuditObject Get(int Id)
        {
            return AuditDataAccess.Get(Id);
        }

        public static List<AuditObject> Get(IEnumerable<string> Tags, Dictionary<string, string> StrAttribs, DateTime? DateFrom, DateTime? DateTo, int OrganisationId, int PageSize, int Offset, bool ShowDeleted)
        {
            return AuditDataAccess.Get(Tags, StrAttribs, DateFrom, DateTo, OrganisationId, PageSize, Offset, ShowDeleted);
        }

        public static List<AuditObject> Get(AuditObject SearchObject)
        {
            return AuditDataAccess.Get(SearchObject);
        }
    }
}