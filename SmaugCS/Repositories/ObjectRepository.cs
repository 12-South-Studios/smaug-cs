using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Repositories
{
    public class ObjectRepository : Repository<long, ObjectTemplate>, ITemplateRepository<ObjectTemplate>
    {
        private ObjectTemplate LastObject { get; set; }

        public ObjectTemplate Create(long vnum, string name)
        {
            Validation.Validate(vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });


            ObjectTemplate newObject = new ObjectTemplate(vnum, name);
            newObject.ExtraFlags.SetBit(ItemExtraFlags.Prototype);

            Add(vnum, newObject);
            LastObject = newObject;
            return newObject;
        }

        public ObjectTemplate Create(long vnum, long cvnum, string name)
        {
            Validation.Validate(cvnum >= 1 && cvnum != vnum && vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                    if (!Contains(cvnum))
                        throw new InvalidDataException(string.Format("Clone vnum {0} is not present", cvnum));
                });

            ObjectTemplate newObject = Create(vnum, name);

            ObjectTemplate cloneObject = Get(cvnum);
            if (cloneObject != null)
                CloneObjectTemplate(newObject, cloneObject);

            return newObject;
        }

        private static void CloneObjectTemplate(ObjectTemplate newObject, ObjectTemplate cloneObject)
        {
            newObject.ShortDescription = cloneObject.ShortDescription;
            newObject.LongDescription = cloneObject.LongDescription;
            newObject.Description = cloneObject.Description;
            newObject.Action = cloneObject.Action;
            newObject.Type = cloneObject.Type;
            newObject.ExtraFlags = cloneObject.ExtraFlags;
            newObject.Flags = cloneObject.Flags;
            newObject.WearFlags = cloneObject.WearFlags;
            Array.Copy(cloneObject.Value, newObject.Value, cloneObject.Value.Length);
            newObject.Weight = cloneObject.Weight;
            newObject.Cost = cloneObject.Cost;
            newObject.ExtraDescriptions = new List<ExtraDescriptionData>(cloneObject.ExtraDescriptions);
            newObject.Affects = new List<AffectData>(cloneObject.Affects);
        }
    }
}
