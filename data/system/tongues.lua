-- TONGUES.LUA
-- This is the Tongues file for the MUD
-- Revised: 2013.12.03
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadLanguages()
	newLang = LCreateLanguage("Common", "Common");
	lang.this = newLang;
	lang.this.Alphabet = "abcdefghijklmnopqrstuvwxyz";
	
	newLang = LCreateLanguage("default", "None");
	lang.this = newLang;
	lang.this.Alphabet = "ekjrugtlohfqbxyvndwmaczsip";
	
	LoadHalfling();
	LoadElvish();
	LoadDwarven();
	LoadOgre();
	LoadTroll();
	LoadGoblin();
	LoadOrcish();
	LoadPixie();
	LoadMagical();
end

function LoadHalfling()
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

function LoadElvish()
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

function LoadDwarven()
	-- opposite of elven.. extreme on the harsh tones..
	newLang = LCreateLanguage("dwarven", "Dwarven");
	lang.this = newLang;
	
	lang.this:AddPreConversion("age", "bahd");
	lang.this:AddPreConversion("alone single", "unh");
	lang.this:AddPreConversion("animal beast", "grukh");
	lang.this:AddPreConversion("area region", "rek");
	lang.this:AddPreConversion("arm", "kurz");
	lang.this:AddPreConversion("army", "dizh");
	lang.this:AddPreConversion("axe hew", "deth");
	lang.this:AddPreConversion("bad rotten", "kuz");
	lang.this:AddPreConversion("bay", "nar");
	lang.this:AddPreConversion("beautiful marble", "bahl");
	lang.this:AddPreConversion("bird eye", "zah");
	lang.this:AddPreConversion("black death dead", "fahn");
	lang.this:AddPreConversion("blood", "fahr");
	lang.this:AddPreConversion("boat", "naz");
	lang.this:AddPreConversion("body", "kurg");
	lang.this:AddPreConversion("book", "trah");
	lang.this:AddPreConversion("bow", "dezh");
	lang.this:AddPreConversion("cannon word", "taeh");
	lang.this:AddPreConversion("child", "kug");
	lang.this:AddPreConversion("city", "khur");
	lang.this:AddPreConversion("cradle home valley", "rakh");
	lang.this:AddPreConversion("darkness disease", "kuth");
	lang.this:AddPreConversion("daughter", "kum");
	lang.this:AddPreConversion("day", "bahz");
	lang.this:AddPreConversion("down underground", "tar");
	lang.this:AddPreConversion("dragon", "vakh");
	lang.this:AddPreConversion("earth brown copper", "mir");
	lang.this:AddPreConversion("east", "tor");
	lang.this:AddPreConversion("eight eighth", "azh");
	lang.this:AddPreConversion("element source iron", "gred");
	lang.this:AddPreConversion("elite veteran", "gran");
	lang.this:AddPreConversion("engineer smith worker", "grath");
	lang.this:AddPreConversion("father governor", "dehr");
	lang.this:AddPreConversion("female woman lady", "kar");
	lang.this:AddPreConversion("five fifth", "akh");
	lang.this:AddPreConversion("flame", "gakh");
	lang.this:AddPreConversion("fortress", "dahr");
	lang.this:AddPreConversion("fury fierce", "gahl");
	lang.this:AddPreConversion("game", "digh");
	lang.this:AddPreConversion("gate entrance", "grah");
	lang.this:AddPreConversion("glory", "dahn");
	lang.this:AddPreConversion("gold sun", "ahm");
	lang.this:AddPreConversion("grass", "mur");
	lang.this:AddPreConversion("gray", "grahs");
	lang.this:AddPreConversion("great", "dahm");
	lang.this:AddPreConversion("green tree", "mun");
	lang.this:AddPreConversion("guard", "grekh");
	lang.this:AddPreConversion("hall cave tunnel", "thor");
	lang.this:AddPreConversion("hate", "dekh");
	lang.this:AddPreConversion("head", "kush");
	lang.this:AddPreConversion("heal health", "ehl");
	lang.this:AddPreConversion("hero champion", "makh");
	lang.this:AddPreConversion("hidden invisible lost", "ukh");
	lang.this:AddPreConversion("history", "karth");
	lang.this:AddPreConversion("home house", "khat");
	lang.this:AddPreConversion("honor", "dahz");
	lang.this:AddPreConversion("hour", "behm");
	lang.this:AddPreConversion("king ruler master lord", "senh");
	lang.this:AddPreConversion("knowledge", "karg");
	lang.this:AddPreConversion("land", "mih");
	lang.this:AddPreConversion("large heavy", "gram");
	lang.this:AddPreConversion("leader general", "sem");
	lang.this:AddPreConversion("leg", "kurth");
	lang.this:AddPreConversion("less weak", "bahm");
	lang.this:AddPreConversion("light create", "mar");
	lang.this:AddPreConversion("love", "lakh");
	lang.this:AddPreConversion("low", "tus");
	lang.this:AddPreConversion("magic power mystery", "morr");
	lang.this:AddPreConversion("male man", "kam");
	lang.this:AddPreConversion("meeting council", "grahd");
	lang.this:AddPreConversion("mighty", "dehn");
	lang.this:AddPreConversion("month", "behd");
	lang.this:AddPreConversion("mother birth", "beht");
	lang.this:AddPreConversion("mountain maker", "tum");
	lang.this:AddPreConversion("nine ninth", "ahl");
	lang.this:AddPreConversion("north", "tak");
	lang.this:AddPreConversion("old elder", "bahg");
	lang.this:AddPreConversion("one first money", "ahn");
	lang.this:AddPreConversion("hundred", "dith");
	lang.this:AddPreConversion("thousand", "grahz");
	lang.this:AddPreConversion("out outer", "nakh");
	lang.this:AddPreConversion("outsider other out", "degh");
	lang.this:AddPreConversion("patience peace", "nag");
	lang.this:AddPreConversion("person dwarf", "khah");
	lang.this:AddPreConversion("protector empire", "sekh");
	lang.this:AddPreConversion("red fire bronze", "gar");
	lang.this:AddPreConversion("river", "lokh");
	lang.this:AddPreConversion("road", "zhor");
	lang.this:AddPreConversion("sea", "lihr");
	lang.this:AddPreConversion("seek hunter attend", "fazh");
	lang.this:AddPreConversion("seven seventh", "ath");
	lang.this:AddPreConversion("shadow evil", "ur");
	lang.this:AddPreConversion("silver moon", "behr");
	lang.this:AddPreConversion("six sixth", "aash");
	lang.this:AddPreConversion("sky", "leir");
	lang.this:AddPreConversion("sleep dream vision", "kud");
	lang.this:AddPreConversion("son", "ren");
	lang.this:AddPreConversion("south", "tok");
	lang.this:AddPreConversion("star", "ebh");
	lang.this:AddPreConversion("steel strong cut", "grakh");
	lang.this:AddPreConversion("step", "zir");
	lang.this:AddPreConversion("stone granite", "zukh");
	lang.this:AddPreConversion("storm", "gruhr");
	lang.this:AddPreConversion("symbol rune", "ezh");
	lang.this:AddPreConversion("tall high", "grul");
	lang.this:AddPreConversion("teacher", "ehd");
	lang.this:AddPreConversion("ten tenth", "dahg");
	lang.this:AddPreConversion("three third", "ahs");
	lang.this:AddPreConversion("time", "behz");
	lang.this:AddPreConversion("tower", "thom");
	lang.this:AddPreConversion("trade", "bakh");
	lang.this:AddPreConversion("two second", "ahr");
	lang.this:AddPreConversion("unit", "duzh");
	lang.this:AddPreConversion("up", "tin");
	lang.this:AddPreConversion("vow promise", "zar");
	lang.this:AddPreConversion("wall", "sakh");
	lang.this:AddPreConversion("war", "dukh");
	lang.this:AddPreConversion("warrior soldier master", "dimh");
	lang.this:AddPreConversion("water blue", "lor");
	lang.this:AddPreConversion("way path", "zur");
	lang.this:AddPreConversion("weapon", "dugh");
	lang.this:AddPreConversion("week", "behg");
	lang.this:AddPreConversion("west", "tir");
	lang.this:AddPreConversion("white life", "fahm");
	lang.this:AddPreConversion("wind", "lah");
	lang.this:AddPreConversion("wise wisdom", "van");
	lang.this:AddPreConversion("word language", "bahr");
	lang.this:AddPreConversion("year", "bahn");
	lang.this:AddPreConversion("young", "bah");
	lang.this:AddPreConversion("zero none", "ahg");

	-- Not Bharatan
	lang.this:AddPreConversion("gesture", "iglishmek");
	lang.this:AddPreConversion("horn", "inbar");
	lang.this:AddPreConversion("record", "mazarbul");
	lang.this:AddPreConversion("you", "menu");
	lang.this:AddPreConversion("path bed", "nala");
	lang.this:AddPreConversion("orc", "rukhs");
	lang.this:AddPreConversion("cloud", "shathur");
	lang.this:AddPreConversion("long", "sigin");
	lang.this:AddPreConversion("lake pool", "zaram");
	lang.this:AddPreConversion("spike", "zigil");
	
	lang.this:AddPreConversion("ee", "au");
	lang.this:AddPreConversion("eth", "ok");
	lang.this:AddPreConversion("ith", "uk");
	lang.this:AddPreConversion("ath", "ak");
	lang.this:AddPreConversion("uth", "uz");
	lang.this:AddPreConversion("th", "gn");
	lang.this:AddPreConversion("oo", "uu");
	lang.this:AddPreConversion("dw", "kh");
	lang.this:AddPreConversion("ar", "az");
	lang.this:AddPreConversion("rf", "zl");
	lang.this:AddPreConversion("en", "yth");
	lang.this:AddPreConversion("ion", "um");
	lang.this:AddPreConversion("es", "ad");
	lang.this:AddPreConversion("you", "enu");
	lang.this:AddPreConversion("ch", "k");
	lang.this:AddPreConversion("ul", "uzl");
	lang.this:AddPreConversion("ick", "uzk");
	lang.this:AddPreConversion("of", "uv");
	lang.this:AddPreConversion("ove", "uzo");
	lang.this:AddPreConversion("ome", "um");
	lang.this:AddPreConversion("my", "kha");
	lang.this:AddPreConversion("me", "ka");
	lang.this:AddPreConversion("to", "or");
	
	lang.this.Alphabet = "ubkdukggagkrzrypkdztibmzgo";
end

function LoadOgre()
	-- harsh and primative
	newLang = LCreateLanguage("ogre", "Ogre");
	lang.this = newLang;
	lang.this:AddPreConversion("ee", "au");
	lang.this:AddPreConversion("eth", "ok");
	lang.this:AddPreConversion("it ickh", "uk");
	lang.this:AddPreConversion("ath", "ak");
	lang.this:AddPreConversion("uth", "uz");
	lang.this:AddPreConversion("th", "gn");
	lang.this:AddPreConversion("oo", "uu");
	lang.this:AddPreConversion("ar", "arg");
	lang.this:AddPreConversion("en", "yth");
	lang.this:AddPreConversion("ion ul ome", "um");
	lang.this:AddPreConversion("es", "ad");
	lang.this:AddPreConversion("you", "guk");
	lang.this:AddPreConversion("ch", "k");
	lang.this:AddPreConversion("of", "uguf");
	lang.this:AddPreConversion("ove", "umm");
	lang.this:AddPreConversion("my me", "ug");
	lang.this:AddPreConversion("to", "og");
	lang.this:AddPreConversion("hi", "grug");
	lang.this:AddPreConversion("hello", "grog");
	lang.this:AddPreConversion("come", "rawg");
	lang.this:AddPreConversion("kill", "hak");
	lang.this:AddPreConversion("with", "(grunt)");
	lang.this:AddPreConversion("buy take steal", "tog");
	
	lang.this.Alphabet = "ufkdubrgugzpnmugkgztabtzom";
end

function LoadTroll()
	-- harsh and primative, similar to ogre
	newLang = LCreateLanguage("trollese", "Trollish");
	lang.this = newLang;
	lang.this:AddPreConversion("ee", "au");
	lang.this:AddPreConversion("eth", "ok");
	lang.this:AddPreConversion("ith ick", "uk");
	lang.this:AddPreConversion("ath", "ak");
	lang.this:AddPreConversion("uth", "uz");
	lang.this:AddPreConversion("th", "gn");
	lang.this:AddPreConversion("oo", "uu");
	lang.this:AddPreConversion("ar", "arg");
	lang.this:AddPreConversion("en", "yth");
	lang.this:AddPreConversion("ion ul ume", "um");
	lang.this:AddPreConversion("es", "ad");
	lang.this:AddPreConversion("you", "guk");
	lang.this:AddPreConversion("ch", "k");
	lang.this:AddPreConversion("of", "uguf");
	lang.this:AddPreConversion("ove", "umm");
	lang.this:AddPreConversion("my me", "ug");
	lang.this:AddPreConversion("to", "og");
	lang.this:AddPreConversion("hi", "grug");
	lang.this:AddPreConversion("hello", "grog");
	lang.this:AddPreConversion("come", "rawg");
	lang.this:AddPreConversion("kill", "hak");
	lang.this:AddPreConversion("with", "(grunt)");
	lang.this:AddPreConversion("buy take steal", "tog");
	
	lang.this.Alphabet = "ufkdubrgugzpnmugkgztabtzom";
end

function LoadGoblin()
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

function LoadOrcish()
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

function LoadPixie()
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

function LoadMagical()
	-- emulate say_spell from magic.c
	newLang = LCreateLanguage("magical", "Magical");
	lang.this = newLang;
	lang.this:AddPreConversion("ar", "abra");
	lang.this:AddPreConversion("au", "kada");
	lang.this:AddPreConversion("bless", "fido");
	lang.this:AddPreConversion("blind", "nose");
	lang.this:AddPreConversion("bur", "mosa");
	lang.this:AddPreConversion("cu", "judi");
	lang.this:AddPreConversion("de", "oculo");
	lang.this:AddPreConversion("en", "unso");
	lang.this:AddPreConversion("light", "dies");
	lang.this:AddPreConversion("lo", "hi");
	lang.this:AddPreConversion("mor", "zak");
	lang.this:AddPreConversion("move", "sido");
	lang.this:AddPreConversion("ness", "lacri");
	lang.this:AddPreConversion("ning", "illa");
	lang.this:AddPreConversion("per", "duda");
	lang.this:AddPreConversion("ra", "gru");
	lang.this:AddPreConversion("re", "candus");
	lang.this:AddPreConversion("son", "sabru");
	lang.this:AddPreConversion("tect", "infra");
	lang.this:AddPreConversion("tri", "cula");
	lang.this:AddPreConversion("ven", "nofo");
	
	lang.this.Alphabet = "abqezyopuytrwiasdfghjzxnlk";
end

LoadLanguages();

-- EOF