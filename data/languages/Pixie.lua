-- PIXIE.LUA
-- This is the Pixie Language file for the MUD
-- Revised: 2015.08.11
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguage()
	-- these are just made up now.. i dunno what a pixie sounds like..
	newLang = LCreateLanguage("pixie", "Pixie");
	lang.this = newLang;
	
	lang.this:AddPreConversion("lair", "sgz");
	lang.this:AddPreConversion("uhm", "luf");
	lang.this:AddPreConversion("get", "ggtz");
	lang.this:AddPreConversion("ll", "fr tpa");
	lang.this:AddPreConversion("ts", "uths");
	lang.this:AddPreConversion("lm", "ml");
	lang.this:AddPreConversion("qu", "arth tv");
	lang.this:AddPreConversion("eer", "s");
	lang.this:AddPreConversion("tr", "mmps");
	
	lang.this.Alphabet = "jdhsidulyywfmieopaexoqybac";
		
	lang.this:AddPostConversion("dfs", "sstg d");
	lang.this:AddPostConversion("r", "grud");
	lang.this:AddPostConversion("tyrl", "x");
end

LoadLanguage();

-- EOF