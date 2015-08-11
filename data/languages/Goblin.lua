-- GOBLIN.LUA
-- This is the Goblin Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	-- bastardization of elvish.  harsh
	newLang = LCreateLanguage("goblin", "Goblin");
	lang.this = newLang;
	lang.this:AddPreConversion("ee", "ou");
	lang.this:AddPreConversion("eth", "om");
	lang.this:AddPreConversion("ith", "uuk");
	lang.this:AddPreConversion("ath", "ohk");
	lang.this:AddPreConversion("uth ion ul", "um");
	lang.this:AddPreConversion("th", "gn");
	lang.this:AddPreConversion("oo", "uu");
	lang.this:AddPreConversion("ar", "arg");
	lang.this:AddPreConversion("en", "yth");
	lang.this:AddPreConversion("es", "ac");
	lang.this:AddPreConversion("you", "snar");
	lang.this:AddPreConversion("ch", "k");
	lang.this:AddPreConversion("ick", "uk");
	lang.this:AddPreConversion("of", "tuk");
	lang.this:AddPreConversion("ove", "aah");
	lang.this:AddPreConversion("ome", "ask");
	lang.this:AddPreConversion("my", "mal");
	lang.this:AddPreConversion("me", "mok");
	lang.this:AddPreConversion("to", "sek");
	lang.this:AddPreConversion("hi", "gorg");
	lang.this:AddPreConversion("hello", "khalok");
	lang.this:AddPreConversion("come", "kurta");
	lang.this:AddPreConversion("kill death", "vak");
	lang.this:AddPreConversion("with", "bat");
	lang.this:AddPreConversion("buy", "muk");
	lang.this:AddPreConversion("take", "kham");
	lang.this:AddPreConversion("steal", "kzam");
	
	lang.this.Alphabet = "oktpabkhugzpnmuxrgztebyzow";
end

LoadLanguage();

-- EOF