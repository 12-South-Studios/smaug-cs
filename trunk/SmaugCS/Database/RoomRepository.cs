using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Templates;
using SmaugCS.Exceptions;

namespace SmaugCS.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomRepository : Repository<long, RoomTemplate>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public RoomTemplate Create(long vnum, AreaData area)
        {
            Validation.Validate(vnum >= 1);
            Validation.IsNotNull(area, "area");
            Validation.Validate(() =>
                {
                    if (Contains(vnum))
                        throw new DuplicateIndexException("Invalid vnum {0}, Index already exists", vnum);
                });

            RoomTemplate newRoom = new RoomTemplate(vnum, "Floating in a void")
                                       {
                                           SectorType = SectorTypes.Inside,
                                           Area = area
                                       };
            newRoom.Flags.SetBit((int)RoomFlags.Prototype);

            Add(vnum, newRoom);
            area.Rooms.Add(newRoom);
            return newRoom;
        }
    }
}
