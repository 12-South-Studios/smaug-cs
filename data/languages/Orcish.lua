-- ORCISH.LUA
-- This is the Orcish Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	-- basically the same as goblin... a little nastier
	newLang = LCreateLanguage("orcish", "Orcish");
	lang.this = newLang;
	lang.this:AddPreConversion("and", "agh");
	lang.this:AddPreConversion("one", "ash");
	lang.this:AddPreConversion("cess", "bag");
	lang.this:AddPreConversion("pool", "ronk");
	lang.this:AddPreConversion("great", "bubhosh");
	lang.this:AddPreConversion("dark", "burz");
	lang.this:AddPreConversion("filth", "dug");
	lang.this:AddPreConversion("rule", "durbat");
	lang.this:AddPreConversion("ruler", "durbat");
	lang.this:AddPreConversion("fire", "ghash");
	lang.this:AddPreConversion("find", "gimb");
	lang.this:AddPreConversion("fool", "glob");
	lang.this:AddPreConversion("wraith ghost spirit", "gul");
	lang.this:AddPreConversion("folk people person", "hai");
	lang.this:AddPreConversion("in", "ishi");
	lang.this:AddPreConversion("bind", "krimp");
	lang.this:AddPreConversion("tower", "lug");
	lang.this:AddPreConversion("ring", "nagh");
	lang.this:AddPreConversion("troll ogre", "olug");
	lang.this:AddPreConversion("stupid dumb", "shai");
	lang.this:AddPreConversion("bastard", "sha");
	lang.this:AddPreConversion("old elder", "sharku");
	lang.this:AddPreConversion("slave", "snaga");
	lang.this:AddPreConversion("bring carry", "thrak");
	lang.this:AddPreConversion("all", "uk ick");
	lang.this:AddPreConversion("them", "ul");
	lang.this:AddPreConversion("ness uth ion ul", "um");
	lang.this:AddPreConversion("orc", "urok");
	lang.this:AddPreConversion("ee", "ou");
	lang.this:AddPreConversion("eth", "om");
	lang.this:AddPreConversion("ith", "uuk");
	lang.this:AddPreConversion("ath", "ohk");
	lang.this:AddPreConversion("th", "gn");
	lang.this:AddPreConversion("oo", "uu");
	lang.this:AddPreConversion("ar", "arg");
	lang.this:AddPreConversion("en", "yth");
	lang.this:AddPreConversion("es", "ac");
	lang.this:AddPreConversion("you", "snar");
	lang.this:AddPreConversion("ch", "k");
	lang.this:AddPreConversion("of", "tuk");
	lang.this:AddPreConversion("ove", "aah");
	lang.this:AddPreConversion("ome", "ask");
	lang.this:AddPreConversion("my", "mal");
	lang.this:AddPreConversion("me", "mok");
	lang.this:AddPreConversion("to", "sek");
	lang.this:AddPreConversion("hi", "gorg");
	lang.this:AddPreConversion("hello", "khalok");
	lang.this:AddPreConversion("come beckon", "kurta");
	lang.this:AddPreConversion("kill death", "vak");
	lang.this:AddPreConversion("with", "bat");
	lang.this:AddPreConversion("buy", "muk");
	lang.this:AddPreConversion("take", "kham");
	lang.this:AddPreConversion("steal", "kzam");
	
	lang.this.Alphabet = "oktpabkhugzpnmuxrgztebyzow";
end

LoadLanguage();

-- EOF