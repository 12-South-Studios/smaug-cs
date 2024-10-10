using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;

namespace SmaugCS.Spells.Fire;

class FireBreath
{
  private static readonly Dictionary<ItemTypes, string> ItemTypeMessages = new()
  {
    { ItemTypes.Container, "$p ignites and burns!" },
    { ItemTypes.Potion, "$p bubbles and boils!" },
    { ItemTypes.Scroll, "$p crackles and burns!" },
    { ItemTypes.Staff, "$p smokes and chars!" },
    { ItemTypes.Wand, "$p sparks and sputters!" },
    { ItemTypes.Cook, "$p blackens and crisps!" },
    { ItemTypes.Food, "$p blackens and crisps!" },
    { ItemTypes.Pill, "$p melts and drips!" }
  };
  
  public static ReturnTypes spell_fire_breath(int sn, int level, CharacterInstance ch, object vo)
  {
    CharacterInstance victim = (CharacterInstance)vo;

    if (ch.Chance(2 * level) && !victim.SavingThrows.CheckSaveVsBreath(level, victim))
    {
      int chance = GameConstants.GetConstant<int>("FireBreathItemDestroyChance");
      foreach (ObjectInstance instance in victim.Carrying.Where(instance => SmaugRandom.D100() < chance))
      {
        if (!ItemTypeMessages.TryGetValue(instance.ItemType, out string msg))
          continue;
        
        handler.separate_obj(instance);
        comm.act(ATTypes.AT_DAMAGE, msg, victim, instance, null, ToTypes.Character);

        if (instance.ItemType == ItemTypes.Container)
        {
          comm.act(ATTypes.AT_OBJECT, "The contents of $p held by $N spill onto the ground.", victim, instance, victim, ToTypes.Room);
          comm.act(ATTypes.AT_OBJECT, "The contents of $p spill out onto the ground!", victim, instance, null, ToTypes.Character);
          instance.Empty(null, victim.CurrentRoom);
        }
        
        instance.Extract(Program.AuctionManager);
      }
    }

    int hpch = 10.GetHighestOfTwoNumbers(ch.CurrentHealth);
    int dam = SmaugRandom.Between(hpch / 16 + 1, hpch / 8);

    if (victim.SavingThrows.CheckSaveVsBreath(level, victim))
      dam /= 2;
    
    return ch.CauseDamageTo(victim, dam, sn);
  }
}