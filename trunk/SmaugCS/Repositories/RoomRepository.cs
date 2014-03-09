using System;
using Realm.Library.Common;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    public class RoomRepository : Repository<long, RoomTemplate>, ITemplateRepository<RoomTemplate>
    {
        private RoomTemplate LastRoom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="cvnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public RoomTemplate Create(long vnum, long cvnum, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vnum"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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
