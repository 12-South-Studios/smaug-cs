using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;

namespace SmaugCS.SpecFuns
{
    public static class Fido
    {
        public static bool DoSpecFido(MobileInstance ch)
        {
            if (!ch.IsAwake())
                return false;

            foreach (ObjectInstance corpse in ch.CurrentRoom.Contents.Where(obj => obj.ItemType == ItemTypes.NpcCorpse))
            {
                comm.act(ATTypes.AT_ACTION, "$n savagely devours a corpse.", ch, null, null, ToTypes.Room);

                foreach (ObjectInstance obj in corpse.Contents)
                {
                    corpse.FromObject(obj);
                    ch.CurrentRoom.ToRoom(obj);
                }

                corpse.Extract();
                return true;
            }

            return false;
        }
    }
}
