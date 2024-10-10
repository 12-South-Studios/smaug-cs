using SmaugCS.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Templates;
using SmaugCS.Extensions.Character;
using SmaugCS.Weather;

namespace SmaugCS.Spells.Electrical;

class CallLightning
{
  public static ReturnTypes spell_call_lightning(int sn, int level, CharacterInstance ch, object vo)
  {
    if (ch.IsOutside())
    {
      ch.SendTo("You must be out of doors.");
      return ReturnTypes.SpellFailed;
    }

    WeatherCell cell = Program.WeatherManager.GetWeather(ch.CurrentRoom.Area);
    if (cell.Precipitation < 40 && cell.Energy < 30)
    {
      ch.SendTo("You need bad weather.");
      return ReturnTypes.SpellFailed;
    }

    ch.SetColor(ATTypes.AT_MAGIC);
    ch.SendTo("God's lightning strikes your foes!");
    comm.act(ATTypes.AT_MAGIC, "$n calls God's lightning to strike $s foes!", ch, null, null, ToTypes.Room);

    RoomTemplate where = ch.CurrentRoom;
    bool ch_died = false;
    ReturnTypes retcode = ReturnTypes.None;
    int dam = SmaugRandom.Roll(8, level / 2);
    
    /*TRV_WORLD lc = trworld_create(TR_CHAR_WORLD_FORW)
    for( vch = first_char; vch; vch = trvch_wnext( lc ) )
    {
      if( !vch->in_room )
        continue;

      if( vch->in_room == where )
      {
        if( !IS_NPC( vch ) && xIS_SET( vch->act, PLR_WIZINVIS ) && vch->pcdata->wizinvis >= LEVEL_IMMORTAL )
          continue;

        if( vch != ch && ( IS_NPC( ch ) ? !IS_NPC( vch ) : IS_NPC( vch ) ) )
          retcode = damage( ch, vch, saves_spell_staff( level, vch ) ? dam / 2 : dam, sn );
        if( retcode == rCHAR_DIED || char_died( ch ) )
          ch_died = TRUE;
      }
      else if( vch->in_room->area == where->area && IS_OUTSIDE( vch ) && IS_AWAKE( vch ) && number_bits( 2 ) == 0 )
        send_to_char( "&BLightning flashes in the sky.\r\n", vch );
    }
    
    trworld_dispose(&lc);
    */
    
    return ch_died ? ReturnTypes.CharacterDied : ReturnTypes.None;
  }
}