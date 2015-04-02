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

DECLARE_DO_FUN( do_balance ); /* Bank balance command - Samson */
DECLARE_DO_FUN( do_deposit ); /* Bank deposit command - Samson */
DECLARE_DO_FUN( do_withdraw );   /* Bank withdraw command - Samson */
