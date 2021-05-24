using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Standard.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using System.IO;
using System.Linq;

namespace SmaugCS.Repository
{
    public class ObjectRepository : Repository<long, ObjectTemplate>, ITemplateRepository<ObjectTemplate>
    {
        private ObjectTemplate LastObject { get; set; }

        public ObjectTemplate Create(long id, string name)
        {
            Validation.Validate(id >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(id))
                        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
                });


            var newObject = new ObjectTemplate(id, name);
            newObject.ExtraFlags.SetBit((int)ItemExtraFlags.Prototype);

            Add(id, newObject);
            LastObject = newObject;
            return newObject;
        }

        public ObjectTemplate Create(long id, long cloneId, string name)
        {
            Validation.Validate(cloneId >= 1 && cloneId != id && id >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(id))
                        throw new DuplicateIndexException("Invalid ID {0}, Index already exists", id);
                    if (!Contains(cloneId))
                        throw new InvalidDataException($"Clone ID {cloneId} is not present");
                });

            var newObject = Create(id, name);

            var cloneObject = Get(cloneId);
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
            newObject.Values = cloneObject.Values;
            newObject.Weight = cloneObject.Weight;
            newObject.Cost = cloneObject.Cost;
            newObject.ExtraDescriptions.Clear();
            newObject.ExtraDescriptions.ToList().AddRange(cloneObject.ExtraDescriptions);
            newObject.Affects.Clear();
            newObject.Affects.ToList().AddRange(cloneObject.Affects);
        }
    }
}
