using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Silverbear.Enterprise.Audit.BusinessRules;
using Silverbear.Enterprise.Audit.Domain;

namespace Silverbear.Enterprise.Audit
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuditService : IAudit
    {
        /// <summary>
        /// Create a new Audit record
        /// </summary>
        /// <param name="NewAuditRecord">Serialised audit record to be added</param>
        [WebInvoke(UriTemplate = "", Method = "POST")]
        public int Create(Stream NewAuditRecord)
        {

            var ser = new DataContractSerializer(typeof(AuditObject));
            var audit = (AuditObject)ser.ReadObject(NewAuditRecord);

            string bla = "this is a test";

            try
            {
                return AuditBr.Add(audit);
            }
            catch (InvalidLevelException ex)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("ErrorMessage", ex.Message);
                throw;
            }
            catch (InvalidOrganisationException ex)
            {
                if (WebOperationContext.Current != null)
                    WebOperationContext.Current.OutgoingResponse.Headers.Add("Error Message", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Function to (soft) delete an audit record from the system
        /// </summary>
        /// <param name="Id">Id of the audit record to be deleted</param>
        [WebInvoke(UriTemplate = "{Id}", Method = "DELETE")]
        public void Delete(string Id)
        {
            int id;

            if (int.TryParse(Id, out id) && id > 0)
                AuditBr.Delete(id);
        }

        [WebGet(UriTemplate = "{Id}")]
        public AuditObject Get(string Id)
        {
            int id;
            int.TryParse(Id, out id);
            return AuditBr.Get(id);
        }

        /// <summary>
        /// Search the Audit database for recording matching the criteria
        /// passed.  All criteria is logically ANDed
        /// </summary>
        /// <param name="OrganisationId">Integer Id of the organisation that
        /// the audit records you are searching for are associated to.</param>
        /// <param name="Tags">+ separated list of tags</param>
        /// <param name="StringAttributes">+ separated string attributes search</param>
        /// <param name="DateFrom">Date From to search for.</param>
        /// <param name="DateTo">Date to to be searched for</param>
        /// <param name="PageSize">The amount of records to return</param>
        /// <param name="Offset">The amount of records already seen</param>
        /// <returns>Collection of matching Audit objects</returns>
        [WebGet(UriTemplate = "?organisationid={OrganisationId}&tags={Tags}&attribs={StringAttributes}&datefrom={DateFrom}&dateto={DateTo}&pagesize={PageSize}&offset={Offset}&showdeleted={ShowDeleted}")]
        public List<AuditObject> Search(string OrganisationId, string Tags, string StringAttributes, string DateFrom, string DateTo, string PageSize, string Offset, string ShowDeleted)
        {
            IEnumerable<string> tags = new string[0];
            var attributes = new Dictionary<string, string>();
            int organisationId;
            bool showDeleted = false;

            bool.TryParse(ShowDeleted, out showDeleted);

            if (int.TryParse(OrganisationId, out organisationId))
            {
                if (!string.IsNullOrEmpty(Tags))
                    ParseTags(Tags, out tags);

                int pageSize;
                int.TryParse(PageSize, out pageSize);
                int offset;
                int.TryParse(Offset, out offset);

                if (!string.IsNullOrEmpty(StringAttributes))
                    ParseAttributes(StringAttributes, out attributes);

                DateTime dateFrom;
                DateTime.TryParse(DateFrom, out dateFrom);
                DateTime dateTo;
                DateTime.TryParse(DateTo, out dateTo);

                Func<DateTime, DateTime?> action =
                    (dateTime) => dateTime == DateTime.MinValue ? null : (DateTime?)dateTime;

                return AuditBr.Get(tags, attributes, action(dateFrom), action(dateTo), organisationId, pageSize, offset, showDeleted);
            }
            return new List<AuditObject>();
        }

        private static void ParseTags(string Str, out IEnumerable<string> Tags)
        {
            IEnumerable<string> tags = Str.Split('|');
            Tags = tags;
            return;
        }

        private static void ParseAttributes(string Str, out Dictionary<string, string> Attributes)
        {
            var attributes = new Dictionary<string, string>();
            IEnumerable<string> attribSplit = Str.Split('|');

            foreach (var stringVal in attribSplit)
            {
                int splitPos = stringVal.IndexOf("^");

                if (splitPos >= 0)
                {
                    string key = stringVal.Substring(0, splitPos);
                    string value = stringVal.Substring(splitPos + 1);

                    if (!attributes.ContainsKey(key))
                    {
                        attributes.Add(key, value);
                    }
                }
            }
            Attributes = attributes;
            return;
        }
    }
}