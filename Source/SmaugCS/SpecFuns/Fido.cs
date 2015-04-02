using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;

namespace SmaugCS.SpecFuns
{
    public static class Fido
    {
        public static bool DoSpecFido(MobileInstance ch)
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
