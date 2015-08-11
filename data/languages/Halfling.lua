-- HAHFLING.LUA
-- This is the Halfling Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	newLang = LCreateLanguage("halfling", "Halfling");
	lang.this = newLang;
	
	lang.this:AddPreConversion("halfling", "banakil");
	lang.this:AddPreConversion("hobbit", "kuduk");
	lang.this:AddPreConversion("horrible terrible bad", "balc");
	lang.this:AddPreConversion("simple base single", "banazir");
	lang.this:AddPreConversion("half", "ban");
	lang.this:AddPreConversion("quick fast", "bara");
	lang.this:AddPreConversion("wich", "bas");
	lang.this:AddPreConversion("talk talker speak", "batta");
	lang.this:AddPreConversion("bulge hill pile", "bolg");
	lang.this:AddPreConversion("heady", "bralda");
	lang.this:AddPreConversion("border march", "branda");
	lang.this:AddPreConversion("coin gold", "castar");
	lang.this:AddPreConversion("stay stand", "gad");
	lang.this:AddPreConversion("game play", "galab");
	lang.this:AddPreConversion("goat", "gamba");
	lang.this:AddPreConversion("valley gulch", "gul");
	lang.this:AddPreConversion("ale drink", "him");
	lang.this:AddPreConversion("cottage cot house", "hloth");
	lang.this:AddPreConversion("cotton cloth", "hlothran");
	lang.this:AddPreConversion("merry jolly gay happy", "kali");
	lang.this:AddPreConversion("cloven split", "karnin");
	lang.this:AddPreConversion("bag sack", "laban");
	lang.this:AddPreConversion("fluff", "luthran");
	lang.this:AddPreConversion("dwarf dwarven", "narag");
	lang.this:AddPreConversion("people men human", "nas");
	lang.this:AddPreConversion("end final stop", "neg");
	lang.this:AddPreConversion("water", "nin");
	lang.this:AddPreConversion("speech orate", "phare");
	lang.this:AddPreConversion("delve dig mine", "phur");
	lang.this:AddPreConversion("blower", "puta");
	lang.this:AddPreConversion("blow", "put");
	lang.this:AddPreConversion("village town", "ran");
	lang.this:AddPreConversion("burr shiver", "raph");
	lang.this:AddPreConversion("horn", "ras");
	lang.this:AddPreConversion("stranger outsider", "raza");
	lang.this:AddPreConversion("apple fruit", "razar");
	lang.this:AddPreConversion("common", "soval");
	lang.this:AddPreConversion("shire region", "suza");
	lang.this:AddPreConversion("rabbit coney", "rapuc");
	lang.this:AddPreConversion("quarter fourth", "tharantin");
	lang.this:AddPreConversion("smial", "tran");
	lang.this:AddPreConversion("guard soldier warrior", "tudnas");
	lang.this:AddPreConversion("old elder", "zara");
	lang.this:AddPreConversion("wise wisdom", "zir");
	lang.this:AddPreConversion("butter", "zilib");
	
	lang.this.Alphabet = "epcdalbhojcrnmigqfsbuvvxyz";
end

LoadLanguage();

-- EOF