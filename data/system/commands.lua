-- COMMANDS.LUA
-- This is the Commands data file for the MUD
-- Revised: 2013.11.22
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadCommands()
	LCreateCommand("'~", "do_say", 106, 0, 0, 0);
	LCreateCommand(",~", "do_emote", 106, 0, 0, 0);
	LCreateCommand(".~", "do_chat", 104, 0, 0, 1);
	LCreateCommand(";~", "do_gtell", 100, 0, 0, 0);
	LCreateCommand("affected", "do_affected", 104, 0, 0, 0);
	LCreateCommand("afk", "do_afk", 104, 0, 0, 0);
	LCreateCommand("ansi", "do_ansi", 100, 0, 0, 0);
	LCreateCommand("answer", "do_answer", 104, 0, 0, 1);
	LCreateCommand("appraise", "do_appraise", 106, 0, 0, 0);
	LCreateCommand("areas", "do_areas", 100, 0, 0, 0);
	LCreateCommand("ask", "do_ask", 104, 0, 0, 1);
	LCreateCommand("apply", "do_apply", 100, 1, 0, 0);
	LCreateCommand("atobj", "do_atobj", 100, 52, 0, 0);
	LCreateCommand("buy", "do_buy", 106, 0, 0, 0);
	LCreateCommand("brandish", "do_brandish", 108, 0, 0, 0);
	LCreateCommand("bs", "do_backstab", 112, 0, 0, 0);
	LCreateCommand("bio", "do_bio", 100, 1, 0, 0);
	LCreateCommand("bug", "do_bug", 100, 0, 0, 0);
	
end

function LoadAdminCommands()
	LCreateCommand(":~", "do_immtalk", 100, 51, 0, 1);
	LCreateCommand(":~", "do_avtalk", 100, 50, 0, 1);
	LCreateCommand("authorize", "do_authorize", 100, 51, 1, 0);
	LCreateCommand("at", "do_at", 100, 52, 0, 0);	
	LCreateCommand("aassign", "do_aassign", 100, 53, 1, 0);
	LCreateCommand("advance", "do_advance", 100, 61, 1, 0);
	LCreateCommand("allow", "do_allow", 100, 58, 1, 0);
	LCreateCommand("aset", "do_aset", 100, 59, 1, 0);
	LCreateCommand("astat", "do_astat", 100, 59, 0, 0);
	LCreateCommand("avtalk", "do_avtalk", 100, 51, 0, 1);
	LCreateCommand("alinks", "do_alinks", 100, 51, 0, 0);	
	LCreateCommand("balzhur", "do_balzhur", 108, 59, 1, 4);
	LCreateCommand("bamfin", "do_bamfin", 100, 51, 0, 0);
	LCreateCommand("bamfout", "do_bamfout", 100, 51, 0, 0);
	LCreateCommand("ban", "do_ban", 100, 58, 1, 0);
	LCreateCommand("bestow", "do_bestow", 100, 62, 1, 0);
	LCreateCommand("bodybag", "do_bodybag", 100, 51, 1, 0);
	LCreateCommand("boards", "do_boards", 100, 58, 0, 0);
	LCreateCommand("bset", "do_bset", 100, 61, 1, 0);
	LCreateCommand("bstat", "do_bstat", 100, 58, 0, 0);
	
end

LoadCommands();
LoadAdminCommands();

--[[
#COMMAND
Name        bury~
Code        do_bury
Position    112
Level       0
Log         0
End

#COMMAND
Name        bestowarea~
Code        do_bestowarea
Position    100
Level       59
Log         1
End

#COMMAND
Name        bolt~
Code        do_bolt
Position    100
Level       0
Log         0
End

#COMMAND
Name        chess~
Code        do_chess
Position    100
Level       10
Log         0
End

#COMMAND
Name        cast~
Code        do_cast
Position    104
Level       0
Log         0
End

#COMMAND
Name        clantalk~
Code        do_clantalk
Position    100
Level       0
Log         0
Flags       4
End

#COMMAND
Name        close~
Code        do_close
Position    106
Level       0
Log         0
End

#COMMAND
Name        consider~
Code        do_consider
Position    106
Level       0
Log         0
End

#COMMAND
Name        channels~
Code        do_channels
Position    100
Level       0
Log         0
End

#COMMAND
Name        chat~
Code        do_chat
Position    103
Level       2
Log         0
Flags       3
End

#COMMAND
Name        checkvnums~
Code        do_check_vnums
Position    100
Level       58
Log         0
End

#COMMAND
Name        clans~
Code        do_clans
Position    100
Level       0
Log         0
End

#COMMAND
Name        cedit~
Code        do_cedit
Position    100
Level       61
Log         1
End

#COMMAND
Name        coinduct~
Code        do_council_induct
Position    100
Level       55
Log         0
End

#COMMAND
Name        compare~
Code        do_compare
Position    106
Level       0
Log         0
End

#COMMAND
Name        commands~
Code        do_commands
Position    100
Level       0
Log         0
End

#COMMAND
Name        comment~
Code        do_comment
Position    106
Level       51
Log         0
End

#COMMAND
Name        config~
Code        do_config
Position    100
Level       0
Log         0
End

#COMMAND
Name        cooutcast~
Code        do_council_outcast
Position    100
Level       55
Log         0
End

#COMMAND
Name        counciltalk~
Code        do_counciltalk
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        councils~
Code        do_councils
Position    100
Level       0
Log         0
End

#COMMAND
Name        credits~
Code        do_credits
Position    100
Level       0
Log         0
End

#COMMAND
Name        cset~
Code        do_cset
Position    100
Level       61
Log         1
End

#COMMAND
Name        cmdtable~
Code        do_cmdtable
Position    100
Level       57
Log         0
End

#COMMAND
Name        cook~
Code        do_cook
Position    100
Level       1
Log         0
End

#COMMAND
Name        color~
Code        do_color
Position    100
Level       0
Log         0
End

#COMMAND
Name        down~
Code        do_down
Position    112
Level       0
Log         0
End

#COMMAND
Name        drink~
Code        do_drink
Position    106
Level       0
Log         0
End

#COMMAND
Name        drop~
Code        do_drop
Position    106
Level       0
Log         0
End

#COMMAND
Name        deny~
Code        do_deny
Position    100
Level       58
Log         1
Flags       4
End

#COMMAND
Name        description~
Code        do_description
Position    100
Level       0
Log         0
End

#COMMAND
Name        destro~
Code        do_destro
Position    112
Level       60
Log         0
End

#COMMAND
Name        destroy~
Code        do_destroy
Position    112
Level       65
Log         1
Flags       4
End

#COMMAND
Name        dig~
Code        do_dig
Position    112
Level       3
Log         0
End

#COMMAND
Name        disconnect~
Code        do_disconnect
Position    100
Level       58
Log         1
End

#COMMAND
Name        dmesg~
Code        do_dmesg
Position    100
Level       58
Log         1
End

#COMMAND
Name        drag~
Code        do_drag
Position    112
Level       0
Log         0
End

#COMMAND
Name        devote~
Code        do_devote
Position    106
Level       5
Log         0
End

#COMMAND
Name        deities~
Code        do_deities
Position    100
Level       1
Log         0
End

#COMMAND
Name        dismount~
Code        do_dismount
Position    100
Level       0
Log         0
End

#COMMAND
Name        dismiss~
Code        do_dismiss
Position    100
Level       2
Log         0
End

#COMMAND
Name        delay~
Code        do_delay
Position    100
Level       58
Log         0
End

#COMMAND
Name        date~
Code        do_time
Position    100
Level       0
Log         0
End

#COMMAND
Name        dnd~
Code        do_dnd
Position    100
Level       0
Log         0
End

#COMMAND
Name        east~
Code        do_east
Position    112
Level       0
Log         0
End

#COMMAND
Name        eat~
Code        do_eat
Position    106
Level       0
Log         0
End

#COMMAND
Name        emote~
Code        do_emote
Position    106
Level       0
Log         0
End

#COMMAND
Name        exits~
Code        do_exits
Position    106
Level       0
Log         0
End

#COMMAND
Name        examine~
Code        do_examine
Position    106
Level       0
Log         0
End

#COMMAND
Name        equipment~
Code        do_equipment
Position    100
Level       0
Log         0
End

#COMMAND
Name        echo~
Code        do_echo
Position    100
Level       54
Log         1
End

#COMMAND
Name        empty~
Code        do_empty
Position    108
Level       0
Log         0
End

#COMMAND
Name        enter~
Code        do_enter
Position    112
Level       0
Log         0
End

#COMMAND
Name        elevate~
Code        do_elevate
Position    100
Level       60
Log         0
End

#COMMAND
Name        fill~
Code        do_fill
Position    106
Level       0
Log         0
End

#COMMAND
Name        follow~
Code        do_follow
Position    106
Level       0
Log         0
End

#COMMAND
Name        fixchar~
Code        do_fixchar
Position    100
Level       62
Log         1
End

#COMMAND
Name        flee~
Code        do_flee
Position    107
Level       0
Log         0
End

#COMMAND
Name        foldarea~
Code        do_foldarea
Position    100
Level       61
Log         1
End

#COMMAND
Name        for~
Code        do_for
Position    100
Level       59
Log         1
End

#COMMAND
Name        force~
Code        do_force
Position    100
Level       52
Log         1
End

#COMMAND
Name        forceclose~
Code        do_forceclose
Position    100
Level       55
Log         1
End

#COMMAND
Name        formpass~
Code        do_form_password
Position    100
Level       61
Log         0
End

#COMMAND
Name        freeze~
Code        do_freeze
Position    100
Level       58
Log         1
End

#COMMAND
Name        fquit~
Code        do_fquit
Position    100
Level       51
Log         1
End

#COMMAND
Name        fixed~
Code        do_fixed
Position    100
Level       51
Log         0
End

#COMMAND
Name        fprompt~
Code        do_fprompt
Position    100
Level       2
Log         0
End

#COMMAND
Name        findnote~
Code        do_findnote
Position    100
Level       2
Log         0
End

#COMMAND
Name        fire~
Code        do_fire
Position    100
Level       5
Log         0
End

#COMMAND
Name        fshow~
Code        do_fshow
Position    100
Level       58
Log         0
End

#COMMAND
Name        findexit~
Code        do_findexit
Position    100
Level       51
Log         0
End

#COMMAND
Name        get~
Code        do_get
Position    106
Level       0
Log         0
End

#COMMAND
Name        give~
Code        do_give
Position    106
Level       0
Log         0
End

#COMMAND
Name        gtell~
Code        do_gtell
Position    100
Level       0
Log         0
End

#COMMAND
Name        group~
Code        do_group
Position    104
Level       0
Log         0
End

#COMMAND
Name        guildtalk~
Code        do_guildtalk
Position    100
Level       0
Log         0
End

#COMMAND
Name        glance~
Code        do_glance
Position    106
Level       0
Log         0
End

#COMMAND
Name        goto~
Code        do_goto
Position    100
Level       51
Log         0
End

#COMMAND
Name        guilds~
Code        do_guilds
Position    100
Level       0
Log         0
End

#COMMAND
Name        gold~
Code        do_gold
Position    100
Level       1
Log         0
End

#COMMAND
Name        gfighting~
Code        do_gfighting
Position    100
Level       61
Log         0
End

#COMMAND
Name        gwhere~
Code        do_gwhere
Position    100
Level       63
Log         0
End

#COMMAND
Name        help~
Code        do_help
Position    100
Level       0
Log         0
End

#COMMAND
Name        hold~
Code        do_wear
Position    107
Level       0
Log         0
End

#COMMAND
Name        hedit~
Code        do_hedit
Position    100
Level       59
Log         1
End

#COMMAND
Name        hell~
Code        do_hell
Position    100
Level       51
Log         4
End

#COMMAND
Name        hl~
Code        do_hl
Position    100
Level       0
Log         0
End

#COMMAND
Name        hlist~
Code        do_hlist
Position    100
Level       0
Log         0
End

#COMMAND
Name        holylight~
Code        do_holylight
Position    100
Level       51
Log         0
End

#COMMAND
Name        homepage~
Code        do_homepage
Position    100
Level       10
Log         0
End

#COMMAND
Name        hset~
Code        do_hset
Position    100
Level       59
Log         1
End

#COMMAND
Name        holidays~
Code        do_holidays
Position    100
Level       0
Log         0
End

#COMMAND
Name        hotboot~
Code        do_hotboot
Position    100
Level       60
Log         1
End

#COMMAND
Name        inventory~
Code        do_inventory
Position    100
Level       0
Log         0
End

#COMMAND
Name        ide~
Code        do_ide
Position    100
Level       0
Log         0
End

#COMMAND
Name        idea~
Code        do_idea
Position    100
Level       0
Log         0
End

#COMMAND
Name        immtalk~
Code        do_immtalk
Position    100
Level       51
Log         0
Flags       1
End

#COMMAND
Name        immtalk~
Code        do_avtalk
Position    100
Level       50
Log         0
End

#COMMAND
Name        immortalize~
Code        do_immortalize
Position    100
Level       59
Log         1
End

#COMMAND
Name        induct~
Code        do_induct
Position    112
Level       50
Log         1
End

#COMMAND
Name        installarea~
Code        do_installarea
Position    100
Level       65
Log         1
End

#COMMAND
Name        instazone~
Code        do_instazone
Position    100
Level       61
Log         3
End

#COMMAND
Name        invis~
Code        do_invis
Position    100
Level       51
Log         0
End

#COMMAND
Name        instaroom~
Code        do_instaroom
Position    100
Level       54
Log         3
End

#COMMAND
Name        ignore~
Code        do_ignore
Position    100
Level       1
Log         0
End

#COMMAND
Name        immhost~
Code        do_add_imm_host
Position    100
Level       61
Log         0
End

#COMMAND
Name        ipcompare~
Code        do_ipcompare
Position    100
Level       52
Log         0
End

#COMMAND
Name        kill~
Code        do_kill
Position    109
Level       0
Log         0
End

#COMMAND
Name        khistory~
Code        do_khistory
Position    100
Level       58
Log         0
End

#COMMAND
Name        look~
Code        do_look
Position    106
Level       0
Log         0
End

#COMMAND
Name        lock~
Code        do_lock
Position    106
Level       0
Log         0
End

#COMMAND
Name        level~
Code        do_level
Position    100
Level       0
Log         0
End

#COMMAND
Name        list~
Code        do_list
Position    106
Level       0
Log         0
End

#COMMAND
Name        languages~
Code        do_languages
Position    106
Level       0
Log         0
End

#COMMAND
Name        last~
Code        do_last
Position    100
Level       54
Log         1
End

#COMMAND
Name        leave~
Code        do_leave
Position    112
Level       0
Log         0
End

#COMMAND
Name        light~
Code        do_light
Position    108
Level       0
Log         0
End

#COMMAND
Name        litterbug~
Code        do_litterbug
Position    100
Level       52
Log         1
End

#COMMAND
Name        loadarea~
Code        do_loadarea
Position    100
Level       54
Log         1
End

#COMMAND
Name        loadup~
Code        do_loadup
Position    100
Level       58
Log         1
End

#COMMAND
Name        log~
Code        do_log
Position    100
Level       55
Log         1
End

#COMMAND
Name        laws~
Code        do_laws
Position    100
Level       1
Log         0
End

#COMMAND
Name        mpoowner~
Code        do_mpoowner
Position    100
Level       0
Log         0
End

#COMMAND
Name        morph~
Code        do_imm_morph
Position    100
Level       51
Log         0
End

#COMMAND
Name        morphstat~
Code        do_morphstat
Position    100
Level       51
Log         0
End

#COMMAND
Name        morphcreate~
Code        do_morphcreate
Position    100
Level       51
Log         0
End

#COMMAND
Name        morphset~
Code        do_morphset
Position    100
Level       51
Log         0
End

#COMMAND
Name        morphdestroy~
Code        do_morphdestroy
Position    100
Level       51
Log         0
End

#COMMAND
Name        mpecho~
Code        do_mpecho
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpechoat~
Code        do_mpechoat
Position    100
Level       0
Log         0
End

#COMMAND
Name        mea~
Code        do_mpechoat
Position    100
Level       0
Log         0
End

#COMMAND
Name        mpat~
Code        do_mpat
Position    100
Level       0
Log         0
Flags       5
End

#COMMAND
Name        mpforce~
Code        do_mpforce
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpechoaround~
Code        do_mpechoaround
Position    100
Level       0
Log         0
End

#COMMAND
Name        mer~
Code        do_mpechoaround
Position    100
Level       0
Log         0
End

#COMMAND
Name        mpasound~
Code        do_mpasound
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpoload~
Code        do_mpoload
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpjunk~
Code        do_mpjunk
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpgoto~
Code        do_mpgoto
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpdamage~
Code        do_mp_damage
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpdelay~
Code        do_mpdelay
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpdeposit~
Code        do_mp_deposit
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mprestore~
Code        do_mp_restore
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpkill~
Code        do_mpkill
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mptransfer~
Code        do_mptransfer
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpmload~
Code        do_mpmload
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpnothing~
Code        do_mpnothing
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mppurge~
Code        do_mppurge
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpinvis~
Code        do_mpinvis
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpadvance~
Code        do_mpadvance
Position    100
Level       0
Log         1
Flags       1
End

#COMMAND
Name        mpapply~
Code        do_mpapply
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpapplyb~
Code        do_mpapplyb
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mppkset~
Code        do_mppkset
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpclosepassage~
Code        do_mp_close_passage
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpopenpassage~
Code        do_mp_open_passage
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpdream~
Code        do_mpdream
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpslay~
Code        do_mp_slay
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mppractice~
Code        do_mp_practice
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpwithdraw~
Code        do_mp_withdraw
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mail~
Code        do_mailroom
Position    106
Level       0
Log         0
End

#COMMAND
Name        make~
Code        do_make
Position    100
Level       50
Log         1
Flags       4
End

#COMMAND
Name        makeboard~
Code        do_makeboard
Position    100
Level       61
Log         1
End

#COMMAND
Name        makeclan~
Code        do_makeclan
Position    100
Level       61
Log         1
End

#COMMAND
Name        makecouncil~
Code        do_makecouncil
Position    100
Level       61
Log         1
End

#COMMAND
Name        makerepair~
Code        do_makerepair
Position    100
Level       54
Log         3
End

#COMMAND
Name        makeshop~
Code        do_makeshop
Position    100
Level       54
Log         3
End

#COMMAND
Name        makewizlist~
Code        do_makewizlist
Position    100
Level       61
Log         1
End

#COMMAND
Name        mapout~
Code        do_mapout
Position    100
Level       57
Log         0
End

#COMMAND
Name        mcreate~
Code        do_mcreate
Position    100
Level       55
Log         3
End

#COMMAND
Name        mdelete~
Code        do_mdelete
Position    100
Level       54
Log         1
End

#COMMAND
Name        memory~
Code        do_memory
Position    100
Level       57
Log         1
End

#COMMAND
Name        mfind~
Code        do_mfind
Position    100
Level       51
Log         0
End

#COMMAND
Name        minvoke~
Code        do_minvoke
Position    100
Level       54
Log         1
End

#COMMAND
Name        mlist~
Code        do_mlist
Position    100
Level       54
Log         0
End

#COMMAND
Name        mpedit~
Code        do_mpedit
Position    100
Level       55
Log         3
Flags       1
End

#COMMAND
Name        mpstat~
Code        do_mpstat
Position    100
Level       54
Log         0
Flags       1
End

#COMMAND
Name        mset~
Code        do_mset
Position    100
Level       54
Log         3
End

#COMMAND
Name        mstat~
Code        do_mstat
Position    100
Level       53
Log         0
End

#COMMAND
Name        murde~
Code        do_murde
Position    109
Level       5
Log         0
End

#COMMAND
Name        murder~
Code        do_murder
Position    109
Level       5
Log         0
End

#COMMAND
Name        music~
Code        do_music
Position    104
Level       0
Log         0
Flags       1
End

#COMMAND
Name        muse~
Code        do_muse
Position    100
Level       55
Log         0
Flags       1
End

#COMMAND
Name        mwhere~
Code        do_mwhere
Position    100
Level       52
Log         0
End

#COMMAND
Name        makedeity~
Code        do_makedeity
Position    100
Level       61
Log         1
End

#COMMAND
Name        mpfavor~
Code        do_mpfavor
Position    100
Level       1
Log         0
Flags       1
End

#COMMAND
Name        mposet~
Code        do_mposet
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpmset~
Code        do_mpmset
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mortalize~
Code        do_mortalize
Position    100
Level       58
Log         1
End

#COMMAND
Name        mppardon~
Code        do_mppardon
Position    100
Level       0
Log         1
Flags       1
End

#COMMAND
Name        mpscatter~
Code        do_mpscatter
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mppeace~
Code        do_mppeace
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpechozone~
Code        do_mpechozone
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mez~
Code        do_mpechozone
Position    100
Level       0
Log         0
End

#COMMAND
Name        mplog~
Code        do_mp_log
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpasupress~
Code        do_mpasupress
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        mpmorph~
Code        do_mpmorph
Position    100
Level       1
Log         0
Flags       1
End

#COMMAND
Name        mpunmorph~
Code        do_mpunmorph
Position    100
Level       1
Log         0
Flags       1
End

#COMMAND
Name        mpunnuisance~
Code        do_mpunnuisance
Position    100
Level       1
Log         0
Flags       1
End

#COMMAND
Name        mpnuisance~
Code        do_nuisance
Position    100
Level       1
Log         0
End

#COMMAND
Name        mpbodybag~
Code        do_mpbodybag
Position    100
Level       1
Log         0
Flags       1
End

#COMMAND
Name        morphlist~
Code        do_morphlist
Position    100
Level       51
Log         0
End

#COMMAND
Name        mphate~
Code        do_mphate
Position    100
Level       0
Log         0
End

#COMMAND
Name        mphunt~
Code        do_mphunt
Position    100
Level       0
Log         0
End

#COMMAND
Name        mpfear~
Code        do_mpfear
Position    100
Level       0
Log         0
End

#COMMAND
Name        mix~
Code        do_mix
Position    100
Level       0
Log         0
End

#COMMAND
Name        north~
Code        do_north
Position    112
Level       0
Log         0
End

#COMMAND
Name        name~
Code        do_name
Position    100
Level       0
Log         0
End

#COMMAND
Name        ne~
Code        do_northeast
Position    112
Level       0
Log         0
End

#COMMAND
Name        nw~
Code        do_northwest
Position    112
Level       0
Log         0
End

#COMMAND
Name        note~
Code        do_noteroom
Position    106
Level       0
Log         0
End

#COMMAND
Name        newbiechat~
Code        do_newbiechat
Position    100
Level       0
Log         0
Flags       1
End

#COMMAND
Name        newbieset~
Code        do_newbieset
Position    100
Level       51
Log         0
End

#COMMAND
Name        newzones~
Code        do_newzones
Position    100
Level       53
Log         0
End

#COMMAND
Name        noemote~
Code        do_noemote
Position    100
Level       53
Log         0
End

#COMMAND
Name        noresolve~
Code        do_noresolve
Position    100
Level       61
Log         1
End

#COMMAND
Name        northeast~
Code        do_northeast
Position    112
Level       0
Log         0
End

#COMMAND
Name        northwest~
Code        do_northwest
Position    112
Level       0
Log         0
End

#COMMAND
Name        notell~
Code        do_notell
Position    100
Level       52
Log         1
End

#COMMAND
Name        notitle~
Code        do_notitle
Position    100
Level       53
Log         1
End

#COMMAND
Name        nuisance~
Code        do_nuisance
Position    100
Level       54
Log         1
End

#COMMAND
Name        oowner~
Code        do_oowner
Position    100
Level       59
Log         0
End

#COMMAND
Name        order~
Code        do_order
Position    106
Level       0
Log         0
End

#COMMAND
Name        open~
Code        do_open
Position    106
Level       0
Log         0
End

#COMMAND
Name        ocreate~
Code        do_ocreate
Position    100
Level       54
Log         3
End

#COMMAND
Name        odelete~
Code        do_odelete
Position    100
Level       54
Log         1
End

#COMMAND
Name        ofind~
Code        do_ofind
Position    100
Level       54
Log         0
End

#COMMAND
Name        oinvoke~
Code        do_oinvoke
Position    100
Level       54
Log         1
End

#COMMAND
Name        olist~
Code        do_olist
Position    100
Level       54
Log         0
End

#COMMAND
Name        opedit~
Code        do_opedit
Position    100
Level       54
Log         3
End

#COMMAND
Name        opstat~
Code        do_opstat
Position    100
Level       54
Log         0
End

#COMMAND
Name        orders~
Code        do_orders
Position    100
Level       0
Log         0
End

#COMMAND
Name        ordertalk~
Code        do_ordertalk
Position    100
Level       0
Log         0
End

#COMMAND
Name        ostat~
Code        do_ostat
Position    100
Level       54
Log         0
End

#COMMAND
Name        ot~
Code        do_ordertalk
Position    100
Level       0
Log         0
End

#COMMAND
Name        outcast~
Code        do_outcast
Position    112
Level       50
Log         1
End

#COMMAND
Name        oset~
Code        do_oset
Position    100
Level       54
Log         3
End

#COMMAND
Name        put~
Code        do_put
Position    106
Level       0
Log         0
End

#COMMAND
Name        password~
Code        do_password
Position    100
Level       0
Log         2
End

#COMMAND
Name        practice~
Code        do_practice
Position    104
Level       0
Log         0
End

#COMMAND
Name        pardon~
Code        do_pardon
Position    100
Level       51
Log         1
End

#COMMAND
Name        peace~
Code        do_peace
Position    100
Level       51
Log         0
End

#COMMAND
Name        pull~
Code        do_pull
Position    106
Level       0
Log         0
End

#COMMAND
Name        purge~
Code        do_purge
Position    100
Level       57
Log         0
End

#COMMAND
Name        purge~
Code        do_low_purge
Position    100
Level       54
Log         0
End

#COMMAND
Name        push~
Code        do_push
Position    106
Level       0
Log         0
End

#COMMAND
Name        prompt~
Code        do_prompt
Position    100
Level       2
Log         0
End

#COMMAND
Name        pager~
Code        do_pager
Position    100
Level       1
Log         0
End

#COMMAND
Name        project~
Code        do_project
Position    100
Level       51
Log         0
End

#COMMAND
Name        pcrename~
Code        do_pcrename
Position    100
Level       62
Log         0
End

#COMMAND
Name        quaff~
Code        do_quaff
Position    105
Level       0
Log         0
End

#COMMAND
Name        qui~
Code        do_qui
Position    100
Level       0
Log         0
End

#COMMAND
Name        quit~
Code        do_quit
Position    100
Level       0
Log         0
End

#COMMAND
Name        quest~
Code        do_quest
Position    104
Level       0
Log         0
Flags       1
End

#COMMAND
Name        qpset~
Code        do_qpset
Position    100
Level       51
Log         1
End

#COMMAND
Name        qpstat~
Code        do_qpstat
Position    100
Level       51
Log         0
End

#COMMAND
Name        renumber~
Code        do_renumber
Position    100
Level       55
Log         1
End

#COMMAND
Name        rest~
Code        do_rest
Position    104
Level       0
Log         0
End

#COMMAND
Name        report~
Code        do_report
Position    106
Level       0
Log         0
End

#COMMAND
Name        repair~
Code        do_repair
Position    106
Level       0
Log         0
End

#COMMAND
Name        reply~
Code        do_reply
Position    106
Level       0
Log         0
End

#COMMAND
Name        remove~
Code        do_remove
Position    107
Level       0
Log         0
End

#COMMAND
Name        rank~
Code        do_rank
Position    100
Level       56
Log         0
End

#COMMAND
Name        rat~
Code        do_rat
Position    100
Level       61
Log         1
End

#COMMAND
Name        rdelete~
Code        do_rdelete
Position    100
Level       53
Log         1
End

#COMMAND
Name        reboo~
Code        do_reboo
Position    100
Level       60
Log         0
End

#COMMAND
Name        reboot~
Code        do_reboot
Position    100
Level       60
Log         1
End

#COMMAND
Name        recite~
Code        do_recite
Position    106
Level       0
Log         0
End

#COMMAND
Name        recho~
Code        do_recho
Position    100
Level       52
Log         1
End

#COMMAND
Name        redit~
Code        do_redit
Position    100
Level       53
Log         3
End

#COMMAND
Name        regoto~
Code        do_regoto
Position    100
Level       51
Log         0
End

#COMMAND
Name        rent~
Code        do_rent
Position    100
Level       0
Log         0
End

#COMMAND
Name        repairset~
Code        do_repairset
Position    100
Level       54
Log         3
End

#COMMAND
Name        repairshops~
Code        do_repairshops
Position    100
Level       54
Log         0
End

#COMMAND
Name        repairstat~
Code        do_repairstat
Position    100
Level       54
Log         0
End

#COMMAND
Name        reset~
Code        do_reset
Position    100
Level       53
Log         3
End

#COMMAND
Name        restore~
Code        do_restore
Position    100
Level       54
Log         1
End

#COMMAND
Name        restoretime~
Code        do_restoretime
Position    100
Level       54
Log         0
End

#COMMAND
Name        restrict~
Code        do_restrict
Position    100
Level       57
Log         0
End

#COMMAND
Name        return~
Code        do_return
Position    100
Level       51
Log         1
End

#COMMAND
Name        retran~
Code        do_retran
Position    100
Level       52
Log         1
End

#COMMAND
Name        retire~
Code        do_retire
Position    100
Level       59
Log         1
End

#COMMAND
Name        rip~
Code        do_rip
Position    100
Level       0
Log         0
End

#COMMAND
Name        rlist~
Code        do_rlist
Position    100
Level       53
Log         0
End

#COMMAND
Name        rpedit~
Code        do_rpedit
Position    100
Level       54
Log         3
End

#COMMAND
Name        rpstat~
Code        do_rpstat
Position    100
Level       54
Log         0
End

#COMMAND
Name        rstat~
Code        do_rstat
Position    100
Level       53
Log         0
End

#COMMAND
Name        reserve~
Code        do_reserve
Position    100
Level       59
Log         1
End

#COMMAND
Name        racetalk~
Code        do_racetalk
Position    100
Level       2
Log         0
Flags       1
End

#COMMAND
Name        rap~
Code        do_rap
Position    100
Level       51
Log         0
End

#COMMAND
Name        remains~
Code        do_remains
Position    100
Level       5
Log         0
End

#COMMAND
Name        retell~
Code        do_retell
Position    100
Level       1
Log         0
End

#COMMAND
Name        repeat~
Code        do_repeat
Position    100
Level       51
Log         0
End

#COMMAND
Name        revert~
Code        do_revert
Position    100
Level       0
Log         0
End

#COMMAND
Name        roster~
Code        do_roster
Position    100
Level       0
Log         0
End

#COMMAND
Name        south~
Code        do_south
Position    112
Level       0
Log         0
End

#COMMAND
Name        skin~
Code        do_skin
Position    100
Level       2
Log         0
End

#COMMAND
Name        se~
Code        do_southeast
Position    112
Level       0
Log         0
End

#COMMAND
Name        sw~
Code        do_southwest
Position    112
Level       0
Log         0
End

#COMMAND
Name        say~
Code        do_say
Position    106
Level       0
Log         0
End

#COMMAND
Name        save~
Code        do_save
Position    100
Level       0
Log         0
End

#COMMAND
Name        sacrifice~
Code        do_sacrifice
Position    106
Level       0
Log         0
End

#COMMAND
Name        score~
Code        do_score
Position    100
Level       0
Log         0
End

#COMMAND
Name        sleep~
Code        do_sleep
Position    104
Level       0
Log         0
End

#COMMAND
Name        stand~
Code        do_stand
Position    104
Level       0
Log         0
End

#COMMAND
Name        stat~
Code        do_stat
Position    100
Level       50
Log         0
End

#COMMAND
Name        statreport~
Code        do_statreport
Position    100
Level       50
Log         0
End

#COMMAND
Name        savearea~
Code        do_savearea
Position    100
Level       53
Log         3
End

#COMMAND
Name        sell~
Code        do_sell
Position    106
Level       0
Log         0
End

#COMMAND
Name        sedit~
Code        do_sedit
Position    100
Level       58
Log         1
End

#COMMAND
Name        setboot~
Code        do_set_boot_time
Position    100
Level       60
Log         1
End

#COMMAND
Name        setclan~
Code        do_setclan
Position    100
Level       58
Log         1
End

#COMMAND
Name        setcouncil~
Code        do_setcouncil
Position    100
Level       61
Log         1
End

#COMMAND
Name        setmssp~
Code        do_setmssp
Position    100
Level       61
Log         1
End

#COMMAND
Name        shout~
Code        do_shout
Position    106
Level       3
Log         0
Flags       1
End

#COMMAND
Name        shops~
Code        do_shops
Position    100
Level       54
Log         3
End

#COMMAND
Name        shopset~
Code        do_shopset
Position    100
Level       54
Log         1
End

#COMMAND
Name        shopstat~
Code        do_shopstat
Position    100
Level       54
Log         0
End

#COMMAND
Name        shove~
Code        do_shove
Position    112
Level       0
Log         0
End

#COMMAND
Name        showclan~
Code        do_showclan
Position    100
Level       52
Log         0
End

#COMMAND
Name        showcouncil~
Code        do_showcouncil
Position    100
Level       52
Log         0
End

#COMMAND
Name        shutdow~
Code        do_shutdow
Position    100
Level       61
Log         0
End

#COMMAND
Name        shutdown~
Code        do_shutdown
Position    100
Level       61
Log         1
End

#COMMAND
Name        sit~
Code        do_sit
Position    104
Level       0
Log         0
End

#COMMAND
Name        silence~
Code        do_silence
Position    100
Level       51
Log         1
End

#COMMAND
Name        sla~
Code        do_sla
Position    100
Level       57
Log         0
End

#COMMAND
Name        slay~
Code        do_slay
Position    100
Level       57
Log         1
End

#COMMAND
Name        slist~
Code        do_slist
Position    104
Level       0
Log         0
End

#COMMAND
Name        slookup~
Code        do_slookup
Position    100
Level       54
Log         0
End

#COMMAND
Name        smoke~
Code        do_smoke
Position    106
Level       1
Log         0
End

#COMMAND
Name        snoop~
Code        do_snoop
Position    100
Level       55
Log         4
Flags       4
End

#COMMAND
Name        sober~
Code        do_sober
Position    100
Level       52
Log         4
End

#COMMAND
Name        socials~
Code        do_socials
Position    100
Level       0
Log         0
End

#COMMAND
Name        southeast~
Code        do_southeast
Position    112
Level       0
Log         0
End

#COMMAND
Name        southwest~
Code        do_southwest
Position    112
Level       0
Log         0
End

#COMMAND
Name        speak~
Code        do_speak
Position    106
Level       0
Log         0
End

#COMMAND
Name        split~
Code        do_split
Position    106
Level       0
Log         0
End

#COMMAND
Name        sset~
Code        do_sset
Position    100
Level       57
Log         1
End

#COMMAND
Name        switch~
Code        do_switch
Position    100
Level       52
Log         1
Flags       4
End

#COMMAND
Name        setdeity~
Code        do_setdeity
Position    100
Level       61
Log         1
End

#COMMAND
Name        showdeity~
Code        do_showdeity
Position    100
Level       51
Log         0
End

#COMMAND
Name        supplicate~
Code        do_supplicate
Position    100
Level       5
Log         0
End

#COMMAND
Name        showclass~
Code        do_showclass
Position    100
Level       56
Log         0
End

#COMMAND
Name        setclass~
Code        do_setclass
Position    100
Level       60
Log         1
End

#COMMAND
Name        scatter~
Code        do_scatter
Position    100
Level       61
Log         1
End

#COMMAND
Name        style~
Code        do_style
Position    105
Level       1
Log         0
End

#COMMAND
Name        showweather~
Code        do_showweather
Position    100
Level       51
Log         0
End

#COMMAND
Name        setholiday~
Code        do_setholiday
Position    100
Level       60
Log         1
End

#COMMAND
Name        saveholiday~
Code        do_saveholiday
Position    100
Level       60
Log         0
End

#COMMAND
Name        setweather~
Code        do_setweather
Position    100
Level       60
Log         0
End

#COMMAND
Name        strew~
Code        do_strew
Position    100
Level       63
Log         1
End

#COMMAND
Name        showorder~
Code        do_showclan
Position    100
Level       52
Log         0
End

#COMMAND
Name        showguild~
Code        do_showclan
Position    100
Level       52
Log         0
End

#COMMAND
Name        strip~
Code        do_strip
Position    100
Level       59
Log         1
End

#COMMAND
Name        showrace~
Code        do_showrace
Position    100
Level       51
Log         0
End

#COMMAND
Name        setrace~
Code        do_setrace
Position    100
Level       60
Log         0
End

#COMMAND
Name        showliquid~
Code        do_showliquid
Position    100
Level       51
Log         0
End

#COMMAND
Name        setliquid~
Code        do_setliquid
Position    100
Level       56
Log         0
End

#COMMAND
Name        showmixture~
Code        do_showmixture
Position    100
Level       51
Log         0
End

#COMMAND
Name        setmixture~
Code        do_setmixture
Position    100
Level       56
Log         0
End

#COMMAND
Name        tell~
Code        do_tell
Position    106
Level       0
Log         0
Flags       1
End

#COMMAND
Name        take~
Code        do_get
Position    106
Level       0
Log         0
End

#COMMAND
Name        tamp~
Code        do_tamp
Position    106
Level       0
Log         0
End

#COMMAND
Name        think~
Code        do_think
Position    100
Level       58
Log         0
Flags       1
End

#COMMAND
Name        time~
Code        do_time
Position    100
Level       0
Log         0
End

#COMMAND
Name        timezone~
Code        do_timezone
Position    100
Level       0
Log         0
End

#COMMAND
Name        title~
Code        do_title
Position    100
Level       0
Log         0
End

#COMMAND
Name        transfer~
Code        do_transfer
Position    100
Level       52
Log         1
End

#COMMAND
Name        traffic~
Code        do_traffic
Position    100
Level       0
Log         0
End

#COMMAND
Name        trust~
Code        do_trust
Position    100
Level       65
Log         1
End

#COMMAND
Name        typo~
Code        do_typo
Position    100
Level       0
Log         0
End

#COMMAND
Name        timecmd~
Code        do_timecmd
Position    100
Level       65
Log         0
End

#COMMAND
Name        up~
Code        do_up
Position    112
Level       0
Log         0
End

#COMMAND
Name        unmorph~
Code        do_imm_unmorph
Position    100
Level       51
Log         0
End

#COMMAND
Name        unlock~
Code        do_unlock
Position    106
Level       0
Log         0
End

#COMMAND
Name        unfoldarea~
Code        do_unfoldarea
Position    100
Level       65
Log         1
End

#COMMAND
Name        unhell~
Code        do_unhell
Position    100
Level       51
Log         4
End

#COMMAND
Name        users~
Code        do_users
Position    100
Level       54
Log         0
End

#COMMAND
Name        unsilence~
Code        do_unsilence
Position    100
Level       51
Log         1
End

#COMMAND
Name        unnuisance~
Code        do_unnuisance
Position    100
Level       54
Log         1
End

#COMMAND
Name        unbolt~
Code        do_unbolt
Position    100
Level       0
Log         0
End

#COMMAND
Name        value~
Code        do_value
Position    106
Level       0
Log         0
End

#COMMAND
Name        vassign~
Code        do_vassign
Position    100
Level       61
Log         1
End

#COMMAND
Name        version~
Code        do_version
Position    100
Level       0
Log         0
End

#COMMAND
Name        vstat~
Code        do_vstat
Position    100
Level       54
Log         0
End

#COMMAND
Name        visible~
Code        do_visible
Position    104
Level       0
Log         0
End

#COMMAND
Name        vnums~
Code        do_vnums
Position    100
Level       55
Log         0
End

#COMMAND
Name        vsearch~
Code        do_vsearch
Position    100
Level       58
Log         0
End

#COMMAND
Name        victories~
Code        do_victories
Position    100
Level       10
Log         0
End

#COMMAND
Name        west~
Code        do_west
Position    112
Level       0
Log         0
End

#COMMAND
Name        wake~
Code        do_wake
Position    104
Level       0
Log         0
End

#COMMAND
Name        wartalk~
Code        do_wartalk
Position    104
Level       2
Log         0
Flags       1
End

#COMMAND
Name        who~
Code        do_who
Position    100
Level       0
Log         0
End

#COMMAND
Name        wear~
Code        do_wear
Position    107
Level       0
Log         0
End

#COMMAND
Name        whois~
Code        do_whois
Position    100
Level       0
Log         0
End

#COMMAND
Name        wield~
Code        do_wear
Position    107
Level       0
Log         0
End

#COMMAND
Name        where~
Code        do_where
Position    106
Level       0
Log         0
End

#COMMAND
Name        weather~
Code        do_weather
Position    106
Level       0
Log         0
End

#COMMAND
Name        wimpy~
Code        do_wimpy
Position    100
Level       0
Log         0
End

#COMMAND
Name        wizlist~
Code        do_wizlist
Position    104
Level       0
Log         0
End

#COMMAND
Name        wizhelp~
Code        do_wizhelp
Position    100
Level       51
Log         0
End

#COMMAND
Name        wizlock~
Code        do_wizlock
Position    100
Level       61
Log         1
End

#COMMAND
Name        warn~
Code        do_warn
Position    100
Level       58
Log         0
End

#COMMAND
Name        watch~
Code        do_watch
Position    100
Level       55
Log         0
End

#COMMAND
Name        worth~
Code        do_worth
Position    100
Level       2
Log         0
End

#COMMAND
Name        whisper~
Code        do_whisper
Position    100
Level       1
Log         0
Flags       1
End

#COMMAND
Name        yell~
Code        do_yell
Position    106
Level       0
Log         0
Flags       1
End

#COMMAND
Name        zap~
Code        do_zap
Position    106
Level       0
Log         0
End

#COMMAND
Name        zones~
Code        do_zones
Position    100
Level       55
Log         0
End

#END
--]]

-- EOF