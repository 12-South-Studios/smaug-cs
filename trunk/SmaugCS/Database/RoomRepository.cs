using System;
using Realm.Library.Patterns.Repository;
using SmaugCS.Enums;
using SmaugCS.Exceptions;
using SmaugCS.Objects;

namespace SmaugCS.Database
{
    public class RoomRepository : Repository<int, RoomTemplate>
    {
        public RoomTemplate make_room(int vnum, AreaData area)
        {
            if (area == null || vnum < 1)
                throw new Exception("Invalid data");
            if (Contains(vnum))
                throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);

            RoomTemplate newRoom = new RoomTemplate
                                       {
                                           Name = "Floating in a void",
                                           SectorType = SectorTypes.Inside,
                                           Area = area,
                                           Vnum = vnum
                                       };
            newRoom.Flags.SetBit((int)RoomFlags.Prototype);

            Add(vnum, newRoom);
            area.Rooms.Add(newRoom);
            return newRoom;
        }
    }
}
