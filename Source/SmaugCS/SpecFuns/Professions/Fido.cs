using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using System.Linq;

namespace SmaugCS.SpecFuns
{
    public static class Fido
    {
        public static bool Execute(MobileInstance ch, IManager dbManager)
        {
            if (!ch.IsAwake())
                return false;

            foreach (var corpse in ch.CurrentRoom.Contents.Where(obj => obj.ItemType == ItemTypes.NpcCorpse))
            {
                comm.act(ATTypes.AT_ACTION, "$n savagely devours a corpse.", ch, null, null, ToTypes.Room);

                foreach (var obj in corpse.Contents)
                {
                    corpse.RemoveFrom(obj);
                    ch.CurrentRoom.AddTo(obj);
                }

                corpse.Extract();
                return true;
            }

            return false;
        }
    }
}
