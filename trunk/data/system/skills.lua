-- SKILLS.LUA
-- This is the skills data file for the MUD
-- Revised: 2013.11.22
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadSpells()
	skill = CreateSkill("reserved", "spell", 112, 95, 51);
	skill.WearOffMessage = "???";
	skill.SpellFunctionName = "spell_null";
	
	skill = CreateSkill("venomshield", "unknown", 0, 0, 51);
	skill.SpellFunctionName = "spell_smaug";
	
	skill = CreateSkill("wrath of dominus", "spell", 109, 246, 51);
	skill:SetFlags("secretskill noscribe");
	skill:SetTargetByValue(2);
	skill.MinimumMana = 10;
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("10", 13, "-75", -1));
	skill:AddAffect(CreateSmaugAffect("10", 31, "-3", -1));	
	
	skill = CreateSkill("astral walk", "spell", 112, 90, 30);
	skill.MinimumMana = 60;
	skill.Rounds = 12;
	skill.WearOffMessage = "!Astral Walk!";
	skill.SpellFunctionName = "spell_astral_walk";

	skill = CreateSkill("acetum primus", "spell", 105, 302, 37);
	skill.Info = 909;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 15;
	skill.Rounds = 8;
	skill.DamageMessage = "Acetum Primus";
	skill.WearOffMessage = "!WEAROFF!";
	skill.SpellFunctionName = "spell_acetum_primus";
	
	skill = CreateSkill("acid blast", "spell", 109, 70, 20);
	skill.Info = 5;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 20;
	skill.Rounds = 12;
	skill.DamageMessage = "Acid Blast";
	skill.WearOffMessage = "!Acid Blast!";
	skill.SpellFunctionName = "spell_acid_blast";
	
	skill = CreateSkill("acid breath", "spell", 109, 200, 43);
	skill.Info = 2053;
	skill:SetFlags("stoponfail");
	skill:SetTargetByValue(1);
	skill:SetSaveVsByValue(5);
	skill.MinimumMana = 20;
	skill.Rounds = 4;
	skill.DamageMessage = "Blast of Acid";
	skill.WearOffMessage = "!Acid Breath!";
	skill.SpellFunctionName = "spell_acid_breath";
	
	skill = CreateSkill("acidmist", "spell", 0, 256, 51);
	skill.Info = 397;
	skill:SetTargetByValue(1);
	skill.Rounds = 20;
	skill.SpellFunctionName = "spell_smaug";
	
	skill = CreateSkill("alertness", "spell", 111, 102, 23);
	skill:SetTargetByValue(3);
	skill.MinimumMana = 60;
	skill.Rounds = 15;
	skill.WearOffMessage = "You are suddenly less alert.";
	skill.HitVictimMessage = "You suddenly feel alert.";
	skill.HitRoomMessage = "$N's eyes dart about the room in an alert manner.";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("l*15", 27, "1024", -1));

	skill = CreateSkill("antimagic shell", "spell", 111, 224, 17);
	skill:SetTargetByValue(3);
	skill.MinimumMana = 40;
	skill.Rounds = 12;
	skill.WearOffMessage = "The shimmering shell and its protection from magic fade away.";
	skill.HitVictimMessage = "A shimmering translucent shell forms about you.";
	skill.HitRoomMessage = "A shimmering translucent shell forms about $N.";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("l*23", 27, "1048576", -1));
	
	skill = CreateSkill("aqua breath", "spell", 112, 236, 14);
	skill:SetFlags("noscribe");
	skill:SetTargetByValue(2);
	skill.MinimumMana = 50;
	skill.Rounds = 12;
	skill.WearOffMessage = "Your lungs revert to their original state.";
	skill.HitCharacterMessage = "$N's lungs take on the ability to breathe water...";
	skill.HitVictimMessage = "Your lungs take on the ability to breathe water...";
	skill.HitRoomMessage = "$N's lungs take on the ability to breathe water...";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("l*23", 26, "aqua breath", 31));
	
	skill = CreateSkill("armor", "spell", 111, 1, 1);
	skill:SetTargetByValue(2);
	skill.MinimumMana = 5;
	skill.Rounds = 12;
	skill.WearOffMessage = "Your armor returns to its mundane value.";
	skill.HitCharacterMessage = "$N's armor begins to glow softly as it is enhanced by a cantrip.";
	skill.HitVictimMessage = "Your armor begins to glow softly as it is enhanced by a cantrip.";
	skill.HitRoomMessage = "$N's armor begins to glow softly as it is enhanced by a cantrip.";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("l*10", 17, "-20", -1));
	
	skill = CreateSkill("benediction", "spell", 110, 95, 19);
	skill:SetTargetByValue(2);
	skill.Info = 840;
	skill.MinimumMana = 15;
	skill.Rounds = 15;
	skill.Flags = 14336;
	skill.WearOffMessage = "Your time in The Protection of The High Gods is over.";
	skill.HitCharacterMessage = "You lay The Protection of The High Gods upon $N.";
	skill.HitVictimMessage = "The Protection of The High Gods is temporarily given to you.";
	skill.HitRoomMessage = "The Protection of The High Gods is temporarily given to $N.";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("l", 26, "protection", 13));
	
	skill = CreateSkill("benefic aura", "spell", 111, 342, 30);
	skill:SetTargetByValue(3);
	skill.MinimumMana = 50;
	skill.Rounds = 12;
	skill.WearOffMessage = "Your protection from evil slowly unravels.";
	skill.HitVictimMessage = "A faint glow rises about you as you are instilled with a ward against evil.";
	skill.HitRoomMessage = "A faint glow rises protectively about $N.";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("l*7", 26, "protect", 13));
	skill:AddComponent(CreateSpellComponent("V", "65", "@"));
	
	skill = CreateSkill("bethsaidean touch", "spell", 111, 343, 9);
	skill:SetTargetByValue(2);
	skill.MinimumMana = 30;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_bethsaidean_touch";
	
	skill = CreateSkill("bless", "spell", 109, 3, 5);
	skill:SetTargetByValue(2);
	skill.MinimumMana = 5;
	skill.Rounds = 12;
	skill.WearOffMessage = "The blessing fades away.";
	skill.HitCharacterMessage = "You lay the blessing of your god upon $N.";
	skill.HitVictimMessage = "A powerful blessing is laid upon you.";
	skill.HitRoomMessage = "$N beams as a powerful blessing is laid upon $M.";
	skill.SpellFunctionName = "spell_smaug";
	skill:AddAffect(CreateSmaugAffect("", 60, "95", -1));
	skill:AddAffect(CreateSmaugAffect("1*23", 24, "-(1/8)", -1));
	skill:AddAffect(CreateSmaugAffect("1*23", 18, "1/8", -1));
	
	skill = CreateSkill("blindness", "spell", 105, 4, 5);
	skill:SetTargetByValue(1);
	skill.MinimumMana = 5;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_blindness";
	skill.WearOffMessage = "You can see again.";
	
	skill = CreateSkill("animate dead", "spell", 110, 231, 23);
	skill.MinimumMana = 220;
	skill.Rounds = 12;
	skill.WearOffMessage = "!Animate Dead!";
	skill.SpellFunctionName = "spell_animate_dead";
	
	skill = CreateSkill("black breath", "spell", 109, 403, 51);
	skill.Info = 2440.
	skill.DamageMessage = "black breath";
	skill.SpellFunctionName = "spell_smaug";
	skill:SetFlags("area");
	skill:AddAffect(CreateSmaugAffect("5", 26, "blind", 0));
	
	skill = CreateSkill("black fist", "spell", 109, 310, 23);
	skill.Info = 911;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 15;
	skill.Rounds = 8;
	skill.SpellFunctionName = "spell_black_fist";
	skill.DamageMessage = "Black Fist";
	skill.WearOffMessage = "!WEAROFF!";
	
	skill = CreateSkill("black hand", "spell", 109, 301, 2);
	skill.Info = 9;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 8;
	skill.Rounds = 8;
	skill.SpellFunctionName = "spell_black_hand";
	skill.DamageMessage = "Black Hand";
	skill.WearOffMessage = "!WEAROFF!";
	skill:AddTeacher(10340);
	
	skill = CreateSkill("black lightning", "spell", 109, 303, 46);
	skill.Info = 908;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 15;
	skill.Rounds = 8;
	skill.SpellFunctionName = "spell_black_lightning";
	skill.DamageMessage = "Black Lightning";
	skill.WearOffMessage = "!WEAROFF!";

	skill = CreateSkill("blasphemy", "spell", 110, 94, 30);
	skill.Info = 1928;
	skill.Flags = 38912;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 80;
	skill.Rounds = 8;
	skill.SpellFunctionName = "spell_smaug";
	skill.DamageMessage = "blasphemy";
	skill.WearOffMessage = "You are no longer afflicted by the curse of the Nephandi!";
	skill.HitCharacterMessage = "You utter a curse against the Gods and bring their wrath upon $N!";
	skill.HitVictimMessage = "$n has BLASPHEMED you!";
	skill.HitRoomMessage = "$n utters Edaj and infuriates the Gods!";
	skill.Dice = "50d1";
	skill:AddComponent(CreateSpellComponent("V", "482", "+"));
	skill:AddAffect(CreateSmaugAffect("", 60, "17", -1));
	skill:AddAffect(CreateSmaugAffect("l*10", 31, "-3", -1));
	skill:AddAffect(CreateSmaugAffect("l*3", 26, "curse", -1));
	
	skill = CreateSkill("call lightning", "spell", 109, 6, 12);
	skill.Info = 3;
	skill.Flags = 6144;
	skill.MinimumMana = 15;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_call_lightning";
	skill.DamageMessage = "lightning bolt";
	skill.WearOffMessage = "!Call Lightning!";
	
	skill = CreateSkill("blazebane", "spell", 110, 216, 30);
	skill:SetTargetByValue(1);
	skill.MinimumMana = 70;
	skill.Rounds = 15;
	skill.SpellFunctionName = "spell_smaug";
	skill.WearOffMessage = "Your flesh grows less susceptible to fire.";
	skill.HitCharacterMessage = "You place a fear of flames in $N's mind...";
	skill.HitVictimMessage = "Your flesh grows more susceptible to fire.";
	skill.HitRoomMessage = "$N begins to mutter about a fear of flames...";
	skill.ImmuneCharacterMessage = "Mysteriously, $N was not affected by your spell.";
	skill:AddAffect(CreateSmaugAffect("l*23", 29, "1", -1));

	skill = CreateSkill("blazeward", "spell", 112, 215, 27);
	skill:SetTargetByValue(3);
	skill.MinimumMana = 70;
	skill.Rounds = 15;
	skill.SpellFunctionName = "spell_smaug";
	skill.WearOffMessage = "The ward of flames ceases to protect you.";
	skill.HitCharacterMessage = "A yellow glow surrounds you, protecting you from intense heat.";
	skill.HitVictimMessage = "$N begins to radiate a yellow light which repels intense heat.";
	skill:AddAffect(CreateSmaugAffect("l*23", 27, "1", -1));
	
	skill = CreateSkill("burning hands", "spell", 109, 5, 5);
	skill:SetTargetByValue(1);
	skill.Info = 1;
	skill.MinimumMana = 15;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_burning_hands";
	skill.DamageMessage = "burning hand";
	skill.WearOffMessage = "!Burning Hands!";
	
	skill = CreateSkill("cause critical", "spell", 109, 63, 9);
	skill.Info = 4;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 20;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_cause_critical";
	skill.DamageMessage = "spell";
	skill.WearOffMessage = "!Cause Critical!";
	
	skill = CreateSkill("cause light", "spell", 110, 62, 1);
	skill.Info = 4;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 15;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_cause_light";
	skill.DamageMessage = "spell";
	skill.WearOffMessage = "!Cause Light!";
	
	skill = CreateSkill("cause serious", "spell", 107, 64, 5);
	skill.Info = 4;
	skill:SetTargetByValue(1);
	skill.MinimumMana = 17;
	skill.Rounds = 12;
	skill.SpellFunctionName = "spell_cause_serious";
	skill.DamageMessage = "spell";
	skill.WearOffMessage = "!Cause Serious!";
--[[
#SKILL
Name         caustic fount~
Type         Spell
Info         909
Flags        0
Target       1
Minpos       107
Slot         313
Mana         15
Rounds       8
Code         spell_caustic_fount
Dammsg       Caustic Fount~
Wearoff      !WEAROFF!~
Minlevel     34
End

#SKILL
Name         caustic touch~
Type         Spell
Info         398
Flags        6144
Target       1
Minpos       105
Mana         18
Rounds       10
Code         spell_smaug
Dammsg       ~
Wearoff      The toxins in your blood subside.~
Hitchar      You lay a hand on $N and infuse $S body with poison.~
Hitvict      $n lays a hand on you and poison infuses you.~
Hitroom      $N shivers under $n's caustic touch.~
Affect       'l*25' 26 'poison' 12
Minlevel     51
End

#SKILL
Name         change sex~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         82
Mana         15
Rounds       12
Code         spell_change_sex
Dammsg       ~
Wearoff      Your body feels familiar again.~
Hitchar      $N cries out as $S gender is changed.~
Hitvict      A chill runs through you as your gender changes.~
Hitroom      $N cries out as $S gender is changed.~
Affect       'l*230' 6 '1' -1
Minlevel     51
End

#SKILL
Name         charged beacon~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       110
Slot         105
Mana         65
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      You no longer attract electricity.~
Hitchar      $N is now susceptible to the powers of electricity.~
Hitvict      You are now attractive to the powers of electricity.~
Hitroom      $N is now susceptible to the powers of electricity.~
Immchar      Your spell has no effect upon $N.~
Affect       'l*17' 29 '4' -1
Minlevel     28
End

#SKILL
Name         charm person~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       111
Slot         7
Mana         5
Rounds       12
Code         spell_charm_person
Dammsg       ~
Wearoff      You feel more self-confident.~
Minlevel     14
End

#SKILL
Name         chill touch~
Type         Spell
Info         2
Flags        0
Target       1
Minpos       105
Slot         8
Mana         15
Rounds       12
Code         spell_chill_touch
Dammsg       chilling touch~
Wearoff      You feel less cold.~
Minlevel     3
End

#SKILL
Name         colour spray~
Type         Spell
Info         4
Flags        0
Target       1
Minpos       107
Slot         10
Mana         15
Rounds       12
Code         spell_colour_spray
Dammsg       colour spray~
Wearoff      !Colour Spray!~
Minlevel     11
End

#SKILL
Name         continual light~
Type         Spell
Info         520
Flags        16384
Minpos       111
Slot         57
Mana         7
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Continual Light!~
Hitchar      Shards of iridescent light collide to form a dazzling ball...~
Hitroom      Shards of iridescent light collide to form a dazzling ball...~
Dice         0~
Value        21
Minlevel     2
End

#SKILL
Name         control weather~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         11
Mana         25
Rounds       12
Code         spell_control_weather
Dammsg       ~
Wearoff      !Control Weather!~
Minlevel     10
End

#SKILL
Name         create fire~
Type         Spell
Info         521
Flags        16384
Minpos       111
Slot         85
Mana         10
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Create Fire!~
Hitchar      You call upon the Gods to invoke the power of fire!~
Hitroom      $n invokes the power of fire!~
Dice         L~
Value        30
Minlevel     1
End

#SKILL
Name         create food~
Type         Spell
Info         520
Flags        16384
Minpos       111
Slot         12
Mana         5
Rounds       12
Code         spell_create_food
Dammsg       ~
Wearoff      !Create Food!~
Hitchar      A magic mushroom appears in your hands.~
Hitroom      A magic mushroom appears in $n's hands.~
Dice         0~
Value        20
Minlevel     2
End

#SKILL
Name         create spring~
Type         Spell
Info         520
Flags        16384
Minpos       109
Slot         80
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Create Spring!~
Hitchar      Tracing a ring before you, the graceful flow of a mystical spring emerges.~
Hitroom      As $n traces a ring through the air, the flow of a mystical spring emerges.~
Dice         L~
Value        22
Minlevel     6
End

#SKILL
Name         create symbol~
Type         Spell
Info         520
Flags        16384
Minpos       111
Slot         101
Mana         35
Rounds       20
Code         spell_smaug
Dammsg       ~
Hitchar      A shining symbol of faith appears in your hands!~
Hitroom      $n has invoked a holy symbol!~
Dice         0~
Value        43
Minlevel     10
End

#SKILL
Name         create water~
Type         Spell
Info         0
Flags        0
Target       4
Minpos       111
Slot         13
Mana         5
Rounds       12
Code         spell_create_water
Dammsg       ~
Wearoff      !Create Water!~
Minlevel     2
End

#SKILL
Name         cure blindness~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         14
Mana         5
Rounds       12
Code         spell_cure_blindness
Dammsg       ~
Wearoff      !Cure Blindness!~
Minlevel     4
End

#SKILL
Name         cure critical~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         15
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Cure Critical!~
Hitchar      You cure $N's critical wounds.~
Hitvict      Your critical wounds close and your pain ebbs away.~
Affect       '' 13 '3d8+(l-6)' -1
Minlevel     9
End

#SKILL
Name         cure light~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       107
Slot         16
Mana         10
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your ability to see in the dark fades away.~
Hitchar      You cure $N's light wounds.~
Hitvict      Your light wounds mend and your pain ebbs slightly.~
Affect       '' 13 '1d8+(l/3)' -1
Minlevel     1
End

#SKILL
Name         cure poison~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       110
Slot         43
Mana         5
Rounds       12
Code         spell_cure_poison
Dammsg       ~
Wearoff      !Cure Poison!~
Minlevel     8
End

#SKILL
Name         cure serious~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       110
Slot         61
Mana         15
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Cure Serious!~
Hitchar      You cure $N's serious wounds.~
Hitvict      Your serious wounds mend and your pain ebbs away.~
Affect       '' 13 '2d8+(l/2)' -1
Minlevel     5
End

#SKILL
Name         curse~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       107
Slot         17
Mana         20
Rounds       12
Code         spell_curse
Dammsg       curse~
Wearoff      The curse wears off.~
Minlevel     12
End

#SKILL
Name         dehydrate~
Type         Spell
Info         855
Flags        132096
Target       1
Minpos       108
Slot         95
Mana         40
Rounds       14
Code         spell_smaug
Dammsg       dehydration~
Wearoff      You feel less dehydrated.~
Hitchar      You dehydrate $N!~
Hitvict      You feel parched!~
Hitroom      $N is dehydrated!~
Affect       'l*5' 64 '-10' -1
Affect       '' 26 'dehydration' -1
Affect       '' 60 '95' -1
Minlevel     24
End

#SKILL
Name         demonic aura~
Type         Spell
Info         1864
Flags        6144
Target       3
Minpos       107
Slot         98
Mana         40
Rounds       10
Code         spell_smaug
Dammsg       demonic aura~
Wearoff      The dark powers no longer protect you.~
Hitchar      You are enveloped by the powers of darkness!~
Hitvict      A demonic aura protects you from harm!~
Misschar     The evil powers fail to respond to your plea for protection.~
Affect       'l*15' 27 '257' -1
Minlevel     12
End

#SKILL
Name         demonskin~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         210
Mana         55
Rounds       25
Code         spell_smaug
Dammsg       ~
Wearoff      Your leathery skin grows thinner as your spell wanes.~
Hitvict      Your skin becomes thick and leathery, similar to that of a demon.~
Hitroom      $N's skin becomes thick and leathery, similar to that of a demon.~
Affect       'l*23' 27 '64' -1
Minlevel     14
End

#SKILL
Name         desecrate~
Type         Spell
Info         3982
Flags        0
Target       4
Minpos       110
Saves        1
Slot         125
Mana         15
Rounds       10
Code         spell_smaug
Dammsg       desecrate~
Hitvict      The desecration is successful!~
Hitroom      Nephandi rejoice as another item is desecrated!~
Affect       'l*4' 26 'poison' -1
Affect       '' 26 'desecration' -1
Minlevel     48
End

#SKILL
Name         detect evil~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       110
Slot         18
Mana         5
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The red outlines fade from your vision.~
Hitvict      Traces of red outline all evil in plain sight.~
Hitroom      A tint of red appears in $N's eyes, mirroring $S own vision.~
Affect       'l*24' 26 'detect evil' 2
Minlevel     4
End

#SKILL
Name         detect hidden~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       112
Slot         44
Mana         5
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      You feel less aware of your surroundings.~
Hitvict      Your senses are heightened to those of an animal.~
Hitroom      $N's senses are heightened to those of an animal.~
Affect       'l*24' 26 'detect hidden' 5
Minlevel     7
End

#SKILL
Name         detect invis~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       105
Slot         19
Mana         5
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      You no longer see invisible objects.~
Hitvict      Your eyes fixate as they gain the ability to see the unseen.~
Hitroom      $N's eyes fixate as they gain the ability to see the unseen.~
Affect       'l*24' 26 'detect invis' 3
Minlevel     2
End

#SKILL
Name         detect magic~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         20
Mana         5
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The blue outlines disappear from your vision.~
Hitvict      Traces of blue outline the magical objects in your field of vision.~
Hitroom      A tint of blue in $N's eyes mirrors $S own perception.~
Affect       'l*24' 26 'detect magic' 4
Minlevel     2
End

#SKILL
Name         detect poison~
Type         Spell
Info         0
Flags        0
Target       4
Minpos       112
Slot         21
Mana         5
Rounds       12
Code         spell_detect_poison
Dammsg       ~
Wearoff      !Detect Poison!~
Minlevel     4
End

#SKILL
Name         detect traps~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       112
Slot         86
Mana         15
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      You feel less aware of the dangers about you.~
Hitvict      You suddenly grow aware of the dangers about you.~
Hitroom      $N peers about the room, intent on finding all manner of danger.~
Affect       'l*24' 26 'detect traps' 23
Minlevel     7
End

#SKILL
Name         dispel evil~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       107
Slot         22
Mana         15
Rounds       12
Code         spell_dispel_evil
Dammsg       dispel evil~
Wearoff      !Dispel Evil!~
Minlevel     10
End

#SKILL
Name         dispel magic~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       109
Slot         59
Mana         15
Rounds       12
Code         spell_dispel_magic
Dammsg       ~
Wearoff      !Dispel Magic!~
Minlevel     11
End

#SKILL
Name         disruption~
Type         Spell
Info         908
Flags        0
Target       1
Minpos       110
Slot         305
Mana         15
Rounds       8
Code         spell_disruption
Dammsg       Disruption~
Wearoff      !WEAROFF!~
Minlevel     8
End

#SKILL
Name         divination~
Type         Spell
Info         0
Flags        6144
Target       4
Minpos       111
Mana         10
Rounds       10
Code         spell_identify
Dammsg       ~
Hitchar      You focus your divining powers on $O...~
Components   V@387~
Minlevel     51
End

#SKILL
Name         divinity~
Type         Spell
Info         0
Flags        6144
Target       2
Minpos       110
Slot         112
Mana         115
Rounds       12
Code         spell_smaug
Dammsg       ~
Hitchar      You allow a divine presence to flow through $N.~
Hitvict      You are filled with a divine presence.~
Components   V@43~
Affect       '' 13 '200' -1
Minlevel     42
End

#SKILL
Name         dragon wit~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         227
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The wit of the dragon withdraws from your mind.~
Hitchar      $N's eyes glimmer with the wit of the dragon.~
Hitvict      Your mind awakens in reception to the dragon's wit.~
Hitroom      $N's eyes glimmer with the wit of the dragon.~
Affect       'l*24' 3 '1+(l/17)' -1
Minlevel     9
End

#SKILL
Name         dragonskin~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         212
Mana         45
Rounds       14
Code         spell_smaug
Dammsg       ~
Wearoff      Your flesh sheds its draconian aspects.~
Hitvict      Your flesh changes to emulate the scaly skin of a dragon.~
Hitroom      $N's flesh assumes a draconian form...~
Affect       'l*23' 27 '32' -1
Minlevel     16
End

#SKILL
Name         dream~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         233
Mana         5
Rounds       12
Code         spell_dream
Dammsg       ~
Wearoff      !Dream!~
Minlevel     17
End

#SKILL
Name         earthquake~
Type         Spell
Info         0
Flags        6144
Minpos       109
Slot         23
Mana         15
Rounds       12
Code         spell_earthquake
Dammsg       earthquake~
Wearoff      !Earthquake!~
Minlevel     7
End

#SKILL
Name         eldritch sphere~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         207
Mana         70
Rounds       20
Code         spell_smaug
Dammsg       ~
Wearoff      The eldritch sphere about you winks from existence.~
Hitvict      A magical eldritch sphere forms about you...~
Hitroom      A shimmering eldritch sphere forms about $N...~
Affect       '(l*3)+25' 27 '8192' -1
Affect       '(l*3)+25' 27 '1048576' -1
Minlevel     33
End

#SKILL
Name         elven beauty~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         226
Mana         14
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your elven features wane and you feel less charismatic.~
Hitchar      $N's ears grow pointed and $S voice takes on a musical aspect.~
Hitvict      Your face is blessed with elven features as you grow more attractive.~
Hitroom      $N's ears grow pointed and $S voice takes on a musical aspect.~
Affect       'l*24' 25 '1+(l/17)' -1
Minlevel     3
End

#SKILL
Name         enchant weapon~
Type         Spell
Info         0
Flags        0
Target       4
Minpos       112
Slot         24
Mana         100
Rounds       24
Code         spell_enchant_weapon
Dammsg       ~
Wearoff      !Enchant Weapon!~
Minlevel     12
End

#SKILL
Name         energy drain~
Type         Spell
Info         4
Flags        0
Target       1
Minpos       109
Slot         25
Mana         35
Rounds       12
Code         spell_energy_drain
Dammsg       energy drain~
Wearoff      !Energy Drain!~
Minlevel     13
End

#SKILL
Name         ethereal fist~
Type         Spell
Info         908
Flags        0
Target       1
Minpos       109
Slot         312
Mana         15
Rounds       8
Code         spell_ethereal_fist
Dammsg       Ethereal Fist~
Wearoff      !WEAROFF!~
Minlevel     32
End

#SKILL
Name         ethereal funnel~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       110
Slot         218
Mana         75
Rounds       22
Code         spell_smaug
Dammsg       ~
Wearoff      The ethereal funnel about you ceases to exist.~
Hitchar      You erect an ethereal funnel about $N...~
Hitvict      An aura surrounds you, channeling violent energies in your direction!~
Hitroom      An ethereal funnel forms about $N...~
Immchar      The ethereal funnel dissipates before it reaches $N.~
Affect       'l*23' 29 '8' -1
Minlevel     45
End

#SKILL
Name         ethereal shield~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         217
Mana         75
Rounds       22
Code         spell_smaug
Dammsg       ~
Wearoff      You are returned to the mundane energy continuum.~
Hitvict      You fade from the mundane energy continuum.~
Hitroom      An ethereal shield divides $N from the mundane energy continuum.~
Affect       'l*23' 27 '8' -1
Minlevel     40
End

#SKILL
Name         execrate~
Type         Spell
Info         904
Flags        6288
Minpos       109
Slot         95
Mana         30
Rounds       18
Code         spell_smaug
Dammsg       execration~
Wearoff      The execration wears off.~
Hitchar      You curse everyone within your reach.~
Hitvict      An execration is laid upon you.~
Affect       'l*23' 26 'curse' 10
Affect       'l*23' 18 '-1' -1
Affect       'l*23' 20 '1' -1
Affect       '' 60 '95' -1
Minlevel     3
End

#SKILL
Name         exorcism~
Type         Spell
Info         840
Flags        0
Target       2
Minpos       112
Slot         99
Mana         50
Rounds       10
Code         spell_smaug
Dammsg       exorcism~
Hitchar      You chant wildly and perform an exorcism on $N.~
Hitvict      The demons within you are painfully extracted by $n. You feel relieved.~
Hitroom      $N has undergone an exorcism!~
Components   v+10216~
Affect       '' 60 '99' -1
Affect       '' 60 '17' -1
Affect       '' 60 '94' -1
Minlevel     25
End

#SKILL
Name         expurgation~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       110
Slot         340
Mana         25
Rounds       12
Code         spell_expurgation
Dammsg       ~
Minlevel     13
End

#SKILL
Name         extradimensional portal~
Type         Spell
Info         1544
Flags        16384
Minpos       111
Slot         95
Mana         40
Rounds       24
Code         spell_smaug
Dammsg       !EXTRADIMENSIONAL_PORTAL!~
Wearoff      !EXTRADIMENSIONAL_PORTAL!~
Value        63
Minlevel     13
End

#SKILL
Name         faerie fire~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       109
Slot         72
Mana         5
Rounds       12
Code         spell_faerie_fire
Dammsg       faerie fire~
Wearoff      The pink aura around you fades away.~
Minlevel     2
End

#SKILL
Name         faerie fog~
Type         Spell
Info         0
Flags        0
Minpos       109
Slot         73
Mana         12
Rounds       12
Code         spell_faerie_fog
Dammsg       faerie fog~
Wearoff      !Faerie Fog!~
Minlevel     10
End

#SKILL
Name         farsight~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         222
Mana         15
Rounds       12
Code         spell_farsight
Dammsg       ~
Wearoff      !Farsight!~
Minlevel     16
End

#SKILL
Name         fatigue~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       109
Slot         103
Mana         60
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      You no longer feel so sleepy.~
Hitchar      $N suddenly appears very tired and drowsy.~
Hitvict      You suddenly grow very tired and drowsy.~
Hitroom      $N suddenly appears very tired and drowsy.~
Immchar      Mysteriously, $N was not affected by your spell.~
Affect       'l*15' 29 '1024' -1
Minlevel     23
End

#SKILL
Name         feebleness~
Type         Spell
Info         0
Flags        6144
Target       1
Minpos       109
Slot         107
Mana         30
Rounds       10
Code         spell_smaug
Dammsg       ~
Wearoff      You no longer feel so feeble.~
Hitchar      You lay a curse of feebleness upon $N.~
Hitvict      You grow feeble from a powerful curse.~
Hitroom      $n lays a curse of feebleness upon $N.~
Immchar      $N is not affected by your spell.~
Affect       'l*19' 29 '512' -1
Minlevel     33
End

#SKILL
Name         fire breath~
Type         Spell
Info         1
Flags        0
Target       1
Minpos       109
Slot         201
Mana         15
Rounds       4
Code         spell_fire_breath
Dammsg       blast of flame~
Wearoff      !Fire Breath!~
Minlevel     44
End

#SKILL
Name         fireball~
Type         Spell
Info         1
Flags        0
Target       1
Minpos       109
Slot         26
Mana         15
Rounds       12
Code         spell_fireball
Dammsg       fireball~
Wearoff      !Fireball!~
Minlevel     13
End

#SKILL
Name         fireshield~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         88
Mana         85
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The mystical flames are quelled by a sudden rush of air.~
Hitchar      Mystical flames rise to enshroud $N.~
Hitvict      Mystical flames rise to enshroud you.~
Hitroom      Mystical flames rise to enshroud $N.~
Affect       '(l*4)+8' 26 'fireshield' 25
Minlevel     28
End

#SKILL
Name         flamestrike~
Type         Spell
Info         1
Flags        0
Target       1
Minpos       110
Slot         65
Mana         20
Rounds       12
Code         spell_flamestrike
Dammsg       flamestrike~
Wearoff      !Flamestrike!~
Minlevel     13
End

#SKILL
Name         flare~
Type         Spell
Info         393
Flags        0
Target       1
Minpos       109
Slot         60
Mana         15
Rounds       12
Code         spell_fireball
Dammsg       shield of flame~
Missroom     supress~
Minlevel     51
End

#SKILL
Name         flesh armor~
Type         Spell
Info         840
Flags        4096
Target       3
Minpos       112
Slot         95
Mana         50
Rounds       25
Code         spell_smaug
Dammsg       ~
Wearoff      Your flesh returns to its normal state.~
Hitchar      You utilize magic to create an armor from your own flesh.~
Hitvict      You form an armor from your own flesh and magic!~
Hitroom      $n's flesh becomes a veil of armor and magic!~
Missvict     Your flesh crawls but fails to form a protective aura.~
Affect       'l*20' 26 'flesh armor' -1
Affect       'l*20' 17 '-50' -1
Minlevel     12
End

#SKILL
Name         fletch~
Type         Spell
Info         8
Flags        16384
Minpos       111
Slot         95
Mana         10
Rounds       10
Code         spell_smaug
Dammsg       ~
Hitchar      You fashion an arrow from a crude piece of flint.~
Hitroom      $n creates an arrow from a piece of flint.~
Misschar     You attempt to make an arrow, but fail miserably.~
Value        306
Components   Kflint~
Minlevel     20
End

#SKILL
Name         float~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         292
Mana         8
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      Your feet float slowly to the surface.~
Hitchar      $N begins to float in mid-air...~
Hitvict      You begin to float in mid-air...~
Hitroom      $N begins to float in mid-air...~
Affect       'l*24' 26 'float' 21
Minlevel     1
End

#SKILL
Name         fly~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         56
Mana         10
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      You slowly float to the ground.~
Hitchar      $N rises into the currents of air...~
Hitvict      You rise into the currents of air...~
Hitroom      $N rises into the currents of air...~
Affect       'l*24' 26 'fly' 19
Minlevel     7
End

#SKILL
Name         fortify~
Type         Spell
Info         840
Flags        14336
Target       2
Minpos       109
Slot         95
Mana         40
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      !FORTIFY!~
Hitvict      Your group's wounds close and mend.~
Affect       '' 13 '3d8+(l-6)' -1
Minlevel     15
End

#SKILL
Name         frost breath~
Type         Spell
Info         2
Flags        0
Target       1
Minpos       110
Slot         202
Mana         15
Rounds       4
Code         spell_frost_breath
Dammsg       blast of frost~
Wearoff      !Frost Breath!~
Minlevel     41
End

#SKILL
Name         galvanic whip~
Type         Spell
Info         907
Flags        0
Target       1
Minpos       109
Slot         304
Mana         15
Rounds       8
Code         spell_galvanic_whip
Dammsg       Galvanic Whip~
Wearoff      !WEAROFF!~
Minlevel     4
End

#SKILL
Name         gas breath~
Type         Spell
Info         0
Flags        6144
Minpos       109
Slot         203
Mana         15
Rounds       4
Code         spell_gas_breath
Dammsg       blast of gas~
Wearoff      !Gas Breath!~
Minlevel     45
End

#SKILL
Name         gate~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         83
Mana         50
Rounds       12
Code         spell_gate
Dammsg       ~
Wearoff      !Gate!~
Minlevel     51
End

#SKILL
Name         grounding~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         104
Mana         60
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      You are no longer able to ground electricity.~
Hitvict      You gain the ability to ground electricity.~
Hitroom      $N gains the ability to ground electricity.~
Affect       'l*17' 27 '4' -1
Minlevel     28
End

#SKILL
Name         hand of chaos~
Type         Spell
Info         908
Flags        0
Target       1
Minpos       107
Slot         307
Mana         15
Rounds       8
Code         spell_hand_of_chaos
Dammsg       Hand of Chaos~
Wearoff      !WEAROFF!~
Minlevel     37
End

#SKILL
Name         harm~
Type         Spell
Info         4
Flags        0
Target       1
Minpos       107
Slot         27
Mana         35
Rounds       12
Code         spell_harm
Dammsg       harm spell~
Wearoff      !Harm!~
Minlevel     13
End

#SKILL
Name         heal~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       107
Slot         28
Mana         50
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Heal!~
Hitchar      You lay a hand of healing upon $N.~
Hitvict      A warm feeling fills your body.~
Affect       '' 13 '100' -1
Minlevel     14
End

#SKILL
Name         helical flow~
Type         Spell
Info         0
Flags        0
Minpos       110
Slot         298
Mana         180
Rounds       12
Code         spell_helical_flow
Dammsg       ~
Wearoff      !Helical Flow!~
Minlevel     42
End

#SKILL
Name         hezekiahs cure~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         341
Mana         40
Rounds       12
Code         spell_smaug
Dammsg       ~
Hitchar      You infuse $N with a healing vitality.~
Hitvict      Your wounds are soothed by a healing vitality.~
Components   V@65~
Affect       '' 13 '5*(l/3)' -1
Minlevel     13
End

#SKILL
Name         holy sanctity~
Type         Spell
Info         0
Flags        14336
Minpos       111
Slot         111
Mana         250
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      Your sanctity ends abruptly.~
Hitvict      You are sanctified by a powerful blessing.~
Hitroom      $N is sanctified by a powerful blessing.~
Components   V@43~
Affect       '(l*4)+30' 26 'sanctify' 7
Minlevel     45
End

#SKILL
Name         ice storm~
Type         Spell
Info         8074
Flags        16
Minpos       109
Saves        5
Slot         401
Mana         10
Rounds       12
Code         spell_smaug
Dammsg       storm of ice~
Hitchar      You call a storm of ice!~
Hitroom      $n calls an ice storm!~
Dice         100d20~
Affect       '30' 1 '-1' -1
Minlevel     51
End

#SKILL
Name         iceshard~
Type         Spell
Info         394
Flags        0
Target       1
Minpos       109
Slot         299
Mana         15
Rounds       12
Code         spell_chill_touch
Dammsg       hail of ice~
Missroom     supress~
Minlevel     51
End

#SKILL
Name         iceshield~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       112
Slot         221
Mana         85
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The hail of ice rapidly melts and evaporates into nothingness.~
Hitchar      A glistening shield of ice encompasses $N!~
Hitvict      A glistening shield of ice encompasses you!~
Hitroom      A glistening hail of ice encompasses $N.~
Affect       '(l*4)+8' 26 'iceshield' 28
Minlevel     22
End

#SKILL
Name         identify~
Type         Spell
Info         0
Flags        0
Minpos       111
Slot         53
Mana         12
Rounds       24
Code         spell_identify
Dammsg       ~
Wearoff      !Identify!~
Minlevel     10
End

#SKILL
Name         ill fortune~
Type         Spell
Info         0
Flags        6144
Target       1
Minpos       110
Slot         108
Mana         40
Rounds       13
Code         spell_smaug
Dammsg       ~
Wearoff      Your ill luck finally fades away.~
Hitchar      $N's luck suddenly turns ill.~
Hitvict      Your luck suddenly turns ill.~
Hitroom      $N's luck suddenly turns ill.~
Immchar      Your spell has no effect upon $N.~
Affect       'l*18' 31 '-3' -1
Minlevel     31
End

#SKILL
Name         indignation~
Type         Spell
Info         840
Flags        14336
Target       2
Minpos       111
Slot         95
Mana         90
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      The Fury of The High Gods seeps from your veins, leaving you drained.~
Hitchar      You lay The Fury of The High Gods upon $N.~
Hitvict      The Fury of The High Gods is temporarily given to you.~
Hitroom      The Fury of The High Gods is temporarily given to $N.~
Affect       'l/2' 18 '1+(l/17)' -1
Affect       'l/2' 19 '1+(l/17)' -1
Minlevel     27
End

#SKILL
Name         infernal node~
Type         Spell
Info         0
Flags        6144
Target       3
Minpos       110
Mana         89
Rounds       19
Code         spell_teleport
Dammsg       ~
Minlevel     33
End

#SKILL
Name         infravision~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         77
Mana         5
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      You no longer see in the dark.~
Hitchar      $N's eyes dart about as they grow accustomed to infravision.~
Hitvict      Heat appears red through your eyes.~
Hitroom      $N's eyes dart about as they grow accustomed to infravision.~
Affect       'l*24' 26 'infravision' 9
Minlevel     1
End

#SKILL
Name         inner warmth~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         213
Mana         65
Rounds       8
Code         spell_smaug
Dammsg       ~
Wearoff      The magical warmth within you subsides.~
Hitchar      A mysterious warmth radiates from $N...~
Hitvict      A comforting warmth spreads through your frame.~
Hitroom      A mysterious warmth radiates from $N...~
Affect       'l*23' 27 '2' -1
Minlevel     20
End

#SKILL
Name         invis~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         29
Mana         5
Rounds       12
Code         spell_invis
Dammsg       ~
Wearoff      You are no longer invisible.~
Hitchar      $N fades from existence.~
Hitvict      You fade from existence.~
Hitroom      $N fades from existence.~
Affect       'l*11' 26 'invis' 1
Minlevel     4
End

#SKILL
Name         kindred strength~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         39
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your vampiric potence slowly withers away...~
Hitchar      $N's muscles ripple as vampiric powers flow through $M...~
Hitvict      The strength of the Kindred flows through your veins...~
Hitroom      $N's muscles ripple as vampiric powers flow through $M...~
Affect       'l*24' 1 '1+(l/17)' -1
Minlevel     7
End

#SKILL
Name         knock~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         234
Mana         10
Rounds       12
Code         spell_knock
Dammsg       ~
Wearoff      !Knock!~
Minlevel     32
End

#SKILL
Name         know alignment~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       110
Slot         58
Mana         9
Rounds       12
Code         spell_know_alignment
Dammsg       ~
Wearoff      !Know Alignment!~
Minlevel     5
End

#SKILL
Name         lethargy~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       111
Slot         109
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      You are no longer so lethargic.~
Hitchar      $N is assailed by a magical lethargy.~
Hitvict      You are assailed by a magical lethargy.~
Hitroom      $N is assailed by a magical lethargy.~
Immchar      Your spell has no effect upon $N.~
Affect       'l*15' 2 '-2' -1
Minlevel     12
End

#SKILL
Name         levitate~
Type         Spell
Info         0
Flags        6144
Target       3
Minpos       112
Slot         95
Mana         40
Rounds       8
Code         spell_smaug
Dammsg       ~
Wearoff      Your levitation spell wears off and you return to the
ground.~
Hitchar      You slowly rise into the air.~
Hitvict      Your feet slowly rise off of the ground.~
Hitroom      $N bows $S head in concentration and $S feet rise off the
ground.~
Misschar     You concentrate heavily, but your feet stay anchored to the
earth.~
Affect       'l*23' 26 'floating' 21
Minlevel     27
End

#SKILL
Name         lightning bolt~
Type         Spell
Info         3
Flags        0
Target       1
Minpos       110
Slot         30
Mana         15
Rounds       12
Code         spell_lightning_bolt
Dammsg       lightning bolt~
Wearoff      !Lightning Bolt!~
Minlevel     9
End

#SKILL
Name         lightning breath~
Type         Spell
Info         3
Flags        0
Target       1
Minpos       109
Slot         204
Mana         15
Rounds       4
Code         spell_lightning_breath
Dammsg       blast of lightning~
Wearoff      !Lightning Breath!~
Minlevel     42
End

#SKILL
Name         locate object~
Type         Spell
Info         0
Flags        6144
Minpos       111
Slot         31
Mana         40
Rounds       24
Code         spell_locate_object
Dammsg       ~
Wearoff      !Locate Object!~
Minlevel     6
End

#SKILL
Name         magic missile~
Type         Spell
Info         396
Flags        0
Minpos       109
Slot         32
Mana         7
Rounds       6
Range        1
Code         spell_smaug
Dammsg       magic missile~
Wearoff      !Magic Missile!~
Dice         l/3 + 3~
Minlevel     1
End

#SKILL
Name         magnetic thrust~
Type         Spell
Info         907
Flags        0
Target       1
Minpos       109
Slot         311
Mana         15
Rounds       8
Code         spell_magnetic_thrust
Dammsg       Magnetic Thrust~
Wearoff      !WEAROFF!~
Minlevel     27
End

#SKILL
Name         major invocation~
Type         Spell
Info         840
Flags        14336
Target       2
Minpos       112
Slot         95
Mana         90
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      Your time in The Sanctuary of The High Gods is over.~
Hitchar      You lay The Sanctuary of The High Gods upon $N.~
Hitvict      The Sanctuary of The High Gods is temporarily given to you.~
Hitroom      The Sanctuary of The High Gods is temporarily given to $N.~
Affect       'l' 26 'sanctuary' 7
Minlevel     18
End

#SKILL
Name         mass invis~
Type         Spell
Info         0
Flags        8192
Target       2
Minpos       112
Slot         69
Mana         20
Rounds       24
Code         spell_smaug
Dammsg       ~
Wearoff      You are no longer invisible.~
Hitvict      You fade from existence.~
Hitroom      $N fades from existence.~
Affect       'l*11' 26 'mass invis' 1
Minlevel     15
End

#SKILL
Name         mental anguish~
Type         Spell
Info         1932
Flags        6144
Target       1
Minpos       111
Mana         60
Rounds       10
Code         spell_smaug
Dammsg       mental anguish~
Wearoff      Your mind is now free of anguish.~
Hitchar      You inflict mental anguish on $N!~
Hitvict      $n tortures you with mental anguish!~
Misschar     You fail to inflict mental anguish on $N!~
Affect       '' 60 '206' -1
Affect       '' 26 'mental anguish' -1
Affect       'l*10' 3 '-1' -1
Affect       'l*10' 5 '-1' -1
Affect       'l*10' 18 '-1' -1
Minlevel     44
End

#SKILL
Name         midas touch~
Type         Spell
Info         1552
Flags        0
Target       4
Minpos       111
Slot         95
Mana         50
Rounds       24
Code         spell_midas_touch
Dammsg       !MIDAS_TOUCH!~
Wearoff      !MIDAS_TOUCH!~
Minlevel     13
End

#SKILL
Name         mind fortress~
Type         Spell
Info         1432
Flags        14336
Target       2
Minpos       111
Slot         95
Mana         100
Rounds       40
Code         spell_smaug
Dammsg       'mind fortress'~
Wearoff      Your mind fortress dissolves!~
Hitvict      Your mental powers block out magic!~
Affect       'l*24' 26 'mind fortress' -1
Affect       'l*24' 24 '-6' -1
Affect       'l*24' 3 '+1' -1
Minlevel     8
End

#SKILL
Name         minor invocation~
Type         Spell
Info         840
Flags        14336
Target       2
Minpos       111
Slot         95
Mana         10
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      The minor invocation fades away.~
Hitchar      You lay a minor invocation of your god upon $N.~
Hitvict      A minor invocation is laid upon you.~
Hitroom      $N beams as a minor invocation is laid upon $M.~
Affect       'l*23' 18 '(l/8)' -1
Affect       'l*23' 24 '-(l/8)' -1
Affect       '' 60 '3' -1
Minlevel     10
End

#SKILL
Name         mystic awareness~
Type         Spell
Info         840
Flags        0
Target       3
Minpos       109
Slot         310
Mana         50
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your awareness of your surroundings fades.~
Hitvict      Visions in the distance gain clarity.~
Affect       'l*20' 26 'scry' 24
Minlevel     9
End

#SKILL
Name         necromantic touch~
Type         Spell
Info         8591
Flags        6144
Target       1
Minpos       109
Saves        5
Slot         114
Mana         25
Rounds       12
Code         spell_smaug
Dammsg       necromantic touch~
Hitchar      You drain $N's life essence.~
Hitvict      Your life energies are stolen by $n!~
Hitroom      $n drains the life essence from $N!~
Dice         150d2~
Affect       '' 1013 '2d75' -1
Minlevel     40
End

#SKILL
Name         nightmare~
Type         Spell
Info         0
Flags        0
Minpos       109
Mana         40
Rounds       12
Code         spell_dream
Dammsg       ~
Hitchar      You force your twisted thoughts into the mind of $N.~
Hitvict      Your mind is tortured with twisted visions of death and despair.~
Minlevel     26
End

#SKILL
Name         nostrum~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       107
Slot         297
Mana         50
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Nostrum!~
Hitvict      A sense of revival rushes through your body.~
Affect       '' 13 '100' -1
Minlevel     29
End

#SKILL
Name         occular explosium~
Type         Spell
Info         3413
Flags        38912
Target       1
Minpos       112
Slot         95
Mana         40
Rounds       10
Code         spell_smaug
Dammsg       occular explosium~
Hitchar      You nearly cause $N's eyes to explode!~
Hitvict      Blood spurts from your eye sockets as you experience the pain
of oc
cular explosium!~
Dice         20d1~
Components   v+49~
Affect       '' 60 '95' -1
Affect       'l*3' 29 'blindness' -1
Minlevel     16
End

#SKILL
Name         paranoia~
Type         Spell
Info         2952
Flags        6144
Target       1
Minpos       111
Saves        5
Mana         20
Rounds       10
Code         spell_smaug
Dammsg       paranoia attack~
Wearoff      You are no longer paranoid.~
Hitchar      You cause $N to become paranoid!~
Hitvict      It seems as if everyone is out to kill you! Beware!~
Misschar     $N is too stable to become paranoid.~
Affect       '' 60 '227' -1
Affect       'l+25' 26 'paranoia' -1
Affect       'l+25' 3 '-1' -1
Minlevel     38
End

#SKILL
Name         pass door~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         74
Mana         50
Rounds       12
Code         spell_pass_door
Dammsg       ~
Wearoff      You feel solid again.~
Minlevel     18
End

#SKILL
Name         pentagram~
Type         Spell
Info         8
Flags        22528
Minpos       110
Slot         97
Mana         80
Rounds       20
Code         spell_smaug
Dammsg       pentagram~
Wearoff      The evil force surrounding you slowly dissipates... ~
Hitchar      An evil force surrounds you as you make the mark of the Pentagram.~
Hitvict      A Pentagram appears!~
Hitroom      $n fashions a Pentagram from the forces of chaos!~
Misschar     You fail to complete the pentagram.~
Value        482
Affect       '' 12 '50' -1
Affect       'l*20' 17 '-20' -1
Affect       'l*20' 26 'protect' -1
Minlevel     16
End

#SKILL
Name         pestilence~
Type         Spell
Info         1864
Flags        0
Minpos       111
Slot         95
Mana         60
Rounds       10
Code         spell_smaug
Dammsg       ~
Value        453
Components   v482 v2072~
Minlevel     50
End

#SKILL
Name         plague~
Type         Spell
Info         2390
Flags        6160
Minpos       111
Saves        1
Slot         96
Mana         90
Rounds       14
Code         spell_smaug
Dammsg       plague~
Hitchar      A black blister erupts as the Plague spreads!~
Hitvict      Your skin erupts in black blisters!~
Hitroom      $n calls forth the ancient curse of the Plague!~
Dice         1d50~
Components   v+482~
Affect       '' 26 'plague' -1
Affect       'l*4' 14 '-10' -1
Minlevel     18
End

#SKILL
Name         plant pass~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         291
Mana         60
Rounds       12
Code         spell_plant_pass
Dammsg       ~
Wearoff      !Plant Pass!~
Minlevel     35
End

#SKILL
Name         poison~
Type         Spell
Info         0
Flags        131072
Target       1
Minpos       112
Slot         33
Mana         10
Rounds       20
Code         spell_poison
Dammsg       poison~
Wearoff      You feel less sick.~
Minlevel     6
End

#SKILL
Name         polymorph~
Type         Spell
Info         0
Flags        0
Minpos       111
Slot         294
Mana         60
Rounds       12
Code         spell_null
Dammsg       ~
Wearoff      !Polymorph!~
Minlevel     51
End

#SKILL
Name         portal~
Type         Spell
Info         0
Flags        0
Minpos       111
Slot         220
Mana         70
Rounds       25
Code         spell_portal
Dammsg       ~
Wearoff      !Portal!~
Minlevel     21
End

#SKILL
Name         possess~
Type         Spell
Info         0
Flags        0
Minpos       111
Slot         232
Mana         120
Rounds       30
Code         spell_possess
Dammsg       possess~
Wearoff      You return to your body.~
Components   v+13~
Minlevel     35
End

#SKILL
Name         protection~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         34
Mana         5
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      You feel less protected from evil.~
Hitvict      You are blessed with a protection from evil.~
Hitroom      $N is blessed with a protection from evil.~
Affect       'l*10' 26 'protection' 13
Minlevel     6
End

#SKILL
Name         psionic blast~
Type         Spell
Info         911
Flags        6144
Minpos       110
Slot         95
Mana         10
Rounds       10
Range        1
Code         spell_smaug
Dammsg       Psionic Blast~
Hitchar      Your psionic blast drains $N!~
Hitroom      $n blasts $N with $s psionic powers!~
Dice         75d2~
Affect       '' 12 '-10' -1
Affect       'l*3' 1012 'i/2' -1
Minlevel     50
End

#SKILL
Name         psychic surge~
Type         Spell
Info         12684
Flags        6144
Target       1
Minpos       111
Mana         20
Rounds       10
Code         spell_smaug
Dammsg       psychic surge~
Wearoff      !Psychic Surge!~
Hitchar      You send a wave of psychic energy toward $N and $E howls in pain.~
Hitvict      $n points at you and you are engulfed in blinding pain.~
Dice         L/3~
Affect       'i' 26 'blind' 0
Minlevel     51
End

#SKILL
Name         quantum spike~
Type         Spell
Info         908
Flags        0
Target       1
Minpos       107
Slot         314
Mana         15
Rounds       10
Code         spell_quantum_spike
Dammsg       Quantum Spike~
Wearoff      !WEAROFF!~
Minlevel     48
End

#SKILL
Name         quickening~
Type         Spell
Info         328
Flags        0
Target       3
Minpos       110
Slot         95
Mana         60
Rounds       10
Code         spell_smaug
Dammsg       ~
Wearoff      Your pulse returns to a normal rate.~
Hitvict      You feel your pulse quicken!~
Missvict     You fail to gather enough magic to perform. spell.~
Affect       'l+50' 26 'quickening' -1
Affect       'l+50' 2 '+2' -1
Minlevel     21
End

#SKILL
Name         razorbait~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       110
Slot         211
Mana         45
Rounds       14
Code         spell_smaug
Dammsg       ~
Wearoff      You are no longer so frightened of stabbing weapons.~
Hitchar      You place a fear of stabbing weapons in $N's mind...~
Hitvict      You suddenly grow weary of stabbing weapons...~
Hitroom      $N flinches as someone brandishes a stabbing weapon.~
Immchar      $N shrugs off the fear you try to plant in his mind.~
Affect       'l*23' 29 '32' -1
Minlevel     18
End

#SKILL
Name         recharge~
Type         Spell
Info         0
Flags        0
Target       4
Minpos       111
Slot         229
Mana         100
Rounds       24
Code         spell_recharge
Dammsg       ~
Wearoff      !Recharge!~
Minlevel     33
End

#SKILL
Name         refresh~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         81
Mana         12
Rounds       18
Code         spell_smaug
Dammsg       refresh~
Wearoff      !Refresh!~
Hitchar      You allow blooming vitality to flow from you to $N.~
Hitvict      Blooming vitality flows through you.~
Affect       '' 14 'l' -1
Minlevel     3
End

#SKILL
Name         remove curse~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         35
Mana         5
Rounds       12
Code         spell_remove_curse
Dammsg       ~
Wearoff      !Remove Curse!~
Minlevel     12
End

#SKILL
Name         remove invis~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         230
Mana         10
Rounds       12
Code         spell_remove_invis
Dammsg       ~
Wearoff      !Remove Invis!~
Minlevel     6
End

#SKILL
Name         remove trap~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         87
Mana         35
Rounds       12
Code         spell_remove_trap
Dammsg       ~
Wearoff      !Remove trap!~
Minlevel     15
End

#SKILL
Name         resilience~
Type         Spell
Info         0
Flags        6144
Target       2
Minpos       111
Slot         106
Mana         47
Rounds       13
Code         spell_smaug
Dammsg       ~
Wearoff      You no longer feel resilient.~
Hitchar      You bless $N with a holy resilience.~
Hitvict      You feel resilient.~
Hitroom      $N grows resilient from a powerful spell.~
Affect       'l*23' 27 '512' -1
Minlevel     40
End

#SKILL
Name         restoration~
Type         Spell
Info         0
Flags        14336
Minpos       110
Slot         113
Mana         80
Rounds       20
Code         spell_smaug
Dammsg       ~
Hitvict      A warm feeling flows through you.~
Hitroom      A warm feeling flows through $N.~
Participants 2
Components   V@43~
Affect       '' 13 '125' -1
Minlevel     30
End

#SKILL
Name         restore mana~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       110
Slot         84
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      !Restore Mana!~
Hitvict      A surge of mana spreads throughout your being.~
Affect       '' 12 'l+2d10' -1
Minlevel     51
End

#SKILL
Name         reveal~
Type         Spell
Info         328
Flags        6144
Target       3
Minpos       112
Mana         50
Rounds       13
Code         spell_smaug
Dammsg       ~
Wearoff      You can no longer see the invisible forces around you.~
Hitchar      The invisible world is revealed unto you!~
Hitvict      The invisible world is revealed unto you!~
Affect       'l*24' 26 'detect_invis' 3
Minlevel     20
End

#SKILL
Name         rid toxins~
Type         Spell
Info         328
Flags        6144
Target       3
Minpos       110
Mana         80
Rounds       10
Code         spell_cure_poison
Dammsg       ~
Wearoff      !Cure Poison!~
Hitvict      The rite of cleansing has freed you from curses and toxins!~
Hitroom      $n summons the Umbral demons to rid $mself of toxins!~
Minlevel     14
End

#SKILL
Name         sacral divinity~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       112
Slot         345
Mana         100
Rounds       12
Code         spell_sacral_divinity
Dammsg       ~
Wearoff      You are no longer protected by the Gods.~
Components   V@65~
Minlevel     35
End

#SKILL
Name         sagacity~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         228
Mana         12
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Furrowing your brow, you sense the weight of wisdom leave you.~
Hitchar      $N grows serious as wisdom takes root within $M.~
Hitvict      The wisdom of your elders blossoms within you.~
Hitroom      $N grows serious as wisdom takes root within $M.~
Affect       'l*24' 4 '1+(l/17)' -1
Minlevel     12
End

#SKILL
Name         sanctuary~
Type         Spell
Info         336
Flags        0
Target       2
Minpos       112
Slot         36
Mana         75
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The luminous aura about your body fades away.~
Hitchar      A luminous aura spreads slowly over $N's body.~
Hitvict      A luminous aura spreads slowly over your body.~
Hitroom      A luminous aura spreads slowly over $N's body.~
Affect       '(l*4)+30' 26 'sanctuary' 7
Minlevel     13
End

#SKILL
Name         sand of Hades~
Type         Spell
Info         3976
Flags        133138
Minpos       112
Saves        5
Slot         128
Mana         70
Rounds       18
Code         spell_smaug
Dammsg       sandstorm~
Wearoff      The sand storm dissipates.~
Hitvict      You're blinded by a swirling sand storm!~
Affect       'l*4' 26 'blind' 0
Affect       'l*4' 26 'blind' -1
Affect       '' 14 '-10' -1
Minlevel     37
End

#SKILL
Name         saturation~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       112
Mana         1
Rounds       12
Code         spell_smaug
Dammsg       MOB SPELL ONLY~
Wearoff      The last drops of water finally evaporate from your body~
Hitchar      You call forth the powers of the water and soak $N to the bone!~
Hitvict      Water slowly seeps into your equipment, making you feel heavy.~
Affect       '' 14 '-(l*4+50)' -1
Affect       '1d100+100' 14 '-(l*4+25)' -1
Minlevel     51
End

#SKILL
Name         scorching surge~
Type         Spell
Info         1
Flags        0
Target       1
Minpos       109
Slot         296
Mana         25
Rounds       8
Code         spell_scorching_surge
Dammsg       scorching surge~
Wearoff      !Scorching Surge!~
Minlevel     28
End

#SKILL
Name         scry~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         91
Mana         20
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your clairvoyance slowly disintegrates...~
Hitchar      $N's eyes glaze over as $E endures a vision...~
Hitvict      You receive a revelatory vision...~
Hitroom      $N's eyes glaze over as $E endures a vision...~
Affect       'l*23' 26 'scry' 24
Minlevel     15
End

#SKILL
Name         seduction~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       110
Mana         75
Rounds       20
Code         spell_charm_person
Dammsg       ~
Hitchar      $N has fallen prey to your seduction!~
Hitvict      $n has seduced you against your will!~
Components   v@13~
Minlevel     18
End

#SKILL
Name         shadowform~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         225
Mana         110
Rounds       20
Code         spell_smaug
Dammsg       ~
Wearoff      The shadows surrounding your form dissipate...~
Hitvict      You dematerialize to shadow form.~
Hitroom      Swirling masses of shadows rise to consume $N.~
Affect       'l*10' 27 '8192' -1
Minlevel     47
End

#SKILL
Name         shield~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       109
Slot         67
Mana         12
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      Your force shield shimmers then fades away.~
Hitchar      A force shield of shimmering blue surrounds $N.~
Hitvict      A force shield of shimmering blue surrounds you.~
Hitroom      A force shield of shimmering blue surrounds $N.~
Affect       'l*24' 17 '-20' -1
Minlevel     13
End

#SKILL
Name         shocking grasp~
Type         Spell
Info         3
Flags        0
Target       1
Minpos       105
Slot         51
Mana         15
Rounds       12
Code         spell_shocking_grasp
Dammsg       shocking grasp~
Wearoff      !Shocking Grasp!~
Minlevel     7
End

#SKILL
Name         shockshield~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       111
Slot         89
Mana         95
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The torrents of cascading energy suddenly fade away.~
Hitvict      Torrents of cascading energy form around you.~
Hitroom      Torrents of cascading energy form around $N.~
Affect       '(l*3)+14' 26 'shockshield' 26
Minlevel     35
End

#SKILL
Name         sleep~
Type         Spell
Info         0
Flags        131072
Minpos       112
Slot         38
Mana         15
Rounds       12
Code         spell_sleep
Dammsg       ~
Wearoff      You feel less tired.~
Minlevel     12
End

#SKILL
Name         slink~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         205
Mana         12
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      You suddenly feel less coordinated...~
Hitchar      $N suddenly appears more agile...~
Hitvict      You suddenly feel more nimble...~
Hitroom      $N suddenly appears more agile...~
Affect       'l*24' 2 '1+(l/17)' -1
Minlevel     9
End

#SKILL
Name         solar flight~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         293
Mana         60
Rounds       12
Code         spell_solar_flight
Dammsg       ~
Wearoff      !Solar flight!~
Minlevel     39
End

#SKILL
Name         solomonic invocation~
Type         Spell
Info         520
Flags        16384
Minpos       111
Slot         344
Mana         50
Rounds       24
Code         spell_smaug
Dammsg       ~
Hitchar      A brilliant sphere of light coalesces into a gleaming silver cross.~
Hitroom      A brilliant sphere of light coalesces into a gleaming silver cross in $n's hands.~
Value        65
Minlevel     9
End

#SKILL
Name         sonic resonance~
Type         Spell
Info         908
Flags        0
Target       1
Minpos       109
Slot         309
Mana         15
Rounds       8
Code         spell_sonic_resonance
Dammsg       Sonic Resonance~
Wearoff      !WEAROFF!~
Minlevel     19
End

#SKILL
Name         soul petrification~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       108
Slot         129
Mana         30
Rounds       13
Code         spell_smaug
Dammsg       petrification~
Wearoff      Your soul becomes vulnerable again.~
Hitchar      You petrify the soul of yet another!~
Hitroom      $n petrifies the soul of $N!~
Affect       'l*25' 26 'petrification' -1
Affect       'l*25' 17 '-40' -1
Minlevel     48
End

#SKILL
Name         spectral furor~
Type         Spell
Info         908
Flags        0
Target       1
Minpos       109
Slot         306
Mana         15
Rounds       8
Code         spell_spectral_furor
Dammsg       Spectral Furor~
Wearoff      !WEAROFF!~
Minlevel     14
End

#SKILL
Name         spiral blast~
Type         Spell
Info         0
Flags        0
Minpos       107
Slot         295
Mana         35
Rounds       4
Code         spell_spiral_blast
Dammsg       spiral blast~
Wearoff      !Spiral Blast!~
Minlevel     46
End

#SKILL
Name         spiritual wrath~
Type         Spell
Info         396
Flags        6160
Minpos       110
Slot         115
Mana         120
Rounds       24
Code         spell_smaug
Dammsg       spiritual wrath~
Dice         44*W~
Participants 2
Components   V@43~
Affect       '6' 26 'blindness' 0
Minlevel     48
End

#SKILL
Name         stone skin~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       109
Slot         66
Mana         12
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      Your skin feels soft again.~
Hitvict      Your skin hardens to a malleable stone.~
Hitroom      $N's skin hardens to a malleable stone.~
Affect       'l*23' 17 '-40' -1
Minlevel     17
End

#SKILL
Name         sulfurous spray~
Type         Spell
Info         909
Flags        0
Target       1
Minpos       111
Slot         308
Mana         15
Rounds       8
Code         spell_sulfurous_spray
Dammsg       Sulfurous spray~
Wearoff      !WEAROFF!~
Minlevel     18
End

#SKILL
Name         summon~
Type         Spell
Info         0
Flags        0
Minpos       111
Slot         40
Mana         50
Rounds       12
Code         spell_summon
Dammsg       ~
Wearoff      !Summon!~
Minlevel     8
End

#SKILL
Name         swordbait~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       111
Slot         209
Mana         55
Rounds       25
Code         spell_smaug
Dammsg       ~
Wearoff      Your fear of slashing weapons dissipates.~
Hitchar      You place a fear of slashing weapons in $N's mind...~
Hitvict      A fear of slashing weapons occupies your thoughts.~
Hitroom      $N flinches as someone brandishes a slashing weapon.~
Immchar      $N shrugs off the fear you try to plant in his mind.~
Affect       'l*23' 29 '64' -1
Minlevel     23
End

#SKILL
Name         teleport~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       110
Slot         2
Mana         35
Rounds       12
Code         spell_teleport
Dammsg       ~
Wearoff      !Teleport!~
Minlevel     8
End

#SKILL
Name         torrent~
Type         Spell
Info         395
Flags        0
Target       1
Minpos       109
Slot         400
Mana         15
Rounds       12
Code         spell_lightning_bolt
Dammsg       tendril of lightning~
Missroom     supress~
Minlevel     51
End

#SKILL
Name         transport~
Type         Spell
Info         0
Flags        0
Minpos       111
Slot         219
Mana         70
Rounds       15
Code         spell_transport
Dammsg       ~
Wearoff      !Transport!~
Minlevel     17
End

#SKILL
Name         trollish vigor~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         206
Mana         16
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your vigor subsides, leaving you more charismatic.~
Hitchar      $N's face contorts with a bestial vigor...~
Hitvict      You sense a bestial vigor consume you...~
Hitroom      $N's face contorts with a bestial vigor...~
Affect       'l*24' 5 '1+(l/17)' -1
Affect       'l*24' 25 '-1' -1
Minlevel     6
End

#SKILL
Name         true sight~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       112
Slot         235
Mana         70
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      Your vision descends to the material plane.~
Hitvict      Your vision is elevated to the highest plane.~
Hitroom      $N's eyes begin to glow a soft white.~
Affect       'l-10' 26 'truesight' 22
Minlevel     38
End

#SKILL
Name         umbral spear~
Type         Spell
Info         13199
Flags        0
Target       1
Minpos       110
Slot         95
Mana         40
Rounds       24
Code         spell_smaug
Dammsg       umbral spear~
Hitchar      You fashion a spear of deepest umbra and unleash it on $N!~
Hitvict      $n unleashes an Umbral Spear on you!~
Hitroom      $n's Umbral Spear penetrates $N's flesh!~
Dice         25d2~
Affect       'l*3' 18 '-1' -1
Affect       '' 60 '95' -1
Minlevel     30
End

#SKILL
Name         unknown~
Type         Spell
Info         0
Flags        0
Minpos       105
Slot         95
Code         spell_smaug
Dammsg       ~
Minlevel     51
End

#SKILL
Name         unravel defense~
Type         Spell
Info         0
Flags        6144
Target       1
Minpos       109
Slot         110
Mana         70
Rounds       15
Code         spell_smaug
Dammsg       ~
Wearoff      Your intricate defenses are restored to their original quality.~
Hitchar      $N's eyes widen in fear as $S defenses are unravelled.~
Hitvict      Your defenses are unravelled before your very eyes.~
Hitroom      $N's eyes widen in fear as $S defenses are unravelled.~
Immchar      Your spell has no effect upon $N.~
Affect       '(l*3)+25' 29 '1048576' -1
Affect       '(l*3)+25' 29 '8192' -1
Minlevel     33
End

#SKILL
Name         uplift~
Type         Spell
Info         0
Flags        32800
Minpos       112
Slot         95
Mana         50
Rounds       16
Code         spell_smaug
Dammsg       ~
Hitchar      You reach across the planes and aid $N...~
Hitvict      The mystical hand of $n appears from nowhere and aids you...~
Hitroom      A mystical hand appears from nowhere and aids $N...~
Misschar     A barrier in the planes prevents you from reaching them.~
Affect       '' 13 '20' -1
Minlevel     37
End

#SKILL
Name         valiance~
Type         Spell
Info         0
Flags        0
Target       2
Minpos       111
Slot         208
Mana         40
Rounds       18
Code         spell_smaug
Dammsg       ~
Wearoff      Your resistance to paralysis slowly fades away...~
Hitchar      $N is surrounded by an aura which protects $M from paralysis.~
Hitvict      A magical resistance to paralysis consumes you.~
Hitroom      $N is surrounded by an aura which protects $M from paralysis.~
Affect       'l*23' 27 '2097152' -1
Minlevel     14
End

#SKILL
Name         venomous touch~
Type         Spell
Info         398
Flags        0
Target       1
Minpos       112
Mana         5
Rounds       12
Code         spell_poison
Dammsg       ~
Wearoff      The venom in your blood is finally purged.~
Hitchar      Toxic venom passes into the body of $N.~
Hitvict      You shiver as $n reaches towards you and infuses you with toxic venom!~
Hitroom      $N shivers as $n infuses $M body with toxic venom.~
Misschar     You reach a hand toward $N, but $E evades your venomous
touch.~
Minlevel     5
End

#SKILL
Name         venomshot~
Type         Spell
Info         1934
Flags        0
Target       1
Rounds       1
Code         spell_smaug
Dammsg       venomous tentacle~
Dice         1000d5~
Affect       'l*50' 26 'poison' 12
Minlevel     51
End

#SKILL
Name         ventriloquate~
Type         Spell
Info         0
Flags        0
Minpos       112
Slot         41
Mana         5
Rounds       12
Code         spell_ventriloquate
Dammsg       ~
Wearoff      !Ventriloquate!~
Minlevel     1
End

#SKILL
Name         weaken~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       107
Slot         68
Mana         20
Rounds       12
Code         spell_weaken
Dammsg       spell~
Wearoff      You feel stronger.~
Minlevel     2
End

#SKILL
Name         wine invocation~
Type         Spell
Info         8
Flags        16384
Minpos       112
Slot         95
Mana         40
Rounds       5
Code         spell_smaug
Dammsg       ~
Hitchar      You use your mental powers to turn water into wine!~
Value        936
Components   Kwater~
Minlevel     8
End

#SKILL
Name         winter mist~
Type         Spell
Info         0
Flags        0
Target       1
Minpos       110
Slot         214
Mana         65
Rounds       8
Code         spell_smaug
Dammsg       ~
Wearoff      The magical chilling mist about you subsides.~
Hitchar      You erect a chilling mist about $N...~
Hitvict      A magical chilling mist flurries about you...~
Hitroom      A magical chilling mist flurries about $N...~
Immchar      The chilling mist dissipates before it can reach $N.~
Affect       'l*23' 29 '2' -1
Minlevel     26
End

#SKILL
Name         word of recall~
Type         Spell
Info         0
Flags        0
Target       3
Minpos       106
Slot         42
Mana         5
Rounds       12
Code         spell_word_of_recall
Dammsg       ~
Wearoff      !Word of Recall!~
Minlevel     5
End

#SKILL
Name         zidros Wrath~
Type         Spell
Info         1935
Flags        6144
Target       1
Minpos       112
Slot         300
Mana         125
Code         spell_smaug
Dammsg       Wrath of Zidros~
Hitchar      You summon the Wrath of Zidros to steal your victims magic.~
Hitvict      You feel your magic powers dwindle as the Wrath of Zidros is laid upon you!~
Hitroom      $n has summoned the Wrath of Zidros!~
Components   v+482~
Affect       'l*2' 4 '-1' -1
Affect       'l*2' 12 '-50' -1
Affect       '50' 1012 'l/2' -1
Affect       '' 60 '400' -1
Minlevel     45
End
--]]
end

function LoadSkills()
	skill = CreateSkill("aggressive style", "skill", 107, 0, 1);
	skill.Rounds = 4;
	skill.WearOffMessage = "!aggressive style!";
	skill:AddTeacher(10340);
	skill:AddTeacher(3004);
	
	skill = CreateSkill("aid", "skill", 107, 0, 4);
	skill.Rounds = 12;
	skill.SkillFunctionName = "do_aid";
	skill.WearOffMessage = "!Aid!";
	
	skill = CreateSkill("backstab", "skill", 112, 0, 1);
	skill.Rounds = 12;
	skill.SkillFunctionName = "do_backstab";
	skill.DamageMessage = "backstab";
	skill.WearOffMessage = "!Backstab!";
	
	skill = CreateSkill("bash", "skill", 105, 0, 12);
	skill:SetTargetByValue(1);
	skill.rounds = 26;
	skill.SkillFunctionName = "do_bash";
	skill.DamageMessage = "bash";
	skill.WearOffMessage = "!Bash!";
	
	skill = CreateSkill("berserk", "skill", 111, 0, 51);
	skill:SetTargetByValue(1);
	skill.Rounds = 4;
	skill.SkillFunctionName = "do_berserk";
	skill.WearOffMessage = "You regain your composure...";
	skill:AddTeacher(1);
	
	skill = CreateSkill("berserk style", "skill", 110, 0, 5);
	skill.Rounds = 4;
	skill.WearOffMessage = "!berserk style!";
	skill:AddTeacher(10340);
	skill:AddTeacher(3004);
	
--[[
#SKILL
Name         bite~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       105
Rounds       8
Code         do_bite
Dammsg       bite~
Wearoff      !Bite!~
Minlevel     11
End

#SKILL
Name         blitz~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       105
Rounds       8
Code         spell_smaug
Dammsg       Blitz~
Hitchar      You rush $N and tackle them to the ground.~
Hitvict      $n rushes at you and tackles you to the ground.~
Hitroom      $n rushes at $N and tackles them to the ground.~
Misschar     You rush your opponent hoping to tackle them, but you miss and nearly fall down.~
Missvict     $n rushes forward to tackle you, but misses and nearly falls flat on $s face.~
Missroom     $n rushes forward to tackle $N, but misses and nearly falls flat on $s face.~
Dice         l/2 { 40~
Minlevel     28
End

#SKILL
Name         bloodlet~
Type         Skill
Info         0
Flags        0
Minpos       105
Rounds       12
Code         do_bloodlet
Dammsg       ~
Minlevel     11
End

#SKILL
Name         brew~
Type         Skill
Info         0
Flags        0
Target       4
Minpos       112
Rounds       8
Code         do_brew
Dammsg       ~
Wearoff      !Brew!~
Minlevel     25
End

#SKILL
Name         broach~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       24
Code         do_broach
Dammsg       ~
Minlevel     41
End

#SKILL
Name         circle~
Type         Skill
Info         0
Flags        0
Minpos       107
Rounds       8
Code         do_circle
Dammsg       circle~
Wearoff      !Circle!~
Minlevel     25
End

#SKILL
Name         claw~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       105
Rounds       8
Code         spell_null
Dammsg       slash~
Minlevel     51
End

#SKILL
Name         cleave~
Type         Skill
Info         0
Flags        0
Minpos       106
Rounds       18
Guild        99
Code         do_cleave
Dammsg       cleave~
Minlevel     50
End

#SKILL
Name         climb~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       112
Rounds       10
Code         do_climb
Dammsg       climb~
Wearoff      !climb!~
Minlevel     1
End

#SKILL
Name         cook~
Type         Skill
Info         0
Flags        0
Minpos       110
Slot         100
Rounds       14
Code         do_cook
Dammsg       ~
Misschar     You aren't a very good cook yet.~
Minlevel     1
End

#SKILL
Name         cuff~
Type         Skill
Info         9096
Flags        128
Target       1
Minpos       109
Rounds       8
Code         spell_smaug
Dammsg       cuff~
Dice         L/3~
Minlevel     10
End

#SKILL
Name         defensive style~
Type         Skill
Info         0
Flags        0
Rounds       4
Code         spell_null
Dammsg       ~
Wearoff      !defensive style!~
Teachers     10340 3004~
Minlevel     1
End

#SKILL
Name         detrap~
Type         Skill
Info         0
Flags        0
Minpos       108
Rounds       24
Code         do_detrap
Dammsg       ~
Wearoff      !Detrap!~
Minlevel     16
End

#SKILL
Name         dig~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       20
Code         do_dig
Dammsg       ~
Minlevel     1
End

#SKILL
Name         disarm~
Type         Skill
Info         0
Flags        0
Minpos       107
Rounds       24
Code         do_disarm
Dammsg       ~
Wearoff      !Disarm!~
Minlevel     10
End

#SKILL
Name         dodge~
Type         Skill
Info         0
Flags        0
Minpos       107
Code         spell_null
Dammsg       ~
Wearoff      !Dodge!~
Minlevel     1
End

#SKILL
Name         dominate~
Type         Skill
Info         2048
Flags        0
Target       1
Minpos       112
Mana         5
Rounds       12
Code         spell_charm_person
Dammsg       ~
Minlevel     14
End

#SKILL
Name         doorbash~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       24
Code         do_bashdoor
Dammsg       bashdoor~
Wearoff      !Bash Door!~
Minlevel     7
End

#SKILL
Name         dual wield~
Type         Skill
Info         0
Flags        0
Minpos       107
Code         spell_null
Dammsg       ~
Wearoff      !Dual Wield!~
Minlevel     10
End

#SKILL
Name         elbow~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       107
Rounds       8
Code         spell_smaug
Dammsg       elbow~
Hitchar      You swing your elbow in a vicious arc hitting $N.~
Hitvict      $n swings $s elbow in a vicious arc and hits you.~
Hitroom      $n swings $s elbow in a vicious arc, hitting $N.~
Misschar     You swing your elbow in a vicious arc intended to obliterate your target, but miss.~
Missvict     $n swings $s elbow in a vicious arc fully intent on obliterating you, but fortunately misses.~
Missroom     $n swings $s elbow in a vicious arc, intent on obliterating $s target, but misses altogether.~
Dice         l/2 { 25~
Minlevel     15
End

#SKILL
Name         enhanced damage~
Type         Skill
Info         0
Flags        0
Minpos       105
Code         spell_null
Dammsg       ~
Wearoff      !Enhanced Damage!~
Minlevel     1
End

#SKILL
Name         evasive style~
Type         Skill
Info         0
Flags        0
Rounds       4
Code         spell_null
Dammsg       ~
Wearoff      !evasive style!~
Teachers     10340 3004~
Minlevel     1
End

#SKILL
Name         feed~
Type         Skill
Info         0
Flags        0
Minpos       105
Rounds       10
Code         do_feed
Dammsg       bite~
Wearoff      !Feed!~
Minlevel     3
End

#SKILL
Name         fifth attack~
Type         Skill
Info         0
Flags        0
Minpos       105
Code         spell_null
Dammsg       ~
Wearoff      !Fifth Attack!~
Minlevel     42
End

#SKILL
Name         fourth attack~
Type         Skill
Info         0
Flags        0
Minpos       105
Code         spell_null
Dammsg       ~
Wearoff      !Fourth Attack!~
Minlevel     32
End

#SKILL
Name         gouge~
Type         Skill
Info         0
Flags        0
Minpos       105
Rounds       10
Code         do_gouge
Dammsg       gouge~
Wearoff      !Gouge!~
Minlevel     20
End

#SKILL
Name         grapple~
Type         Skill
Info         0
Flags        0
Minpos       109
Rounds       36
Guild        99
Code         do_grapple
Dammsg       !grapple!~
Wearoff      You stop grappling.~
Minlevel     50
End

#SKILL
Name         grasp suspiria~
Type         Skill
Info         8591
Flags        0
Target       1
Minpos       105
Saves        5
Mana         15
Rounds       12
Code         spell_smaug
Dammsg       suspiric grasp~
Hitchar      $N's deathrattle fills the air as you drain $S life...~
Hitvict      A deathrattle escapes your throat as $n drains your life.~
Hitroom      A deathrattle escapes $N's throat as $n drains $S life.~
Misschar     You clench a clawed fist in anger as $N escapes your grasp.~
Missroom     $n grimaces at $N, clenching a clawed fist in anger...~
Immchar      $N is unphased by your grasp...~
Dice         (7d4*i)+20~
Minlevel     50
End

#SKILL
Name         grip~
Type         Skill
Info         0
Flags        0
Minpos       105
Code         spell_null
Dammsg       ~
Wearoff      !Grip!~
Minlevel     8
End

#SKILL
Name         headbutt~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       105
Rounds       8
Code         spell_smaug
Dammsg       headbutt~
Hitchar      You rear your head back and headbutt $N.~
Hitvict      $n rears $s head back and headbutts you in the face.~
Hitroom      $n rears $s head back and headbutts $N.~
Misschar     You rear your head back prepared to headbutt your opponent, but they move out of range of your blow.~
Missvict     $n rears back $s head preparing to headbutt you, but you move back and out of range of their blow.~
Missroom     $n rears back $s head preparing to headbutt $s foe, but $N steps back out of range of their blow.~
Dice         l/2 { 35~
Minlevel     20
End

#SKILL
Name         hide~
Type         Skill
Info         0
Flags        0
Target       3
Minpos       112
Rounds       12
Code         do_hide
Dammsg       ~
Wearoff      You are no longer hidden.~
Hitvict      You attempt to hide.~
Misschar     You attempt to hide.~
Affect       'l*23' 26 'hide' 16
Minlevel     1
End

#SKILL
Name         hitall~
Type         Skill
Info         0
Flags        65536
Minpos       112
Rounds       12
Code         do_hitall
Dammsg       hit~
Teachers     25199~
Minlevel     50
End

#SKILL
Name         jab~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       107
Rounds       8
Code         spell_smaug
Dammsg       jab~
Hitchar      You throw a light jab at $N.~
Hitvict      $n throws a light jab and hits you.~
Hitroom      $n throws a light jab at $N.~
Misschar     You throw a light jab at $N but miss totally.~
Missvict     $n throws a light jab at your face, but misses totally.~
Missroom     $n throws a light jab at $N but misses totally.~
Dice         l/2 { 20~
Minlevel     20
End

#SKILL
Name         kick~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       105
Rounds       8
Code         do_kick
Dammsg       kick~
Minlevel     1
End

#SKILL
Name         knee~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       107
Rounds       8
Code         spell_smaug
Dammsg       knee~
Hitchar      You strike out at $N with your knee.~
Dice         l/2 { 25~
Minlevel     15
End

#SKILL
Name         leap~
Type         Skill
Info         904
Flags        6272
Target       1
Minpos       105
Rounds       8
Code         spell_smaug
Dammsg       leap~
Hitchar      You leap into the air and kick $N on the chest with both of your feet.~
Hitvict      $n leaps into the air and kicks you on the chest with both of $s feet.~
Hitroom      $n leaps into the air and kicks $N on the chest with both of $s feet.~
Misschar     You leap into the air attempting to kick $N with both feet, but miss and fall flat on your rump.~
Missvict     $n leaps into the air attempting to kick you with both feet, but misses and falls flat on $s rump.~
Missroom     $n leaps into the air attempting to kick $N with both feet, but misses and falls flat on $s rump.~
Dice         l/2 { 35~
Minlevel     18
End

#SKILL
Name         lunge~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       107
Rounds       8
Code         spell_smaug
Dammsg       lunge~
Hitchar      You lunge at $N, catching $m off guard and knocking $m to the floor.~
Hitvict      $n lunges at you, catching you off guard and knocking you to the floor.~
Hitroom      $n lunges at $N, catching $m off guard and knocking $m to the floor.~
Misschar     You lunge at $N intending to catch $m off guard, but fall short and miss altogether.~
Missvict     $n lunges at you intending to catch you off guard, but falls short and misses altogether.~
Missroom     $n lunges at $s target intent on catching $N off guard, but falls short and misses altogether.~
Dice         l/2 { 35~
Minlevel     20
End

#SKILL
Name         meditate~
Type         Skill
Info         0
Flags        530432
Target       3
Minpos       106
Rounds       35
Code         do_meditate
Dammsg       ~
Hitvict      You meditate peacefully, collecting mana from the cosmos.~
Missvict     You spend several minutes in deep concentration, but fail to collect any mana.~
Minlevel     12
End

#SKILL
Name         mistform~
Type         Skill
Info         0
Flags        0
Target       3
Minpos       106
Mana         40
Rounds       6
Code         spell_smaug
Dammsg       ~
Wearoff      Your form becomes once again solid.~
Hitvict      You assume a lucent form...~
Hitroom      $N's form grows lucent...~
Affect       'l*5' 26 'pass_door' 20
Minlevel     18
End

#SKILL
Name         mistwalk~
Type         Skill
Info         0
Flags        0
Minpos       112
Slot         290
Mana         50
Rounds       12
Code         do_mistwalk
Dammsg       ~
Wearoff      !Mist Walk!~
Minlevel     31
End

#SKILL
Name         mount~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       10
Code         do_mount
Dammsg       ~
Wearoff      !Search!~
Minlevel     2
End

#SKILL
Name         occulutus visum~
Type         Skill
Info         0
Flags        32768
Target       3
Minpos       106
Mana         30
Rounds       12
Code         spell_smaug
Dammsg       ~
Wearoff      The fire in your eyes slowly fades...~
Hitvict      Your eyes blaze like coals, filling your sight with unseen forms.~
Hitroom      $N's eyes burn like coals in the darkness.~
Affect       'l*14' 26 'detect_hidden' 5
Affect       'l*14' 26 'detect_invis' 3
Minlevel     12
End

#SKILL
Name         parry~
Type         Skill
Info         0
Flags        0
Minpos       107
Code         spell_null
Dammsg       ~
Wearoff      !Parry!~
Minlevel     1
End

#SKILL
Name         peek~
Type         Skill
Info         0
Flags        0
Minpos       112
Code         spell_null
Dammsg       ~
Wearoff      !Peek!~
Minlevel     1
End

#SKILL
Name         pelt~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       114
Rounds       8
Code         spell_smaug
Dammsg       pelt~
Hitchar      You pelt at $N with a sharp blow!~
Hitvict      $n pelts you with a sharp blow!~
Dice         l/2 { 30~
Minlevel     51
End

#SKILL
Name         pick lock~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       12
Code         do_pick
Dammsg       ~
Wearoff      !Pick!~
Minlevel     1
End

#SKILL
Name         poison weapon~
Type         Skill
Info         0
Flags        0
Target       4
Minpos       112
Rounds       12
Code         do_poison_weapon
Dammsg       poisonous concoction~
Wearoff      !Poison Weapon!~
Minlevel     27
End

#SKILL
Name         pounce~
Type         Skill
Info         0
Flags        0
Minpos       112
Mana         15
Rounds       12
Guild        99
Code         do_pounce
Dammsg       pounce~
Minlevel     5
End

#SKILL
Name         pummel~
Type         Skill
Info         9096
Flags        6144
Target       1
Minpos       105
Rounds       8
Code         spell_smaug
Dammsg       pummel~
Hitchar      You strike out angrily with both fists pummeling $N to the ground.~
Hitvict      $n strikes out angrily with both fists pummeling you to the ground.~
Hitroom      $n strikes out angrily with both fists pummeling $N to the ground.~
Misschar     You strike out angrily with both fists hoping to pummel your opponent into the ground, but miss altogether.~
Missvict     $n strikes out angrily with both fists hoping to pummel you into the ground, but misses altogether.~
Missroom     $n strikes out angrily with both fists hoping to pummel $N into the ground, but misses altogether.~
Dice         l/1 { 40~
Minlevel     30
End

#SKILL
Name         punch~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       107
Rounds       8
Code         do_punch
Dammsg       punch~
Minlevel     25
End

#SKILL
Name         punt~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       109
Rounds       8
Code         spell_smaug
Dammsg       punt~
Hitchar      You raise your leg and deal $N a vicious kick.~
Hitvict      $n raises $s leg and deals you a vicious kick.~
Hitroom      $n raises $s leg and deals $N a vicious kick.~
Misschar     You raise your leg to deal a vicious kick, but lose your balance and miss your target.~
Missvict     $n raises his leg to deal you a vicious kick, but loses $s balance and misses you by a mile.~
Missroom     $n raises his leg to deal $s target a vicious kick, but loses $s balance and misses completely.~
Dice         l/2 { 25~
Minlevel     15
End

#SKILL
Name         rescue~
Type         Skill
Info         0
Flags        0
Minpos       105
Rounds       12
Code         do_rescue
Dammsg       ~
Wearoff      !Rescue!~
Minlevel     4
End

#SKILL
Name         roundhouse~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       107
Rounds       8
Code         spell_smaug
Dammsg       roundhouse~
Hitchar      You strike out with a vicious roundhouse kick.~
Hitvict      $n strikes out with a vicious roundhouse kick which catches you on the side of the head.~
Hitroom      $n strikes out at $N with a vicious roundhouse kick.~
Misschar     You strike out with a powerful roundhouse kick which glances off of your opponent.~
Missvict     $n strikes at you with a powerful roundhouse kick which glances off of you.~
Missroom     $n strikes at $N with a powerful roundhouse kick which glances off of $M.~
Dice         l/2 { 30~
Minlevel     25
End

#SKILL
Name         scan~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       12
Code         do_scan
Dammsg       ~
Wearoff      !Scan!~
Minlevel     3
End

#SKILL
Name         scribe~
Type         Skill
Info         0
Flags        0
Target       4
Minpos       112
Rounds       8
Code         do_scribe
Dammsg       ~
Minlevel     30
End

#SKILL
Name         search~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       20
Code         do_search
Dammsg       ~
Minlevel     3
End

#SKILL
Name         second attack~
Type         Skill
Info         0
Flags        0
Minpos       105
Code         spell_null
Dammsg       ~
Minlevel     5
End

#SKILL
Name         shoulder~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       109
Rounds       8
Code         spell_smaug
Dammsg       shoulder~
Hitchar      You crouch low and rush $N with your shoulder.~
Hitvict      $n crouches low and rushes you with $s shoulder.~
Hitroom      $n crouches low and rushes $N with $s shoulder.~
Misschar     You crouch low to rush your opponent with your shoulder, but miss and run right past.~
Missvict     $n crouches low to rush you with $s shoulder, but misjudges and runs right past you.~
Missroom     $n crouches low to rush $s intended target with $s shoulders, but misses totally and almost runs right into you.~
Dice         l/2 { 25~
Minlevel     18
End

#SKILL
Name         shriek~
Type         Skill
Info         392
Flags        16
Minpos       109
Mana         15
Rounds       12
Code         spell_smaug
Dammsg       shriek~
Hitchar      Unfettered anguish explodes from your throat in an ear piercing wail!~
Hitroom      The screams of lost souls erupt from $n, shattering all those around!~
Dice         l~
Minlevel     35
End

#SKILL
Name         slice~
Type         Skill
Info         392
Flags        0
Target       1
Minpos       112
Rounds       12
Code         do_slice
Dammsg       ~
Minlevel     51
End

#SKILL
Name         sneak~
Type         Skill
Info         0
Flags        0
Target       3
Minpos       112
Rounds       1
Code         spell_smaug
Dammsg       ~
Hitvict      You attempt to move silently.~
Misschar     You attempt to move silently.~
Affect       'l*23' 26 'sneak' 15
Minlevel     1
End

#SKILL
Name         spinkick~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       109
Rounds       12
Code         spell_smaug
Dammsg       spinkick~
Wearoff      !spinkick!~
Hitchar      You whirl in a circle and hit $N with a kick.~
Misschar     Your spinkick misses $N.~
Missvict     $n narrowly misses you with a spinkick.~
Dice         l/2 { 30~
Minlevel     19
End

#SKILL
Name         spurn~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       105
Rounds       8
Code         spell_smaug
Dammsg       spurn~
Hitchar      You spurn $N with a blow!~
Dice         l/2 { 35~
Minlevel     10
End

#SKILL
Name         standard style~
Type         Skill
Info         0
Flags        0
Rounds       4
Code         spell_null
Dammsg       ~
Teachers     10340 3004~
Minlevel     1
End

#SKILL
Name         steal~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       24
Code         do_steal
Dammsg       ~
Minlevel     1
End

#SKILL
Name         sting~
Type         Skill
Info         0
Flags        128
Target       1
Minpos       109
Rounds       8
Code         spell_null
Dammsg       sting~
Minlevel     51
End

#SKILL
Name         strike~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       107
Rounds       8
Code         spell_smaug
Dammsg       strike~
Hitchar      You strike out angrily with both fists hitting your opponent squarely in the chest.~
Hitvict      $n strikes out angrily at you with both fists hitting you squarely in the chest.~
Hitroom      $n strikes out angrily with both fists hitting $N squarely in the chest.~
Misschar     You strike out blindly with both your fists and miss your mark.~
Missvict     $n strikes out blindly with both $s fists but completely misses you.~
Missroom     $n strikes out blindly with both $s fists, missing $s intended target and almost hitting you.~
Dice         l/2 { 20~
Minlevel     2
End

#SKILL
Name         stun~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       107
Rounds       8
Code         do_stun
Dammsg       stun~
Wearoff      You regain consciousness.~
Minlevel     10
End

#SKILL
Name         swat~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       109
Rounds       8
Code         spell_smaug
Dammsg       swat~
Hitchar      You swat at $N, hitting $M with the back of your hand.~
Hitvict      $n swats at you, hitting you with the back of $s hand.~
Hitroom      $n swats at $N, hitting $M with the back of $s hand.~
Misschar     You swat at $N, but manage only a glancing blow.~
Missvict     $n swats at you, but manages only a glancing blow.~
Missroom     $n swats at $N, but only manages a glancing blow.~
Dice         l/2 { 15~
Minlevel     10
End

#SKILL
Name         swipe~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       109
Rounds       8
Code         spell_smaug
Dammsg       swipe~
Hitchar      You swipe at $N with your hand.~
Hitvict      $n swipes at you with $s hand.~
Dice         l/2 { 20~
Minlevel     10
End

#SKILL
Name         tail~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       109
Slot         95
Mana         95
Rounds       8
Code         spell_null
Dammsg       tail~
Wearoff      !Tail!~
Minlevel     51
End

#SKILL
Name         tend~
Type         Skill
Info         0
Flags        0
Target       2
Minpos       112
Mana         60
Rounds       12
Code         spell_smaug
Dammsg       ~
Hitchar      You kneel down and tend to $N's wounds.~
Hitvict      Your wounds are soothed and lessened.~
Components   V@65~
Affect       '' 13 'l*2' -1
Minlevel     45
End

#SKILL
Name         third attack~
Type         Skill
Info         0
Flags        0
Minpos       105
Code         spell_null
Dammsg       ~
Minlevel     20
End

#SKILL
Name         track~
Type         Skill
Info         0
Flags        0
Minpos       112
Rounds       14
Code         do_track
Dammsg       ~
Minlevel     6
End

#SKILL
Name         trance~
Type         Skill
Info         0
Flags        530432
Target       3
Minpos       106
Rounds       45
Code         do_trance
Dammsg       ~
Hitvict      You enter a peaceful trance, collecting mana from the cosmos.~
Missvict     You spend several minutes in a deep trance, but fail to collect any mana.~
Minlevel     15
End

#SKILL
Name         tumble~
Type         Skill
Info         0
Flags        0
Target       2
Minpos       107
Code         spell_null
Dammsg       ~
Wearoff      !Tumble!~
Minlevel     46
End

#SKILL
Name         uppercut~
Type         Skill
Info         8584
Flags        6272
Target       1
Minpos       109
Rounds       8
Code         spell_smaug
Dammsg       uppercut~
Hitchar      You lunge forward with an uppercut which hits $N.~
Hitvict      $n lunges forward with an uppercut which hits you solidly on the chin.~
Hitroom      $n lunges forward with an uppercut which hits $N. ~
Misschar     You lunge upward with a wild uppercut which misses completely throwing you slightly off balance.~
Missvict     $n lunges upward with a wild uppercut intended to put out your lights, but misses.~
Missroom     $n lunges upward with a wild uppercut intended to put out $N's lights but misses.~
Dice         l/2 { 30~
Minlevel     28
End

#SKILL
Name         vault~
Type         Skill
Info         9096
Flags        6272
Target       1
Minpos       105
Rounds       8
Code         spell_smaug
Dammsg       vault~
Hitchar      You vault through the air and crash foot first into $N.~
Hitvict      $n vaults through the air and crashes foot first into you.~
Hitroom      $n vaults through the air and crashes feet first into $N.~
Misschar     You vault through the air hoping to catch $N off guard, but miss and roll right by $M.~
Missvict     $n vaults through the air hoping to catch you off guard, but misses and rolls right by you.~
Missroom     $n vaults through the air at $N hoping to catch $M off guard, but misses and rolls right by $M.~
Dice         l/2 { 40~
Minlevel     25
End

#SKILL
Name         veiled steps~
Type         Skill
Info         0
Flags        6144
Target       3
Minpos       112
Rounds       8
Code         spell_smaug
Dammsg       ~
Wearoff      Your footsteps and form once again become discernible.~
Hitchar      Your steps become silent and your form hidden.~
Hitvict      Your steps become silent and your form hidden.~
Misschar     You clumsily attempt to mask your footsteps and hide in
shadows.~
Missvict     You attempt to mask your footsteps.~
Affect       'l*6' 26 'sneak' 15
Affect       'l*6' 26 'hide' 16
Minlevel     17
End

#SKILL
Name         vomica pravus~
Type         Skill
Info         0
Flags        0
Target       1
Minpos       106
Mana         30
Rounds       6
Code         spell_smaug
Dammsg       ~
Hitchar      $N convulses as you spread your curse through $S body...~
Hitvict      You convulse as $n spreads a silent curse through your body.~
Hitroom      $N convulses as $n inflicts a silent curse...~
Misschar     There seems to be no effect...~
Missvict     $n extends a clawed hand toward you...~
Missroom     $n extends a clawed hand toward $N...~
Affect       '12' 26 'curse' 10
Affect       '12' 18 '-(l/8)' -1
Affect       '12' 19 '-(l/8)' -1
Affect       '12' 24 'l/18' -1
Affect       '12' 31 '-1' -1
Minlevel     13
End
--]]
end

function LoadWeapons()
	skill = CreateSkill("bludgeons", "weapon", 0, 0, 1);
	skill.WearOffMessage = "!Bludgeons!";
	
	CreateSkill("flexible arms", "weapon", 0, 0, 1);
	CreateSkill("long blades", "weapon", 0, 0, 1);
	CreateSkill("missile weapons", "weapon", 0, 0, 20);
	CreateSkill("pugilism", "weapon", 0, 0, 1);
	CreateSkill("short blades", "weapon", 0, 0, 1);
	CreateSkill("talonous arms", "weapon", 0, 0, 1);
end

function LoadTongues()
	CreateSkill("common", "tongue", 0, 0, 1);
	CreateSkill("dwarven", "tongue", 0, 0, 1);
	CreateSkill("elvish", "tongue", 0, 0, 1);
	CreateSkill("gith", "tongue", 0, 0, 1);
	CreateSkill("gnomish", "tongue", 0, 0, 1);
	CreateSkill("goblin", "tongue", 0, 0, 1);
	CreateSkill("halfling", "tongue", 0, 0, 1);
	CreateSkill("ogre", "tongue", 0, 0, 1);
	CreateSkill("orcish", "tongue", 0, 0, 1);
	CreateSkill("pixie", "tongue", 0, 0, 1);
	CreateSkill("trollese", "tongue", 0, 0, 1);
end

function CreateSkill(name, skillType, minPosition, slot, minLevel)
	newSkill = LCreateSkill(name, skillType);
	skill.this = newSkill;
	skill.this.MinimumPosition = minPosition;
	skill.this.Slot = slot;
	skill.this.MinimumLevel = minLevel;
	return skill.this;
end

function CreateSmaugAffect(duration, location, modifier, flags)
	newAffect = LCreateSmaugAffect(duration, location, modifier, flags);
	affect.this = newAffect;
	return affect.this;
end

function CreateSpellComponent(requiredType, dataValue, operatorType)
	newComponent = LCreateSpellComponent(requiredType, dataValue, operatorType);
	component.this = newComponent;
	return component.this;
end

LoadSpells();
LoadSkills();
LoadWeapons();
LoadTongues();

-- EOF