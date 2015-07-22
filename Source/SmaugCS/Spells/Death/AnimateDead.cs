using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Exceptions;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions;
using SmaugCS.Extensions.Character;
using SmaugCS.Extensions.Objects;
using SmaugCS.Extensions.Player;
using SmaugCS.Helpers;
using SmaugCS.Repository;
using EnumerationExtensions = Realm.Library.Common.EnumerationExtensions;

namespace SmaugCS.Spells
{
    public static class AnimateDead
    {
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "vo")]
        public static ReturnTypes spell_animate_dead(int sn, int level, CharacterInstance ch, object vo)
        {
            var corpse =
                ch.CurrentRoom.Contents.FirstOrDefault(x => x.ItemType == ItemTypes.NpcCorpse && x.Cost != -5);
            if (CheckFunctions.CheckIfNullObject(ch, corpse, "You cannot find a suitable corpse here"))
                return ReturnTypes.SpellFailed;

            var skill = RepositoryManager.Instance.GetEntity<SkillData>(sn);

            var template = RepositoryManager.Instance.MOBILETEMPLATES.Get(VnumConstants.MOB_VNUM_ANIMATED_CORPSE);
            if (template == null)
                throw new ObjectNotFoundException("Animated Corpse VNUM template was not found.");

            // TODO Get the template using the corpse cost?  huh?

            if (CheckFunctions.CheckIfEquivalent(ch, template,
                RepositoryManager.Instance.MOBILETEMPLATES.Get(VnumConstants.MOB_VNUM_DEITY),
                "You can't animate the corpse of a deity's Avatar.")) return ReturnTypes.SpellFailed;

            if (!ch.IsNpc())
            {
                if (ch.IsVampire())
                {
                    if (CheckFunctions.CheckIfTrue(ch,
                        !ch.IsImmortal() &&
                        (((PlayerInstance)ch).PlayerData.GetConditionValue(ConditionTypes.Bloodthirsty) - (template.Level / 3)) < 0,
                        "You don't have the power to reanimate this corpse.")) return ReturnTypes.SpellFailed;

                    ((PlayerInstance)ch).GainCondition(ConditionTypes.Bloodthirsty, template.Level / 3);
                }
                else if ((ch.CurrentMana - (template.Level/4)) < 0)
                {
                    ch.SendTo("You do not have enough mana to reanimate this corpse.");
                    return ReturnTypes.SpellFailed;
                }
                else
                    ch.CurrentMana -= (template.Level*4);
            }

            if (ch.IsImmortal() || (ch.Chance(75) && template.Level - ch.Level < 10))
                return AnimateCorpse(level, ch, template, corpse);

            ch.FailedCast(skill);
            return ReturnTypes.SpellFailed;
        }

        private static ReturnTypes AnimateCorpse(int level, CharacterInstance ch, MobTemplate template, ObjectInstance corpse)
        {
            CreateAnimatedCorpse(level, ch, template, corpse);

            corpse.Split();
            corpse.Extract();

            comm.act(ATTypes.AT_MAGIC, "$n makes $T rise from the grave!", ch, null, template.ShortDescription, ToTypes.Room);
            comm.act(ATTypes.AT_MAGIC, "You make $T rise from the grave!", ch, null, template.ShortDescription,
                ToTypes.Character);

            return ReturnTypes.None;
        }

        private static void CreateAnimatedCorpse(int level, CharacterInstance ch, MobTemplate template, ObjectInstance corpse)
        {
            var mob = RepositoryManager.Instance.CHARACTERS.Create(template, 0,
                string.Format("animated corpse {0}", template.PlayerName));

            ch.CurrentRoom.AddTo(mob);
            mob.Level = (ch.Level/2).GetLowestOfTwoNumbers(template.Level);
            mob.CurrentRace = EnumerationExtensions.GetEnumByName<RaceTypes>(template.Race);

            mob.MaximumHealth = template.Level*8 +
                                SmaugRandom.Between(template.Level*template.Level/4, template.Level*template.Level);
            mob.MaximumHealth = (mob.MaximumHealth/4).GetNumberThatIsBetween(
                (mob.MaximumHealth * corpse.Value.ToList()[3]) / 100, ch.Level * SmaugRandom.D20(10));
            mob.MaximumHealth = mob.MaximumHealth.GetHighestOfTwoNumbers(1);
            mob.CurrentHealth = mob.MaximumHealth;
            mob.DamageRoll = new DiceData {SizeOf = ch.Level/8};
            mob.HitRoll = new DiceData {SizeOf = ch.Level/6};
            mob.CurrentAlignment = ch.CurrentAlignment;
            mob.ShortDescription = string.Format("The animated corpse of {0}", template.ShortDescription);
            mob.LongDescription =
                string.Format("An animated corpse of {0} struggles with the horror of its undeath.",
                    template.ShortDescription);
            mob.AddFollower(ch);

            MakeCorpseTemporary(level, mob);
            TransferContentsOfCorpse(corpse);
        }

        private static void MakeCorpseTemporary(int level, CharacterInstance mob)
        {
            var af = new AffectData
            {
                Type = AffectedByTypes.Charm,
                Duration = (SmaugRandom.Fuzzy((level + 1)/4) + 1)*
                           GameConstants.GetSystemValue<int>("AffectDurationConversionValue")
            };
            mob.AddAffect(af);
        }

        private static void TransferContentsOfCorpse(ObjectInstance corpse)
        {
            if (!corpse.Contents.Any()) return;

            foreach (var obj in corpse.Contents)
            {
                obj.RemoveFrom(obj);
                corpse.InRoom.AddTo(obj);
            }
        }
    }
}
