using System.Collections.Generic;
using System.IO;

namespace Silverbear.Enterprise.Audit.Domain
{
    public interface IAudit
    {
        AuditObject Get(string Id);

        int Create(Stream NewAuditRecord);

        List<AuditObject> Search(string OrganisationId, string Tags, string StringAttributes, string DateFrom,
                                 string DateTo, string PageSize, string Offset, string ShowDeleted);
    }
}