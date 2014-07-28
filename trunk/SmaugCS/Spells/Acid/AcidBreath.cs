using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;

namespace SmaugCS.Spells
{
    public static class AcidBreath
    {
        public static ReturnTypes spell_acid_breath(int sn, int level, CharacterInstance ch, object vo)
        {
            CharacterInstance victim = (CharacterInstance) vo;

            if (handler.chance(ch, 2*level) && !victim.SavingThrows.CheckSaveVsBreath(level, victim))
            {
                foreach (ObjectInstance obj in victim.Carrying)
                {
                    if (SmaugRandom.Bits(2) != 0) continue;

                    if (obj.ItemType == ItemTypes.Armor)
                        CheckDamageArmor(obj, victim);
                    else if (obj.ItemType == ItemTypes.Container)
                        CheckDamageContainer(obj, victim);
                }
            }

            int hpChange = 10.GetHighestOfTwoNumbers(ch.CurrentHealth);
            int damage = SmaugRandom.Between(hpChange/16 + 1, hpChange/8);

            if (victim.SavingThrows.CheckSaveVsBreath(level, victim))
                damage /= 2;
            return fight.damage(ch, victim, damage, sn);
        }

        private static void CheckDamageContainer(ObjectInstance obj, CharacterInstance victim)
        {
            handler.separate_obj(obj);
            comm.act(ATTypes.AT_DAMAGE, "$p fumes and dissolves!", victim, obj, null, ToTypes.Character);
            comm.act(ATTypes.AT_OBJECT, "The contents of $p held by $N spill onto the ground.", victim, obj, victim, ToTypes.Room);
            comm.act(ATTypes.AT_OBJECT, "The contents of $p spill out onto the ground!", victim, obj, null, ToTypes.Character);
            handler.empty_obj(obj, null, victim.CurrentRoom);
            handler.extract_obj(obj);
        }

        private static void CheckDamageArmor(ObjectInstance obj, CharacterInstance victim)
        {
            if (obj.Value[0] <= 0) return;
            
            handler.separate_obj(obj);
            comm.act(ATTypes.AT_DAMAGE, "$p is pitted and etched!", victim, obj, null, ToTypes.Character);

            WearLocations wearLoc = obj.WearLocation;
            if (wearLoc != WearLocations.None)
                victim.ArmorClass += obj.ApplyArmorClass;
            obj.Value[0] -= 1;
            obj.Cost = 0;
            if (wearLoc != WearLocations.None)
                victim.ArmorClass -= obj.ApplyArmorClass;
        }
    }
}
