using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Lua;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;
using SmaugCS.Managers;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectRepository : Repository<long, ObjectTemplate>
    {
        private ObjectTemplate LastObject { get; set; }

        [LuaFunction("LProcessObject", "Processes an object script", "script text")]
        public static ObjectTemplate LuaProcessObject(string text)
        {
            LuaManager.Instance.Proxy.DoString(text);
            return DatabaseManager.Instance.OBJECT_INDEXES.LastObject;
        }

        [LuaFunction("LCreateObject", "Creates a new object", "Id of the Object", "Name of the Object")]
        public static ObjectTemplate LuaCreateObject(string id, string name)
        {
            long objId = Convert.ToInt64(id);
            if (DatabaseManager.Instance.OBJECT_INDEXES.Contains(objId))
                throw new DuplicateEntryException("Repository contains Object with Id {0}", objId);

            ObjectTemplate newObj = DatabaseManager.Instance.OBJECT_INDEXES.Create(objId, name);
            LuaManager.Instance.Proxy.CreateTable("object");
            return newObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectTemplate Create(long vnum, string name)
        {
            Validation.Validate(vnum >= 1 && !name.IsNullOrWhitespace());
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });


            ObjectTemplate newObject = new ObjectTemplate(vnum, name)
            {
                ShortDescription = string.Format("A newly created {0}", name),
                Description = string.Format("Somebody dropped a newly created {0} here.", name),
                Type = ItemTypes.Trash,
                Weight = 1,
            };
            newObject.ExtraFlags.SetBit((int)ItemExtraFlags.Prototype);

            Add(vnum, newObject);
            LastObject = newObject;
            return newObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="cvnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
            {
                newObject.ShortDescription = cloneObject.ShortDescription;
                newObject.LongDescription = cloneObject.LongDescription;
                newObject.Description = cloneObject.Description;
                newObject.Action = cloneObject.Action;
                newObject.Type = cloneObject.Type;
                newObject.ExtraFlags = new ExtendedBitvector(cloneObject.ExtraFlags);
                newObject.Flags = cloneObject.Flags;
                newObject.WearFlags = cloneObject.WearFlags;
                Array.Copy(cloneObject.Value, newObject.Value, cloneObject.Value.Length);
                newObject.Weight = cloneObject.Weight;
                newObject.Cost = cloneObject.Cost;
                newObject.ExtraDescriptions = new List<ExtraDescriptionData>(cloneObject.ExtraDescriptions);
                newObject.Affects = new List<AffectData>(cloneObject.Affects);
            }
            return newObject;
        }
    }
}
