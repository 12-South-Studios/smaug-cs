using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Realm.Library.Patterns.Repository;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Exceptions;
using SmaugCS.Objects;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectRepository : Repository<int, ObjectTemplate>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectTemplate Create(int vnum, string name)
        {
            if (vnum < 1 || name.IsNullOrWhitespace())
                throw new Exception("Invalid data");
            if (Contains(vnum))
                throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);


            ObjectTemplate newObject = new ObjectTemplate
            {
                Name = string.Format("A newly created {0}", name),
                Description = string.Format("Somebody dropped a newly created {0} here.", name),
                Type = ItemTypes.Trash,
                Weight = 1,
                Vnum = vnum
            };
            newObject.ExtraFlags.SetBit((int)ItemExtraFlags.Prototype);

            Add(vnum, newObject);
            return newObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="cvnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectTemplate Create(int vnum, int cvnum, string name)
        {
            if (cvnum < 1 || cvnum == vnum || vnum < 1 || name.IsNullOrWhitespace())
                throw new Exception("Invalid data");
            if (Contains(vnum))
                throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
            if (!Contains(cvnum))
                throw new InvalidDataException(string.Format("Clone vnum {0} is not present", cvnum));

            ObjectTemplate newObject = Create(vnum, name);
            ObjectTemplate cloneObject = Get(cvnum);
            if (cloneObject != null)
            {
                newObject.ShortDescription = cloneObject.ShortDescription;
                newObject.Description = cloneObject.Description;
                newObject.ActionDescription = cloneObject.ActionDescription;
                newObject.Type = cloneObject.Type;
                newObject.ExtraFlags = new ExtendedBitvector(cloneObject.ExtraFlags);
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
