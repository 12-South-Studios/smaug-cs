using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmaugCS
{
    public static class ResetExtensions
    {
        private static readonly Dictionary<ResetTypes, Action<ResetData, IRepositoryManager>> ProcessTable = new Dictionary
            <ResetTypes, Action<ResetData, IRepositoryManager>>
        {
            {ResetTypes.Mob, ProcessMobReset},
            {ResetTypes.Obj, ProcessObjectReset},
            {ResetTypes.Trap, ProcessTrapReset},
            {ResetTypes.Door, ProcessDoorReset},
            {ResetTypes.Rand, ProcessRandomizeReset}
        };

        public static void Process(this ResetData reset, IRepositoryManager repositoryManager = null)
        {
            var dbManager = repositoryManager ?? RepositoryManager.Instance;

            if (ProcessTable.ContainsKey(reset.Type))
                ProcessTable[reset.Type].Invoke(reset, dbManager);
        }

        private static void ProcessMobReset(ResetData reset, IRepositoryManager dbManager)
        {
            var mobTemplate = dbManager.MOBILETEMPLATES.Values.FirstOrDefault(x => x.ID == reset.Args.ToList()[0]);
            if (mobTemplate == null)
            {
                // todo bug
                return;
            }

            var room = dbManager.ROOMS.Get(reset.Args.ToList()[2]);
            if (room == null)
            {
                // todo bug
                return;
            }

            if (!reset.sreset) return;  // todo what does sreset mean?

            var mob = dbManager.CHARACTERS.Create(mobTemplate);

            // todo finish mob reset

            // todo sub-resets allowed are Equip/Give
            // todo sub-sub-resets allowed are Hidden, Put
        }

        private static void ProcessObjectReset(ResetData reset, IRepositoryManager dbManager)
        {

            // todo sub-resets allowed are Hidden, Trap, Put
        }

        private static void ProcessTrapReset(ResetData reset, IRepositoryManager dbManager)
        {
            if (reset.Extra.IsSet(TrapTriggerTypes.Object))
            {
                // todo object trap in room reset
                return;
            }

            var room = dbManager.ROOMS.Get(reset.Args.ToList()[2]);
            if (room == null)
            {
                // todo bug
                return;
            }

            // todo finish trap reset
            //    if (room->area->nplayer > 0
            //        || count_obj_list(get_obj_index(OBJ_VNUM_TRAP), pRoomIndex->first_content) > 0)
            //        break;
            //    to_obj = make_trap(pReset->arg1, pReset->arg1, 10, pReset->extra);
            //    obj_to_room(to_obj, pRoomIndex);
        }

        private static void ProcessDoorReset(ResetData reset, IRepositoryManager dbManager)
        {
            var room = dbManager.ROOMS.Get(reset.Args.ToList()[0]);

            var exit = room?.GetExit(reset.Args.ToList()[1]);
            if (exit == null)
            {
                // todo not found error
                return;
            }

            var resetStyle = reset.Args.ToList()[2];
            switch (resetStyle)
            {
                case 0:
                    exit.Flags = exit.Flags.RemoveBit(ExitFlags.Closed);
                    exit.Flags = exit.Flags.RemoveBit(ExitFlags.Locked);
                    break;
                case 1:
                    exit.Flags = exit.Flags.SetBit(ExitFlags.Closed);
                    exit.Flags = exit.Flags.RemoveBit(ExitFlags.Locked);
                    if (exit.Flags.IsSet(ExitFlags.xSearchable))
                        exit.Flags = exit.Flags.SetBit(ExitFlags.Secret);
                    break;
                case 2:
                    exit.Flags = exit.Flags.SetBit(ExitFlags.Closed);
                    exit.Flags = exit.Flags.SetBit(ExitFlags.Locked);
                    if (exit.Flags.IsSet(ExitFlags.xSearchable))
                        exit.Flags = exit.Flags.SetBit(ExitFlags.Secret);
                    break;
            }
        }

        private static void ProcessRandomizeReset(ResetData reset, IRepositoryManager dbManager)
        {
            var room = dbManager.ROOMS.Get(reset.Args.ToList()[0]);
            if (room == null)
            {
                // todo bug
                return;
            }

            // todo randomize_exits(reset.Args.ToList()[1]);
        }
    }
}
