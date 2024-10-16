﻿using System.Collections.Generic;
using System.Linq;
using Library.Common.Extensions;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants;

public static class LookupConstants
{
    public static int GetFlagIndex(string name, IEnumerable<string> values)
    {
        return values.ToList().FindIndex(x => x.EqualsIgnoreCase(name));
    }

    public static readonly List<int> PartVnums =
    [
        12,
        /* Head */
        14,
        /* arms */
        15,
        /* legs */
        13,
        /* heart */
        44,
        /* brains */
        16,
        /* guts */
        45,
        /* hands */
        46,
        /* feet */
        47,
        /* fingers */
        48,
        /* ear */
        49,
        /* eye */
        50,
        /* long_tongue */
        51,
        /* eyestalks */
        52,
        /* tentacles */
        53,
        /* fins */
        54,
        /* wings */
        55,
        /* tail */
        56,
        /* scales */
        59,
        /* claws */
        87,
        /* fangs */
        58,
        /* horns */
        57,
        /* tusks */
        55,
        /* tailattack */
        85,
        /* sharpscales */
        84,
        /* beak */
        86,
        /* haunches */
        83,
        /* hooves */
        82,
        /* paws */
        81,
        /* forelegs */
        80,
        /* feathers */
        0,
        /* r1 */
        0
    ];

    public static string CANT_PRAC = "Tongue";

    public static readonly List<string> pc_displays =
    [
        "black",
        "dred",
        "dgreen",
        "orange",
        "dblue",
        "purple",
        "cyan",
        "grey",
        "dgrey",
        "red",
        "green",
        "yellow",
        "blue",
        "pink",
        "lblue",
        "white",
        "blink",
        "bdred",
        "bdgreen",
        "bdorange",
        "bdblue",
        "bpurple",
        "bcyan",
        "bgrey",
        "bdgrey",
        "bred",
        "bgreen",
        "byellow",
        "bblue",
        "bpink",
        "blblue",
        "bwhite",
        "plain",
        "action",
        "say",
        "chat",
        "yells",
        "tell",
        "hit",
        "hitme",
        "immortal",
        "hurt",
        "falling",
        "danger",
        "magic",
        "consider",
        "report",
        "poison",
        "social",
        "dying",
        "dead",
        "skill",
        "carnage",
        "damage",
        "fleeing",
        "rmname",
        "rmdesc",
        "objects",
        "people",
        "list",
        "bye",
        "gold",
        "gtells",
        "note",
        "hungry",
        "thirsty",
        "fire",
        "sober",
        "wearoff",
        "exits",
        "score",
        "reset",
        "log",
        "die_msg",
        "wartalk",
        "arena",
        "muse",
        "think",
        "aflags",
        "who",
        "racetalk",
        "ignore",
        "whisper",
        "divider",
        "CurrentMorph",
        "shout",
        "rflags",
        "stype",
        "aname",
        "auction",
        "score2",
        "score3",
        "score4",
        "who2",
        "who3",
        "who4",
        "intermud",
        "helpfiles",
        "who5",
        "score5",
        "who6",
        "who7",
        "prac",
        "prac2",
        "prac3",
        "prac4",
        "mxpprompt",
        "guildtalk",
        "board",
        "board2",
        "board3"
    ];

    public static List<ATTypes> default_set =
    [
        ATTypes.None,
        ATTypes.AT_BLOOD,
        ATTypes.AT_DGREEN,
        ATTypes.AT_ORANGE,
        /*  3 */
        ATTypes.AT_DBLUE,
        ATTypes.AT_PURPLE,
        ATTypes.AT_CYAN,
        ATTypes.AT_GREY,
        /*  7 */
        ATTypes.AT_DGREY,
        ATTypes.AT_RED,
        ATTypes.AT_GREEN,
        ATTypes.AT_YELLOW,
        /* 11 */
        ATTypes.AT_BLUE,
        ATTypes.AT_PINK,
        ATTypes.AT_LBLUE,
        ATTypes.AT_WHITE,
        /* 15 */
        ATTypes.AT_BLACK_BLINK,
        ATTypes.AT_BLOOD_BLINK,
        ATTypes.AT_DGREEN_BLINK,
        ATTypes.AT_ORANGE_BLINK,
        /* 19 */
        ATTypes.AT_DBLUE_BLINK,
        ATTypes.AT_PURPLE_BLINK,
        ATTypes.AT_CYAN_BLINK,
        ATTypes.AT_GREY_BLINK,
        /* 23 */
        ATTypes.AT_DGREY_BLINK,
        ATTypes.AT_RED_BLINK,
        ATTypes.AT_GREEN_BLINK,
        ATTypes.AT_YELLOW_BLINK,
        /* 27 */
        ATTypes.AT_BLUE_BLINK,
        ATTypes.AT_PINK_BLINK,
        ATTypes.AT_LBLUE_BLINK,
        ATTypes.AT_WHITE_BLINK,
        /* 31 */
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        ATTypes.AT_BLUE,
        /* 34 */
        ATTypes.AT_GREEN,
        ATTypes.AT_LBLUE,
        ATTypes.AT_WHITE,
        ATTypes.AT_GREY,
        /* 38 */
        ATTypes.AT_GREY,
        ATTypes.AT_YELLOW,
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        /* 42 */
        ATTypes.AT_GREY,
        ATTypes.AT_BLUE,
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        /* 46 */
        ATTypes.AT_DGREEN,
        ATTypes.AT_CYAN,
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        /* 50 */
        ATTypes.AT_BLUE,
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        /* 54 */
        ATTypes.AT_RED,
        ATTypes.AT_GREY,
        ATTypes.AT_BLUE,
        ATTypes.AT_PINK,
        /* 58 */
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        ATTypes.AT_YELLOW,
        ATTypes.AT_GREY,
        /* 62 */
        ATTypes.AT_GREY,
        ATTypes.AT_ORANGE,
        ATTypes.AT_BLUE,
        ATTypes.AT_RED,
        /* 66 */
        ATTypes.AT_GREY,
        ATTypes.AT_GREY,
        ATTypes.AT_GREEN,
        ATTypes.AT_DGREEN,
        /* 70 */
        ATTypes.AT_DGREEN,
        ATTypes.AT_ORANGE,
        ATTypes.AT_GREY,
        ATTypes.AT_RED,
        /* 74 */
        ATTypes.AT_GREY,
        ATTypes.AT_DGREEN,
        ATTypes.AT_RED,
        ATTypes.AT_BLUE,
        /* 78 */
        ATTypes.AT_RED,
        ATTypes.AT_CYAN,
        ATTypes.AT_YELLOW,
        ATTypes.AT_PINK,
        /* 82 */
        ATTypes.AT_DGREEN,
        ATTypes.AT_PINK,
        ATTypes.AT_WHITE,
        ATTypes.AT_BLUE,
        /* 86 */
        ATTypes.AT_BLUE,
        ATTypes.AT_BLUE,
        ATTypes.AT_GREEN,
        ATTypes.AT_GREY,
        /* 90 */
        ATTypes.AT_GREEN,
        ATTypes.AT_GREEN,
        ATTypes.AT_YELLOW,
        ATTypes.AT_DGREY,
        /* 94 */
        ATTypes.AT_GREEN,
        ATTypes.AT_PINK,
        ATTypes.AT_DGREEN,
        ATTypes.AT_CYAN,
        /* 98 */
        ATTypes.AT_RED,
        ATTypes.AT_WHITE,
        ATTypes.AT_BLUE,
        ATTypes.AT_DGREEN,
        /* 102 */
        ATTypes.AT_CYAN,
        ATTypes.AT_BLOOD,
        ATTypes.AT_RED,
        ATTypes.AT_DGREEN,
        /* 106 */
        ATTypes.AT_GREEN,
        ATTypes.AT_GREY,
        ATTypes.AT_GREEN,
        ATTypes.AT_WHITE
    ];

    public static List<TimezoneData> tzone_table =
    [
        new TimezoneData("GMT-12", "Eniwetok", -12, 0),
        new TimezoneData("GMT-11", "Samoa", -11, 0),
        new TimezoneData("GMT-10", "Hawaii", -10, 0),
        new TimezoneData("GMT-9", "Alaska", -9, 0),
        new TimezoneData("GMT-8", "Pacific US", -8, -7),
        new TimezoneData("GMT-7", "Mountain US", -7, -6),
        new TimezoneData("GMT-6", "Central US", -6, -5),
        new TimezoneData("GMT-5", "Eastern US", -5, -4),
        new TimezoneData("GMT-4", "Atlantic, Canada", -4, 0),
        new TimezoneData("GMT-3", "Brazilia, Buenos Aries", -3, 0),
        new TimezoneData("GMT-2", "Mid-Atlantic", -2, 0),
        new TimezoneData("GMT-1", "Cape Verdes", -1, 0),
        new TimezoneData("GMT", "Greenwich Mean Time, Greenwich", 0,
            0),

        new TimezoneData("GMT+1", "Berlin, Rome", 1, 0),
        new TimezoneData("GMT+2", "Israel, Cairo", 2, 0),
        new TimezoneData("GMT+3", "Moscow, Kuwait", 3, 0),
        new TimezoneData("GMT+4", "Abu Dhabi, Muscat", 4, 0),
        new TimezoneData("GMT+5", "Islamabad, Karachi", 5, 0),
        new TimezoneData("GMT+6", "Almaty, Dhaka", 6, 0),
        new TimezoneData("GMT+7", "Bangkok, Jakarta", 7, 0),
        new TimezoneData("GMT+8", "Hong Kong, Beijing", 8, 0),
        new TimezoneData("GMT+9", "Tokyo, Osaka", 9, 0),
        new TimezoneData("GMT+10", "Sydney, Melbourne, Guam", 10, 0),
        new TimezoneData("GMT+11", "Magadan, Soloman Is.", 11, 0),
        new TimezoneData("GMT+12", "Fiji, Wellington, Auckland", 12,
            0)
    ];

    public static List<string> save_flag =
    [
        "death",
        "kill",
        "passwd",
        "drop",
        "put",
        "give",
        "auto",
        "zap",
        "auction",
        "get",
        "receive",
        "idle",
        "backup",
        "quitbackup",
        "fill",
        "empty",
        "r16",
        "r17",
        "r18",
        "r19",
        "r20",
        "r21",
        "r22",
        "r23",
        "r24",
        "r25",
        "r26",
        "r27",
        "r28",
        "r29",
        "r30",
        "r31"
    ];


    public static readonly List<short> rev_dir = [2, 3, 0, 1, 5, 4, 9, 8, 7, 6, 10];


    public static readonly List<int> SentTotals = [3, 5, 4, 4, 1, 1, 1, 1, 1, 2, 2, 25, 1, 1];

    public static readonly List<List<string>> RoomSents =
    [
        [
            "rough hewn walls of granite with the occasional spider crawling around",
            "signs of a recent battle from the bloodstains on the floor",
            "a damp musty odour not unlike rotting vegetation"
        ],


        [
            "the occasional stray digging through some garbage",
            "merchants trying to lure customers to their tents",
            "some street people putting on an interesting display of talent",
            "an argument between a customer and a merchant about the price of an item",
            "several shady figures talking down a dark alleyway"
        ],


        [
            "sparce patches of brush and shrubs",
            "a small cluster of trees far off in the distance",
            "grassy fields as far as the eye can see",
            "a wide variety of weeds and wildflowers"
        ],


        [
            "tall, dark evergreens prevent you from seeing very far",
            "many huge oak trees that look several hundred years old",
            "a solitary lonely weeping willow",
            "a patch of bright white birch trees slender and tall"
        ],


        ["rolling hills lightly speckled with violet wildflowers"],


        ["the rocky mountain pass offers many hiding places"],


        ["the water is smooth as glass"],


        ["rough waves splash about angrily"],


        ["a small school of fish"],


        [
            "the land far below",
            "a misty haze of clouds"
        ],


        [
            "sand as far as the eye can see",
            "an oasis far in the distance"
        ],


        [
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual",
            "nothing unusual"
        ],


        ["rocks and coral which litter the ocean floor."],


        ["a lengthy tunnel of rock."]
    ];

    /****************** CONSTELLATIONS and STARS *****************************
            Cygnus     Mars        Orion      Dragon       Cassiopeia          Venus
                    Ursa Ninor                           Mercurius     Pluto
                        Uranus              Leo                Crown       Raptor
        *************************************************************************/

    public static readonly Dictionary<char, string> StarCharacterMap = new()
    {
        {':', ":"}, {'.', "."}, {'*', "*"}, {'G', "&G"}, {'g', "&g"}, {'R', "&R"}, {'r', "&r"},
        {'C', "&C"}, {'O', "&O"}, {'B', "&B"}, {'P', "&P"}, {'W', "&W"}, {'b', "&b"}, {'p', "&p"},
        {'Y', "&Y"}, {'c', "&c"}
    };

    //public static readonly List<str_app_type> str_app = new List<str_app_type>
    //{
    //                                                   new str_app_type(-5, -4, 0, 0),
    //                                                   new str_app_type(-5, -4, 3, 1),
    //                                                   new str_app_type(-3, -2, 3, 2),
    //                                                   new str_app_type(-3, -1, 10, 3),
    //                                                   new str_app_type(-2, -1, 55, 5),
    //                                                   new str_app_type(-1, 0, 80, 6),
    //                                                   new str_app_type(-1, 0, 90, 7),
    //                                                   new str_app_type(0, 0, 100, 8),
    //                                                   new str_app_type(0, 0, 100, 9),
    //                                                   new str_app_type(0, 0, 115, 10),
    //                                                   new str_app_type(0, 0, 115, 11),
    //                                                   new str_app_type(0, 0, 140, 12),
    //                                                   new str_app_type(0, 0, 140, 13),
    //                                                   new str_app_type(0, 1, 170, 14),
    //                                                   new str_app_type(1, 1, 170, 15),
    //                                                   new str_app_type(1, 2, 195, 16),
    //                                                   new str_app_type(2, 3, 220, 22),
    //                                                   new str_app_type(2, 4, 250, 25),
    //                                                   new str_app_type(3, 5, 400, 30),
    //                                                   new str_app_type(3, 6, 500, 35),
    //                                                   new str_app_type(4, 7, 600, 40),
    //                                                   new str_app_type(5, 7, 700, 45),
    //                                                   new str_app_type(6, 8, 800, 50),
    //                                                   new str_app_type(8, 10, 900, 55),
    //                                                   new str_app_type(10, 12, 999, 60)
    //                                               };

    public static List<int_app_type> int_app =
    [
        new int_app_type(3),
        new int_app_type(5),
        new int_app_type(7),
        new int_app_type(8),
        new int_app_type(9),
        new int_app_type(10),
        new int_app_type(11),
        new int_app_type(12),
        new int_app_type(13),
        new int_app_type(15),
        new int_app_type(17),
        new int_app_type(19),
        new int_app_type(22),
        new int_app_type(25),
        new int_app_type(28),
        new int_app_type(31),
        new int_app_type(34),
        new int_app_type(37),
        new int_app_type(40),
        new int_app_type(44),
        new int_app_type(49),
        new int_app_type(55),
        new int_app_type(60),
        new int_app_type(70),
        new int_app_type(85),
        new int_app_type(99)
    ];

    public static readonly List<wis_app_type> wis_app =
    [
        new wis_app_type(0),
        new wis_app_type(0),
        new wis_app_type(0),
        new wis_app_type(0), // 3
        new wis_app_type(0),
        new wis_app_type(1),
        new wis_app_type(1),
        new wis_app_type(1),
        new wis_app_type(1),
        new wis_app_type(2),
        new wis_app_type(2), // 10
        new wis_app_type(2),
        new wis_app_type(2),
        new wis_app_type(2),
        new wis_app_type(2),
        new wis_app_type(3), // 15
        new wis_app_type(3),
        new wis_app_type(4),
        new wis_app_type(5), // 18
        new wis_app_type(5),
        new wis_app_type(5),
        new wis_app_type(6), // 21
        new wis_app_type(6),
        new wis_app_type(6),
        new wis_app_type(6),
        new wis_app_type(7)
    ];

    public static readonly List<dex_app_type> dex_app =
    [
        new dex_app_type(60),
        new dex_app_type(50),
        new dex_app_type(50),
        new dex_app_type(40),
        new dex_app_type(30),
        new dex_app_type(20), // 5
        new dex_app_type(10),
        new dex_app_type(0),
        new dex_app_type(0),
        new dex_app_type(0),
        new dex_app_type(0), // 10
        new dex_app_type(0),
        new dex_app_type(0),
        new dex_app_type(0),
        new dex_app_type(0),
        new dex_app_type(-10), // 15
        new dex_app_type(-15),
        new dex_app_type(-20),
        new dex_app_type(-30),
        new dex_app_type(-40),
        new dex_app_type(-50), // 20
        new dex_app_type(-60),
        new dex_app_type(-75),
        new dex_app_type(-90),
        new dex_app_type(-105),
        new dex_app_type(-120)
    ];

    public static readonly List<con_app_type> con_app =
    [
        new con_app_type(-4, 20),
        new con_app_type(-3, 25),
        new con_app_type(-2, 30),
        new con_app_type(-2, 35), // 3
        new con_app_type(-1, 40),
        new con_app_type(-1, 45),
        new con_app_type(-1, 50),
        new con_app_type(0, 55),
        new con_app_type(0, 60),
        new con_app_type(0, 65),
        new con_app_type(0, 70), // 10
        new con_app_type(0, 75),
        new con_app_type(0, 80),
        new con_app_type(0, 85),
        new con_app_type(0, 88),
        new con_app_type(1, 90), // 15
        new con_app_type(2, 95),
        new con_app_type(2, 97),
        new con_app_type(3, 99),
        new con_app_type(3, 99),
        new con_app_type(4, 99), // 20
        new con_app_type(4, 99),
        new con_app_type(5, 99),
        new con_app_type(6, 99),
        new con_app_type(7, 99),
        new con_app_type(8, 99)
    ];

    public static List<cha_app_type> cha_app =
    [
        new cha_app_type(-60),
        new cha_app_type(-50),
        new cha_app_type(-50),
        new cha_app_type(-40), // 3
        new cha_app_type(-30),
        new cha_app_type(-20),
        new cha_app_type(-10),
        new cha_app_type(-5),
        new cha_app_type(-1),
        new cha_app_type(0),
        new cha_app_type(0), // 10
        new cha_app_type(0),
        new cha_app_type(0),
        new cha_app_type(0),
        new cha_app_type(1),
        new cha_app_type(5), // 15
        new cha_app_type(10),
        new cha_app_type(20),
        new cha_app_type(30),
        new cha_app_type(40),
        new cha_app_type(50), // 20
        new cha_app_type(60),
        new cha_app_type(70),
        new cha_app_type(80),
        new cha_app_type(90),
        new cha_app_type(99)
    ];

    public static List<lck_app_type> lck_app =
    [
        new lck_app_type(60),
        new lck_app_type(50),
        new lck_app_type(50),
        new lck_app_type(40), // 3
        new lck_app_type(30),
        new lck_app_type(20),
        new lck_app_type(10),
        new lck_app_type(0),
        new lck_app_type(0),
        new lck_app_type(0),
        new lck_app_type(0), // 10
        new lck_app_type(0),
        new lck_app_type(0),
        new lck_app_type(0),
        new lck_app_type(0),
        new lck_app_type(-10), // 15
        new lck_app_type(-15),
        new lck_app_type(-20),
        new lck_app_type(-30),
        new lck_app_type(-40),
        new lck_app_type(-50), // 20
        new lck_app_type(-60),
        new lck_app_type(-75),
        new lck_app_type(-90),
        new lck_app_type(-105),
        new lck_app_type(-120)
    ];


    public static IEnumerable<string> s_message_table(string type)
    {
        switch (type.ToLower())
        {
            case "slice":
            case "stab":
            case "slash":
            case "claw":
            case "bite":
            case "pierce":
                return Program.LookupManager.GetLookups("SlashBladeMessages");
            case "whip":
            case "pound":
            case "crush":
            case "suction":
                return Program.LookupManager.GetLookups("SlashBluntMessages");
            default:
                return Program.LookupManager.GetLookups("SlashGenericMessages");
        }
    }

    public static IEnumerable<string> p_message_Table(string type)
    {
        switch (type.ToLower())
        {
            case "slice":
            case "stab":
            case "slash":
            case "claw":
            case "bite":
            case "pierce":
                return Program.LookupManager.GetLookups("PierceBladeMessages");
            case "whip":
            case "pound":
            case "crush":
            case "suction":
                return Program.LookupManager.GetLookups("PierceBluntMessages");
            default:
                return Program.LookupManager.GetLookups("PierceGenericMessages");
        }
    }

    public static List<List<string>> preciptemp_msg =
    [
        [
            "Frigid temperatures settle over the land",
            "It is bitterly cold",
            "The weather is crisp and dry",
            "A comfortable warmth sets in",
            "A dry heat warms the land",
            "Seething heat bakes the land"
        ],

        [
            "A few flurries drift from the high clouds",
            "Frozen drops of rain fall from the sky",
            "An occasional raindrop falls to the ground",
            "Mild drops of rain seep from the clouds",
            "It is very warm, and the sky is overcast",
            "High humidity intensifies the seering heat"
        ],

        [
            "A brief snow squall dusts the earth",
            "A light flurry dusts the ground",
            "Light snow drifts down from the heavens",
            "A light drizzle mars an otherwise perfect day",
            "A few drops of rain fall to the warm ground",
            "A light rain falls through the sweltering sky"
        ],

        [
            "Snowfall covers the frigid earth",
            "Light snow falls to the ground",
            "A brief shower moistens the crisp air",
            "A pleasant rain falls from the heavens",
            "The warm air is heavy with rain",
            "A refreshing shower eases the oppresive heat"
        ],

        [
            "Sleet falls in sheets through the frosty air",
            "Snow falls quickly, piling upon the cold earth",
            "Rain pelts the ground on this crisp day",
            "Rain drums the ground rythmically",
            "A warm rain drums the ground loudly",
            "Tropical rain showers pelt the seering ground"
        ],

        [
            "A downpour of frozen rain covers the land in ice",
            "A blizzard blankets everything in pristine white",
            "Torrents of rain fall from a cool sky",
            "A drenching downpour obscures the temperate day",
            "Warm rain pours from the sky",
            "A torrent of rain soaks the heated earth"
        ]
    ];

    public static List<List<string>> windtemp_msg =
    [
        [
            "The frigid air is completely still",
            "A cold temperature hangs over the area",
            "The crisp air is eerily calm",
            "The warm air is still",
            "No wind makes the day uncomfortably warm",
            "The stagnant heat is sweltering"
        ],

        [
            "A light breeze makes the frigid air seem colder",
            "A stirring of the air intensifies the cold",
            "A touch of wind makes the day cool",
            "It is a temperate day, with a slight breeze",
            "It is very warm, the air stirs slightly",
            "A faint breeze stirs the feverish air"
        ],

        [
            "A breeze gives the frigid air bite",
            "A breeze swirls the cold air",
            "A lively breeze cools the area",
            "It is a temperate day, with a pleasant breeze",
            "Very warm breezes buffet the area",
            "A breeze ciculates the sweltering air"
        ],

        [
            "Stiff gusts add cold to the frigid air",
            "The cold air is agitated by gusts of wind",
            "Wind blows in from the north, cooling the area",
            "Gusty winds mix the temperate air",
            "Brief gusts of wind punctuate the warm day",
            "Wind attempts to cut the sweltering heat"
        ],

        [
            "The frigid air whirls in gusts of wind",
            "A strong, cold wind blows in from the north",
            "Strong wind makes the cool air nip",
            "It is a pleasant day, with gusty winds",
            "Warm, gusty winds move through the area",
            "Blustering winds punctuate the seering heat"
        ],

        [
            "A frigid gale sets bones shivering",
            "Howling gusts of wind cut the cold air",
            "An angry wind whips the air into a frenzy",
            "Fierce winds tear through the tepid air",
            "Gale-like winds whip up the warm air",
            "Monsoon winds tear the feverish air"
        ]
    ];

    public static List<string> precip_msg =
    [
        "there is not a cloud in the sky",
        "pristine white clouds are in the sky",
        "thick, grey clouds mask the sun"
    ];

    public static List<string> wind_msg =
    [
        "there is not a breath of wind in the air",
        "a slight breeze stirs the air",
        "a breeze wafts through the area",
        "brief gusts of wind punctuate the air",
        "angry gusts of wind blow",
        "howling winds whip the air into a frenzy"
    ];
}