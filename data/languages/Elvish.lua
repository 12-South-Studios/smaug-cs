-- ELVISH.LUA
-- This is the Elvish Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	-- 'flutey' language.. slam some harsh sounds..
	newLang = LCreateLanguage("elvish", "Elven");
	lang.this = newLang;
	
	lang.this:AddPreConversion("hour time", "lumenn");
	lang.this:AddPreConversion("meeting gathering council", "omentielvo");
	lang.this:AddPreConversion("shine shiny silver", "sila");
	lang.this:AddPreConversion("metal steel", "tinco");
	lang.this:AddPreConversion("book read scroll", "parma");
	lang.this:AddPreConversion("lamp light see", "calma");
	lang.this:AddPreConversion("feather bird fly", "quesse");
	lang.this:AddPreConversion("gate entrance enter", "ando");
	lang.this:AddPreConversion("fate death", "umbar");
	lang.this:AddPreConversion("iron sword", "anga");
	lang.this:AddPreConversion("web net", "ungwe");
	lang.this:AddPreConversion("spirit ghost", "thule");
	lang.this:AddPreConversion("north", "formen");
	lang.this:AddPreConversion("treasure loot", "harma");
	lang.this:AddPreConversion("rage anger", "aha");
	lang.this:AddPreConversion("breeze wind", "hwesta");
	lang.this:AddPreConversion("mouth eat", "anto");
	lang.this:AddPreConversion("hook", "ampa");
	lang.this:AddPreConversion("jaws", "anca");
	lang.this:AddPreConversion("hollow empty", "unque");
	lang.this:AddPreConversion("west", "numen");
	lang.this:AddPreConversion("gold coin", "malta");
	lang.this:AddPreConversion("torment torture", "nwalme");
	lang.this:AddPreConversion("heart love", "ore");
	lang.this:AddPreConversion("power energy", "vaya");
	lang.this:AddPreConversion("gift", "ana");
	lang.this:AddPreConversion("air sky", "vilna");
	lang.this:AddPreConversion("east", "romen");
	lang.this:AddPreConversion("region kindgom", "arta");
	lang.this:AddPreConversion("tongue language", "lambe");
	lang.this:AddPreConversion("tree", "alda");
	lang.this:AddPreConversion("starlight", "selme");
	lang.this:AddPreConversion("name", "eshe");
	lang.this:AddPreConversion("sunlight", "are");
	lang.this:AddPreConversion("south", "hyarmen");
	lang.this:AddPreConversion("bridge", "yanta");
	lang.this:AddPreConversion("heat fire", "ure");
	lang.this:AddPreConversion("say speak", "pedo");
	lang.this:AddPreConversion("friend", "malin");
	lang.this:AddPreConversion("enter", "minno");
	lang.this:AddPreConversion("them other", "hain");
	lang.this:AddPreConversion("made create", "echant");
	lang.this:AddPreConversion("drew write", "teithant");
	lang.this:AddPreConversion("these", "thiw");
	lang.this:AddPreConversion("sign", "hin");
	lang.this:AddPreConversion("spring water", "thuile");
	lang.this:AddPreConversion("summer", "laire");
	lang.this:AddPreConversion("autumn", "yavie");
	lang.this:AddPreConversion("fade age", "quelle");
	lang.this:AddPreConversion("winter", "hrive");
	lang.this:AddPreConversion("stirring", "coire");
	lang.this:AddPreConversion("dream sleep", "loren");
	lang.this:AddPreConversion("bow hunt", "luva");
	lang.this:AddPreConversion("grow", "loa");
	lang.this:AddPreConversion("mist fog", "hisime");
	lang.this:AddPreConversion("century", "haranye");
	lang.this:AddPreConversion("week", "enquie");
	lang.this:AddPreConversion("star", "elen");
	lang.this:AddPreConversion("moon", "isin");
	lang.this:AddPreConversion("stone earth", "ssar");
	lang.this:AddPreConversion("and", "owe");
	lang.this:AddPreConversion("of", "ane");
	lang.this:AddPreConversion("xx", "sth");
	lang.this:AddPreConversion("g z", "s v");
	lang.this:AddPreConversion("th", "n");
	lang.this:AddPreConversion("ng", "r");
	lang.this:AddPreConversion("st", "ma");
	lang.this:AddPreConversion("sh", "vr");
	lang.this:AddPreConversion("sc", "a");
	lang.this:AddPreConversion("ck", "ss");
	lang.this:AddPreConversion("r", "th");
	lang.this:AddPreConversion("d", "ng");
	lang.this:AddPreConversion("p", "st");
	lang.this:AddPreConversion("j", "sh");
	lang.this:AddPreConversion("z", "dh");
	lang.this:AddPreConversion("es", "a");
	
	lang.this.Alphabet = "iqqdakvtujfwghepcrslybszoz";
	
	lang.this:AddPostConversion("rr", "r");
	lang.this:AddPostConversion("qq", "q");
end

LoadLanguage();

-- EOF