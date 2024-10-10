using SmaugCS.Common;
using SmaugCS.Data.Instances;
using SmaugCS.Language;
using SmaugCS.Repository;

namespace SmaugCS.Extensions.Character;

public static class Languages
{
  public static int KnowsLanguage(this CharacterInstance ch, int language, CharacterInstance cch)
  {
    if (!ch.IsNpc() && ch.IsImmortal())
      return 100;
    if (ch.IsNpc() && (ch.Speaks == 0
                       || ch.Speaks.IsSet(language & ~(int)LanguageTypes.Clan)))
      return 100;
    if (language.IsSet((int)LanguageTypes.Common))
      return 100;

    if ((language & (int)LanguageTypes.Clan) > 0)
    {
      if (ch.IsNpc() || cch.IsNpc())
        return 100;

      // TODO fix
      //if (((PlayerInstance)ch).PlayerData.Clan == ((PlayerInstance)cch).PlayerData.Clan
      //    && ((PlayerInstance)ch).PlayerData.Clan != null)
      //    return 100;
    }

    if (!ch.IsNpc())
    {
      if (Program.RepositoryManager.GetRace(ch.CurrentRace).Language.IsSet(language))
        return 100;

      // TODO
      /*for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)



           f (i == (int)LanguageTypes.Unknown)

               reak;


           f (language.IsSet(i) && ch.Speaks.IsSet(i))



               killData skill = Program.RepositoryManager.GetSkill(GameConstants.LanguageTable[i]);

               f (skill.Slot != -1)

                   eturn ch.PlayerData.Learned[skill.Slot];



       */
    }

    return 0;
  }

  public static bool CanLearnLanguage(this CharacterInstance ch, int language)
  {
    if ((language & (int)LanguageTypes.Clan) > 0)
      return false;
    if (ch.IsNpc() || ch.IsImmortal())
      return false;
    if ((Program.RepositoryManager.GetRace(ch.CurrentRace).Language & language) > 0)
      return false;

    if ((ch.Speaks & language) > 0)
    {
      // TODO
      /*for (int i = 0; i < GameConstants.LanguageTable.Keys.Count; i++)



           f (i == (int)LanguageTypes.Unknown)

               reak;


           f ((language & i) > 0)



               f (((int)LanguageTypes.ValidLanguages & i) == 0)

                   eturn false;


               killData skill = Program.RepositoryManager.GetSkill(GameConstants.LanguageTable[i]);

               f (skill == null)



                   rogram.LogManager.Bug("can_learn_lang: valid language without sn: %d", i);

                   ontinue;






           f (ch.PlayerData.Learned[i] >= 99)

               eturn false;

       */
    }

    return ((int)LanguageTypes.ValidLanguages & language) > 0;
  }
}