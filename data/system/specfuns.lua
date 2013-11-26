-- SPECFUNS.LUA
-- This is the SpecFuns data file for the MUD
-- Revised: 2013.11.22
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadSpecFuns()
	LCreateSpecFun("spec_breath_any");
	LCreateSpecFun("spec_breath_acid");
	LCreateSpecFun("spec_breath_fire");
	LCreateSpecFun("spec_breath_frost");
	LCreateSpecFun("spec_breath_gas");
	LCreateSpecFun("spec_breath_lightning");
	LCreateSpecFun("spec_cast_adept");
	LCreateSpecFun("spec_cast_cleric");
	LCreateSpecFun("spec_cast_mage");
	LCreateSpecFun("spec_cast_undead");
	LCreateSpecFun("spec_executioner");
	LCreateSpecFun("spec_fido");
	LCreateSpecFun("spec_guard");
	LCreateSpecFun("spec_janitor");
	LCreateSpecFun("spec_mayor");
	LCreateSpecFun("spec_poison");
	LCreateSpecFun("spec_thief");
	LCreateSpecFun("spec_wanderer");
end

LoadSpecFuns();

-- EOF