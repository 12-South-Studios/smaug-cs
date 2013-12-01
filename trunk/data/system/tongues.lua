-- AUGURER.LUA
-- This is the Augurer Clan file for the MUD
-- Revised: 2013.11.27
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadTongues()
	newLang = LCreateLanguage("common");
	lang.this = newLang;
	lang.this.Alphabet = "abcdefghijklmnopqrstuvwxyz";
	
	newLang = LCreateLanguage("default");
	lang.this = newLang;
	lang.this.Alphabet = "ekjrugtlohfqbxyvndwmaczsip";
	
	LoadHalfling();
	LoadElvish();
end

function LoadHalfling()
	newLang = LCreateLanguage("halfling");
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
	lang.this:AddPreConversion("ale", "him");
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
	lang.this:AddPreConversion("burr", "raph");
	lang.this:AddPreConversion("horn", "ras");
	lang.this:AddPreConversion("stranger outsider", "raza");
	lang.this:AddPreConversion("apple", "razar");
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

function LoadElvish()
	newLang = LCreateLanguage("elvish");
	lang.this = newLang;
	lang.this:AddPreConversion("the hour", "lumenn");
	lang.this:AddPreConversion("our meeting", "omentielvo");
	lang.this:AddPreConversion("shines", "sila");
	
	lang.this.Alphabet = "iqqdakvtujfwghepcrslybszoz";
	lang.this:AddPostConversion("rr", "r");
	lang.this:AddPostConversion("qq", "q");
end

--[[

#elvish   * 'flutey' language.. slam some harsh sounds..
'the hour' 'lumenn'
'our meeting' 'omentielvo'
'shines' 'sila'
'metal' 'tinco'
'book' 'parma'
'lamp' 'calma'
'feather' 'quesse'
'gate' 'ando'
'fate' 'umbar'
'iron' 'anga'
'web' 'ungwe'
'spirit' 'thule'
'north' 'formen'
'treasure' 'harma'
'rage' 'aha'
'breeze' 'hwesta'
'mouth' 'anto'
'hook' 'ampa'
'jaws' 'anca'
'hollow' 'unque'
'west' 'numen'
'gold' 'malta'
'torment' 'nwalme'
'heart' 'ore'
'power' 'vala'
'gift' 'anna'
'air' 'vilya'
'sky' 'vilya'
'east' 'romen'
'region' 'arda'
'tongue' 'lambe'
'tree' 'alda'
'starlight' 'silme'
'name' 'esse'
'sunlight' 'are'
'south' 'hyarmen'
'bridge' 'yanta'
'heat' 'ure'
'say' 'pedo'
'friend' 'mellon'
'enter' 'minno'
'them' 'hain'
'made' 'echant'
'drew' 'teithant'
'these' thiw'
'signs' 'hin'
'spring' 'thuile'
'summer' 'laire'
'autumn' 'yavie'
'fading' 'quelle'
'winter' 'hrive'
'stirring' 'coire'
'dream' 'lorien'
'bow' 'luva'
'growth' 'loa'
'mistiness' 'hisime'
'century' 'haranye'
'week' 'enquie'
'star' 'elen'
'moon' 'isil'
'shines' 'sila'
'stone' 'ssar'
'and' 'owe'
'of' 'ane'
'xx' 'sth'
'g z' 's v'
'th' 'n'
'ng' 'r'
'st' 'ma'
'sh' 'vr'
'sc' 'a'
'ck' 'ss'
'r' 'th'
'd' 'ng'
'p' 'st'
'j' 'sh'
'z' 'dh'
'es' 'a'
~
iqqdakvtujfwghepcrslybszoz~
'rr' 'r'
'qq' 'q'
~

#dwarven   * opposite of elven.. extreme on the harsh tones..
'language' 'aglab'
'upon you' 'ai-menu'
'upon' 'aya'
'red' 'baraz'
'axes' 'baruk'
'axe' 'burk'
'dale' 'bizar'
'valley' 'bizar'
'head' 'bund'
'mansion' 'dum'
'hall' 'dum'
'excavation' 'dum'
'hew' 'felek'
'chisel' 'felak'
'great' 'gabil'
'fortress' 'gathol'
'underground' 'gundu'
'tunnel' 'gunud'
'gesture' 'iglishmek'
'horn' 'inbar'
'the dwarves' 'khazad'
'dwarves' 'khazad'
'dwarf' 'khazl'
'dwarven' 'khazlyth'
'silver' 'kibil'
'records' 'mazarbul'
'you' 'menu'
'path' 'nala'
'bed' 'nala'
'lode' 'nala'
'orc' 'rukhs'
'clouds' 'shathur'
'long' 'sigin'
'lord' 'uzbad'
'lake' 'zaram'
'pool' 'zaram'
'spike' 'zigil'
'ee' 'au'
'eth' 'ok'
'ith' 'uk'
'ath' 'ak'
'uth' 'uz'
'th' 'gn'
'oo' 'uu'
'dw' 'kh'
'ar' 'az'
'rf' 'zl'
'en' 'yth'
'ion' 'um'
'es' 'ad'
'you' 'enu'
'ch' 'k'
'ul' 'uzl'
'ick' 'uzk'
'of' 'uv'
'ove' 'uzo'
'ome' 'um'
'my' 'kaz'
'me' 'ka'
'to' 'or'
~
ubkdukggagkrzrypkdztibmzgo~
~

#ogre   * harsh and primative
'ee' 'au'
'eth' 'ok'
'ith' 'uk'
'ath' 'ak'
'uth' 'uz'
'th' 'gn'
'oo' 'uu'
'ar' 'arg'
'en' 'yth'
'ion' 'um'
'es' 'ad'
'you' 'guk'
'ch' 'k'
'ul' 'um'
'ick' 'uk'
'of' 'uguf'
'ove' 'umm'
'ome' 'um'
'my' 'ug'
'me' 'ug'
'to' 'og'
'hi' 'grug'
'hello' 'grog'
'come' 'rawg'
'kill' 'hak'
'with' '(grunt)'
'buy' 'tog'
'take' 'tog'
'steal' 'tog'
~
ufkdubrgugzpnmugkgztabtzom~
~

#troll   * harsh and primative, similar to ogre
'ee' 'au'
'eth' 'ok'
'ith' 'uk'
'ath' 'ak'
'uth' 'uz'
'th' 'gn'
'oo' 'uu'
'ar' 'arg'
'en' 'yth'
'ion' 'um'
'es' 'ad'
'you' 'guk'
'ch' 'k'
'ul' 'um'
'ick' 'uk'
'of' 'uguf'
'ove' 'umm'
'ome' 'um'
'my' 'ug'
'me' 'ug'
'to' 'og'
'hi' 'grug'
'hello' 'grog'
'come' 'rawg'
'kill' 'hak'
'with' '(grunt)'
'buy' 'tog'
'take' 'tog'
'steal' 'tog'
~
ufkdubrgugzpnmugkgztabtzom~
~

#goblin   * bastardization of elvish.  harsh
'ee' 'ou'
'eth' 'om'
'ith' 'uuk'
'ath' 'ohk'
'uth' 'um'
'th' 'gn'
'oo' 'uu'
'ar' 'arg'
'en' 'yth'
'ion' 'um'
'es' 'ac'
'you' 'snar'
'ch' 'k'
'ul' 'um'
'ick' 'uk'
'of' 'tuk'
'ove' 'aah'
'ome' 'ask'
'my' 'mal'
'me' 'mok'
'to' 'sek'
'hi' 'gorg'
'hello' 'khalok'
'come' 'kurta'
'kill' 'vak'
'with' 'bat'
'buy' 'muk'
'take' 'kham'
'steal' 'kzam'
~
oktpabkhugzpnmuxrgztebyzow~
~

#orcish   * basically the same as goblin... a little nastier
'and' 'agh'
'one' 'ash'
'cess' 'bag'
'pool' 'ronk'
'great' 'bubhosh'
'dark' 'burz'
'filth' 'dug'
'to rule' 'durbat'
'rule' 'durb'
'fire' 'ghash'
'to find' 'gimbat'
'find' 'gimb'
' fool' '-glob'
'fool' 'glob'
'wraith' 'gul'
'folk' 'hai'
'in' 'ishi'
'to bind' 'krimpat'
'bind' 'krimp'
'tower' 'lug'
'ring' 'nazg'
'ringwraith' 'nazgul'
'troll' 'olog'
'stupid' 'shai'
'bastard' 'sha'
'old man' 'sharku'
'slave' 'snaga'
'to bring' 'thrakat'
'bring' 'thrak'
' all' 'uk'
'all' 'uk'
' them' 'ul'
'them' 'ul'
' ness' 'um'
'ness' 'um'
'orc' 'uruk'
'ee' 'ou'
'eth' 'om'
'ith' 'uuk'
'ath' 'ohk'
'uth' 'um'
'th' 'gn'
'oo' 'uu'
'ar' 'arg'
'en' 'yth'
'ion' 'um'
'es' 'ac'
'you' 'snar'
'ch' 'k'
'ul' 'um'
'ick' 'uk'
'of' 'tuk'
'ove' 'aah'
'ome' 'ask'
'my' 'mal'
'me' 'mok'
'to' 'sek'
'hi' 'gorg'
'hello' 'khalok'
'come' 'kurta'
'kill' 'vak'
'with' 'bat'
'buy' 'muk'
'take' 'kham'
'steal' 'kzam'
~
oktpabkhugzpnmuxrgztebyzow~
~

#pixie   * these are just made up now.. i dunno what a pixie sounds like..
'lar' 'sgz'
'uhm' 'luf'
'get' 'ggtz'
'll' 'fr tpa'
'ts' 'uths'
'lm' 'ml'
'qu' 'arth tv'
'eer' 's'
'tr' 'mmps'
~
jdhsidulyywfmieopaexoqybac~
'dfs' 'sstg d'
'r' 'grud'
'tyrl' 'x'
~

#magical   * emulate say_spell from magic.c
'ar' 'abra'
'au' 'kada'
'bless' 'fido'
'blind' 'nose'
'bur' 'mosa'
'cu' 'judi'
'de' 'oculo'
'en' 'unso'
'light' 'dies'
'lo' 'hi'
'mor' 'zak'
'move' 'sido'
'ness' 'lacri'
'ning' 'illa'
'per' 'duda'
'ra' 'gru'
're' 'candus'
'son' 'sabru'
'tect' 'infra'
'tri' 'cula'
'ven' 'nofo'
~
abqezyopuytrwiasdfghjzxnlk~
~

#end
--]]

LoadTongues();

-- EOF