-- MARON.LUA
-- This is the Maron Deity file for the MUD
-- Revised: 2013.11.27
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadDeity()
	newDeity = LCreateDeity("Maron");
	deity.this = newDeity;
	deity.this.Description = [[The ruler of Amaron and leader of the Athlei and the Forces of Light, 
	Maron has vowed to rebuild what was lost during the first two Great Wars and to stop his 
	brother, Urman, from causing more destruction.]];
	deity.this.Worshippers = 1673;
	deity.this.Flee = -2;
	deity.this.Kill = 10;
	deity.this.KillMagic = 1;
	deity.this.Sacrifice = 2;
	deity.this.AidSpell = 1;
	deity.this.Aid = 3;
	deity.this.Die = -125;
	deity.this.DieNpcFoe = -65;
	deity.this.SpellAid = 1;
	deity.this.DigCorpse = -6;
	deity.this.SCorpse = 1425;
	deity.this.SAvatar = 700;
	deity.this.SDeityObject = 750;
	deity.this.SRecall = 900;
	deity.this.Race = -1;
	deity.this.Class = 4;
	deity.this.Sex = -1;
	deity.this.NpcFoe = 90;
	deity.this.Race2 = -1;
	deity.this.ObjStat = 4;
end

LoadDeity();

-- EOF