using SmaugCS.Common;
using SmaugCS.Data.Instances;
using SmaugCS.Language;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character
{
    public static class Languages
    {
        public static int KnowsLanguage(this CharacterInstance ch, int language, CharacterInstance cch)
        {
            if (!ch.IsNpc() && ch.IsImmortal())
                return 100;
            if (ch.IsNpc() && (ch.Speaks == 0
                || ch.Speaks.IsSet((language & ~(int)LanguageTypes.Clan))))
                return 100;
            if (language.IsSet((int)LanguageTypes.Common))
                return 100;

            if ((language & (int)LanguageTypes.Clan) > 0)
            {
                if (ch.IsNpc() || cch.IsNpc())
                    return 100;
                if (((PlayerInstance)ch).PlayerData.Clan == ((PlayerInstance)cch).PlayerData.Clan
                    && ((PlayerInstance)ch).PlayerData.Clan != null)
                    return 100;
            }

            if (!ch.IsNpc())
            {
                if (RepositoryManager.Instance.GetRace(ch.CurrentRace).Language.IsSet(language))
                    return 100;

                /*for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)
                {
                    if (i == (int)LanguageTypes.Unknown)
                        break;

                    if (language.IsSet(i) && ch.Speaks.IsSet(i))
                    {
                        SkillData skill = RepositoryManager.Instance.GetSkill(GameConstants.LanguageTable[i]);
                        if (skill.Slot != -1)
                            return ch.PlayerData.Learned[skill.Slot];
                    }
                }*/
            }

            return 0;
        }

        public static bool CanLearnLanguage(this CharacterInstance ch, int language)
        {
            if ((language & (int)LanguageTypes.Clan) > 0)
                return false;
            if (ch.IsNpc() || ch.IsImmortal())
                return false;
            if ((RepositoryManager.Instance.GetRace(ch.CurrentRace).Language & language) > 0)
                return false;

            if ((ch.Speaks & language) > 0)
            {
                /*for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)
                {
                    if (i == (int)LanguageTypes.Unknown)
                        break;

                    if ((language & i) > 0)
                    {
                        if (((int)LanguageTypes.ValidLanguages & i) == 0)
                            return false;

                        SkillData skill = RepositoryManager.Instance.GetSkill(GameConstants.LanguageTable[i]);
                        if (skill == null)
                        {
                            LogManager.Instance.Bug("can_learn_lang: valid language without sn: %d", i);
                            continue;
                        }
                    }

                    if (ch.PlayerData.Learned[i] >= 99)
                        return false;
                }*/
            }

            return ((int)LanguageTypes.ValidLanguages & language) > 0;
        }
    }
}
