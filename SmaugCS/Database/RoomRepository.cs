using System;
using Realm.Library.Patterns.Repository;
using SmaugCS.Enums;
using SmaugCS.Exceptions;
using SmaugCS.Objects;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomRepository : Repository<int, RoomTemplate>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public RoomTemplate Create(int vnum, AreaData area)
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
