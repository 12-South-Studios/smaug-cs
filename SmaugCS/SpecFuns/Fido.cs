using System.Linq;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.SpecFuns
{
    public static class Fido
    {
        public static bool DoSpecFido(CharacterInstance ch)
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

                handler.extract_obj(corpse);
                return true;
            }

            return false;
        }
    }
}
