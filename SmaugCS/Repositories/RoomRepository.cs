using System;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;

namespace SmaugCS.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomRepository : Repository<long, RoomTemplate>, ITemplateRepository<RoomTemplate>
    {
        private RoomTemplate LastRoom { get; set; }

        public RoomTemplate Create(long vnum, long cvnum, string name)
        {
            throw new NotImplementedException();
        }

        public RoomTemplate Create(long vnum, string name)
        {
            Validation.Validate(vnum >= 1);
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            RoomTemplate newRoom = new RoomTemplate(vnum, name.IsNullOrEmpty() ? "Floating in a Void" : name)
                                       {
                                           SectorType = SectorTypes.Inside,
                                       };
            newRoom.Flags.SetBit((int)RoomFlags.Prototype);

            Add(vnum, newRoom);
            return newRoom;
        }
    }
}
