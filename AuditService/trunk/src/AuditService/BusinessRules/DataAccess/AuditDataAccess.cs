using System;
using System.Collections.Generic;
using System.Linq;
using Silverbear.Enterprise.Audit.Domain;

namespace Silverbear.Enterprise.Audit.BusinessRules.DataAccess
{
    public class AuditDataAccess
    {
        #region AuditRecord Object Converter

        private static List<Audit> Convert(IEnumerable<AuditObject> AuditObjects, AuditEntities Ctx)
        {
            return AuditObjects.Select(C => Convert(C, Ctx)).ToList();
        }

        private static List<AuditObject> Convert(IEnumerable<Audit> Audits)
        {
            return Audits.Select(Convert).ToList();
        }

        private static AuditObject Convert(Audit AuditRecord)
        {
            var auditObject = new AuditObject
                                  {
                                      DateCreated = AuditRecord.DateCreated,
                                      Level = AuditRecord.AuditLevel.Id,
                                      Application = AuditRecord.Application
                                  };

            auditObject = ConvertFileAttributes(AuditRecord, auditObject);
            auditObject = ConvertTagAttributes(AuditRecord, auditObject);
            auditObject = ConvertBinaryAttributes(AuditRecord, auditObject);
            auditObject = ConvertStringAttributes(AuditRecord, auditObject);
            auditObject = ConvertOrganisation(AuditRecord, auditObject);
            return auditObject;
        }

        private static Audit Convert(AuditObject AuditRecord, AuditEntities Ctx)
        {
            var audit = new Audit
                            {
                                DateCreated = AuditRecord.DateCreated,
                                Application = string.IsNullOrEmpty(AuditRecord.Application) ? "UNKNOWN" : AuditRecord.Application
                            };

            var level = GetLevel(AuditRecord.Level, Ctx);
            if (level != null)
            {
                audit.AuditLevel = level;
            }
            else
            {
                throw new InvalidLevelException(string.Format("Audit Level {0} does not exist", AuditRecord.Level));
            }

            var organisation = GetOrganisation(AuditRecord.Organisation.Id, Ctx);
            if (organisation != null)
            {
                audit.Organisation = organisation;
            }
            else
            {
                throw new InvalidOrganisationException(string.Format("Organisation {0} does not exist", AuditRecord.Organisation.Id));
            }

            audit = ConvertFileAttributes(AuditRecord, audit);
            audit = ConvertStringAttributes(AuditRecord, audit);
            audit = ConvertBinaryAttributes(AuditRecord, audit);
            audit = ConvertTagAttributes(AuditRecord, audit, Ctx);
            return audit;
        }

        #endregion AuditRecord Object Converter

        #region FileAttribute Converters

        /// <summary>
        /// Function to convert DataAccess File Attributes to FileAttribute
        /// objects and return the Domain AuditBR Object with the files attached.
        /// </summary>
        /// <param name="AuditRecord">DataAccess AuditBR object that will have the
        /// fileauditattributes to be converted</param>
        /// <param name="AuditDomainObject">The returning Auditobject with the Files attached</param>
        /// <returns>The returning Auditobject with the Files attached</returns>
        public static AuditObject ConvertFileAttributes(Audit AuditRecord, AuditObject AuditDomainObject)
        {
            foreach (var fileAttribute in AuditRecord.Attribute.OfType<FileAttribute>())
            {
                var fileAttributeObject = new FileAuditAttributeObject
                                              {
                                                  DateCreated = fileAttribute.DateCreated,
                                                  DateUpdated = fileAttribute.DateUpdated,
                                                  DateDeleted = fileAttribute.DateDeleted,
                                                  Name = fileAttribute.FileName,
                                                  Extension = fileAttribute.Extension,
                                                  Value = fileAttribute.Value
                                              };
                AuditDomainObject.AuditAttributes.Add(fileAttribute.Key, fileAttributeObject);
            }
            return AuditDomainObject;
        }

        /// <summary>
        /// Domain to data access converter for FileAttributes
        /// </summary>
        /// <param name="AuditDomainObject"></param>
        /// <param name="AuditRecord"></param>
        /// <returns></returns>
        public static Audit ConvertFileAttributes(AuditObject AuditDomainObject, Audit AuditRecord)
        {
            var recordsToDelete = new List<string>();
            foreach (var attachmentObject in AuditDomainObject.AuditAttributes.Where(P => P.Value is FileAuditAttributeObject))
            {
                var attachment = attachmentObject.Value as FileAuditAttributeObject;

                if (attachment != null)
                {
                    var fileAttribute = new FileAttribute
                                            {
                                                Key = attachmentObject.Key,
                                                DateCreated =
                                                    attachment.DateCreated == DateTime.MinValue
                                                        ? DateTime.Now
                                                        : attachment.DateCreated,
                                                DateDeleted = attachment.DateDeleted,
                                                DateUpdated = attachment.DateUpdated,
                                                FileName = attachment.Name,
                                                Extension = attachment.Extension,
                                                Value = attachment.Value
                                            };
                    AuditRecord.Attribute.Add(fileAttribute);
                }
                recordsToDelete.Add(attachmentObject.Key);
            }
            recordsToDelete.ForEach(I => AuditDomainObject.AuditAttributes.Remove(I));
            return AuditRecord;
        }

        #endregion FileAttribute Converters

        #region StringAttributeConverters

        public static AuditObject ConvertStringAttributes(Entity AuditRecord, AuditObject AuditDomainObject)
        {
            foreach (var stringAttribute in AuditRecord.Attribute.OfType<StringAttribute>())
            {
                AuditDomainObject.AuditAttributes.Add(stringAttribute.Key,
                                                      new StringAuditAttributeObject
                                                          {
                                                              Value = stringAttribute.Value,
                                                              DateCreated = stringAttribute.DateCreated,
                                                              DateUpdated = stringAttribute.DateUpdated,
                                                              DateDeleted = stringAttribute.DateDeleted,
                                                          });
            }
            return AuditDomainObject;
        }

        /// <summary>
        /// Domain to data access converter for FileAttributes
        /// </summary>
        /// <param name="AuditDomainObject"></param>
        /// <param name="AuditRecord"></param>
        /// <returns></returns>
        public static Audit ConvertStringAttributes(AuditObject AuditDomainObject, Audit AuditRecord)
        {
            foreach (var stringObject in AuditDomainObject.AuditAttributes.Where(P => P.Value is StringAuditAttributeObject))
            {
                var attribute = stringObject.Value as StringAuditAttributeObject;

                if (attribute != null)
                {
                    var stringAttribute = new StringAttribute
                                              {
                                                  Key = stringObject.Key,
                                                  Value = attribute.Value,
                                                  DateCreated = attribute.DateCreated == DateTime.MinValue
                                                                      ? DateTime.Now
                                                                      : attribute.DateCreated
                                              };
                    AuditRecord.Attribute.Add(stringAttribute);
                }
            }
            return AuditRecord;
        }

        #endregion StringAttributeConverters

        #region BinaryAttribute Converters

        public static AuditObject ConvertBinaryAttributes(Audit AuditRecord, AuditObject AuditDomainObject)
        {
            foreach (var binaryAuditAttribute in AuditRecord.Attribute.OfType<BinaryAttribute>())
            {
                if (!AuditDomainObject.AuditAttributes.Keys.Contains(binaryAuditAttribute.Key))
                    AuditDomainObject.AuditAttributes.Add(binaryAuditAttribute.Key,
                                                          new BinaryAuditAttributeObject { Value = binaryAuditAttribute.Value });
            }
            return AuditDomainObject;
        }

        public static Audit ConvertBinaryAttributes(AuditObject AuditDomainObject, Audit AuditRecord)
        {
            foreach (var binaryObject in AuditDomainObject.AuditAttributes.Where(P => P.Value is BinaryAuditAttributeObject))
            {
                var attribute = binaryObject.Value as BinaryAuditAttributeObject;

                if (attribute != null)
                {
                    var binaryAttribute = new BinaryAttribute
                                              {
                                                  Key = binaryObject.Key,
                                                  Value = attribute.Value,
                                                  DateCreated = attribute.DateCreated == DateTime.MinValue ? DateTime.Now : attribute.DateCreated,
                                                  DateUpdated = attribute.DateUpdated,
                                                  DateDeleted = attribute.DateDeleted,
                                              };
                    AuditRecord.Attribute.Add(binaryAttribute);
                }
            }
            return AuditRecord;
        }

        #endregion BinaryAttribute Converters

        #region Tag Converters

        /// <summary>
        /// Function to take the tags on an Audit data record and apply them
        /// to the Audit domain object.
        /// </summary>
        /// <param name="AuditRecord">Data record with the tags associated to it.</param>
        /// <param name="AuditDomainObject">Audit domain object that will have the tags associated to.</param>
        /// <returns>returns a copy of the Audit Domain object with the tags associated.</returns>
        public static AuditObject ConvertTagAttributes(Entity AuditRecord, AuditObject AuditDomainObject)
        {
            foreach (var tagAuditAttribute in AuditRecord.Tag)
            {
                AuditDomainObject.Tags.Add(tagAuditAttribute.TagName);
            }
            return AuditDomainObject;
        }

        /// <summary>
        /// Convert the Tags from domain object to data object.  If the tag
        /// already exists in the database the tag will be attached to the
        /// existing record stopping duplicates being created.
        /// </summary>
        /// <param name="AuditDomainObject">Domain object with any tags associated.</param>
        /// <param name="AuditRecord">Audit data record where the tags need to
        /// be attached if they already exist and added if they do not.</param>
        /// <param name="Ctx">Context to manage the tags being attached or added.</param>
        /// <returns></returns>
        public static Audit ConvertTagAttributes(AuditObject AuditDomainObject, Audit AuditRecord, AuditEntities Ctx)
        {
            foreach (var tagObject in AuditDomainObject.Tags)
            {
                Tag tag = GetTag(tagObject, Ctx);

                if (tag != null)
                    AuditRecord.Tag.Add(tag);
                else
                {
                    var tagAttribute = new Tag
                                           {
                                               TagName = tagObject,
                                               DateCreated = DateTime.Now
                                           };
                    AuditRecord.Tag.Add(tagAttribute);
                }
            }
            return AuditRecord;
        }

        #endregion Tag Converters

        #region Organisaton Converters

        /// <summary>
        /// function that applies the Organisation to the Audit data object.
        /// The Organisation must already exist in the system.
        /// </summary>
        /// <param name="AuditDomainObject">Audit Domain object that has the associated Organisation</param>
        /// <param name="AuditRecord">Audit data object that will have the organisation associated to.</param>
        /// <returns>returns the Audit data object with the Organisation associated.</returns>
        public static Entity ConvertOrganisation(AuditObject AuditDomainObject, Entity AuditRecord)
        {
            if (AuditRecord.Organisation == null)
                AuditRecord.Organisation = new Organisation();

            AuditRecord.Organisation.Id = AuditDomainObject.Organisation.Id;
            AuditRecord.Organisation.OrganisationName = AuditDomainObject.Organisation.Name;

            return AuditRecord;
        }

        /// <summary>
        /// Function that applies the Organisation to the Audit domain object.
        /// </summary>
        /// <param name="AuditRecord">Audit data object that has the
        /// organisation associated.</param>
        /// <param name="AuditDomainObject">Audit Domain object that will have
        /// the organisation associated to it.</param>
        /// <returns>Returns the audit domain object with the organisation
        /// associated.</returns>
        public static AuditObject ConvertOrganisation(Entity AuditRecord, AuditObject AuditDomainObject)
        {
            if (AuditDomainObject.Organisation == null)
                AuditDomainObject.Organisation = new OrganisationObject();
            AuditDomainObject.Organisation.Id = AuditRecord.Organisation.Id;
            AuditDomainObject.Organisation.Name = AuditRecord.Organisation.OrganisationName;
            return AuditDomainObject;
        }

        #endregion Organisaton Converters

        #region CRUD Functionality

        /// <summary>
        /// Function the creates a new audit record.
        /// </summary>
        /// <param name="AuditRecord">Audit Domain object populated that needs
        /// saving away.</param>
        public static int Add(AuditObject AuditRecord)
        {
            using (var ctx = new AuditEntities())
            {
                Entity newRecord = Convert(AuditRecord, ctx);
                // Add the new AuditRecord record
                ctx.Entity.AddObject(newRecord);
                ctx.SaveChanges();
                return newRecord.Id;
            }
        }

        public static void Delete(int Id)
        {
            using (var ctx = new AuditEntities())
            {
                var results = from a in ctx.Entity.OfType<Audit>()
                              where a.Id == Id
                              select a;

                var recordToDelete = results.FirstOrDefault();

                if (recordToDelete != null)
                {
                    recordToDelete.DateDeleted = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Get an Audit record based on the unique Id of the record.
        /// </summary>
        /// <param name="Id">Id of the Audit record to be retrieved.</param>
        /// <returns>The Audit domain object based on the Id passed in.  if no
        /// record is found the function will return null.</returns>
        public static AuditObject Get(int Id)
        {
            using (var ctx = new AuditEntities())
            {
                var results = from a in ctx.Entity.OfType<Audit>()
                              where a.Id == Id
                              select a;

                return results.FirstOrDefault() != null ? Convert(results.FirstOrDefault()) : null;
            }
        }

        /// <summary>
        /// Suspect this function is NOT USED !!!!!!
        /// </summary>
        /// <param name="SearchCriteria"></param>
        /// <returns></returns>
        public static List<AuditObject> Get(AuditObject SearchCriteria)
        {
            using (var ctx = new AuditEntities())
            {
                var records = from a in ctx.Entity.OfType<Audit>() select a;

                records = SearchCriteria.Tags.Aggregate(records, (Current, Record) => Current.Where(T => T.Tag.Any(R => R.TagName == Record)));
                return Convert(records.ToList());
            }
        }

        /// <summary>
        /// Search function to retrieve Audit records based on Tags, String
        /// Attributes and Date range.  All parameters are optional.
        /// </summary>
        /// <param name="Tags">Tag list where list is separated by '|'</param>
        /// <param name="StrAttribs">string key value list. Each pair is
        /// separated by '|'.  The key value pair is separated by '^'</param>
        /// <param name="DateFrom">Date From that the search should be associated to.</param>
        /// <param name="DateTo">Date To that the search should be associated to.</param>
        /// <param name="OrganisationId">Id of the organisation that the records are associated to.</param>
        /// <param name="Offset">The amount of records already been viewed by the client</param>
        /// <param name="ShowDeleted">Boolean to define if deleted records get returned.</param>
        /// <param name="PageSize">The amount of records to record</param>
        /// <returns>A list of Audit Domain objects that the search matches.</returns>
        public static List<AuditObject> Get(IEnumerable<string> Tags,
            Dictionary<string, string> StrAttribs, DateTime? DateFrom,
            DateTime? DateTo, int OrganisationId, int PageSize, int Offset, bool ShowDeleted)
        {
            using (var ctx = new AuditEntities())
            {
                var records = from a in ctx.Entity.OfType<Audit>()
                              .Include("Attribute")
                              select a;

                if (ShowDeleted == false)
                {
                    records = records.Where(R => R.DateDeleted == null);
                }

                if (DateFrom.HasValue)
                {
                    records = records.Where(R => R.DateCreated >= DateFrom);
                }

                if (DateTo.HasValue)
                {
                    records = records.Where(R => R.DateCreated <= DateTo);
                }

                records = records.Where(O => O.Organisation.Id == OrganisationId);
                records = records.OrderByDescending(O => O.DateCreated);

                records = Tags.Aggregate(records,
                                         (Current, Tag) => Current.Where(T => T.Tag.Any(Tn => Tn.TagName == Tag)));
                records = StrAttribs.Aggregate(records,
                                               (Current, StrAttrib) =>
                                               Current.Where(
                                                   S =>
                                                   S.Attribute.OfType<StringAttribute>().Any(
                                                       Sv => Sv.Value == StrAttrib.Value && Sv.Key == StrAttrib.Key)));
                // Sort the paging
                if (PageSize > 0 || Offset > 0)
                {
                    records = records.Skip(Offset);
                    records = records.Take(PageSize);
                }
                return Convert(records.ToList());
            }
        }

        #endregion CRUD Functionality

        #region Private Functions

        /// <summary>
        /// function to find an existing Tag in the database with the same name.
        /// </summary>
        /// <param name="Name">String description of the tag to be located.</param>
        /// <param name="Ctx">Context being used to search for the tag.</param>
        /// <returns>Returns a Tag object if one is located.  If no tag is
        /// located by name the function will return null.</returns>
        private static Tag GetTag(string Name, AuditEntities Ctx)
        {
            var results = from t in Ctx.Tag
                          where t.TagName == Name
                          select t;
            return results.FirstOrDefault();
        }

        /// <summary>
        /// function to find an existing Tag in the database with the same Id.
        /// </summary>
        /// <param name="Id">Id of the audit Level you are trying to locate.</param>
        /// <param name="Ctx">Context being used to search for the tag.</param>
        /// <returns>Returns an AuditLevel if one is located.  If no AuditLevel is
        /// located by name the function will return null.</returns>
        private static AuditLevel GetLevel(int Id, AuditEntities Ctx)
        {
            var results = from l in Ctx.AuditLevel
                          where l.Id == Id
                          select l;
            return results.FirstOrDefault();
        }

        /// <summary>
        /// function to find an existing Organisation in the database with the same Id.
        /// </summary>
        /// <param name="Id">Id of the Organisation you are trying to locate.</param>
        /// <param name="Ctx">Context being used to search for the organisation.</param>
        /// <returns>Returns an Organisation if one is located.  If no Organisation is
        /// located by name the function will return null.</returns>
        private static Organisation GetOrganisation(int Id, AuditEntities Ctx)
        {
            var results = from o in Ctx.Organisation
                          where o.Id == Id
                          select o;
            return results.FirstOrDefault();
        }

        #endregion Private Functions
    }
}