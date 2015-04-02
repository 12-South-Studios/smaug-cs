/****************************************************************************
 *                   ^     +----- |  / ^     ^ |     | +-\                  *
 *                  / \    |      | /  |\   /| |     | |  \                 *
 *                 /   \   +---   |<   | \ / | |     | |  |                 *
 *                /-----\  |      | \  |  v  | |     | |  /                 *
 *               /       \ |      |  \ |     | +-----+ +-/                  *
 ****************************************************************************
 * AFKMud Copyright 1997-2003 by Roger Libiez (Samson),                     *
 * Levi Beckerson (Whir), Michael Ward (Tarl), Erik Wolfe (Dwip),           *
 * Cameron Carroll (Cam), Cyberfox, Karangi, Rathian, Raine, and Adjani.    *
 * All Rights Reserved.                                                     *
 *                                                                          *
 * Original SMAUG 1.4a written by Thoric (Derek Snider) with Altrag,        *
 * Blodkai, Haus, Narn, Scryn, Swordbearer, Tricops, Gorog, Rennard,        *
 * Grishnakh, Fireblade, and Nivek.                                         *
 *                                                                          *
 * Original MERC 2.1 code by Hatchet, Furey, and Kahn.                      *
 *                                                                          *
 * Original DikuMUD code by: Hans Staerfeldt, Katja Nyboe, Tom Madsen,      *
 * Michael Seifert, and Sebastian Hammer.                                   *
 ****************************************************************************
 *                              Bank module                                 *
 ****************************************************************************/

/***************************************************************************  
 *                          SMAUG Banking Support Code                     *
 ***************************************************************************
 *                                                                         *
 * This code may be used freely, as long as credit is given in the help    *
 * file. Thanks.                                                           *
 *                                                                         *
 *                                        -= Minas Ravenblood =-           *
 *                                 Implementor of The Apocalypse Theatre   *
 *                                      (email: krisco7@hotmail.com)       *
 *                                                                         *
 ***************************************************************************/

/* Modifications to original source by Samson */

#include <stdio.h>
#include "mud.h"

/* You can add this or just put it in the do_bank code. I don't really know
   why I made a seperate function for this, but I did. If you do add it,
   don't forget to declare it - Minas */
/* Finds banker mobs in a room. Installed by Samson on unknown date */
/* NOTE: Smaug 1.02a Users - Your compiler probably died on this
   function - if so, remove the x in front of IS_SET and recompile */
CHAR_DATA *find_banker( CHAR_DATA * ch )
{
   CHAR_DATA *banker = NULL;

   for( banker = ch->in_room->first_person; banker; banker = banker->next_in_room )
      if( IS_NPC( banker ) && xIS_SET( banker->act, ACT_BANKER ) )
         break;

   return banker;
}

/* SMAUG Bank Support
 * Coded by Minas Ravenblood for The Apocalypse Theatre
 * (email: krisco7@hotmail.com)
 */
/* Deposit, withdraw, balance and transfer commands */
void do_deposit( CHAR_DATA * ch, char *argument )
{
   CHAR_DATA *banker;
   char arg1[MAX_INPUT_LENGTH];
   char buf[MAX_STRING_LENGTH];
   int amount;

   if( !( banker = find_banker( ch ) ) )
   {
      send_to_char( "You're not in a bank!\r\n", ch );
      return;
   }

   if( IS_NPC( ch ) )
   {
      snprintf( buf, MAX_STRING_LENGTH, "Sorry, %s, we don't do business with mobs.", ch->short_descr );
      do_say( banker, buf );
      return;
   }

   if( argument[0] == '\0' )
   {
      do_say( banker, "If you need help, see HELP BANK." );
      return;
   }

   argument = one_argument( argument, arg1 );

   if( arg1 == '\0' )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s How much gold do you wish to deposit?", ch->name );
      do_tell( banker, buf );
      return;
   }

   if( str_cmp( arg1, "all" ) && !is_number( arg1 ) )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s How much gold do you wish to deposit?", ch->name );
      do_tell( banker, buf );
      return;
   }

   if( !str_cmp( arg1, "all" ) )
      amount = ch->gold;
   else
      amount = atoi( arg1 );

   if( amount > ch->gold )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s Sorry, but you don't have that much gold to deposit.", ch->name );
      do_tell( banker, buf );
      return;
   }

   if( amount <= 0 )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s Oh, I see.. your a comedian.", ch->name );
      do_tell( banker, buf );
      return;
   }

   ch->gold -= amount;
   ch->pcdata->balance += amount;
   set_char_color( AT_PLAIN, ch );
   ch_printf( ch, "You deposit %d gold.\r\n", amount );
   snprintf( buf, MAX_STRING_LENGTH, "$n deposits %d gold.\r\n", amount );
   act( AT_PLAIN, buf, ch, NULL, NULL, TO_ROOM );
   save_char_obj( ch );
   return;
}

void do_withdraw( CHAR_DATA * ch, char *argument )
{
   CHAR_DATA *banker;
   char arg1[MAX_INPUT_LENGTH];
   char buf[MAX_STRING_LENGTH];
   int amount;

   if( !( banker = find_banker( ch ) ) )
   {
      send_to_char( "You're not in a bank!\r\n", ch );
      return;
   }

   if( IS_NPC( ch ) )
   {
      snprintf( buf, MAX_STRING_LENGTH, "Sorry, %s, we don't do business with mobs.", ch->short_descr );
      do_say( banker, buf );
      return;
   }

   if( argument[0] == '\0' )
   {
      do_say( banker, "If you need help, see HELP BANK." );
      return;
   }

   argument = one_argument( argument, arg1 );

   if( arg1 == '\0' )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s How much gold do you wish to withdraw?", ch->name );
      do_tell( banker, buf );
      return;
   }
   if( str_cmp( arg1, "all" ) && !is_number( arg1 ) )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s How much gold do you wish to withdraw?", ch->name );
      do_tell( banker, buf );
      return;
   }

   if( !str_cmp( arg1, "all" ) )
      amount = ch->pcdata->balance;
   else
      amount = atoi( arg1 );

   if( amount > ch->pcdata->balance )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s But you do not have that much gold in your account!", ch->name );
      do_tell( banker, buf );
      return;
   }

   if( amount <= 0 )
   {
      snprintf( buf, MAX_STRING_LENGTH, "%s Oh I see.. your a comedian.", ch->name );
      do_tell( banker, buf );
      return;
   }

   ch->pcdata->balance -= amount;
   ch->gold += amount;
   set_char_color( AT_PLAIN, ch );
   ch_printf( ch, "You withdraw %d gold.\r\n", amount );
   snprintf( buf, MAX_STRING_LENGTH, "$n withdraws %d gold.\r\n", amount );
   act( AT_PLAIN, buf, ch, NULL, NULL, TO_ROOM );
   save_char_obj( ch );
   return;
}

void do_balance( CHAR_DATA * ch, char *argument )
{
   CHAR_DATA *banker;
   char buf[MAX_STRING_LENGTH];

   if( !( banker = find_banker( ch ) ) )
   {
      send_to_char( "You're not in a bank!\r\n", ch );
      return;
   }

   if( IS_NPC( ch ) )
   {
      snprintf( buf, MAX_STRING_LENGTH, "Sorry, %s, we don't do business with mobs.", ch->short_descr );
      do_say( banker, buf );
      return;
   }

   ch_printf( ch, "You have %d gold in the bank.\r\n", ch->pcdata->balance );
   return;
}

/* End of new bank support */
