-- SOCIALS.LUA
-- This is the Socials data file for the MUD
-- Revised: 2013.11.22
-- Author: Jason Murdick
-- Version: 1.0
f = loadfile(LDataPath() .. "\\modules\\module_base.lua")();

function LoadSocials()
	social = CreateSocial("accuse");
	social.CharNoArg = "Accuse whom?";
	social.OthersNoArg = "$n is in an accusing mood.";
	social.CharFound = "You look accusingly at $N.";
	social.OthersFound = "$n looks accusingly at $N.";
	social.VictFound = "$n looks accusingly at you.";
	social.CharAuto = "You accuse yourself.";
	social.OthersAuto = "$n seems to have a bad conscience.";
	
	social = CreateSocial("ack");
	social.CharNoArg = "You gasp and say 'ACK!' at your mistake.";
	social.OthersNoArg = "$n ACKS at $s big mistake.";
	social.CharFound = "You ACK $M.";
	social.OthersFound = "$n ACKS $N.";
	social.VictFound = "$n ACKS you.";
	social.CharAuto = "You ACK yourself.";
	social.OthersAuto = "$n ACKS $mself.  Must be a bad day.";
	
end

function CreateSocial(name)
	newSocial = LCreateSocial(name);
	social.this = newSocial;
	return social.this;
end

LoadSocials();

--[[

#SOCIAL
Name        addict~
CharNoArg   You stand and admit to all in the room, 'Hi, I'm $n, and I'm a mud addict.'~
OthersNoArg $n stands and says, 'Hi, I'm $n, and I'm a mud addict.'~
CharFound   You tell $M that you are addicted to $S love.~
OthersFound $n tells $N that $e is addicted to $S love.~
VictFound   $n tells you that $e is addicted to your love.~
CharAuto    You stand and admit to all in the room, 'Hi, I'm $n, and I'm a mud addict.'~
OthersAuto  $n stands and says, 'Hi, I'm $n, and I'm a mud addict.'~
End

#SOCIAL
Name        adore~
CharNoArg   You are looking for someone to adore!~
OthersNoArg $n is looking for someone to adore!~
CharFound   You look at $N with adoring eyes.~
OthersFound $n looks at $N with adoring eyes.~
VictFound   $n looks at you with adoring eyes.~
CharAuto    Aww! You adore yourself!~
OthersAuto  $n adores $mself soooo much!~
End

#SOCIAL
Name        afkcheck~
CharNoArg   You wonder how many afk characters there are in this room.~
OthersNoArg $n looks at the characters in this room, muttering something about afkers.~
CharFound   You slowly move your hand across across $N's sight a few times, trying to figure out if $E is afk or not.~
OthersFound $n slowly moves $s hand across $N's sight a few times. It looks like $e thinks $N is afk!~
VictFound   $n slowly moves $s hand across your sight a few times. It looks like $e thinks you're afk!~
CharAuto    You try to figure out if you're afk or not.~
OthersAuto  $n wonders if $e's afk or not. ~
End

#SOCIAL
Name        again~
CharNoArg   You think, "not again!"~
OthersNoArg $n hits $s head on a tree and cries, "Not AGAIN!?"~
CharFound   You beg $N to do it again!  You must have enjoyed the first time.~
OthersFound $n looks at $N and says, "Thank you, may I have another?"~
VictFound   $n begs you for a repeat performance. Do it again!~
CharAuto    You make a mental note to do that again.~
OthersAuto  $n makes a mental note to do what $e just did more often!~
End

#SOCIAL
Name        agree~
CharNoArg   You seem to be in an agreeable mood.~
OthersNoArg $n seems to agree.~
CharFound   You agree with $M.~
OthersFound $n agrees with $N.~
VictFound   $n agrees with you.~
CharAuto    Well I hope you would agree with yourself!~
OthersAuto  $n agrees with $mself, of course.~
End

#SOCIAL
Name        ahem~
CharNoArg   You clear your throat loudly and look slowly around.~
OthersNoArg $n clears $s throat loudly and looks around.~
CharFound   You cock an eyebrow at $N and clear your throat.~
OthersFound $n cocks an eyebrow at $N and clears $s throat.~
VictFound   $n cocks an eyebrow at you and clears $s throat.~
CharAuto    You quickly clear your throat and glance around nervously.~
OthersAuto  $n suddenly clears $s throat and glances around nervously.~
End

#SOCIAL
Name        ahh~
CharNoArg   You nod your head in understanding "Ahhh yes..."~
OthersNoArg $n nods $s head in understanding, saying "Ahhh, yes.."~
CharFound   You look at $M and think, "Ahhh....Yes!"~
OthersFound $n says "Ahhh, yes..." and nods to $N.~
VictFound   $n nods to you and says "Ahhh, yes..."~
CharAuto    Ahhh, yes....you're quite right!~
OthersAuto  $n mumbles "Ahh, yes..." lost in thought.~
End

#SOCIAL
Name        alfalfa~
CharNoArg   You hear a strange voice yell 'Spaaanky!'~
OthersNoArg $n's eyes look puzzled as $e hears strange voices...~
CharFound   You point a finger, making a lone hair on the back of $N's noggin sproing to attention.~
OthersFound $n points a finger, making a lone hair on the back of $N's noggin sproing to attention.~
VictFound   $n points a finger, making a lone hair on the back of your noggin sproing to attention.~
CharAuto    Buy some mousse.~
OthersAuto  $n dreams of mousse.~
End

#SOCIAL
Name        alligator~
CharNoArg   You hop around like you were just bitten by an alligator.~
OthersNoArg $n was just bitten by an alligator!! OW!!~
CharFound   You throw an alligator at $N.~
OthersFound $n throws an alligator at $N!!~
VictFound   $n throws an alligator at you!!~
CharAuto    You wrestle with an alligator.~
OthersAuto  $n wrestles with an alligator, the alligator seems to have the upper hand.~
End

#SOCIAL
Name        amused~
CharNoArg   You seem amused about something.~
OthersNoArg $n gives an amused look.~
CharFound   You give $N an amused grin.~
OthersFound $n gives $N an amused grin.~
VictFound   $n gives you an amused grin.~
CharAuto    You give yourself an amused grin.~
OthersAuto  $n seems rather amused with $mself.~
End

#SOCIAL
Name        angel~
CharNoArg   You attempt to straighten your halo.~
OthersNoArg $n attempts to straighten $s halo.~
CharFound   You try to convince $N that you're an angel. Think $E believes it?~
OthersFound $n tries to convince $N that $e's an angel. Think $e's lying?~
VictFound   $n is trying to convince you $e's an angel. Don't buy it!~
CharAuto    You try convincing yourself that you're an angel.~
OthersAuto  $n tries convincing $mself that $e's an angel.~
End

#SOCIAL
Name        apologize~
CharNoArg   You apologize for your behavior.~
OthersNoArg $n apologizes for $s rude behavior.~
CharFound   You apologize to $M.~
OthersFound $n apologizes to $N.~
VictFound   $n apologizes to you.~
CharAuto    You apologize to yourself.~
OthersAuto  $n apologizes to $mself.  Hmmmm.~
End

#SOCIAL
Name        applaud~
CharNoArg   Clap, clap, clap.~
OthersNoArg $n gives a round of applause.~
CharFound   You clap at $S actions.~
OthersFound $n claps at $N's actions.~
VictFound   $n gives you a round of applause.  You MUST'VE done something good!~
CharAuto    You applaud at yourself.  Boy, are we conceited!~
OthersAuto  $n applauds at $mself.  Boy, are we conceited!~
End

#SOCIAL
Name        arena~
CharNoArg   You glare around the room, looking for a fight.~
OthersNoArg $n glares around the room, looking for a fight.~
CharFound   You glare at $N and say, 'Let's take this to the arena.'~
OthersFound $n glares at $N and says, 'Let's take this to the arena.'~
VictFound   $n glares at you and says, 'Let's take this to the arena.'~
CharAuto    You give up on trying to find a fight and pkill yourself!~
OthersAuto  $n gives up $s search for a fight, and decides to pkill $mself.~
End

#SOCIAL
Name        arf~
CharNoArg   You exclaim, "Arf! Arf! Arf!"~
OthersNoArg $n exclaims, "Arf! Arf! Arf!"~
CharFound   You look at $N and exclaim, "Arf! Arf! Arf!"~
OthersFound $n looks at $N and exclaims, "Arf! Arf! Arf!"~
VictFound   $n looks at you and exclaims, "Arf! Arf! Arf!"~
CharAuto    You run around the room exclaiming, "Arf! Arf! Arf!"~
OthersAuto  $n runs around the room exclaiming, "Arf! Arf! Arf!"~
End

#SOCIAL
Name        attention~
CharNoArg   Are you in need of a little attention?~
OthersNoArg $n is looking for some attention.~
CharFound   You tap $N on the shoulder.~
OthersFound $n taps $N on the shoulder.~
VictFound   $n taps you on the shoulder.~
CharAuto    You give yourself attention since no one else will.~
OthersAuto  $n gives $mself the attention $e deserves.~
End

#SOCIAL
Name        aww~
CharNoArg   You smile and say 'awww'.~
OthersNoArg $n smiles and says 'awww'.~
CharFound   You look at $N and say 'Awwww, you're so sweet!'~
OthersFound $n looks at $N and says 'Awwww, you're so sweet!'~
VictFound   $n looks at you and says 'Awwww, you're so sweet!'~
CharAuto    You hold up a mirror and say 'awww.' Hrm.~
OthersAuto  $n holds up a mirror and says 'awww.' Hrm.~
End

#SOCIAL
Name        babble~
CharNoArg   You begin to babble like an idiot.~
OthersNoArg $n begins to babble like an idiot.~
CharFound   You begin to babble at $N and $E can't understand a word you are saying.~
OthersFound $n tries to say something to $N but just babbles like an idiot.~
VictFound   $n tries to talk to you but just babbles like an idiot.~
CharAuto    You babble at yourself.~
OthersAuto  $n begins to babble like an idiot.~
End

#SOCIAL
Name        babe~
CharNoArg   You strike a pose in your most babe-like style!~
OthersNoArg $n strikes a pose in $s most babe-like style!~
CharFound   You look at $N and think, "Wow! What a babe!"~
OthersFound $n looks at $N and thinks, "Wow! What a babe!"~
VictFound   $n thinks you are a supreme babe!~
CharAuto    You think you are a babe!~
OthersAuto  $n seems to think that $e is a babe!~
End

#SOCIAL
Name        baffle~
CharNoArg   You scrunch up your face cuz you are totally baffled!~
OthersNoArg $n scrunches up $s face cuz $e is totally baffled!~
CharFound   You scrunch up your face at $N cuz $S behavior totally baffles you!~
OthersFound $n scrunches up $s face at $N cuz $n is totally baffled by $N!~
VictFound   $n scrunches up $s face at you cuz you totally baffle $m!~
CharAuto    You baffle yourself.~
OthersAuto  $n seems to be baffled by $s own thoughts.~
End

#SOCIAL
Name        bagel~
CharNoArg   You pop a bagel in the fire and toast it till it's golden brown.~
OthersNoArg $n pops a bagel in the fire and toasts it till it's golden brown.~
CharFound   You pull a golden brown bagel from the fire and give it to $N.~
OthersFound $n pulls a golden brown bagel from the fire and gives it to $N.~
VictFound   $n pulls a golden brown bagel from the fire and gives it to you.~
CharAuto    Your pull a golden brown bagel from the fire and pop it in your mouth. Yummy!~
OthersAuto  $n pulls a golden brown bagel from the fire and pops it into $s mouth. Yummy!~
End

#SOCIAL
Name        bah~
CharNoArg   You utter "Bah" out of frustration.~
OthersNoArg $n says, "Bah" and turns away in contempt.~
CharFound   You utter incoherent words in your state of anxiety.~
OthersFound $n begins making sheeplike noises.~
VictFound   $n bahs at you.~
CharAuto    Talking to yourself again?~
OthersAuto  $n is obviously frustrated at something or someone..~
End

#SOCIAL
Name        banana~
CharNoArg   Daylight come, me want go home.~
OthersNoArg $n leans way back, and bellows forth, 'DAAAAAAAYO!'~
CharFound   You sneak up on $N, and sing, 'DAAAAAYO!'  Bananas are dancing before $S eyes.~
OthersFound $n sneaks up behind $N and screams 'DAAAAAAAAAAAAAY-O' at the top of $s lungs.. Bananas are dancing before $S eyes now.~
VictFound   DAAAAAYO! Ack! Bananas! Bananas everywhere!  $n snickers at your predicament.~
CharAuto    Daylight come, me want go home.~
OthersAuto  $n bellows forth, 'DAAAAAYO!' and dances a little rumba.~
End

#SOCIAL
Name        barf~
CharNoArg   You scream out "I'm gonna be sick!" and then heave your stomach contents onto the ground.~
OthersNoArg $n is so sick that $e tells everyone "I gotta barf!".~
CharFound   You yell at $N "Run!  I gotta barf" and then spill out your dinner on $N.~
OthersFound $n barfs all over $N!  Yecchh what a mess!~
VictFound   $n has just projectile vomited all over your nice, clean armor!~
CharAuto    You say to yourself "Am I ever sick!" and then barf up your dinner!~
OthersAuto  $n begins to projectile vomit over everything in the room!~
End

#SOCIAL
Name        bark~
CharNoArg   You bark, "Bark! Bark! Bark!"~
OthersNoArg $n barks, "Bark! Bark! Bark!"~
CharFound   You bark at $M.~
OthersFound $n barks at $N.~
VictFound   $n barks at you.~
CharAuto    You bark at yourself.  Woof!  Woof!~
OthersAuto  $n barks at $mself.  Woof!  Woof!~
End

#SOCIAL
Name        bashful~
CharNoArg   For some reason, you start feeling very bashful.~
OthersNoArg $n begins looking quite bashful.~
CharFound   You look up at $N, look at the ground and bashfully trace figure eights with your foot.~
OthersFound $n looks at $N, then bashfully stares at $s feet.~
VictFound   $n looks up at you, then bashfully traces figure eights with $s foot.~
CharAuto    For some reason, you start feeling very bashful.~
OthersAuto  $n begins looking quite bashful.~
End

#SOCIAL
Name        bath~
CharNoArg   You sense a funk in the air. You need to take a bath.~
OthersNoArg $n is tired of smelling like a pig. $n takes a bath.~
CharFound   You find $N's aroma to be too strong. You give $N a bath.~
OthersFound $n thinks $N needs a bath.~
VictFound   $n objects to your strong aroma and would appreciate if you would take a bath.~
CharAuto    Maybe you should find a private place to do that. Exhibitionist.~
OthersAuto  With a bold display of exhibitionism, $n is going to bathe right here!~
End

#SOCIAL
Name        bbl~
CharNoArg   You announce that you will be back later.~
OthersNoArg $n announces that $e'll be back later.~
CharFound   You inform $M that you will be back later.~
OthersFound $n informs $N that $e will be back later~
VictFound   $n informs you that $e will be back later~
CharAuto    You mumble to yourself that you'll be back later.~
OthersAuto  $n mumbles to $mself that $e'll be back later.~
End

#SOCIAL
Name        bday~
CharNoArg   You leap into the air and shout "Happy Birthday"~
OthersNoArg $n leaps into the air and shouts "Happy Birthday"~
CharFound   You leap into the air and shout "Happy Birthday $N"~
OthersFound $n leaps into the air and shouts "Happy Birthday $N"~
VictFound   $n leaps into the air and shouts "Happy Birthday $N"~
CharAuto    You sing a nice birthday song to yourself, drowning in loneliness.~
OthersAuto  $n dances about, singing "Happy Birthday to me!"~
End

#SOCIAL
Name        beam~
CharNoArg   You beam brightly.  Why is that?~
OthersNoArg $n beams brightly.  Why is that?~
CharFound   You beam brightly at $M.~
OthersFound $n beams brightly.  No doubt $N has something to do with it.~
VictFound   $n beams brightly at you.~
CharAuto    You beam brightly.~
OthersAuto  $n beams brightly at $mself.  Obviously $e's quite happy.~
End

#SOCIAL
Name        bearhug~
CharNoArg   You hug a grizzly bear.~
OthersNoArg $n hugs a flea-infested grizzly bear.~
CharFound   You bearhug $M.~
OthersFound $n bearhugs $N.  Some ribs break.~
VictFound   $n bearhugs you.  Wonder what's coming next?~
CharAuto    You bearhug yourself.~
OthersAuto  $n bearhugs $mself.~
End

#SOCIAL
Name        beer~
CharNoArg   You draw a cold, frosty beer.~
OthersNoArg $n downs a cold, frosty beer.~
CharFound   You draw a cold, frosty beer for $N.~
OthersFound $n draws a cold, frosty beer for $N.~
VictFound   $n draws a cold, frosty beer for you.~
CharAuto    You pour yourself a cold, frosty beer and think, "It just doesn't get better than this."~
OthersAuto  $n draws $mself a beer.~
End

#SOCIAL
Name        beg~
CharNoArg   You beg the gods for mercy.~
OthersNoArg The gods fall down laughing at $n's request for mercy.~
CharFound   You desperately beg for a favor from $N.~
OthersFound $n gets on $s knees begging for a favor from $N..how unbecoming.~
VictFound   $n is begging you for something.~
CharAuto    Begging yourself for money doesn't help.~
OthersAuto  $n begs for a favor.~
End

#SOCIAL
Name        behead~
CharNoArg   You look around for some heads to cut off.~
OthersNoArg $n looks around for some heads to cut off.~
CharFound   You grin evilly at $N and brandish your weapon.~
OthersFound $n grins evilly at $N while brandishing $s weapon!~
VictFound   $n grins evilly at you, brandishing $s weapon.~
CharAuto    I really don't think you want to do that...~
OthersAuto  $n is so desperate for exp that $e tries to decapitate $mself!~
End

#SOCIAL
Name        belly~
CharNoArg   You rub your belly, you must be hungry.~
OthersNoArg $n rubs $s belly, $e must be hungry.~
CharFound   You rub $N's belly.~
OthersFound $n rubs $N's belly.~
VictFound   $n rubs your belly.~
CharAuto    You rub your belly, you must be hungry.~
OthersAuto  $n rubs $s belly, $e must be hungry.~
End

#SOCIAL
Name        bkiss~
CharNoArg   Blow a kiss to whom?~
OthersNoArg $n blows at $s hand.~
CharFound   You blow a kiss to $M.~
OthersFound $n blows a kiss to $N.  Touching, ain't it?~
VictFound   $n blows a kiss to you.  Not as good as a real one, huh?~
CharAuto    You blow a kiss to yourself.~
OthersAuto  $n blows a kiss to $mself.  Weird.~
End

#SOCIAL
Name        blank~
CharNoArg   You get a blank look on your face.~
OthersNoArg $n gets a blank look on $s face.~
CharFound   You look at $N but draw a complete blank.~
OthersFound $n looks at $N but draws a blank.~
VictFound   $n looks at you and draws a total blank.~
End

#SOCIAL
Name        blast~
CharNoArg   You count down 10 9 8 7 6 5 4 3 2 1 and blast off!~
OthersNoArg $n counts down 10 9 8 7 6 5 4 3 2 1 and blasts off.~
CharFound   You look at $N and give $M a real blast.~
OthersFound $n looks at $N and gives $M a real blast!~
VictFound   $n looks at you and gives you a real blast!~
CharAuto    Do you really think that's polite?~
OthersAuto  $n looks as though $e wants a blast.~
End

#SOCIAL
Name        bleat~
CharNoArg   You bleat.~
OthersNoArg $n bleats like a poor little lamb.~
CharFound   You bleat at $N just like a lost little lamb.~
OthersFound $n bleats at $N, just like a lost little lamb.~
VictFound   $n bleats at you, just like a lost little lamb.~
End

#SOCIAL
Name        bleed~
CharNoArg   You bleed all over the room!~
OthersNoArg $n bleeds all over the room!  Get out of $s way!~
CharFound   You bleed all over $M!~
OthersFound $n bleeds all over $N.  Better leave, you may be next!~
VictFound   $n bleeds all over you!  YUCK!~
CharAuto    You bleed all over yourself!~
OthersAuto  $n bleeds all over $mself.~
End

#SOCIAL
Name        bleh~
CharNoArg   You grimace as if tasting something hideous and say "BLEH!"~
OthersNoArg $n sticks $s tongue out and says "Bleh!"~
CharFound   You "bleh!" right in $N's face!~
OthersFound $n says "bleh!" while making a horrid face at $N.~
VictFound   $n looks right at you and says "BLEH!"~
CharAuto    You bleh at yourself in response to something disagreeable.~
OthersAuto  $n blehs at the thought of something.~
End

#SOCIAL
Name        blink~
CharNoArg   You blink in utter disbelief.~
OthersNoArg $n blinks in utter disbelief.~
CharFound   You blink at $M in confusion.~
OthersFound $n blinks at $N in confusion.~
VictFound   $n blinks at you in confusion.~
CharAuto    You are sooooooooooooo confused~
OthersAuto  $n blinks at $mself in complete confusion.~
End

#SOCIAL
Name        blownose~
CharNoArg   You blow your nose loudly.~
OthersNoArg $n blows $s nose loudly.~
CharFound   You blow your nose on $S shirt.~
OthersFound $n blows $s nose on $N's shirt.~
VictFound   $n blows $s nose on your shirt.~
CharAuto    You blow your nose on your shirt.~
OthersAuto  $n blows $s nose on $s shirt.~
End

#SOCIAL
Name        blunk~
CharNoArg   You try to blink but blunk instead.~
OthersNoArg $n tries to blink but $e blunks instead.~
CharFound   You blunk at $N.~
OthersFound $n blunks at $N.~
VictFound   $n blunks at you.~
End

#SOCIAL
Name        blush~
CharNoArg   Your cheeks are burning.~
OthersNoArg $n blushes.~
CharFound   You get all flustered up seeing $M.~
OthersFound $n blushes as $e sees $N here.~
VictFound   $n blushes as $e sees you here.  Such an effect on people!~
CharAuto    You blush at your own folly.~
OthersAuto  $n blushes as $e notices $s boo-boo.~
End

#SOCIAL
Name        bob~
CharNoArg   You bob your head to the funky music.~
OthersNoArg $n bobs $s head to the funky music.~
CharFound   You bob your head to $N's funky beats!~
OthersFound $n bobs $s head to $N's funky rhythm.~
VictFound   $n looks at you and starts bobbing $s head, what a weirdo!~
CharAuto    You bob your head to imaginary music, you weirdo.~
OthersAuto  $n bobs $s head to $s own imaginary music. ~
End

#SOCIAL
Name        boggle~
CharNoArg   You boggle in complete incomprehension.~
OthersNoArg $n boggles in complete incomprehension.~
CharFound   You boggle in complete incomprehension of $S actions.~
OthersFound $n boggles in complete incomprehension of $N's actions.~
VictFound   $n boggles in complete incomprehension of your actions.~
CharAuto    You boggle in complete incomprehension of your own actions.  Confused?~
OthersAuto  $n boggles, plain and simple.~
End

#SOCIAL
Name        bond~
CharNoArg   You whip out some handcuffs and ropes.~
OthersNoArg $n whips out some handcuffs and ropes.~
CharFound   You handcuff $N and tie $M up with some ropes. Now the fun begins!~
OthersFound $n handcuffs $N and ties $M up with some ropes. Let the fun begin!~
VictFound   $n handcuffs you and ties you up with some ropes. Uh oh!~
End

#SOCIAL
Name        bonk~
CharNoArg   You look for someone to bonk!~
OthersNoArg $n looks around for someone to bonk! Better start running now!~
CharFound   You bonk $N! BONK!! BONK!! BONK!!~
OthersFound $n bonks $N! BONK!! BONK!! BONK!!~
VictFound   $n bonks you! BONK!! BONK!! BONK!!~
CharAuto    You bonk yourself! Owwwww!! Waaaaaaaaaaa!~
OthersAuto  $n bonks $mself! Whatta loon.. eh?~
End

#SOCIAL
Name        boogie~
CharNoArg   You boogie.~
OthersNoArg $n boogies!~
CharFound   You boogie with $N!~
OthersFound $n boogies with $N!~
VictFound   $n boogies with you!~
CharAuto    You boogie with yourself.~
OthersAuto  $n boogies with $mself, lost in $s own little world.~
End

#SOCIAL
Name        bounce~
CharNoArg   BOIINNNNNNGG!~
OthersNoArg $n bounces around.~
CharFound   You bounce onto $S lap.~
OthersFound $n bounces onto $N's lap.~
VictFound   $n bounces onto your lap.~
CharAuto    You bounce your head like a basketball.~
OthersAuto  $n plays basketball with $s head.~
End

#SOCIAL
Name        bound~
CharNoArg   You take a flying leap and make a mighty bound across the room.~
OthersNoArg $n takes a flying leap and makes a mighty bound across the room.~
CharFound   You take a flying leap at $N and make a mighty bound across the room.~
OthersFound $n takes a flying leap at $N and makes a mighty bound across the room.~
VictFound   $n takes a flying leap at you and makes a mighty bound across the room.~
End

#SOCIAL
Name        bow~
CharNoArg   You bow deeply.~
OthersNoArg $n bows deeply.~
CharFound   You bow before $M.~
OthersFound $n bows before $N.~
VictFound   $n bows before you.~
CharAuto    You kiss your toes.~
OthersAuto  $n folds up like a jack knife and kisses $s own toes.~
End

#SOCIAL
Name        bowwow~
CharNoArg   You bark, "Bow wow! Bow wow! Bow wow!"~
OthersNoArg $n barks, "Bow wow! Bow wow! Bow wow!"~
CharFound   You bark at $N, "Bow wow! Bow wow! Bow wow!"~
OthersFound $n barks at $N, "Bow wow! Bow wow! Bow wow!"~
VictFound   $n barks at you, "Bow wow! Bow wow! Bow wow!"~
End

#SOCIAL
Name        brainstorm~
CharNoArg   Ignoring the cautions of the gods themselves, you turn your brain on.~
OthersNoArg $n, ignoring the gods' many cautions, turns $s brain on.~
CharFound   $N shivers in terror as thoughts begin to flow through your mighty noggin.~
OthersFound $N shivers in terror as thoughts begin to flow through $n's mighty noggin.~
VictFound   You shiver in terror as you notice thoughts flowing through $n's mighty noggin.~
CharAuto    Even the gods themselves don't know what might possibly be done about you and your noggin.~
OthersAuto  $n attempts to squeeze some thoughts from $s noggin, making the gods themselves, terrorized by this display, run.~
End

#SOCIAL
Name        brb~
CharNoArg   You announce that you will be right back.~
OthersNoArg $n says in a stern voice, 'I'll be back!'~
CharFound   You announce to $M that you will be right back.~
OthersFound $n says to $N in a stern voice, 'I'll be back!'~
VictFound   $n says to you in a stern voice, 'I'll be right back!'~
CharAuto    You mumble to yourself, 'I'll be right back'~
OthersAuto  $n mumbles to $mself, 'I'll be right back, won't I?'~
End

#SOCIAL
Name        bristle~
CharNoArg   You clench your teeth and bristle with anger.~
OthersNoArg $n clenches $s teeth and bristles with anger.~
CharFound   You glare at $N, bristling with anger.~
OthersFound $n glares at $N, bristling with anger.~
VictFound   $n glares at you, bristling with anger.~
CharAuto    Your bristle at your foolishness.~
OthersAuto  $n bristles at $mself and $s foolishness.~
End

#SOCIAL
Name        brownie~
CharNoArg   You munch on a delicious brownie ... You begin to feel light headed.~
OthersNoArg $n munches on a funny-looking brownie. $n begins to feel light headed.~
CharFound   You give $N a funny-looking brownie.~
OthersFound $n gives $N a funny-looking brownie.~
VictFound   $n gives you a funny-looking brownie. Be careful!~
CharAuto    You look very strange giving yourself a brownie.~
OthersAuto  $n gives $mself a brownie, $e might be a BIT strange.~
End

#SOCIAL
Name        brush~
CharNoArg   Brush what? Who? Where?~
OthersNoArg $n seems to be looking for someone to brush.~
CharFound   You brush out $S hair for $M.  Very thoughtful.~
OthersFound $n brushes $N's hair for $M.  Looks better now.~
VictFound   $n brushes out your hair.  How nice of $m.~
CharAuto    You brush out your hair.  There - much better.~
OthersAuto  $n brushes out $s hair.  Looks much better now.~
End

#SOCIAL
Name        bully~
CharNoArg   You puff up your chest and act like a big bully!~
OthersNoArg $n puffs up $s chest and acts like a great big bully!~
CharFound   Your bully poor $N all around the room.~
OthersFound $n bullies poor $N all around the room.~
VictFound   $n bullies you all over the room!~
End

#SOCIAL
Name        bump~
CharNoArg   You bump into the wall. Ouch!~
OthersNoArg $n bumps into the wall. Ouch!~
CharFound   You bump into $N accidently on purpose. Ouch!~
OthersFound $n bumps into $N accidently on purpose. Ouch!~
VictFound   $n bumps into you accidently on purpose. Ouch!~
End

#SOCIAL
Name        bungee~
CharNoArg   You look around slyly, trying to find someone to kick off the mud.~
OthersNoArg $n looks around slyly, trying to find someone to kick off the mud.~
CharFound   You whip out a bungee cord, stick it on $N, and kick $M off the mud!~
OthersFound $n whips out a bungee cord, sticks it on $N, and pushes $M off the mud.~
VictFound   $n whips out a bungee cord, sticks it on you, and pushes you off the mud!! Eeeep!~
CharAuto    You connect yourself to a bungee cord, and jump off the mud! Bombs away!!~
OthersAuto  $n connects itself to a bungee cord and jumps off the mud! Phew, you were hoping $E would leave soon!~
End

#SOCIAL
Name        bunny~
CharNoArg   You strike a pose just like a widdle bunny hunny. Awww.~
OthersNoArg $n strikes a pose just like a widdle bunny hunny. Awww.~
CharFound   You look at $N and say, "That's my widdle bunny hunny!"~
OthersFound $n looks at $N and says, "That's my widdle bunny hunny!"~
VictFound   $n looks at you and says, "That's my widdle bunny hunny!"~
CharAuto    Awww... Do you miss your bunny?~
OthersAuto  $n looks as though $e misses $s little bunny.  Aww...~
End

#SOCIAL
Name        burp~
CharNoArg   You burp loudly.~
OthersNoArg $n burps loudly.~
CharFound   You burp loudly to $M in response.~
OthersFound $n burps loudly in response to $N's remark.~
VictFound   $n burps loudly in response to your remark.~
CharAuto    You burp at yourself.~
OthersAuto  $n burps at $mself.  What a sick sight.~
End

#SOCIAL
Name        butter~
CharNoArg   You smile sweetly.  Butter wouldn't melt in your mouth.~
OthersNoArg $n smiles sweetly. Butter wouldn't melt in $s mouth.~
CharFound   You smile sweetly at $N, protesting your innocence.~
OthersFound $n smiles sweetly at $N, looks like butter wouldn't melt in $s mouth.~
VictFound   $n smiles sweetly at you, looks like butter wouldn't melt in $s mouth.~
CharAuto    You smile sweetly at yourself.~
OthersAuto  $n smiles sweetly at $sself.  How inane!~
End

#SOCIAL
Name        bye~
CharNoArg   You say goodbye to all in the room.~
OthersNoArg $n says goodbye to everyone in the room.~
CharFound   You say goodbye to $N.~
OthersFound $n says goodbye to $N.~
VictFound   $n says goodbye to you.~
CharAuto    You say goodbye to yourself.  Contemplating suicide?~
OthersAuto  $n says goodbye to $mself.  Is $e contemplating suicide?~
End

#SOCIAL
Name        cackle~
CharNoArg   You throw back your head and cackle with insane glee!~
OthersNoArg $n throws back $s head and cackles with insane glee!~
CharFound   You cackle gleefully at $N.~
OthersFound $n cackles gleefully at $N.~
VictFound   $n cackles gleefully at you.  Better keep your distance from $m.~
CharAuto    You cackle at yourself.  Now, THAT'S strange!~
OthersAuto  $n is really crazy now!  $e cackles at $mself.~
End

#SOCIAL
Name        caress~
CharNoArg   Who needs to be caressed?~
OthersNoArg $n caresses $s hands softly because no one else will.~
CharFound   You gently caress $S face.~
OthersFound $n gently caresses $N's face.~
VictFound   $n gently caresses your face.~
CharAuto    You gently caress your face. My, what soft skin.~
OthersAuto  $n gently caresses $s face.~
End

#SOCIAL
Name        catnap~
CharNoArg   You curl into a tiny ball and go to sleep.~
OthersNoArg $n curls $mself into a tiny ball and goes to sleep.~
CharFound   You curl up in $S lap and go to sleep.~
OthersFound $n curls up in $N's lap and goes to sleep.~
VictFound   $n curls up in your lap and goes to sleep.~
CharAuto    You curl into a tiny ball and go to sleep.~
OthersAuto  $n curls $mself into a tiny ball and goes to sleep.~
End

#SOCIAL
Name        caveman~
CharNoArg   You beat your chest and grunt.~
OthersNoArg $n beats $s chest and grunts.~
CharFound   You bonk $N on the head and drag $M to the nearest cave.~
OthersFound $n bonks $N on the head and drags $M to the nearest cave.~
VictFound   $n bonks you on the head and drags you to the nearest cave.~
CharAuto    I don't think you can do that.~
OthersAuto  $n looks confused, grunting to $mself.~
End

#SOCIAL
Name        challenge~
CharNoArg   Challenge who?~
OthersNoArg $n is looking for someone to challenge to a fight to the death!~
CharFound   You challenge $N to a fight to the death.~
OthersFound $n challenges $N to a fight to the death.~
VictFound   $n challenges you to a fight to the death.~
CharAuto    Challenge YOURSELF to a fight to the death?  I think not...~
OthersAuto  $n tries to challenge $mself, how odd.~
End

#SOCIAL
Name        charge~
CharNoArg   Geronimo!!!~
OthersNoArg $n runs smack dab into a nearby wall.  Oh, sweet victory...~
CharFound   You duck your head and charge toward $N.~
OthersFound $n ducks $s head, stamps $s foot three times and charges toward $N.~
VictFound   $n lowers $s head, stamps $s foot three times and runs toward you screaming.. ~
CharAuto    You scream, 'Geronimo!,' as you chase yourself about the room.~
OthersAuto  $n screams, 'Geronimo!,' as they chase themself about the room.~
End

#SOCIAL
Name        cheep~
CharNoArg   You take a deep breath and chirp, "cheep cheep cheep".~
OthersNoArg $n takes a deep breath and chirps, "cheep cheep cheep".~
CharFound   You take a deep breath and chirp at $N, "cheep cheep cheep"~
OthersFound $n takes a deep breath and chirps at $N, "cheep cheep cheep".~
VictFound   $n takes a deep breath and chirps at you, "cheep cheep cheep".~
End

#SOCIAL
Name        cheer~
CharNoArg   And the peasants rejoiced...~
OthersNoArg $n emits an unrivaled cheer! Woo!~
CharFound   You show your wild enthusiasm for your new friend, $N.~
OthersFound $n cheers maniacally as $N shows them who's boss.~
VictFound   $n cheers you on like it's going out of style.~
CharAuto    Go me!~
OthersAuto  $n thinks $e's pretty damned neat.  Go $n!!!~
End

#SOCIAL
Name        cheesecake~
CharNoArg   You wish you had some cheesecake....yummmm.~
OthersNoArg $n wishes $e had some cheesecake....yummmm.~
CharFound   You start begging $N for some cheesecake...how pathetic.~
OthersFound $n is begging $N for some cheesecake...how pathetic.~
VictFound   $n tells you 'Please!  I'll do anything, just give me some cheesecake!'~
CharAuto    Desperate for cheesecake, you start nibbling on yourself!  Yuck!~
OthersAuto  $n starts thinking $e is a cheesecake and begins to nibble on $mself.  Yuck!~
End

#SOCIAL
Name        chicken~
CharNoArg   You peer around the room for cowards.~
OthersNoArg $n is looking for a coward.~
CharFound   You grin at $N and say 'Chicken?'~
OthersFound $n grins at $N and says 'Chicken?'~
VictFound   $n grins at you and says, 'Chicken?'~
CharAuto    You can't believe what a chicken you are.~
OthersAuto  $n shamefully admits $e's a chicken.~
End

#SOCIAL
Name        chill~
CharNoArg   You mutter the words "chill out" to no one in particular.~
OthersNoArg $n stares off into space and mutters "chill out" to $s imaginary friends. They must be on crack or something.~
CharFound   You wish $N would just chill out!~
OthersFound $n turns to $N and shouts, "Chill out, freako!"~
VictFound   $n turns to you and shouts "Chill out, freako!"~
CharAuto    You chill.~
OthersAuto  $n chills.  They're so cool.~
End

#SOCIAL
Name        chocolate~
CharNoArg   You are suffering from chocolate withdrawals.~
OthersNoArg $n is suffering from chocolate withdrawls.~
CharFound   You give $N a rich chocolate truffle. How sweet!~
OthersFound $n gives $N a rich chocolate truffle. MmmMmMmmmm~
VictFound   $n gives you a rich chocolate truffle. MmmMmMmmmm~
CharAuto    You calm your cravings as you enjoy a rich chocolate truffle. MmmMmmm~
OthersAuto  $n puts a rich chocolate truffle to $s lips. You drool uncontrollably.~
End

#SOCIAL
Name        chop~
CharNoArg   You make chopping motions around you.~
OthersNoArg $n makes chopping motions around $m.~
CharFound   You try to chop off $N's limbs.~
OthersFound $n is attempting to chop off $N's limbs.~
VictFound   $n is attempting to chop off your limbs.~
CharAuto    You pretend to chop off pieces of yourself.~
OthersAuto  $n pretends to chop off pieces of $mself.~
End

#SOCIAL
Name        chortle~
CharNoArg   You chortle with glee.~
OthersNoArg $n chortles with glee.~
CharFound   You chortle loudly at $M.~
OthersFound $n chortles loudly at $N.~
VictFound   $n chortles loudly at you.~
CharAuto    You chortle loudly to yourself.~
OthersAuto  $n chortles loudly to $mself.~
End

#SOCIAL
Name        chuckle~
CharNoArg   You chuckle politely.~
OthersNoArg $n chuckles politely.~
CharFound   You chuckle at $S joke.~
OthersFound $n chuckles at $N's joke.~
VictFound   $n chuckles at your joke.~
CharAuto    You chuckle at your own joke, since no one else would.~
OthersAuto  $n chuckles at $s own joke, since none of you would.~
End

#SOCIAL
Name        cigar~
CharNoArg   You light up a big cigar. All the females leave the room.~
OthersNoArg $n lights up a big cigar. All the females leave the room.~
CharFound   You give $N a big cigar and light it for $M. All the females flee the room.~
OthersFound $n gives a big cigar to $N and light it for $M. All the females flee the room.~
VictFound   $n gives you a big cigar and lights it for you. All the females flee the room.~
CharAuto    You give a cigar to yourself, are you feeling ok?~
OthersAuto  $n gives $mself a cigar.  Hmmmmm.~
End

#SOCIAL
Name        clap~
CharNoArg   You clap your hands together.~
OthersNoArg $n shows $s approval by clapping $s hands together.~
CharFound   You clap at $S performance.~
OthersFound $n claps at $N's performance.~
VictFound   $n claps at your performance.~
CharAuto    You clap at your own performance.~
OthersAuto  $n claps at $s own performance.~
End

#SOCIAL
Name        clue~
CharNoArg   You want to give a clue to yourself? You must have no clue, eh?~
OthersNoArg $n thinks $e has a clue ... but $e is wrong.~
CharFound   You try to give $N a clue ... but $E is totally clueless ... whadda dork, eh?~
OthersFound $n tries to give $N a clue ... but $E is totally clueless ... whadda dork, eh?~
VictFound   $n tries to give you a clue ... $e must think you are totally clueless, eh?~
CharAuto    You want to give yourself a clue? Bonehead!~
OthersAuto  $n tries to give themself a clue ... but $e is totally clueless, eh?~
End

#SOCIAL
Name        coffee~
CharNoArg   You wish someone would give you a cup of coffee.~
OthersNoArg $n wishes someone would offer $m a cup of coffee.~
CharFound   You pour a steaming hot cup of coffee and give it to $N.~
OthersFound $n pours a steaming hot cup of coffee and gives it to $N.~
VictFound   $n pours a steaming hot cup of coffee and gives it to you.~
CharAuto    You pour yourself a cup of coffee. It is a necessity of mud life.~
OthersAuto  $n pours a steaming hot cup of coffee. Bet you had one too!~
End

#SOCIAL
Name        comb~
CharNoArg   You comb your hair - perfect.~
OthersNoArg $n combs $s hair, how dashing!~
CharFound   You patiently untangle $N's hair - what a mess!~
OthersFound $n tries patiently to untangle $N's hair.~
VictFound   $n pulls your hair in an attempt to comb it.~
CharAuto    You pull your hair, but it will not be combed.~
OthersAuto  $n tries to comb $s tangled hair.~
End

#SOCIAL
Name        comfort~
CharNoArg   Do you feel uncomfortable?~
OthersNoArg $n tries to comfort $mself, $e looks uncomfortable.~
CharFound   You comfort $M.~
OthersFound $n comforts $N.~
VictFound   $n comforts you.~
CharAuto    You make a vain attempt to comfort yourself.~
OthersAuto  $n has no one to comfort $m but $mself.~
End

#SOCIAL
Name        concentrate~
CharNoArg   You concentrate on the universe for a moment.~
OthersNoArg $n appears to be concentrating quite seriously.~
CharFound   You concentrate on $M for a moment.~
OthersFound $n concentrates.  $N seems to be the reason.~
VictFound   $n is focusing all $s concentration on you.~
CharAuto    You concentrate on your self-image for a bit, and feel much better!~
OthersAuto  $n seems lost in concentration on personal thoughts.~
End

#SOCIAL
Name        contemplate~
CharNoArg   Quiet!  $n is in a very thoughtful mood.~
OthersNoArg $n appears to be deep in thought.~
CharFound   You wish that $N would think about what $E does occasionally.~
OthersFound $n wishes that $N would spend some time thinking before acting!~
VictFound   $n hopes you thought this through for once.~
CharAuto    You slap your forehead and wonder why you don't think before you act!~
OthersAuto  Shaking $s head, $n wonders where $s brains went to today.~
End

#SOCIAL
Name        cookie~
CharNoArg   You munch thoughtfully on a chocolate-chip cookie.~
OthersNoArg $n munches thoughtfully on a chocolate-chip cookie.~
CharFound   You give a delectable chocolate-chip cookie to $N.~
OthersFound $n gives a delectable chocolate-chip cookie to $N.~
VictFound   $n gives a delectable chocolate-chip cookie to you.~
CharAuto    You want to give a cookie to yourself? ~
OthersAuto  $n tries to give a cookie to $mself. How selfish!~
End

#SOCIAL
Name        corner~
CharNoArg   You quietly go to your corner and hide.~
OthersNoArg $n slinks into the corner and curls into a fetal position.~
CharFound   You send $M to the corner for a few moments of reflection.~
OthersFound $n directs $N to the corner for a few moments of time-out.~
VictFound   $n points to you and then the corner.  You're in trouble now.~
CharAuto    You acknowledge your mistake and send yourself to the corner.~
OthersAuto  $n puts $s head down and shamefully walks to the corner.~
End

#SOCIAL
Name        cough~
CharNoArg   You cough to clear your throat and eyes and nose and....~
OthersNoArg $n coughs loudly.~
CharFound   You cough loudly.  It must be $S fault, $E gave you this cold.~
OthersFound $n coughs loudly, and glares at $N, like it is $S fault.~
VictFound   $n coughs loudly, and glares at you.  Did you give $m that cold?~
CharAuto    You cough loudly.  Why don't you take better care of yourself?~
OthersAuto  $n coughs loudly.  $n should take better care of $mself.~
End

#SOCIAL
Name        cower~
CharNoArg   What are you afraid of?~
OthersNoArg $n cowers in the corner from claustrophobia.~
CharFound   You cower in the corner at the sight of $M.~
OthersFound $n cowers in the corner at the sight of $N.~
VictFound   $n cowers in the corner at the sight of you.~
CharAuto    You cower in the corner at the thought of yourself.  You scaredy cat!~
OthersAuto  $n cowers in the corner.  What is wrong with $m now?~
End

#SOCIAL
Name        cpr~
CharNoArg   You lie still hoping someone will try to resuscitate you.~
OthersNoArg $n plays dead. You notice $s lips are puckered...~
CharFound   You give $N mouth-to-mouth resuscitation!~
OthersFound $n locks lips with $N. Is that allowed?~
VictFound   $n tries to resuscitate you. Maybe you should stop struggling?~
CharAuto    You perform great feats of agility, barely saving yourself from death.~
OthersAuto  $n astounds you with $s miraculous self-preservation techniques.~
End

#SOCIAL
Name        cringe~
CharNoArg   You cringe in terror.~
OthersNoArg $n cringes in terror!~
CharFound   You cringe away from $M.~
OthersFound $n cringes away from $N in mortal terror.~
VictFound   $n cringes away from you.~
CharAuto    I beg your pardon?~
OthersAuto  $n cringes away from $mself, that has to be difficult.~
End

#SOCIAL
Name        croak~
CharNoArg   You attempt to sing, but it sounds more like a croak. Take lessons :P~
OthersNoArg $n is in obvious need of singing lessons as $e begins to croak out a song!~
CharFound   You croak out the first verse of "Row Row Row Your Boat" to $N. Ick.~
OthersFound $n begins to croak to $N. You realize that $e is attempting to sing "Row Row Row Your Boat". Ick.~
VictFound   $n looks at you and begins to croak out a song. Ugh!~
CharAuto    You hum to yourself.~
OthersAuto  $n begins to croak to $mself.~
End

#SOCIAL
Name        croon~
CharNoArg   You attempt to do you best Alfalfa crooning imitation.~
OthersNoArg $n attempts to do $s best Alfalfa crooning imitation.~
CharFound   You croon "A Poem as Lovely as a Tree" to $N.~
OthersFound $n croons "A Poem as Lovely as a Tree" to $N.~
VictFound   $n croons "A Poem as Lovely as a Tree" to you.~
CharAuto    You open your throat and release a mind-rattling croak!~
OthersAuto  $n opens $s throat and releases a mind-rattling croak!~
End

#SOCIAL
Name        cross~
CharNoArg   You cross your fingers and hope for the best.~
OthersNoArg $n crosses $s fingers for good luck.~
CharFound   You cross your fingers for $N and wish $M good luck.~
OthersFound $n crosses $s fingers and wishes $N the best of luck.~
VictFound   $n crosses $s fingers for you and wishes you good luck.~
End

#SOCIAL
Name        cruel~
CharNoArg   You make a face so cruel that everyone takes a step back in fear!~
OthersNoArg $n makes a face so cruel that everyone takes a step back in fear!~
CharFound   You give $N a look so cruel that $E fears for $S life!~
OthersFound $n gives $N a look so cruel that $E fears for $S life!~
VictFound   $n gives you a look so cruel that you fear for your life!~
End

#SOCIAL
Name        crush~
CharNoArg   You squint and hold two fingers up, saying 'I'm crushing your heads!'~
OthersNoArg $n squints and holds two fingers up, saying 'I'm crushing your heads!'~
CharFound   You hold two fingers up at $M and say, 'I'm crushing your head!'~
OthersFound $n holds two fingers up at $N and says, 'I'm crushing your head!'~
VictFound   $n holds two fingers up at you and says, 'I'm crushing your head!'~
CharAuto    You crush yourself.  YEEEEOOOUUUUCH!~
OthersAuto  $n crushes $mself into the ground.  OUCH!~
End

#SOCIAL
Name        cry~
CharNoArg   Waaaaah ...~
OthersNoArg $n bursts into tears.~
CharFound   You cry on $S shoulder.~
OthersFound $n cries on $N's shoulder.~
VictFound   $n cries on your shoulder.~
CharAuto    You cry to yourself.~
OthersAuto  $n sobs quietly to $mself.~
End

#SOCIAL
Name        cuddle~
CharNoArg   Whom do you feel like cuddling today?~
OthersNoArg $n looks like $e needs someone to cuddle.~
CharFound   You cuddle $M.~
OthersFound $n cuddles $N.~
VictFound   $n cuddles you.~
CharAuto    You must feel very cuddly indeed.~
OthersAuto  $n cuddles up to $s shadow.  What a sorry sight.~
End

#SOCIAL
Name        curse~
CharNoArg   You swear loudly for a long time.~
OthersNoArg $n swears: @*&^%@*&!~
CharFound   You swear at $M.~
OthersFound $n swears at $N.~
VictFound   $n swears at you!  Where are $s manners?~
CharAuto    You swear at your own mistakes.~
OthersAuto  $n starts swearing at $mself.  Why don't you help?~
End

#SOCIAL
Name        curtsey~
CharNoArg   You curtsey to your audience.~
OthersNoArg $n curtseys gracefully.~
CharFound   You curtsey to $M.~
OthersFound $n curtseys gracefully to $N.~
VictFound   $n curtseys gracefully for you.~
CharAuto    You curtsey to your audience (yourself).~
OthersAuto  $n curtseys to $mself, since no one is paying attention to $m.~
End

#SOCIAL
Name        daggers~
CharNoArg   You glare icy daggers around the room.~
OthersNoArg $n glares icy daggers around the room.~
CharFound   You glare icy daggers at $N.~
OthersFound $n glares icy daggers at $N.~
VictFound   $n glares icy daggers at you. Yikes.~
CharAuto    Buy yourself a straitjacket and a rubber room and you're all set to go.  Psycho.~
OthersAuto  $n is requesting a rubber room and a straitjacket. Pronto!~
End

#SOCIAL
Name        dambit~
CharNoArg   You squint your eyes, look around the room and scream "DAMBIT"!~
OthersNoArg $n squints $s eyes, looks around the room and screams "DAMBIT"!~
CharFound   You squint your eyes at $N and scream "DAMBIT"!~
OthersFound $n squints $s eyes at $N and screams "DAMBIT!"~
VictFound   $n squints $s eyes at you and screams "DAMBIT!"~
CharAuto    You squint your eyes and scream "DAMBIT!" to yourself.~
OthersAuto  $n squints $s eyes and screams "DAMBIT!" at $mself...~
End

#SOCIAL
Name        dance~
CharNoArg   Feels silly, doesn't it?~
OthersNoArg $n tries to break dance, but nearly breaks $s neck!~
CharFound   You sweep $M into a romantic waltz.~
OthersFound $n sweeps $N into a romantic waltz.~
VictFound   $n sweeps you into a romantic waltz.~
CharAuto    You skip and dance around by yourself.~
OthersAuto  $n dances a pas-de-une.~
End

#SOCIAL
Name        dazzle~
CharNoArg   You dazzling flirt you. No one can resist this smile.~
OthersNoArg Dazzling is the only word to describe $n's smile.~
CharFound   You attempt to dazzle $N as you flash $M a huge smile and stand straight.~
OthersFound A flash of white nearly blinds you as $n smiles.~
VictFound   $n has the most amazing teeth, you think, as $e flashes you an incredible smile.~
CharAuto    If you can't kill them, perhaps this dazzling smile will work?~
OthersAuto  $n dazzles everyone with $s smile.~
End

#SOCIAL
Name        dimple~
CharNoArg   You show off your dimples.~
OthersNoArg $n smiles angelically, showing off $s dimples.~
CharFound   You show off your dimples to $N.~
OthersFound $n smiles and shows off $s dimples to $N.~
VictFound   $n smiles angelically at you, showing off $s dimples.~
CharAuto    You smile at yourself... how odd...~
OthersAuto  $n is busy smiling at $mself. Hmm...~
End

#SOCIAL
Name        dive~
CharNoArg   You dive into the ocean.~
OthersNoArg $n dives into the ocean.~
CharFound   You dive behind $M and hide.~
OthersFound $n dives behind $N and hides.~
VictFound   $n dives behind you and hides.~
CharAuto    You take a dive.~
OthersAuto  $n takes a dive.~
End

#SOCIAL
Name        dizzy~
CharNoArg   You are so dizzy from all this chatter.~
OthersNoArg $n spins twice and hits the ground, dizzy from all this chatter.~
CharFound   You are dizzy from all of $N's chatter.~
OthersFound $n spins twice and hits the ground, dizzy from all $N's chatter.~
VictFound   $n spins twice and hits the ground, dizzy from all your chatter.~
CharAuto    You are dizzy from lack of air.  Don't talk so much!~
OthersAuto  $n spins twice and falls to the ground from lack of air.~
End

#SOCIAL
Name        doh~
CharNoArg   You slap your forehead and say "DOH!"~
OthersNoArg $n slaps $mself on the head and yells, "DOH!"~
CharFound   You backhand $N's forehead and say, "DOH!."~
OthersFound $n slaps $N on the head and says, "DOH!".~
VictFound   $n looks at you with disdain before slapping your forehead and saying, "DOH!"~
CharAuto    Slapping your forehead, you yell "DOH!" at yourself.~
OthersAuto  $n keeps hitting $mself in the head and uttering, "Doh!"~
End

#SOCIAL
Name        dork~
CharNoArg   You stand around feeling like a big DORK.~
OthersNoArg $n stands around looking like a big DORK.~
CharFound   You point at $N and scream "$N is a big DORK!"~
OthersFound $n points at $N and screams "$N is a big DORK!"~
VictFound   $n points at you and screams "$N is a big DORK!"~
CharAuto    You point to yourself and scream "I'm a big DORK!"~
OthersAuto  $n points to $mself and screams "I'm a big DORK!"~
End

#SOCIAL
Name        douse~
CharNoArg   Douse who?~
OthersNoArg $n mutters something about the heat and pours cold water over $mself.  Great, now there's a a large puddle of water near you.~
CharFound   You slowly and methodically dump a bucket of ice over $N.~
OthersFound Smirking, $n pours ice water on $N!  That should cool $M off!~
VictFound   $n has doused you with ice water. That should calm you.~
CharAuto    Hmmm...did that cool you off? ~
OthersAuto  Complaining of the heat, $n pours cold water over $s head.~
End

#SOCIAL
Name        doze~
CharNoArg   Your chin drops to your chest as you suddenly get dozy.~
OthersNoArg $n's eyes get heavy as $s head drops in a sudden doze.~
CharFound   You weave your hands in a hypnotic pattern trying to make $N sleepy.~
OthersFound $n weaves $s hands in a hypnotic pattern, attempting to put $N to sleep.~
VictFound   $n weaves $s hands in a hypnotic pattern. You start to feel dozy.~
CharAuto    You weave your hands in an odd pattern, suddenly finding yourself dozing off.~
OthersAuto  $n weaves $s hands in an odd pattern, looking surprized to be dozing off.~
End

#SOCIAL
Name        dream~
CharNoArg   You gaze off into the distance, lost in pleasant thought.~
OthersNoArg $n gazes off into the distance, lost in pleasant thoughts.~
CharFound   You gaze off into the distance, lost in pleasant thoughts of $N~
OthersFound $n gazes off in the distance, lost in pleasant thoughts of $N.~
VictFound   $n gazes off in the distance, lost in pleasant thoughts of you.~
CharAuto    You gaze off into the distance, lost in pleasant thoughts of yourself. How vain!~
OthersAuto  $n gazes off into the distance, lost in pleasant thoughts of $mself, What an ego!~
End

#SOCIAL
Name        dribble~
CharNoArg   You get too excited for your own good and dribble all over yourself.~
OthersNoArg $n gets too excited and dribbles all over $mself.~
CharFound   You are so happy to see $N, you lose control and dribble all over $M.~
OthersFound $n is so happy to see $N, $e loses control and dribbles all over $M.~
VictFound   $n is so happy to see you $e loses control and dribbles all over you.~
End

#SOCIAL
Name        drool~
CharNoArg   You drool on yourself.~
OthersNoArg $n drools on $mself.~
CharFound   You drool all over $N.~
OthersFound $n drools all over $N.~
VictFound   $n drools all over you.~
CharAuto    You drool on yourself.~
OthersAuto  $n drools on $mself.~
End

#SOCIAL
Name        duck~
CharNoArg   Whew!  That was close!~
OthersNoArg $n is narrowly missed by a low-flying dragon.~
CharFound   You duck behind $M.  Whew!  That was close!~
OthersFound $n ducks behind $N to avoid the fray.~
VictFound   $n ducks behind you to avoid the fray.~
CharAuto    You duck behind yourself.  Oww that hurts!~
OthersAuto  $n tries to duck behind $mself.  $n needs help getting untied now.~
End

#SOCIAL
Name        duel~
CharNoArg   You take off your left gauntlet and smack an imaginary foe in the face with it.~
OthersNoArg $n takes of $s left gauntlet and smacks the air with it.~
CharFound   You take off your left gauntlet and smack $N in the face with it.~
OthersFound $n takes off $s left gauntlet and smacks $N in the face with it.~
VictFound   $n takes off $s left gauntlet and smacks you in the face with it.~
CharAuto    Maybe you are ready for the asylum?~
OthersAuto  $n takes off $s left gauntlet and smacks $mself in the face with it, seems like a bad case of mental illness.~
End

#SOCIAL
Name        duh~
CharNoArg   You wonder where the moron who did that is.~
OthersNoArg $n wonders aloud, "Did someone just do something real dumb?"~
CharFound   You gleefully point out what a dummy $N is.~
OthersFound $n shouts, "Boy is $N a dummy!"~
VictFound   Your ears burn as $n calls you a real dummy!~
CharAuto    You are such a dummy!~
OthersAuto  Oh my god! Is $n dumb or what!~
End

#SOCIAL
Name        dukes~
CharNoArg   You stick up your dukes and get ready for a brawl.~
OthersNoArg $n sticks up $s dukes, looks like $e wants a fight!~
CharFound   You growl at $N and stick up your dukes, intent on a brawl.~
OthersFound $n snarls at $N in rage and sticks up $s fists, better leave those two alone for a while.~
VictFound   $n snarls at you angrily and sticks up $s dukes, think $e wants a fight?~
CharAuto    You are so mad at yourself that you throw up your fists in rage!~
OthersAuto  $n is so mad at $mself that $e throws up his fists in rage! Maybe $e needs some alone time...~
End

#SOCIAL
Name        dumpling~
CharNoArg   You are as cute as a lil ol' dumpling.~
OthersNoArg $n is as cute as a lil ol' dumpling.~
CharFound   You think $N is as cute as a lil ol' dumpling.~
OthersFound $n thinks $N is as cute as a lil ol' dumpling.~
VictFound   $n thinks you are as cute as a lil ol' dumpling.~
End

#SOCIAL
Name        ears~
CharNoArg   You wiggle your ears.~
OthersNoArg $n wiggles $s ears.~
CharFound   You pull $N's ears. Ouch!~
OthersFound $n pulls $N's ears. Ouch!~
VictFound   $n pulls your ears. Ouch!~
End

#SOCIAL
Name        eek~
CharNoArg   You eek!~
OthersNoArg $n squeaks out an 'Eek!'~
CharFound   You look at $N and eek!~
OthersFound $n looks at $N and eeks! What has $E done to $m?~
VictFound   $n eeks at you! What have you done to $m?~
CharAuto    You eek! What have you done?!~
OthersAuto  $n eeks! What has $e done?!~
End

#SOCIAL
Name        eep~
CharNoArg   You eep!~
OthersNoArg $n eeps!~
CharFound   You look at $N and say 'Eep!'~
OthersFound $n looks at $N and says 'Eep'!~
VictFound   $n looks at you and says 'Eep'!~
CharAuto    You want to eep yourself? No way!~
End

#SOCIAL
Name        egrin~
CharNoArg   You grin evilly.~
OthersNoArg $n grins evilly.~
CharFound   You grin evilly at $M.~
OthersFound $n grins evilly at $N.~
VictFound   $n grins evilly at you.  Hmmm.  Better keep your distance.~
CharAuto    You grin at yourself.  You must be getting very bad thoughts.~
OthersAuto  $n grins at $mself.  You must wonder what's in $s mind.~
End

#SOCIAL
Name        eh~
CharNoArg   You look around and exclaim, "How's it going, eh?"~
OthersNoArg $n looks around and exclaims, "How's it going, eh?"~
CharFound   You look at $N and exclaim, "How's it going $N, eh?"~
OthersFound $n looks at $N and exclaims, "How's it going $N, eh?" What a hoser!~
VictFound   $n looks at you and exclaims, "How's it going, eh?" What a hoser!~
End

#SOCIAL
Name        embrace~
CharNoArg   Who do you want to hold?~
OthersNoArg $n looks around for someone to hold close to $m.~
CharFound   You hold $M in a warm and loving embrace.~
OthersFound $n holds $N in a warm and loving embrace.~
VictFound   $n holds you in a warm and loving embrace.~
CharAuto    You hold yourself in a warm and loving embrace.  Feels silly doesn't it?~
OthersAuto  $n holds $mself in a warm and loving embrace.  $e looks pretty silly.~
End

#SOCIAL
Name        encore~
CharNoArg   Would you like to do that again?~
OthersNoArg $n would like to do that again!~
CharFound   You think $N did such a great job they should do it again!~
OthersFound $n thinks that $N did such a great job they should do it again!~
VictFound   $n thinks you did such a great job, you should do it again!~
CharAuto    You really had fun doing that. How about another?~
OthersAuto  $n had so much fun that $e would like to do that all over again!~
End

#SOCIAL
Name        eskimo~
CharNoArg   You rub your nose ... just like an Eskimo!~
OthersNoArg $n rubs $s nose ... just like an Eskimo!~
CharFound   You rub noses with $N showing how much you like $M.~
OthersFound $n rubs noses with $N showing $M how much $e likes $M.~
VictFound   $n rubs noses with you showing how much $e likes you.~
End

#SOCIAL
Name        evilgrin~
CharNoArg   You grin so evilly that everyone's alignment drops to -1000.~
OthersNoArg $n grins so evilly that everyone's alignment drops to -1000.~
CharFound   You grin so evilly at $M that $S alignment drops to -1000.~
OthersFound $n grins so evilly at $N that $S alignment drops to -1000.~
VictFound   $n grins so evilly at you that your alignment drops to -1000.~
CharAuto    You grin so evilly at yourself that your alignment drops to -1000.~
OthersAuto  $n grins so evilly that $s alignment drops to -1000.~
End

#SOCIAL
Name        eww~
CharNoArg   You scrunch up your nose and exclaim "EWWWWW!"~
OthersNoArg $n scrunches up $s nose and exclaims "EWWWWW!"~
CharFound   You point at $N and exclaim "EWWWWW!"~
OthersFound $n points at $N and exclaims "EWWWWW!~
VictFound   $n points at you and exclaims "EWWWWW!"~
CharAuto    You point at yourself and exclaim "EWWWWW!"~
OthersAuto  $n points at $mself and exclaims "EWWWWW!" ~
End

#SOCIAL
Name        excellent~
CharNoArg   You grin and say 'Excellent, Party on'~
OthersNoArg $n grins and says 'Excellent, Party on'~
CharFound   You tell $N, 'Excellent, Party on dude'~
OthersFound $n tells $N 'Excellent, Party on dude'~
VictFound   $n tells you 'Excellent, Party on dude'~
CharAuto    You think to yourself 'Excellent, I can party on!'~
OthersAuto  $n looks like $e is enoying a good thought.~
End

#SOCIAL
Name        eyebrow~
CharNoArg   You raise an eyebrow.~
OthersNoArg $n raises an eyebrow.~
CharFound   You raise an eyebrow at $M.~
OthersFound $n raises an eyebrow at $N.~
VictFound   $n raises an eyebrow at you.~
CharAuto    You raise an eyebrow at yourself.  That hurt!~
OthersAuto  $n raises an eyebrow at $mself.  That must have hurt!~
End

#SOCIAL
Name        faint~
CharNoArg   You feel dizzy and hit the ground like a board.~
OthersNoArg $n's eyes roll back in $s head and $e crumples to the ground.~
CharFound   You faint into $S arms.~
OthersFound $n faints into $N's arms.~
VictFound   $n faints into your arms.  How romantic.~
CharAuto    You look down at your condition and faint.~
OthersAuto  $n looks down at $s condition and faints dead away.~
End

#SOCIAL
Name        fakerep~
CharNoArg   You report: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp.~
OthersNoArg $n reports: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp~
CharFound   You report: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp.~
OthersFound $n reports: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp.~
VictFound   $n reports: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp.~
CharAuto    You report: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp.~
OthersAuto  $n reports: 12874/13103 hp 9238/10230 mana 2483/3451 mv 2.3113 xp.~
End

#SOCIAL
Name        fear~
CharNoArg   You are overcome by fear and begin shaking uncontrollably.~
OthersNoArg $n is overcome by fear and begins shaking uncontrollably.~
CharFound   You look at $N and are overcome by fear.~
OthersFound $n looks at $N and is overcome by fear. $n's knees start knocking.~
VictFound   $n takes one look at you and is overcome by fear. $n's knees begin to knock and $s legs buckle.~
End

#SOCIAL
Name        fiddledeedee~
CharNoArg   You exclaim, 'Oh! Fiddle-dee-dee!'~
OthersNoArg $n exclaims, 'Oh! Fiddle-dee-dee!'~
CharFound   You tell $M, 'Fiddle-dee-dee!'~
OthersFound $n looks at $M and says, 'Fiddle-dee-dee!'~
VictFound   $n looks at you and says, 'Fiddle-dee-dee!'~
CharAuto    You mutter to yourself. Fiddle-dee-dee.~
OthersAuto  $n mutters to $mself. Fiddle-dee-dee.~
End

#SOCIAL
Name        fierce~
CharNoArg   You gnash your teeth and grizzle your face and appear incredibly fierce.~
OthersNoArg $n gnashes $s teeth and grizzles $s face and appears incredibly fierce!~
CharFound   You give $N a fierce spanking. $N begins to bleed.~
OthersFound $n gives $N a fierce spanking. $N begins to bleed.~
VictFound   $n gives you a fierce spanking. You begin to bleed.~
End

#SOCIAL
Name        finger~
CharNoArg   You pull your finger.~
OthersNoArg $n pulls $s finger.~
CharFound   You pull $N's finger.~
OthersFound $n pulls $N's finger. Watch out!~
VictFound   $n pulls your finger. Uh oh!~
End

#SOCIAL
Name        flex~
CharNoArg   You flex.~
OthersNoArg $n flexes. Must think $e's buff.~
CharFound   You flex for $M. Impressive!~
OthersFound $n flexes in a vain attempt at impressing $N.~
VictFound   You watch $n flex. Are you impressed, or what?~
CharAuto    You flex, just to make sure you still got it.~
OthersAuto  $n flexes $s muscles.~
End

#SOCIAL
Name        flick~
CharNoArg   Seeing pixies?~
OthersNoArg $n holds $s fingers up, flicking the air.  How odd.~
CharFound   You flick $N right across the room!  Meanie!~
OthersFound $n flicks $N right across the room!~
VictFound   $n flicks you across the room! $e's in for it now...~
CharAuto    You flick yourself right in the middle of your forehead.~
OthersAuto  $n flicks $mself in the forehead.  What a dummy.~
End

#SOCIAL
Name        flip~
CharNoArg   You flip head over heels.~
OthersNoArg $n flips head over heels.~
CharFound   You flip $M over your shoulder.~
OthersFound $n flips $N over $s shoulder.~
VictFound   $n flips you over $s shoulder.  Hmmmm.~
CharAuto    You tumble all over the room.~
OthersAuto  $n does some nice tumbling and gymnastics.~
End

#SOCIAL
Name        flirt~
CharNoArg   Wink wink!~
OthersNoArg $n flirts -- probably needs a date, huh?~
CharFound   You flirt with $M.~
OthersFound $n flirts with $N.~
VictFound   $n wants you to show some interest and is flirting with you.~
CharAuto    You flirt with yourself.~
OthersAuto  $n flirts with $mself.  Hoo boy.~
End

#SOCIAL
Name        fluffle~
CharNoArg   You've got to fluffle SOMEONE.~
CharFound   You fluffle $N's hair playfully.~
OthersFound $n fluffles $N's hair playfully.~
VictFound   $n fluffles your hair playfully.~
CharAuto    You fluffle your hair.~
OthersAuto  $n fluffles $s hair.~
End

#SOCIAL
Name        flutter~
CharNoArg   You flutter your eyelashes.~
OthersNoArg $n flutters $s eyelashes.~
CharFound   You flutter your eyelashes at $M.~
OthersFound $n flutters $s eyelashes in $N's direction.~
VictFound   $n looks at you and flutters $s eyelashes.~
CharAuto    You flutter your eyelashes at the thought of yourself.~
OthersAuto  $n flutters $s eyelashes at no one in particular.~
End

#SOCIAL
Name        fondle~
CharNoArg   You suddenly have the urge to fondle someone~
OthersNoArg $n is looking for someone to fondle, get out while you can!~
CharFound   You fondly fondle $N.~
OthersFound $n starts fondling $N, maybe they should get a room!~
VictFound   $n fondly fondles you.~
CharAuto    You start fondling yourself, lonely eh?~
OthersAuto  $n starts fondling $mself.  $n appears to be going blind.~
End

#SOCIAL
Name        footrub~
CharNoArg   You glance around the room looking for someone to rub your feet.~
OthersNoArg $n glances around the room looking for someone to rub $s feet.~
CharFound   You ask $N to rub your tired feet.~
OthersFound $n asks $N to rub $s tired feet. Better leave before $e asks you.~
VictFound   $n asks you to rub $s tired feet. Better leave your gloves on.~
CharAuto    You rub your tired feet, waiting for a refresh.~
OthersAuto  $n rubs $s tired feet. $e sure could use a refresh.~
End

#SOCIAL
Name        forgive~
CharNoArg   You forgive the world in general for the wrongs it has done to you.~
OthersNoArg $n forgives the world in general for the wrongs it has done $m.~
CharFound   You forgive $N for $S past transgressions and say 'Don't do it again!'~
OthersFound $n forgives $N for $S past transgressions and says 'Don't do it again!'~
VictFound   $n forgives you for your past transgressions and says 'Don't do it again!'~
CharAuto    You forgive yourself for your sins and feel much better.~
OthersAuto  $n forgives $mself for $s sins and looks very relieved.~
End

#SOCIAL
Name        french~
CharNoArg   Kiss whom?~
OthersNoArg $n is looking for someone to kiss.~
CharFound   You give $N a long and passionate kiss.~
OthersFound $n kisses $N passionately.~
VictFound   $n gives you a long and passionate kiss.~
CharAuto    You gather yourself in your arms and try to kiss yourself.~
OthersAuto  $n makes an attempt at kissing $mself.~
End

#SOCIAL
Name        frisky~
CharNoArg   You are feeling frisky, watch out!~
OthersNoArg $n has an odd gleam in $s eye, watch out!~
CharFound   You let $N know you're feeling a little frisky.~
OthersFound $n lets $N know $e is feeling a little frisky at the moment.~
VictFound   $n wants you to know $e is feeling a little frisky at the moment.~
CharAuto    You start getting frisky with yourself. Get a room!~
OthersAuto  $n starts getting frisky with $mself. Time for $m to take a cold shower!~
End

#SOCIAL
Name        frolick~
CharNoArg   You throw confetti into the air and frolick about with gleeful abandon.~
OthersNoArg $n throws confetti into the air and frolicks about with gleeful abandon.~
CharFound   You take $N's hand and frolick about with gleeful abandon.~
OthersFound $n takes $N's hand and frolicks about with gleeful abandon.~
VictFound   $n takes your hand and frolicks about with gleeful abandon.~
End

#SOCIAL
Name        frown~
CharNoArg   What's bothering you ?~
OthersNoArg $n frowns.~
CharFound   You frown at what $N did.~
OthersFound $n frowns at what $N did.~
VictFound   $n frowns at what you did.~
CharAuto    You frown at yourself.  Poor baby.~
OthersAuto  $n frowns at $mself.  Poor baby.~
End

#SOCIAL
Name        fstar~
CharNoArg   A falling star catches your eye.~
OthersNoArg $n is looking for a falling star to wish upon.~
CharFound   You and $N make a wish upon a falling star.~
OthersFound $n and $N wistfully gaze upon a falling star.~
VictFound   $n wishes upon a falling star for your happiness.~
CharAuto    You make a wish upon a falling star.~
OthersAuto  $n watches a falling star.~
End

#SOCIAL
Name        ftkiss~
CharNoArg   You kiss your fingers, looking for someone's forehead to place them upon.~
OthersNoArg $n kisses $s fingers, looking around for someone to place $s affection on.~
CharFound   You kiss your fingers, and gently place them upon $N's forehead.~
OthersFound $n kisses $s fingers, and gently places them upon $N's forehead.~
VictFound   $n kisses $s fingers, and gently places them upon your forehead.~
CharAuto    You kiss your fingertips, and slowly press them upon your forehead.~
OthersAuto  $n kisses $s fingertips, placing them gently upon $s forehead.~
End

#SOCIAL
Name        fume~
CharNoArg   You grit your teeth and fume with rage.~
OthersNoArg $n grits $s teeth and fumes with rage.~
CharFound   You stare at $M, fuming.~
OthersFound $n stares at $N, fuming with rage.~
VictFound   $n stares at you, fuming with rage!~
CharAuto    That's right - hate yourself!~
OthersAuto  $n clenches $s fists and stomps his feet, fuming with anger.~
End

#SOCIAL
Name        gag~
CharNoArg   You gag.~
OthersNoArg $n suddenly starts gagging.~
CharFound   You start gagging in $N's direction.~
OthersFound $n starts gagging in $N's direction.~
VictFound   $n starts gagging in your direction.~
CharAuto    You attempt to gag yourself.~
OthersAuto  $n attempts to gag $mself.~
End

#SOCIAL
Name        gasp~
CharNoArg   You gasp in astonishment.~
OthersNoArg $n gasps in astonishment.~
CharFound   You gasp as you realize what $E did.~
OthersFound $n gasps as $e realizes what $N did.~
VictFound   $n gasps as $e realizes what you did.~
CharAuto    You look at yourself and gasp!~
OthersAuto  $n takes one look at $mself and gasps in astonisment!~
End

#SOCIAL
Name        gawk~
CharNoArg   You gawk at everyone around you.~
OthersNoArg $n gawks at everyone in the room.~
CharFound   You gawk at $M.~
OthersFound $n gawks at $N.~
VictFound   $n gawks at you.~
CharAuto    You gawk as you think what you must look like to others.~
OthersAuto  $n is gawking again.  What is on $s mind?~
End

#SOCIAL
Name        gbhug~
CharNoArg   You hug a great big grizzly bear.~
OthersNoArg $n hugs a great big grizzly bear.~
CharFound   You give $N a great big bearhug. Ooooof!~
OthersFound $n gives a great big bearhug to $N. Oooof!~
VictFound   $n gives you a great big bearhug. Ooooof!~
End

#SOCIAL
Name        ghug~
CharNoArg   GROUP HUG!  GROUP HUG!~
OthersNoArg $n hugs you all in a big group hug.  How sweet!~
CharFound   GROUP HUG!  GROUP HUG!~
OthersFound $n hugs you all in a big group hug.  How sweet!~
VictFound   $n hugs you all in a big group hug.  How sweet!~
CharAuto    GROUP HUG!  GROUP HUG!~
OthersAuto  $n hugs you all in a big group hug.  How sweet!~
End

#SOCIAL
Name        giggle~
CharNoArg   You giggle.~
OthersNoArg $n giggles.~
CharFound   You giggle at $M.~
OthersFound $n giggles at $N's actions.~
VictFound   $n giggles at you.  Hope it's not contagious!~
CharAuto    You giggle at yourself.  You must be nervous or something.~
OthersAuto  $n giggles at $mself.  $n must be nervous or something.~
End

#SOCIAL
Name        girn~
CharNoArg   You try to grin, but somehow get it slightly wrong.~
OthersNoArg $n tries to grin, but somehow gets it slightly wrong.~
CharFound   You try to grin at $N, but $E gives you a funny look.~
OthersFound $n tries to grin at $N, but screws it up badly.~
VictFound   $n turns $s lips in a sad attempt at a lopsided grin.~
CharAuto    Your face becomes a ghastly mask as you fail to grin.~
OthersAuto  $n's face becomes a strange death mask as $e tries to grin.~
End

#SOCIAL
Name        gjob~
CharNoArg   You leap in the air and yell "Good Job!"~
OthersNoArg $n pats $mself on the back and smiles.~
CharFound   You plant a great big gold star on $N.~
OthersFound $n congratulates $N for a job well done.~
VictFound   $n sticks a gold star on your forehead and says "Good Job!"~
CharAuto    You try and stick a gold star on your forehead in order to get attention for your good work.~
OthersAuto  $n tries to praise $mself for a job well done.~
End

#SOCIAL
Name        glare~
CharNoArg   You glare at nothing in particular.~
OthersNoArg $n glares around $m.~
CharFound   You glare icily at $M.~
OthersFound $n glares at $N.~
VictFound   $n glares icily at you, you feel cold to your bones.~
CharAuto    You glare icily at your feet, they are suddenly very cold.~
OthersAuto  $n glares at $s feet, what is bothering $m?~
End

#SOCIAL
Name        glower~
CharNoArg   You glower in frustration. Having a bad day?~
OthersNoArg $n glowers in frustration.~
CharFound   You glower at $N in frustration over what $E said.~
OthersFound $n glowers at $N in frustration.~
VictFound   $n glowers at you in frustration. Did you say something?~
CharAuto    You glower at yourself. Having a bad day?~
OthersAuto  $n glowers at $mself. Maybe $e's having a bad day.~
End

#SOCIAL
Name        gnight~
CharNoArg   You tell everyone goodnight.~
OthersNoArg $n tells everyone goodnight.~
CharFound   You give $N a soft goodnight kiss and wave to $M.~
OthersFound $n gives $N a soft goodnight kiss then waves to $M.~
VictFound   $n gives you a soft goodnight kiss and then waves to you.~
CharAuto    You say goodnight to yourself.~
OthersAuto  $n says goodnight to $mself, $e must really be tired.~
End

#SOCIAL
Name        gobble~
CharNoArg   You clear your throat and gobble several times.~
OthersNoArg $n clears $s throat and gobbles most musically.~
CharFound   You clear your throat and gobble profusely at $N.~
OthersFound $n clears $s throat and gobbles like a turkey at $N.~
VictFound   $n clears $s throat and gobbles like a turkey at you.~
CharAuto    Gobbling at yourself? Okaaaay then.~
OthersAuto  $n starts gobbling to $mself.  How odd.~
End

#SOCIAL
Name        goodluck~
CharNoArg   To whom do you want to wish luck?~
OthersNoArg $n appears to want to wish someone good luck.~
CharFound   You slap $N on the back and wish $M good luck.~
OthersFound $n slaps $N on the back and wishes $M good luck in $S adventures.~
VictFound   $n slaps you on the back and wishes you good luck.~
CharAuto    You must be about to attempt something truly foolish to wish yourself goodluck.~
OthersAuto  $n is about to attempt something that will probably get $mself killed.~
End

#SOCIAL
Name        goodnight~
CharNoArg   You bid goodnight to everyone in the room.~
OthersNoArg $n bids goodnight to everyone.~
CharFound   You wish $N goodnight.~
OthersFound $n says goodnight to $N.~
VictFound   $n wishes you goodnight.~
CharAuto    Now I lay me down to sleep...~
OthersAuto  $n says $s prayers.~
End

#SOCIAL
Name        goose~
CharNoArg   You honk like a goose!~
OthersNoArg It dawns on you that $n looks a lot like a goose.~
CharFound   You goose $N!~
OthersFound $n attempts to goose $N! Uh oh...~
VictFound   $n gooses you and then runs away giggling!~
CharAuto    You try to hatch a goose egg.~
OthersAuto  $n nonchalantly sits on a goose egg and waits patiently.~
End

#SOCIAL
Name        grab~
CharNoArg   You grab.~
OthersNoArg $n grabs thin air. Hallucinations possibly?~
CharFound   You grab $N and scream MINE!~
OthersFound $n grabs $N and says MINE! ~
VictFound   $n grabs you and pulls you close while yelling, "MINE!".~
CharAuto    That's just not right.~
OthersAuto  $n attempted to perform something that was censored by the FCC.~
End

#SOCIAL
Name        gratz~
CharNoArg   Congratulate who?~
OthersNoArg $n is looking for someone to congratulate.~
CharFound   You congratulate $N with a big slap on $S back!~
OthersFound $n congratulates $N with a big hearty slap on the back!~
VictFound   $n congratulates you with a big slap on your back!~
CharAuto    You want to congratulate yourself? You must be an egomaniac!~
OthersAuto  $n congratulates $mself. What a goof!~
End

#SOCIAL
Name        greet~
CharNoArg   You greet everyone in the room.~
OthersNoArg $n greets you with a pleasant smile.~
CharFound   You warmly greet $N, saying "Well met, $N! How fare thee, on this fine day?"~
OthersFound $n greets $N warmly.~
VictFound   $n smiles at you and says, "How fare thee on this fine day, $N?"~
End

#SOCIAL
Name        grimace~
CharNoArg   You contort your face in disgust.~
OthersNoArg $n grimaces in disgust.~
CharFound   You grimace in disgust at $M.~
OthersFound $n grimaces in disgust at $N.~
VictFound   $n grimaces in disgust at you.~
CharAuto    You grimace at yourself in disgust.~
OthersAuto  $n grimaces at $mself in disgust.~
End

#SOCIAL
Name        grin~
CharNoArg   You grin.~
OthersNoArg $n grins.~
CharFound   You grin at $M.~
OthersFound $n grins at $N.~
VictFound   $n grins at you.~
CharAuto    you grin at yourself. What are you thinking?~
OthersAuto  $n grins at $mself. What is $e thinking about?~
End

#SOCIAL
Name        grip~
CharNoArg   You tighten your grip on your weapon, preparing for battle.~
OthersNoArg $n tightens $s grip on $s weapon, preparing for battle.~
CharFound   You grip your weapon, preparing for battle with $N!~
OthersFound $n tightens $s grip on $s weapon, preparing for battle with $N!~
VictFound   $n tightens $s grip on $s weapon, preparing for battle with you!~
CharAuto    You attempt to get a grip on yourself.~
OthersAuto  $n attempts to get a grip on $mself.~
End

#SOCIAL
Name        gripe~
CharNoArg   You gripe.~
OthersNoArg $n gripes about anything and everything to any who will listen.~
CharFound   You gripe to $N about everything from  the plague to chastity belts.~
OthersFound $n gripes to $N. ~
VictFound   $n gripes non stop. Perhaps $s armor is a bit too tight?~
CharAuto    You gripe to no one in particular.~
OthersAuto  $n is griping about something.~
End

#SOCIAL
Name        groan~
CharNoArg   You groan loudly.~
OthersNoArg $n groans loudly.~
CharFound   You groan at the sight of $M.~
OthersFound $n groans at the sight of $N.~
VictFound   $n groans at the sight of you.~
CharAuto    You groan as you realize what you have done.~
OthersAuto  $n groans as $e realizes what $e has done.~
End

#SOCIAL
Name        grope~
CharNoArg   You madly look around for someone to grope.~
OthersNoArg $n is looking for someone to grope, RUN AWAY!~
CharFound   You grope $N!~
OthersFound $n gropes $N, maybe you should leave them alone.~
VictFound   $n gropes you!~
CharAuto    You start groping yourself. Looking for love in all the wrong places, maybe?~
OthersAuto  $n starts groping $mself, maybe you should leave $m alone.~
End

#SOCIAL
Name        grouch~
CharNoArg   You are acting quite grouchy!~
OthersNoArg $n is acting quite grouchy. Better go away!~
CharFound   You are feeling quite grouchy towards $N.~
OthersFound $n is feeling quite grouchy towards $N.~
VictFound   $n is feeling quite grouchy towards you. Perhaps this would be a good time to leave?~
End

#SOCIAL
Name        grovel~
CharNoArg   You grovel in the dirt.~
OthersNoArg $n grovels in the dirt.~
CharFound   You grovel before $M.~
OthersFound $n grovels in the dirt before $N.~
VictFound   $n grovels in the dirt before you.~
CharAuto    That seems a little silly to me.~
OthersAuto  $n starts groveling to $mself, I think there might be a problem here.~
End

#SOCIAL
Name        growl~
CharNoArg   Grrrrrrrrrr ...~
OthersNoArg $n growls.~
CharFound   Grrrrrrrrrr ... take that, $N!~
OthersFound $n growls at $N.  Better leave the room before the fighting starts.~
VictFound   $n growls at you.  Hey, two can play it that way!~
CharAuto    You growl at yourself.  Boy, do you feel bitter!~
OthersAuto  $n growls at $mself.  This could get interesting...~
End

#SOCIAL
Name        grumble~
CharNoArg   You grumble.~
OthersNoArg $n grumbles.~
CharFound   You grumble to $M.~
OthersFound $n grumbles to $N.~
VictFound   $n grumbles to you.~
CharAuto    You grumble under your breath.~
OthersAuto  $n grumbles under $s breath.~
End

#SOCIAL
Name        grumpy~
CharNoArg   You grumpy.~
OthersNoArg $n is feeling very grumpy.~
CharFound   You make $N aware of just how grumpy you are.~
OthersFound $n grumps at $N.~
VictFound   You suddenly realize how grumpy $n is.~
CharAuto    You grump at yourself. Satisfied?~
OthersAuto  $n grumps at $mself. Better steer clear.~
End

#SOCIAL
Name        grunt~
CharNoArg   GRNNNHTTTT.~
OthersNoArg $n grunts like a pig.~
CharFound   GRNNNHTTTT.~
OthersFound $n grunts to $N.  What a pig!~
VictFound   $n grunts to you.  What a pig!~
CharAuto    GRNNNHTTTT.~
OthersAuto  $n grunts to nobody in particular.  What a pig!~
End

#SOCIAL
Name        gulp~
CharNoArg   You gulp nervously and loosen your neckwear.~
OthersNoArg $n gulps nervously and loosens $s neckwear.~
CharFound   You loosen your neckwear and gulp nervously under $N's gaze.~
OthersFound $n loosens $s neckwear and gulps nervously at $N.~
VictFound   $n loosens $s neckwear and gulps nervously at you.~
CharAuto    You gulp nervously and loosen your neckwear.~
OthersAuto  $n gulps nervously and loosens $s neckwear.~
End

#SOCIAL
Name        hail~
CharNoArg   You raise your sword high above your head and yell 'Hail!'~
OthersNoArg $n raises $s sword high above $s head and yells 'Hail!'~
CharFound   You raise your sword high above your head and yell 'Hail $N!'~
OthersFound $n raises $s sword high above $s head and yells 'Hail $N!'~
VictFound   $n hails you with $s sword and yells 'Hail $N!'~
CharAuto    You raise your sword high above your head and yell 'Hail...ME!!'~
OthersAuto  $n raises $s sword high above $s head and yells 'Hail ME!!'~
End

#SOCIAL
Name        hair~
CharNoArg   You run your fingers through your hair.~
OthersNoArg $n runs $s fingers through $s hair, how vain!~
CharFound   You run your fingers through $N's hair.~
OthersFound $n runs $s fingers through $N's hair, how sweet.~
VictFound   $n runs $s fingers through your hair soothingly.~
CharAuto    Having a bad hair day?~
OthersAuto  $n seems to be having a bad hair day.~
End

#SOCIAL
Name        hand~
CharNoArg   Kiss whose hand?~
CharFound   You kiss $S hand.~
OthersFound $n kisses $N's hand.  How continental!~
VictFound   $n kisses your hand.  How continental!~
CharAuto    You kiss your own hand.~
OthersAuto  $n kisses $s own hand.~
End

#SOCIAL
Name        happy~
CharNoArg   You look happy! =)~
OthersNoArg $n looks happy! Awww whatta chum!~
CharFound   You force $N to yell, 'HAPPY HAPPY JOY JOY!!'~
OthersFound $n forces $N to yell, 'HAPPY HAPPY JOY JOY!!'~
VictFound   $n forces you to yell, 'HAPPY HAPPY JOY JOY!!'~
CharAuto    You yell, 'HAPPY HAPPY JOY JOY!!'~
OthersAuto  $n yells, 'HAPPY HAPPY JOY JOY!!' Don't mind $m, $e's a bit looney.~
End

#SOCIAL
Name        hay~
CharNoArg   You look around the room and ask everyone, "How are you?"~
OthersNoArg $n looks around the room and asks everyone, "How are you?"~
CharFound   You look at $N and ask "How are you?"~
OthersFound $n looks at $N and asks "How are you?"~
VictFound   $n looks at you and asks, "How are you?"~
End

#SOCIAL
Name        heal~
CharNoArg   You start yelling for a heal!~
OthersNoArg $n yells 'Hey, how about a heal? I'm DYING here!'~
CharFound   You start yelling at $N for a heal!~
OthersFound $n yells 'Hey $N, how about a heal? I'm DYING here!'~
VictFound   $n yells 'Hey $N, how about a heal? I'm DYING here!'~
CharAuto    You start yelling for a heal!~
OthersAuto  $n yells 'Hey, how about a heal? I'm DYING here!'~
End

#SOCIAL
Name        heh~
CharNoArg   You seem to get a laugh from something saying, "heh heh heh".~
OthersNoArg $n seems to get a laugh from something saying, "heh heh heh".~
CharFound   You seem to get a laugh from $N and say, "heh heh heh".~
OthersFound $n seems to get a laugh from $N and says, "heh heh heh".~
VictFound   $n seems to get a laugh from you and says, "heh heh heh".~
End

#SOCIAL
Name        hello~
CharNoArg   You say hello to everyone in the room.~
OthersNoArg $n says hello to everyone in the room.~
CharFound   You tell $N how truly glad you are to see $M.~
OthersFound $n tells $N 'Hi!'~
VictFound   $n tells you how truly glad $e is that you are here.~
CharAuto    You greet yourself enthusiastically.~
OthersAuto  $n greets $mself enthusiastically.  How odd.~
End

#SOCIAL
Name        hercules~
CharNoArg   You strut around, acting like Hercules.~
OthersNoArg $n struts around, acting like $e is Hercules.~
CharFound   You pose for $N, acting like you are Hercules.~
OthersFound $n poses for $N, acting like $e is Hercules.~
VictFound   $n poses for you, acting like $e is Hercules.~
CharAuto    You think you are Hercules!~
OthersAuto  $n seems to think $e's Hercules.~
End

#SOCIAL
Name        hfive~
CharNoArg   You jump into the air and attempt to give no one a high five, and land flat on your face! Ouch!~
OthersNoArg $n jumps in the air attempting to give an invisible person a high five! Whatta fool!~
CharFound   You jump into the air and give $N a MEGA high five! Woo hoo!~
OthersFound $n jumps into the air and gives $N a MEGA high five!~
VictFound   $n jumps into the air and gives you a MEGA high five! Woo hoo!~
CharAuto    You jump into the air, and perform the ever exciting triple flip, half twist, piked, high five with yourself!~
OthersAuto  $n jumps into the air, and for the lack of anything else to do, high fives $mself!~
End

#SOCIAL
Name        hfoot~
CharNoArg   You clasp your feet behind your back.~
OthersNoArg $n clasps $s feet behind $s back.~
CharFound   You hold $N's foot in your hand.~
OthersFound $n holds $N's foot in $s hand.~
VictFound   $n holds your foot in $s hand. Awwww ... isn't $e sweet?~
End

#SOCIAL
Name        hhand~
CharNoArg   You clasp your hands behind your back.~
OthersNoArg $n clasps $s hands behind $s back.~
CharFound   You take $N's hand in yours.~
OthersFound $n holds $N's hand. Awwww aren't they cute?~
VictFound   $n holds your hand lovingly.~
CharAuto    You clasp your hands behind your back.~
OthersAuto  $n clasps $s hands behind $s back.~
End

#SOCIAL
Name        hiccup~
CharNoArg   You hiccup loudly.~
OthersNoArg $n hiccups loudly.~
CharFound   You hiccup loudly at $M.  How rude!~
OthersFound $n hiccups at $N. How rude!~
VictFound   $n hiccups at you.  How rude!~
CharAuto    You hiccup to yourself.  How musical.~
OthersAuto  $n hiccups to $mself.  How musical.~
End

#SOCIAL
Name        hidecrime~
CharNoArg   You try to make it look like an accident.~
OthersNoArg $n tries to make it look like an accident.~
CharFound   You ask $N to help you hide the evidence.~
OthersFound $n asks $N for help to hide the evidence.~
VictFound   $n asks you if you can help $m hide the evidence.~
CharAuto    You can't fit the evidence in your pocket.~
OthersAuto  $n desperately tries to hide the evidence in $s pocket.~
End

#SOCIAL
Name        hiss~
CharNoArg   You bare your teeth and hiss spitefully.~
OthersNoArg $n bares $s teeth and hisses spitefully.~
CharFound   You hiss spitefully at $N through bared teeth.~
OthersFound $n hisses spitefully at $N through bared teeth.~
VictFound   $n hisses spitefully at you through bared teeth.~
CharAuto    You bare your teeth and hiss spitefully.~
OthersAuto  $n bares $s teeth and hisses spitefully.~
End

#SOCIAL
Name        hmm~
CharNoArg   You Hmmmm out loud.~
OthersNoArg $n thinks, 'Hmmmm.'~
CharFound   You gaze thoughtfully at $M and say 'Hmmm.'~
OthersFound $n gazes thoughtfully at $N and says 'Hmmm.'~
VictFound   $n gazes thoughtfully at you and says 'Hmmm.'~
CharAuto    You Hmmmm out loud.~
OthersAuto  $n thinks, 'Hmmmm.'~
End

#SOCIAL
Name        homage~
CharNoArg   You look around for someone to pay homage to.~
OthersNoArg $n looks around for someone to pay homage to.~
CharFound   You pay homage to $N.~
OthersFound $n pays homage to $N.~
VictFound   $n acknowledges your superiority.~
CharAuto    You look for someone to proclaim your superior.~
OthersAuto  $n looks around for someone to pay homage to.~
End

#SOCIAL
Name        honey~
CharNoArg   You spread honey all over your body. Who will lick it off?~
OthersNoArg $n spreads honey all over $s body. Who will lick it off?~
CharFound   You look at $N with a wild grin as you spread honey all over $S body.~
OthersFound $n looks at $N with a wild grin as $e spreads honey all over $S body.~
VictFound   $n looks at you with a wild grin as $e spreads honey all over your body.~
CharAuto    You feeling a little lonely tonight?~
OthersAuto  $n is spreading honey all over $mself trying to get your attention.~
End

#SOCIAL
Name        honk~
CharNoArg   You pull out a huge hankie and honk your nose loudly.~
OthersNoArg $n pulls out a huge hankie and honks $s nose loudly.~
CharFound   You pull out a large hankie and honk your nose at $N.~
OthersFound $n pulls out a large hankie and honks $s nose at $N.~
VictFound   $n pulls out a large hankie and honks $s nose at you.~
CharAuto    You want to honk yourself? Sounds illegal.~
OthersAuto  $n tries to honk $mself. I'd be leery of $n.~
End

#SOCIAL
Name        hop~
CharNoArg   You hop around like a frog.~
OthersNoArg $n hops around like a frog.~
CharFound   You hop onto $N's head. Youch!~
OthersFound $n hops onto $N's head. Youch!~
VictFound   $n hops right on top of your head. Ouchie Wouchies!!!~
CharAuto    You hop around like a little kid.~
OthersAuto  $n hops around like a little kid.~
End

#SOCIAL
Name        how~
CharNoArg   You ask, "how?"~
OthersNoArg $n asks, "how?"~
CharFound   You ask $N, "how?"~
OthersFound $n asks $N, "how?"~
VictFound   $n asks you, "how?"~
CharAuto    You ask yourself, "how could this happen to me?"~
OthersAuto  $n asks $mself, "how could this happen to me?"~
End

#SOCIAL
Name        hrm~
CharNoArg   You hrm. Hrm.~
OthersNoArg You hear $n hrm.~
CharFound   You hrm in the direction of $N. Hrm!~
OthersFound $n hrms at $N, $e isn't too normal.~
VictFound   $n hrms in your general direction. Is that normal?~
CharAuto    You hrm yourself into submission.~
OthersAuto  $n hrms quietly. $e must be in deep thought.~
End

#SOCIAL
Name        hscratch~
CharNoArg   You scratch your head, puzzled.~
OthersNoArg $n scratches $s head.~
CharFound   You scratch $N's head.~
OthersFound $n scratches $N's head.~
VictFound   $n scratches your head.~
CharAuto    Aaaah.~
OthersAuto  $n looks relieved as $e scratches $s head.~
End

#SOCIAL
Name        hshake~
CharNoArg   You look for someone to shake hands with.~
OthersNoArg $n looks in vain for someone to shake hands with.~
CharFound   You shake $N's hand.~
OthersFound $n shakes $N's hand.~
VictFound   $n shakes your hand.~
CharAuto    You shake hands with yourself.~
OthersAuto  $n shakes hands with $mself.~
End

#SOCIAL
Name        hthink~
CharNoArg   You close your eyes and think happy thoughts.~
OthersNoArg $n closes $s eyes and starts muttering happy thoughts, happy thoughts.~
CharFound   You pat $N on $S back and say "Remember, happy thoughts."~
OthersFound $n pats $N on $S back and says, "Remember, Happy Thoughts. Think of your happy place."~
VictFound   $n reminds you to think "Happy Thoughts!"~
CharAuto    You look around with a goofy smile and mutter, "Must think Happy Thoughts."~
OthersAuto  $n is wandering around with a goofy look on $s face whispering "Happy Thoughts" to no one in particular.~
End

#SOCIAL
Name        hug~
CharNoArg   Hug whom?~
OthersNoArg $n is looking for someone to hug, $e must be lonesome.~
CharFound   You hug $M.~
OthersFound $n hugs $N.~
VictFound   $n hugs you.~
CharAuto    You hug yourself.~
OthersAuto  $n hugs $mself in a vain attempt to get friendship.~
End

#SOCIAL
Name        huh~
CharNoArg   huh?~
OthersNoArg $n huhs? huh?~
CharFound   You huh $N. Huh?~
OthersFound $n huhs $N. Huh?~
VictFound   $n huhs you. Huh?~
End

#SOCIAL
Name        hum~
CharNoArg   Hmm Hmm Hmm Hmmmmmmm.~
OthersNoArg $n hums like a bee with a chest cold.~
CharFound   You hum a little ditty for $M.  Hmm Hmm Hmm Hmmmmmm.~
OthersFound $n hums a little ditty for $N.  Hmm Hmm Hmm Hmmmmmm.~
VictFound   $n hums a little ditty for you.  Hmm Hmm Hmm Hmmmmmm.~
CharAuto    Hmm Hmm Hmmmmmmm.~
OthersAuto  $n hums like a bee with a chest cold.~
End

#SOCIAL
Name        ick~
CharNoArg   You ick.~
OthersNoArg $n icks!~
CharFound   You look at $N and go 'ick'!~
OthersFound $n looks at $N and goes 'ick'!~
VictFound   $n looks at you and goes 'ick'!~
End

#SOCIAL
Name        idiot~
CharNoArg   You grin like an idiot!~
OthersNoArg $n grins like an idiot!~
CharFound   You look at $N and grin like an idiot!~
OthersFound $n looks at $N and grins like an idiot!~
VictFound   $n looks at you and grins like an idiot!~
End

#SOCIAL
Name        inlove~
CharNoArg   You get this glazed look in your eye and announce, 'I'm in love!!!'~
OthersNoArg $n has a really goofy look on $s face, $e must be in love!~
CharFound   You gaze into $N's eyes and tell $M "I'm so in love with you!"~
OthersFound $n gazes into $N's eyes and tells $M, 'I'm in love!'~
VictFound   $n gazes into your eyes and tells you "$N, I'm so in love with you!"~
CharAuto    You are so in love with yourself, it's disgusting really.~
OthersAuto  $n proclaims, I am so in love with.....well, ME!!!~
End

#SOCIAL
Name        innocent~
CharNoArg   You do your best to look utterly innocent.~
OthersNoArg $n looks innocently about $mself.~
CharFound   You do your best to convince $N of your innocence.~
OthersFound $n does $s best to convince $N of $s innocence.~
VictFound   $n gives you the most innocent look you have ever seen.~
CharAuto    You try to convince yourself of your innocence.~
OthersAuto  $n does $s best to prove $s innocence to all, especially $mself.~
End

#SOCIAL
Name        itch~
CharNoArg   You feel an itch where you can't scratch.~
OthersNoArg $n feels an itch where $e can't scratch.~
CharFound   You feel an itch and ask $N to give it a scratch.~
OthersFound $n feels an itch and asks $N to give it a scratch.~
VictFound   $n feels an itch and asks you to give it a scratch. ~
End

#SOCIAL
Name        jealous~
CharNoArg   You are jealous.~
OthersNoArg $n is jealous.~
CharFound   You are jealous of $N for stealing your love.~
OthersFound $n is jealous of $N for stealing $s love.~
VictFound   $n is jealous of you for stealing $s love.~
CharAuto    You are jealous of your own studliness.~
OthersAuto  $n is jealous of $mself, what a dork.~
End

#SOCIAL
Name        jest~
CharNoArg   You smirk at nothing, and say 'Just kidding!' Time to log off?~
OthersNoArg $n smirks at nothing, and says 'Just kidding!' Time for $m to log off.~
CharFound   You smirk at $N, and say 'Just kidding!'~
OthersFound $n smirks at $N, and says 'Just kidding!'~
VictFound   $n smirks at you, and says 'Just kidding!'~
CharAuto    You smirk at yourself, and say 'Just kidding!' By Thoric, you've lost it.~
OthersAuto  $n smirks at $mself, and says 'Just kidding!' By Thoric, $e has lost it.~
End

#SOCIAL
Name        juggle~
CharNoArg   You whip out your flaming torches and begin juggling them madly before the group of awed onlookers!~
OthersNoArg $n whips out $s flaming torches and begins to juggle them. Wow. What skill, what grace!~
CharFound   You whip out your flaming torches and begin to juggle for $N. Won't $E think you're great NOW?  ~
OthersFound $n whips out a bunch of flaming torches to begin $s juggling act. You wonder if this room is non-flammable. Hmmm.~
VictFound   $n gets a crazy look in $s eyes as $e pulls out some flaming torches and begins to juggle. You look around for a fire-extinguisher.~
CharAuto    Feeling very proud of yourself, you begin to juggle three VERY sharp swords. No one can beat you now. NO ONE. Mwahahahaha..~
OthersAuto  $n pulls out some VERY sharp swords and begins to juggle them with a crazed look in $s eyes. Maybe its time to leave ... ~
End

#SOCIAL
Name        kiss~
CharNoArg   Isn't there someone you want to kiss?~
OthersNoArg $n looks for someone to kiss.~
CharFound   You kiss $M.~
OthersFound $n kisses $N.~
VictFound   $n kisses you.~
CharAuto    All the lonely people :(~
OthersAuto  $n tries to kiss $mself.  $e's lonely!~
End

#SOCIAL
Name        kitchie~
CharNoArg   You make a funny face and say, "kitchie kitchie koo".~
OthersNoArg $n makes a funny face and says, "kitchie kitchie koo".~
CharFound   You tickle $N saying, "kitchie kitchie koo"~
OthersFound $n tickles $N saying, "kitchie kitchie koo"~
VictFound   $n tickles you saying, "kitchie kitchie koo".~
End

#SOCIAL
Name        kitten~
CharNoArg   You meow like a kitten.~
OthersNoArg $n meows like a kitten.~
CharFound   You love $N's little kitten. Awwww.~
OthersFound $n loves $N's little kitten. Awwww.~
VictFound   $n loves your little kitten. Awwww.~
CharAuto    You think that you are a cute little kitten. Meeeeow.~
OthersAuto  $n thinks $e is a cute little kitten. Meeeeow.~
End

#SOCIAL
Name        kneel~
CharNoArg   You kneel down.~
OthersNoArg $n kneels down.~
CharFound   You kneel before $N.~
OthersFound $n kneels before $N.~
VictFound   $n kneels before you.~
CharAuto    You drop to your knees.~
OthersAuto  $n kneels down.~
End

#SOCIAL
Name        knight~
CharNoArg   Who shall you proclaim to be Knight?~
OthersNoArg $n glances about the room looking for someone to knight.~
CharFound   You place your blade upon the shoulders of $N and proclaim $M a royal Knight.~
OthersFound $n places $s blade upon the shoulders of $N and proclaims $M a royal Knight.~
VictFound   $n, by royal proclamation, declares you a royal Knight.~
CharAuto    Perhaps you think too highly of yourself?~
OthersAuto  $n struts about the room declaring $Mself a Knight.~
End

#SOCIAL
Name        koochie~
CharNoArg   You make a funny face and say, "koochie koochie koo".~
CharFound   You tickle $N saying, "koochie koochie koo".~
OthersFound $n tickles $N saying, "koochie koochie koo".~
VictFound   $n tickles you saying, "koochie koochie koo".~
End

#SOCIAL
Name        lag~
CharNoArg   You complain about the terrible lag.~
OthersNoArg $n starts complaining about the terrible lag.~
CharFound   You complain to $N about the terrible lag.~
OthersFound $n complains to $N about the terrible lag.~
VictFound   $n complains to you about the terrible lag.~
CharAuto    You start muttering about the awful lag.~
OthersAuto  $n starts muttering about the awful lag.~
End

#SOCIAL
Name        lalala~
CharNoArg   You try to sing. Best you can manage is 'la la la'.~
OthersNoArg $n goes 'la la la'~
CharFound   You croon to $N 'la la la'.~
OthersFound $n croons to $N 'la la la'.~
VictFound   $n croons to you 'la la la'.~
CharAuto    You want to 'la la la' yourself?~
End

#SOCIAL
Name        lap~
CharNoArg   You look around for a cuddly lap to climb into.~
OthersNoArg $n is looking around for a cuddly lap to climb into.~
CharFound   You climb into $N's lap and cuddle up with $M.~
OthersFound $n climbs into $N's lap and cuddles up with $M.~
VictFound   $n climbs into your lap and cuddles up with you.~
CharAuto    You try to climb into your own lap and cuddle with yourself.  Are you lonely?~
OthersAuto  $n looks awful funny trying to crawl up into $s own lap and cuddle.~
End

#SOCIAL
Name        laugh~
CharNoArg   You laugh.~
OthersNoArg $n laughs.~
CharFound   You laugh at $N mercilessly.~
OthersFound $n laughs at $N mercilessly.~
VictFound   $n laughs at you mercilessly.  Hmmmmph.~
CharAuto    You laugh at yourself.  I would, too.~
OthersAuto  $n laughs at $mself.  Let's all join in!!!~
End

#SOCIAL
Name        ldead~
CharNoArg   You try to plug yourself back in.  Who cut the power?~
OthersNoArg $n runs around looking for an outlet to plug $mself into.~
CharFound   You look at $N as you attempt to find an outlet for $S Link-Dead body.~
OthersFound $n looks at $N's Link-Dead body and wishes for $S link to come alive.~
VictFound   $n looks at you with a wicked grin as $e approaches you with a power cord.~
CharAuto    Are you really that bored ?~
OthersAuto  $n is wandering around looking for an outlet for $mself.~
End

#SOCIAL
Name        leer~
CharNoArg   You begin to leer at nothing in particular.~
OthersNoArg $n leers around $m. ~
CharFound   You leer at $N with pure lust.~
OthersFound $n leers at $N unabashedly.~
VictFound   $n makes you feel vulnerable with $s unabashed leer.~
CharAuto    You leer at yourself and think "mmmmmmm, I look good!"~
OthersAuto  $n leers at $mself and smiles.~
End

#SOCIAL
Name        liar~
CharNoArg   You shout "Liar Liar - Pants on fire!"~
OthersNoArg $n shouts "Liar Liar - Pants on fire!"~
CharFound   You shout at $N  -  "Liar Liar - Pants on fire!"~
OthersFound $n shouts at $N  -  "Liar Liar - Pants on fire!"~
VictFound   $n shouts at you  -  "Liar Liar - Pants on fire!"~
End

#SOCIAL
Name        lick~
CharNoArg   You lick your lips and smile.~
OthersNoArg $n licks $s lips and smiles.~
CharFound   You lick $M.~
OthersFound $n licks $N.~
VictFound   $n licks you.~
CharAuto    You lick yourself.~
OthersAuto  $n licks $mself - YUCK.~
End

#SOCIAL
Name        lol~
CharNoArg   You laugh out loud!~
OthersNoArg $n laughs out loud!~
CharFound   You laugh out loud at $N!~
OthersFound $n laughs out loud at $N!~
VictFound   $n laughs out loud at you!~
CharAuto    You laugh out loud at yourself, feeling ok?~
OthersAuto  $n laughs out loud at $mself, maybe you should leave $m alone..~
End

#SOCIAL
Name        loom~
CharNoArg   You loom menacingly, those around you scramble for cover.~
OthersNoArg $n looms menacingly, better run for cover!~
CharFound   You loom menacingly over $N, now $E's scared!~
OthersFound $n looms menacingly over $N, thank goodness it isn't you.~
VictFound   $n looms menacingly over you.  Ooooooo.~
CharAuto    Now stop that.  Self-loomination is impossible.~
End

#SOCIAL
Name        love~
CharNoArg   You love the whole world.~
OthersNoArg $n loves everybody in the world.~
CharFound   You tell your true feelings to $N.~
OthersFound $n whispers softly to $N.~
VictFound   $n whispers to you sweet words of love.~
CharAuto    Well, we already know you love yourself (lucky someone does!)~
OthersAuto  $n loves $mself, can you believe it?~
End

#SOCIAL
Name        lust~
CharNoArg   You are getting lusty feelings!~
OthersNoArg $n looks around lustily.~
CharFound   You stare lustily at $N.~
OthersFound $n stares lustily at $N.~
VictFound   $n stares lustily at you.~
CharAuto    You stare lustily at...youself?~
OthersAuto  $n looks $mself up and down lustily.~
End

#SOCIAL
Name        mad~
CharNoArg   A crazed look of insanity spreads slowly over your face.~
OthersNoArg A crazed look of insanity spreads slowly over $n's face.~
CharFound   You turn to $N and posit, "You're quite mad, you know?"~
OthersFound $n inquires of $N as to whether $E is aware that $E is mentally ill.~
VictFound   $n inquires of you as to whether you are aware that you are mentally ill.~
CharAuto    If you think you're crazy now, wait till you figure out you're talking to yourself.~
OthersAuto  $n mulls $s own ill mental health.~
End

#SOCIAL
Name        maim~
CharNoArg   Who do you want to maim?~
OthersNoArg $n is looking for someone to maim.~
CharFound   You maim $M with your dull fingernails.~
OthersFound $n raises $s hand and tries to maim $N to pieces.~
VictFound   $n raises $s hand and paws at you.  You've been maimed!~
CharAuto    You maim yourself with your dull fingernails.~
OthersAuto  $n raises $s hand and maims $mself to pieces.~
End

#SOCIAL
Name        mambo~
CharNoArg   You dance a wild mambo. Ooh la la!~
OthersNoArg $n dances a wild mambo. Ooh. la la!~
CharFound   You grab $N and dance a wild mambo with $M. Ooh. la. la!~
OthersFound $n grabs $N and dances a wild mambo with $M. Ooh. la. la!~
VictFound   $n grabs you and dances a wild mambo with you. Ooh. la. la!~
End

#SOCIAL
Name        manners~
CharNoArg   You wonder where people get their manners from these days.~
OthersNoArg $n wonders where people get their manners from these days.~
CharFound   You wonder where $N learned $S manners.~
OthersFound $n wonders where $N learned $S manners.~
VictFound   $n wonders where you learned your manners.~
End

#SOCIAL
Name        marshmallow~
CharNoArg   $n looks around fer a nice, fat, squishy, MARSHMALLOW to stick in $s hot chocolate!~
OthersNoArg $n looks around fer a nice, fat, squishy, MARSHMALLOW to stick in $s hot chocolate!~
CharFound   You point at $N and pinching $S widdle, cute cheek, say 'Mmm you look as scrumptious as a marshmallow!'~
OthersFound $n thinks $N looks like a widdle, cute, marshmallow! Don't you agree? Awwwww~
VictFound   $n points at you and pinches your widdle, cute cheek, and says 'Mmm you look as scrumptious as a marshmallow!'~
CharAuto    You wrap your body up into a little ball and dive into the hot chocolate yelling, 'Marshmallows away!!!'~
OthersAuto  $n wraps $s body up into a little ball and dives into the hot chocolate yelling, 'Marshmallows away!!!'~
End

#SOCIAL
Name        massage~
CharNoArg   Massage what?  Thin air?~
CharFound   You gently massage $N's shoulders.~
OthersFound $n massages $N's shoulders.~
VictFound   $n gently massages your shoulders.  Ahhhhhhhhhh ...~
CharAuto    You practice yoga as you try to massage yourself.~
OthersAuto  $n gives a show on yoga positions, trying to massage $mself.~
End

#SOCIAL
Name        mean~
CharNoArg   You look meaner than a junkyard dog!~
OthersNoArg $n looks meaner than a junkyard dog!~
CharFound   You give $N the meanest look $E has ever seen!~
OthersFound $n gives $N the meanest look $E has ever seen!~
VictFound   $n gives you the meanest look you have ever seen!~
End

#SOCIAL
Name        melt~
CharNoArg   Your heart begins to melt as you think of someone special.~
OthersNoArg $n begins to melt before your very eyes.~
CharFound   You have melted into $S arms!~
OthersFound $n appears to melt into $N's arms! Must be love ...~
VictFound   $n melts at the thought of you!~
CharAuto    You begin to melt.~
OthersAuto  $n is ranting like a lunatic.~
End

#SOCIAL
Name        men~
CharNoArg   You throw your hands in the air and say "BAH! Do men even think?!'~
OthersNoArg $n wonders why men were ever invented.~
CharFound   You give $N an understanding look and say "At least they're good for opening cans."~
OthersFound $n commiserates with $N about the existence of men.~
VictFound   $n says "Men should come with warning labels" to you.~
CharAuto    You wish you could trade men for EQ.~
OthersAuto  $n wishes $e could trade men for EQ.~
End

#SOCIAL
Name        meow~
CharNoArg   MEOW.~
OthersNoArg $n meows.  What's $e going to do next, wash $mself with $s tongue?~
CharFound   You meow at $M, hoping $E will give you some milk.~
OthersFound $n meows at $N, hoping $E will give $m some milk. ~
VictFound   $n meows at you.  Maybe $e wants some milk.~
CharAuto    You meow like a kitty cat.~
OthersAuto  $n meows like a kitty cat.~
End

#SOCIAL
Name        milk~
CharNoArg   You pour yourself a glass of cold creamy milk and drink it down.~
OthersNoArg $n pours $mself a glass of cold creamy milk and gulps it down.~
CharFound   You pour a glass of cold creamy milk for $N and give it to $M.~
OthersFound $n pours a glass of cold creamy milk and gives it to $N.~
VictFound   $n pours a glass of cold creamy milk and hands it to you.~
CharAuto    You hoist a glass of cold creamy milk and gulp it down.~
OthersAuto  $n hoists a glass of cold frothy milk and gulps it down.~
End

#SOCIAL
Name        miss~
CharNoArg   Your heart pines for a special someone that you miss so much. Awwww.~
OthersNoArg $n's heart pines for a special someone $e misses so much. Awwwww.~
CharFound   You tell $N that your heart has been aching cuz you've missed $M so much. Awww.~
OthersFound $n tells $N that $s heart has been aching cuz $e missed $M so much. Awww.~
VictFound   $n tells you that $s heart has been aching cuz $e missed you so much. Awww.~
End

#SOCIAL
Name        mistletoe~
CharNoArg   You point at the mistletoe above your head, and pucker up.~
OthersNoArg $n points to the mistletoe above $s head, and puckers up.~
CharFound   You point to the mistletoe above $N's head, puckering up.~
OthersFound $n points to the mistletoe above $N's head, and puckers up.~
VictFound   $n points to the mistletoe above your head, puckering up.~
CharAuto    You point at the mistletoe above your head, and pucker up.~
OthersAuto  $n points to the mistletoe above $s head, and puckers up.~
End

#SOCIAL
Name        mmm~
CharNoArg   You go mmMMmmMMmmMMmm.~
OthersNoArg $n says 'mmMMmmMMmmMMmm.'~
CharFound   You go mmMMmmMMmmMMmm.~
OthersFound $n says 'mmMMmmMMmmMMmm.'~
VictFound   $n thinks of you and says, 'mmMMmmMMmmMMmm.'~
CharAuto    You think of yourself and go mmMMmmMMmmMMmm.~
OthersAuto  $n thinks of $mself and says 'mmMMmmMMmmMMmm.'~
End

#SOCIAL
Name        moan~
CharNoArg   You start to moan.~
OthersNoArg $n starts moaning.~
CharFound   You moan at the sight of $N.~
OthersFound $n moans at the sight of $N.~
VictFound   $n moans at the sight of you.~
CharAuto    You moan at yourself.~
OthersAuto  $n makes $mself moan.~
End

#SOCIAL
Name        moi~
CharNoArg   You look demure and ask, "moi?"~
OthersNoArg $n looks demure and asks, "moi?"~
CharFound   You look at $N demurely and ask, "moi?"~
OthersFound $n looks demurely at $N and asks, "moi?"~
VictFound   $n looks at you demurely and asks, "moi?"~
End

#SOCIAL
Name        monkey~
CharNoArg   You run around looking for a tree to swing on...maybe a banana.~
OthersNoArg $n is looking for a tree to swing on...maybe a banana.~
CharFound   You climb all over $N like a love-struck monkey.~
OthersFound $n is climbing all over $N like a love-struck monkey.~
VictFound   $n climbs all over you like a love-struck monkey.~
CharAuto    You make a monkey out of yourself.~
OthersAuto  $n is making a monkey out of $mself.~
End

#SOCIAL
Name        moocow~
CharNoArg   You Moo like a cow.~
OthersNoArg $n moos like a cow.~
CharFound   You look at $N and say, "Mooooooo".~
OthersFound $n looks at $N and says, "Mooooooo".~
VictFound   $n looks at you and says, "Moooooooo".~
CharAuto    You make cow noises.  Mooooooooooooooooooo!~
OthersAuto  $n Mooooooooooooooooooooooooos like a cow.~
End

#SOCIAL
Name        moon~
CharNoArg   You howl at the moon. Is that fur on your knuckles?~
OthersNoArg $n howls at the moon. Is that fur on $s knuckles?~
CharFound   You howl at $N. Feeling a little feral are we?~
OthersFound $n howls at $N. Could be that $e is feeling a bit feral?~
VictFound   $n howls at you. Could $e be feeling a bit feral?~
CharAuto    Howling to yourself? The moon does seem a bit bright tonight..~
OthersAuto  $n is howling to $mself.  Must be a bright moon tonight..~
End

#SOCIAL
Name        muffin~
CharNoArg   You bite into a big warm muffin. Yummy!~
OthersNoArg $n bites into a big warm muffin. Yummy!~
CharFound   You give $N a big warm muffin. Yummy!~
OthersFound $n gives $N a big warm muffin. Yummy!~
VictFound   $n gives you a big warm muffin. Yum! Yum! Yummy!~
End

#SOCIAL
Name        muhaha~
CharNoArg   You laugh diabolically.  MUHAHAHAHAHAHA!.~
OthersNoArg $n laughs diabolically.  MUHAHAHAHAHAHA!..~
CharFound   You laugh at $M diabolically.  MUHAHAHAHAHAHA!..~
OthersFound $n laughs at $N diabolically.  MUHAHAHAHAHAHA!..~
VictFound   $n laughs at you diabolically.  MUHAHAHAHAHAHA!..~
CharAuto    Muhaha at yourself??  Weird.~
End

#SOCIAL
Name        mumble~
CharNoArg   You mumble incoherently.~
OthersNoArg $n is mumbling under $s breath.~
CharFound   You mumble to $N, hoping $E will listen.~
OthersFound $n is mumbling something to $N.~
VictFound   $n looks right at you and starts mumbling.  Uh-oh.~
CharAuto    You mumble to yourself.~
OthersAuto  $n is mumbling something to $mself.~
End

#SOCIAL
Name        mutter~
CharNoArg   You mutter distractedly.~
OthersNoArg $n mutters distractedly.~
CharFound   You mutter to yourself and shake your head at $M.~
OthersFound $n mutters distractedly and shakes $s head at $N.~
VictFound   $n mutters distractedly and shakes $s head at you.~
CharAuto    You mutter dithyrambically to yourself.~
OthersAuto  $n mutters at $mself.~
End

#SOCIAL
Name        nag~
CharNoArg   You feel nagged.~
OthersNoArg $n feels nagged.~
CharFound   You nag $N. Nag $M like a hag!~
OthersFound $n nags $N. Nags $M like a hag!~
VictFound   $n nags you. Nags you like a hag!~
End

#SOCIAL
Name        nail~
CharNoArg   You nibble nervously on your nails.~
OthersNoArg $n nibbles nervously on $s fingernails.~
CharFound   You nibble nervously on your nails.~
OthersFound $n nibbles nervously on $s fingernails.~
VictFound   $n nibbles nervously on your fingernails.  Yuck!~
CharAuto    You nibble nervously on your nails.~
OthersAuto  $n nibbles nervously on $s fingernails.~
End

#SOCIAL
Name        ngreet~
CharNoArg   You nod in greeting to everyone present.~
OthersNoArg $n nods in greeting to everyone present.~
CharFound   You nod in greeting towards $N.~
OthersFound $n nods in greeting towards $N.~
VictFound   $n nods in greeting towards you.~
CharAuto    You nod to yourself, muttering something about rude people.~
OthersAuto  $n nods to $mself, muttering under $s breath.~
End

#SOCIAL
Name        nibble~
CharNoArg   Nibble on whom?~
CharFound   You nibble on $N's ear.~
OthersFound $n nibbles on $N's ear.~
VictFound   $n nibbles on your ear.~
CharAuto    You nibble on your OWN ear.~
OthersAuto  $n nibbles on $s OWN ear.~
End

#SOCIAL
Name        nocomment~
CharNoArg   You are now config +nocomment.~
OthersNoArg $n slaps $s hands over $s mouth to avoid commenting.~
CharFound   You decide it's best for your health not to comment on $M.~
OthersFound $n looks at $N and says, 'No comment!'~
VictFound   $n isn't even going to attempt to comment on your actions.~
CharAuto    You feel it's best not to make a comment about yourself at this time.~
OthersAuto  $n is now config +nocomment.~
End

#SOCIAL
Name        nod~
CharNoArg   You nod solemnly.~
OthersNoArg $n nods solemnly.~
CharFound   You nod in agreement to $M.~
OthersFound $n nods in agreement to $N.~
VictFound   $n nods in agreement with you.~
CharAuto    You nod at yourself.  Are you getting senile?~
OthersAuto  $n nods to $mself. It appears $e often converses with $mself.~
End

#SOCIAL
Name        nog~
CharNoArg   You nog yourself!~
OthersNoArg $n nogs $mself.~
CharFound   You nog $N.~
OthersFound $n nogs $N.~
VictFound   $n nogs you.~
CharAuto    You nog yourself.  Pervert!~
OthersAuto  $n nogs $mself. What a pervert!~
End

#SOCIAL
Name        noogie~
CharNoArg   You noogie.~
CharFound   You grind your knuckles into the top of $N's head.~
OthersFound $n grinds $s knuckles into the top of $N's head.~
VictFound   $n grinds $s knuckles into the top of your head with glee.~
CharAuto    Doesn't that hurt?~
OthersAuto  $n grinds $s knuckles into $s head, oblivious to the pain.~
End

#SOCIAL
Name        nose~
CharNoArg   You wiggle your nose.~
OthersNoArg $n wiggles $s nose.~
CharFound   You tweak $S nose.~
OthersFound $n tweaks $N's nose.~
VictFound   $n tweaks your nose.~
CharAuto    You tweak your own nose!~
OthersAuto  $n tweaks $s own nose!~
End

#SOCIAL
Name        nudge~
CharNoArg   Nudge whom?~
CharFound   You nudge $M.~
OthersFound $n nudges $N.~
VictFound   $n nudges you.~
CharAuto    You nudge yourself, for some strange reason.~
OthersAuto  $n nudges $mself, to keep $mself awake.~
End

#SOCIAL
Name        nuzzle~
CharNoArg   Nuzzle whom?~
CharFound   You nuzzle $S neck softly.~
OthersFound $n softly nuzzles $N's neck.~
VictFound   $n softly nuzzles your neck.~
CharAuto    I'm sorry, friend, but that's impossible.~
End

#SOCIAL
Name        ogle~
CharNoArg   Whom do you want to ogle?~
CharFound   You ogle $M like $E was a piece of meat.~
OthersFound $n ogles $N.  Maybe you should leave them alone for awhile?~
VictFound   $n ogles you.  Guess what $e is thinking about?~
CharAuto    You ogle yourself.  You may just be too weird for this mud.~
OthersAuto  $n ogles $mself.  Better hope that $e stops there.~
End

#SOCIAL
Name        ohno~
CharNoArg   Oh no!  You did it again!~
OthersNoArg Oh no!  $n did it again!~
CharFound   You exclaim to $M, 'Oh no!  I did it again!'~
OthersFound $n exclaims to $N, 'Oh no!  I did it again!'~
VictFound   $n exclaims to you, 'Oh no!  I did it again!'~
CharAuto    You exclaim to yourself, 'Oh no!  I did it again!'~
OthersAuto  $n exclaims to $mself, 'Oh no!  I did it again!'~
End

#SOCIAL
Name        oink~
CharNoArg   Ooooooink! You're such a pig!~
OthersNoArg $n is acting like a pig!~
CharFound   You look at $N and bellow 'Oooooink!'~
OthersFound $n looks at $N and bellows 'Ooooink!'~
VictFound   $n looks at you and bellows 'Ooooink!'  $e must think you are a pig!~
CharAuto    You start thinking that you are acting like a pig, stop that!~
OthersAuto  $n thinks that $e is acting like a pig!~
End

#SOCIAL
Name        ooo~
CharNoArg   You go ooOOooOOooOOoo.~
OthersNoArg $n says, 'ooOOooOOooOOoo.'~
CharFound   You go ooOOooOOooOOoo.~
OthersFound $n says, 'ooOOooOOooOOoo.'~
VictFound   $n thinks of you and says, 'ooOOooOOooOOoo.'~
CharAuto    You go ooOOooOOooOOoo.~
OthersAuto  $n says, 'ooOOooOOooOOoo.'~
End

#SOCIAL
Name        oops~
CharNoArg   You put your finger in your mouth and say, "Oopsie! Did I do that?"~
OthersNoArg $n puts $s finger in $s mouth and says, "Oopsie! Did I do that?"~
CharFound   You look at $N and say, "Oopsie! Did I do that to you $N?"~
OthersFound $n looks at $N and says, "Oopsie! Did I do that to $N?"~
VictFound   $n looks at you and says, "Oopsie! Did I do that to you $N?"~
CharAuto    You want to oops yourself? Sounds demented.~
OthersAuto  $n wants to oops $mself. $n must be delusional.~
End

#SOCIAL
Name        ouch~
CharNoArg   You say 'ouchie wouchies'!~
OthersNoArg $n says 'ouchie wouchies'!~
CharFound   You look at $N and think 'ouchie wouchies'!~
OthersFound $n looks at $N and thinks 'ouchie wouchies'!~
VictFound   $n looks at you and thinks 'ouchie wouchies'!~
End

#SOCIAL
Name        owl~
CharNoArg   You turn your head around in a circle and ask, "Who? Who? Who?"~
OthersNoArg $n turns $s head around in a circle and asks "Who? Who? Who?"~
CharFound   You turn your head around in a circle and ask $N "Who? Who? Who?"~
OthersFound $n turns $s head around in a circle and asks "Who? Who? Who?"~
VictFound   $n turns $s head around in a circle and asks you, "Who? Who? Who?"~
End

#SOCIAL
Name        pace~
CharNoArg   You pace around the room in agitation.~
OthersNoArg $n paces around the room with an agitated expression.~
CharFound   You pace around $N, wearing a path around $M.~
OthersFound $n paces around $N, wearing a rather large path .~
VictFound   $n paces around you. It's starting to make your head spin.~
CharAuto    You pace around yourself, making yourself quite dizzy.~
OthersAuto  $n paces around $mself, looking rather dizzy.~
End

#SOCIAL
Name        panic~
CharNoArg   You run around in circles, screaming.~
OthersNoArg $n runs around in circles, screaming.~
CharFound   You panic at the sight of $N.~
OthersFound $n panics at the sight of $N.~
VictFound   $n panics at the sight of you.~
CharAuto    You feel a panic welling up inside you...~
OthersAuto  $n runs screaming into a wall...~
End

#SOCIAL
Name        pant~
CharNoArg   You begin to pant loudly and sloppily.~
OthersNoArg $n begins to pant loudly.~
CharFound   You begin to pant loudly at $N.~
OthersFound $n begins to pant loudly at $N.~
VictFound   $n begins to pant loudly at you.~
End

#SOCIAL
Name        pants~
CharNoArg   You remove your pants.~
OthersNoArg $n removes $s pants.~
CharFound   You politely ask $N to remove $S pants.~
OthersFound $n politely asks $N to remove $S pants.~
VictFound   $n politely asks you to remove your pants and any other non-essential garments.~
CharAuto    You hastily put on your pants.~
OthersAuto  $n hastily puts on $s pants.~
End

#SOCIAL
Name        party~
CharNoArg   You wonder where the party is.~
OthersNoArg $n is searching for a party to crash.~
CharFound   You ask $N if $E wants to party with you.~
OthersFound $n asks $N if $E wants to party.~
VictFound   $n asks you to party with $m.~
CharAuto    You get down with your bad self.~
OthersAuto  $n gets down with $s bad self.~
End

#SOCIAL
Name        passout~
CharNoArg   You totter for a bit, then fall flat on your face.~
OthersNoArg $n totters a bit and passes out flat on $s face.~
CharFound   You pass out cold, toppling on to $N.  Poor sot.~
OthersFound $n topples on to $N, passed out cold.  Poor sot.~
VictFound   $n topples on to you, passed out cold.  Poor sot.~
CharAuto    You try to catch yourself as you pass out, but fail miserably.~
OthersAuto  $n tries to catch $mself as $e passes out, but fails miserably.~
End

#SOCIAL
Name        pat~
CharNoArg   Pat whom?~
CharFound   You pat $N on $S back.~
OthersFound $n pats $N on $S back.~
VictFound   $n pats you on your back.~
CharAuto    You pat yourself on your back.~
OthersAuto  $n pats $mself on the back.~
End

#SOCIAL
Name        pbeckon~
CharNoArg   Psst, c'mon.  Anyone?~
OthersNoArg $n beckons to the air.~
CharFound   You beckon $N follow you through rain, snow, dragons' lairs and even *gulp* lag.~
OthersFound $n beckons $N follow $m.  You smell a rat.~
VictFound   $n beckons you follow $m through rain, snow, dragons' lairs and even *gulp* lag.~
CharAuto    Psst, c'mon $n~
OthersAuto  $n beckons $n follow $n.  Ooooooook.~
End

#SOCIAL
Name        peck~
CharNoArg   You peck for seeds on the ground.~
OthersNoArg $n pecks for seeds on the ground.~
CharFound   You give $M a little peck on the cheek.~
OthersFound $n gives $N a small peck on the cheek.~
VictFound   $n gives you a sweet peck on the cheek.~
CharAuto    You kiss your own pectoral muscles.~
OthersAuto  $n pecks $mself on $s pectoral muscles.~
End

#SOCIAL
Name        peer~
CharNoArg   You peer intently about your surroundings.~
OthersNoArg $n peers intently about the area, looking for thieves no doubt.~
CharFound   You peer at $M quizzically.~
OthersFound $n peers at $N quizzically.~
VictFound   $n peers at you quizzically.~
CharAuto    You peer intently about your surroundings.~
OthersAuto  $n peers intently about the area, looking for thieves no doubt.~
End

#SOCIAL
Name        pembrace~
CharNoArg   Who do you want to hold?~
OthersNoArg $n looks around for someone to embrace.~
CharFound   You hold $M in a loving firm embrace, never wanting to let go.~
OthersFound $n holds $N in a loving embrace as if $e will never let go.~
VictFound   $n holds you in a firm loving embrace, letting you know that $e'll never let you go.~
CharAuto    You hug yourself lovingly. Hrmmmm..~
OthersAuto  $n holds $mself trying to keep the pieces together. Must have been a hard fight.~
End

#SOCIAL
Name        perk~
CharNoArg   You perk right up!~
OthersNoArg $n looks perky now!~
CharFound   You look at $M and shout, "Perk up!"~
OthersFound $n looks at $N and shouts, "Perk up!"~
VictFound   $n looks at you and shouts, "Perk up!"~
CharAuto    Yep, you're right perky now.~
OthersAuto  $n makes an effort to perk $mself up.~
End

#SOCIAL
Name        pet~
CharNoArg   You want to pet yourself? Pervert!~
OthersNoArg $n wants to engage in an act of perversion.~
CharFound   You gently pet $N.~
OthersFound $n gently pets $N.~
VictFound   $n pets you.~
CharAuto    Pet yourself? Shame on you.~
OthersAuto  $n is a pervert.~
End

#SOCIAL
Name        pff~
CharNoArg   You purse your lips and go 'pffffffth!!!!!'~
OthersNoArg $n purses $s lips and goes 'pffffffth!!!!!'~
CharFound   You look at $N and go 'pffffffth!!!!!'~
OthersFound $n looks at $N and goes 'pffffffth!!!!!'~
VictFound   $n looks at you and goes 'pffffffth!!!!!'~
End

#SOCIAL
Name        phew~
CharNoArg   Phew! That was too close for words...~
OthersNoArg $n wipes $s brow with obvious relief.~
CharFound   You share your obvious relief with $M.~
OthersFound $n glances at $N, a look of obvious relief on $s face.~
VictFound   $n glances at you, a look of obvious relief on $s face.~
CharAuto    You mutter to yourself in obvious relief, wiping your brow.~
OthersAuto  $n mutters in obvious relief, wiping $s brow.~
End

#SOCIAL
Name        pickon~
CharNoArg   You want to pick on someone.  All the choices!~
OthersNoArg Uh oh! $n is looking for someone to pick on! Run!!~
CharFound   You start picking on $N. Meanie!~
OthersFound $n starts picking on $N. What a meanie!~
VictFound   $n starts picking on you. Some people are just mean!~
CharAuto    You want to pick on yourself? Okay.. nutzo!~
OthersAuto  $n starts picking on $mself. What a nut!~
End

#SOCIAL
Name        pig~
CharNoArg   You shake your head, thinking 'What a pig'~
OthersNoArg $n shakes $s head, obviously offended by someone's piggish behavior.~
CharFound   You shake your head at $N, offended by $S piggish behavior.~
OthersFound $n says 'Pig!', and snorts in disgust at $N's behaviour.~
VictFound   $n says 'Pig!', and snorts in disgust at your behaviour.~
CharAuto    You make a pig of yourself! Have you no shame?~
OthersAuto  $n makes a pig of $mself. Has $e no shame?~
End

#SOCIAL
Name        pillow~
CharNoArg   You pull out a fluffy pillow and glance around the room.~
OthersNoArg $n pulls out a fluffy pillow and looks for a target.~
CharFound   You deliver a devastating hit with your fluffy pillow.~
OthersFound $n delivers a devastating hit to $N with $s fluffy pillow.~
VictFound   $n hits you with $s fluffy pillow.~
CharAuto    You pull out a fluffy pillow and sit down on it.~
OthersAuto  $n pulls out a fluffy pillow and sits down on it.~
End

#SOCIAL
Name        pinch~
CharNoArg   You pinch yourself to see if you're really awake!~
OthersNoArg $n prepares to pinch someone.~
CharFound   You pinch $N playfully..awww. ~
OthersFound $n pinches $N! Must be some strange courting ritual...~
VictFound   $n pinches your cheeks and smiles.~
CharAuto    You pinch yourself and find that it wasn't a good idea. Ow!~
OthersAuto  $n pinches $mself. Now that's lonely!~
End

#SOCIAL
Name        pizza~
CharNoArg   You pop a slice in the oven till it's bubbily delicious.~
OthersNoArg $n pops a slice in the oven till it's bubbily delicious.~
CharFound   You pull a slice out of the oven and give it to $N.~
OthersFound $n pulls a slice out of the oven and gives it to $N.~
VictFound   $n pulls a slice out of the oven and gives it to you.~
CharAuto    Your pull a slice from the oven and pop it in your mouth. Yummy!~
OthersAuto  $n pulls a slice from the oven and pops it into $s mouth. Yummy!~
End

#SOCIAL
Name        please~
CharNoArg   You are very pleased with yourself.~
OthersNoArg $n is very pleased with $mself.~
CharFound   You beg $N, "Please Please Please".~
OthersFound $n gets down on $s knees and begs $N, "Please Please Please".~
VictFound   $n gets down on $s knees and begs you, "Please Please Please".~
End

#SOCIAL
Name        point~
CharNoArg   Point at whom?~
CharFound   You point at $M accusingly.~
OthersFound $n points at $N accusingly.~
VictFound   $n points at you accusingly.~
CharAuto    You point proudly at yourself.~
OthersAuto  $n points proudly at $mself.~
End

#SOCIAL
Name        poke~
CharNoArg   Poke whom?~
CharFound   You poke $M in the ribs.~
OthersFound $n pokes $N in the ribs.~
VictFound   $n pokes you in the ribs.~
CharAuto    You poke yourself in the ribs, feeling very silly.~
OthersAuto  $n pokes $mself in the ribs, looking very sheepish.~
End

#SOCIAL
Name        ponder~
CharNoArg   You ponder the question.~
OthersNoArg $n sits down and thinks deeply.~
End

#SOCIAL
Name        pooky~
CharNoArg   You exclaim "Hello snooky pooky"~
OthersNoArg $n exclaims "Hello snooky pooky"~
CharFound   You exclaim to $N "Hello snooky pooky"~
OthersFound $n exclaims to $N "Hello snooky pooky"~
VictFound   $n exclaims to you "Hello snooky pooky"~
End

#SOCIAL
Name        possum~
CharNoArg   You do your best imitation of a corpse.~
OthersNoArg $n hits the ground... DEAD.~
CharFound   You do your best imitation of a corpse.~
OthersFound $n hits the ground... DEAD.~
VictFound   $n hits the ground... DEAD.~
CharAuto    You do your best imitation of a corpse.~
OthersAuto  $n hits the ground... DEAD.~
End

#SOCIAL
Name        pounce~
CharNoArg   Pounce on whom?~
OthersNoArg $n is looking for someone to pounce on.~
CharFound   You pounce on $N, pinning $M to the ground.~
OthersFound $n pounces on $N, pinning $M to the ground.~
VictFound   $n pounces on you, pinning you to the ground.~
CharAuto    You try pouncing on yourself, but it doesn't quite work.~
OthersAuto  $n tries to pounce on $mself, but it doesn't quite work.~
End

#SOCIAL
Name        pout~
CharNoArg   Ah, don't take it so hard.~
OthersNoArg $n pouts.~
CharFound   You pout at the way $N is treating you.~
OthersFound $n pouts at the way $e is being treated by $N.~
VictFound   $n pouts at the way you are treating $m.~
End

#SOCIAL
Name        pray~
CharNoArg   You feel righteous, and maybe a little foolish.~
OthersNoArg $n begs and grovels to the powers that be.~
CharFound   You crawl in the dust before $M.~
OthersFound $n falls down and grovels in the dirt before $N.~
VictFound   $n kisses the dirt at your feet.~
CharAuto    Talk about narcissism ...~
OthersAuto  $n mumbles a prayer to $mself.~
End

#SOCIAL
Name        prod~
CharNoArg   You peer around, seeing if anyone needs prodding.~
OthersNoArg $n peers around, seeing if anyone needs prodding.~
CharFound   You prod $N in the ribs, and $E enjoys it!~
OthersFound $n prods $N in the ribs, and $E enjoys it!~
VictFound   $n prods you in the ribs!~
CharAuto    You prod yourself in the ribs.  Get to work!~
OthersAuto  $n prods $mself in the chest.  He should really get on with his tasks!~
End

#SOCIAL
Name        propose~
CharNoArg   You look around for someone to marry.~
OthersNoArg $n desperately seeks someone to marry.~
CharFound   You fall onto your knees, and beg $N to marry you!~
OthersFound $n drops to $s knees and begs $N to take $s hand in marriage!~
VictFound   $n drops to one knee and begs you to become $s significant other for all eternity!~
CharAuto    You love yourself so much that you would marry yourself if you could.~
OthersAuto  $n loves $mself far too much.~
End

#SOCIAL
Name        proud~
CharNoArg   You're quite proud of everyone here.~
OthersNoArg $n is quite proud of everyone's accomplishments.~
CharFound   You sigh as you think how much $N has grown since you met $M.~
OthersFound $n seems to admire the way $N has handled $Mself.~
VictFound   $n is exceptionally proud of you.~
CharAuto    You're quite proud of yourself, and deserve a promotion.~
OthersAuto  $n is quite proud of $mself and seems to expect something for it.~
End

#SOCIAL
Name        pucker~
CharNoArg   You try to look your cutest and pucker up.~
OthersNoArg $n tries to look $s cutest and puckers up. Any takers?~
CharFound   You move a little closer to $N and pucker up.~
OthersFound $n inches $s way over to $N and puckers up.~
VictFound   $n inches $s way over to you and puckers up.~
End

#SOCIAL
Name        puke~
CharNoArg   You make loud retching noises and puke on the ground.~
OthersNoArg $n makes loud retching noises and pukes on the ground.~
CharFound   You make loud retching noises and puke on $N's shoes. Ewwwww!~
OthersFound $n makes loud retching noises and pukes on $N's shoes. Ewwww!~
VictFound   $n makes loud retching noises and pukes on your shoes. Ewwwww!~
CharAuto    After emptying your stomach you look down and see that you are ankle deep in puke!~
OthersAuto  $n emptied $s stomach here and has left the room ankle deep in puke!~
End

#SOCIAL
Name        puppyeyes~
CharNoArg   You hope someone notices you.~
OthersNoArg $n is hoping someone notices $s cute puppy-dog eyes.~
CharFound   You give $N your best puppy-dog eyes.~
OthersFound $n is giving $N $s best puppy-dog eyes.~
VictFound   $n is looking at you with $s best puppy-dog eyes.~
CharAuto    You practice your puppy-dog eyes. Hmmm. Better practice some more.~
OthersAuto  $n is making funny faces to $mself. You try your best to ignore $m.~
End

#SOCIAL
Name        purr~
CharNoArg   MMMMEEEEEEEEOOOOOOOOOWWWWWWWWWWWW.~
OthersNoArg $n purrs contentedly.~
CharFound   You purr contentedly in $S lap.~
OthersFound $n purrs contentedly in $N's lap.~
VictFound   $n purrs contentedly in your lap.~
CharAuto    You purr at yourself.~
OthersAuto  $n purrs at $mself.  Must be a cat thing.~
End

#SOCIAL
Name        puzzled~
CharNoArg   But didn't....and then... how puzzling.~
OthersNoArg $n looks truly puzzled.~
CharFound   You quirk a brow at $N, puzzled.~
OthersFound $n looks at $N with a puzzled frown on $s face.~
VictFound   $n looks puzzled by your news.~
CharAuto    Puzzling, isn't it?~
OthersAuto  $n is puzzled by $mself. Aren't we all?~
End

#SOCIAL
Name        qexit~
CharNoArg   You glance about looking for a quick exit.~
OthersNoArg $n glances about, obviously looking for a quick exit.~
CharFound   You say 'Time for a quick exit!' and run out the nearest door.~
OthersFound $n says 'Time for a quick exit!' and runs out the nearest door.~
VictFound   $n says 'Time for a quick exit!' and runs out the nearest door.~
CharAuto    You want to run away from yourself? FREAK!~
OthersAuto  $n says 'Time for a quick exit!' and tries to run away from $mself!~
End

#SOCIAL
Name        quiver~
CharNoArg   You quiver in anticipation of physical delights.~
OthersNoArg $n quivers in anticipation of physical delights.~
CharFound   You quiver all over thinking of $N and $S physical delights.~
OthersFound $n quivers uncontrollably thinking of $N and $S physical delights.~
VictFound   $n quivers uncontrollably thinking of you and your physical delights.~
End

#SOCIAL
Name        raise~
CharNoArg   You raise your hand in response.~
OthersNoArg $n raises $s hand in response.~
CharFound   You raise your hand in response.~
OthersFound $n raises $s hand in response.~
VictFound   $n raises $s hand in response to you.~
CharAuto    You raise your hand in response.~
OthersAuto  $n raises $s hand in response.~
End

#SOCIAL
Name        rampage~
CharNoArg   You rampage merrily.~
OthersNoArg $n rampages merrily.~
CharFound   You rampage all over $N in a very merry manner.~
OthersFound $n rampages all over $N in a very merry manner.~
VictFound   $n rampages all over you in a very merry manner.~
CharAuto    You rampage all over yourself.~
OthersAuto  $n rampages all over $mself.~
End

#SOCIAL
Name        rant~
CharNoArg   You begin to rant and rave like a lunatic..~
OthersNoArg $n begins to rant and rave like a lunatic..~
CharFound   You rant and rave because of $N's actions.~
OthersFound $N rants and raves because of $n's actions.~
VictFound   $n rants and raves because of something you did.~
CharAuto    You rant and rave at yourself. You ok?~
OthersAuto  $n rants and raves at $mself.~
End

#SOCIAL
Name        ready~
CharNoArg   You dig down into the dirt, ready to do battle.~
OthersNoArg $n digs down into the dirt, obviously ready to do battle.~
CharFound   You nod, alerting $N that this foe must now die miserably.~
OthersFound $n nods, alerting $N that their unlucky foe must now die miserably.~
VictFound   $n nods, signaling you that your foe must now die miserably.~
CharAuto    You give yourself a neat motivational speech.~
OthersAuto  $n withdraws into the clutches of some sort of motivational mantra.~
End

#SOCIAL
Name        remember~
CharNoArg   Isn't there something you should be doing about now?~
OthersNoArg $n scratches $s head as $e tries to remember something....~
CharFound   Yeah.  You remember $N.~
OthersFound $n tries to remember if $e has ever met $N before.~
VictFound   $n gets a puzzled look on $s face as $e try to remember who you are.~
CharAuto    Oh yea!  Your....  Wait, just who are you again?~
OthersAuto  $n just can't seem to remember who $e is.  Must be getting senile.~
End

#SOCIAL
Name        repop~
CharNoArg   You begin to squeak like a mouse, hoping for a contagion.~
OthersNoArg $n begins to squeak like a mouse.  Straaange.~
CharFound   Sorry, $N won't be able to conjure squeakies any faster than you.~
OthersFound $n begs $N to conjure some squeakies.  Shrink recommendations?~
VictFound   $n grovels at your boots, drooling rabidly in hopes of squeakies.~
CharAuto    Not even a mouse...~
OthersAuto  $n desperately scours the room in pursuit of those elusive squeakies.~
End

#SOCIAL
Name        ridicule~
CharNoArg   You feel very ridiculous.~
OthersNoArg $n feels very sheepish.~
CharFound   You point and laugh at $N.  What a geek.~
OthersFound $n falls down laughing, pointing at $N.~
VictFound   $n is pointing and laughing at you.~
CharAuto    You feel rather foolish, and grin sheepishly.~
OthersAuto  Feeling foolish, $n laughs sheepishly at $self.~
End

#SOCIAL
Name        roar~
CharNoArg   You ROAR like a dragon.~
OthersNoArg $n ROARS with a ferocity that shakes the earth!~
CharFound   You ROAR in $N's face.~
OthersFound $n ROARS at $N. (Obviously this is the part where you should feel intimidated)~
VictFound   As $n roars in your face you curse the fact that toothpaste is hundreds of years away from being invented.~
CharAuto    You ROARRRRRR!~
OthersAuto  $n ROARS loudly to establish $s dominance.~
End

#SOCIAL
Name        rofl~
CharNoArg   You roll on the floor laughing hysterically.~
OthersNoArg $n rolls on the floor laughing hysterically.~
CharFound   You laugh your head off at $S remark.~
OthersFound $n rolls on the floor laughing at $N's remark.~
VictFound   $n can't stop laughing at your remark.~
CharAuto    You roll on the floor and laugh at yourself.~
OthersAuto  $n laughs at $mself.  Join in the fun.~
End

#SOCIAL
Name        roll~
CharNoArg   You roll your eyes.~
OthersNoArg $n rolls $s eyes.~
CharFound   You roll your eyes at $M.~
OthersFound $n rolls $s eyes at $N.~
VictFound   $n rolls $s eyes at you.~
CharAuto    You roll your eyes at yourself.~
OthersAuto  $n rolls $s eyes at $mself.~
End

#SOCIAL
Name        rub~
CharNoArg   You rub your eyes.  How long have you been at this?~
OthersNoArg $n rubs $s eyes.  $n must have been playing all day.~
CharFound   You rub your eyes.  Has $N been playing as long as you have?~
OthersFound $n rubs $s eyes.  $n must have been playing all day.~
VictFound   $n rubs $s eyes.  Have you been playing as long as $m?~
CharAuto    You rub your eyes.  How long have you been at this?~
OthersAuto  $n rubs $s eyes.  $n must have been playing all day.~
End

#SOCIAL
Name        ruffle~
CharNoArg   You've got to ruffle SOMEONE.~
CharFound   You ruffle $N's hair playfully.~
OthersFound $n ruffles $N's hair playfully.~
VictFound   $n ruffles your hair playfully.~
CharAuto    You ruffle your hair.~
OthersAuto  $n ruffles $s hair.~
End

#SOCIAL
Name        rumpshake~
CharNoArg   You start shakin' your rump to the beat.~
OthersNoArg $n gets carried away by the vibes and starts shakin' $s rump.~
CharFound   You tell $N to start shakin' $S rump!~
OthersFound $n tells $N to start shakin' $S rump, must be a party.~
VictFound   $n starts shakin' $s rump at you.~
CharAuto    You start shakin' your rump.~
OthersAuto  $n starts shakin' $s rump, $e must have the music in $m.~
End

#SOCIAL
Name        runaway~
CharNoArg   You scream 'RUN AWAY! RUN AWAY!'.~
OthersNoArg $n screams 'RUN AWAY! RUN AWAY!'.~
CharFound   You scream '$N, QUICK! RUN AWAY!'.~
OthersFound $n screams '$N, QUICK! RUN AWAY!'.~
VictFound   $n screams '$N, QUICK! RUN AWAY!'.~
CharAuto    You desperately look for somewhere to run to!~
OthersAuto  $n looks like $e's about to run away.~
End

#SOCIAL
Name        sad~
CharNoArg   You put on a glum expression.~
OthersNoArg $n looks particularly glum today.  *sniff*~
CharFound   You give $M your best glum expression.~
OthersFound $n looks at $N glumly.  *sniff*  Poor $n.~
VictFound   $n looks at you glumly.  *sniff*   Poor $n.~
CharAuto    You bow your head and twist your toe in the dirt glumly.~
OthersAuto  $n bows $s head and twists $s toe in the dirt glumly.~
End

#SOCIAL
Name        salute~
CharNoArg   You salute smartly.~
OthersNoArg $n salutes smartly.~
CharFound   You salute $M.~
OthersFound $n salutes $N.~
VictFound   $n salutes you.~
CharAuto    Huh?~
End

#SOCIAL
Name        sappy~
CharNoArg   You start gagging at all the love in the room.~
OthersNoArg $n starts gagging at all the love in the room.~
CharFound   You look at $N and start gagging at $S sappy behavior~
OthersFound $n gags at $N's lovey dovey sappy behavior~
VictFound   $n look at you and gags at your sappy behavior~
End

#SOCIAL
Name        scare~
CharNoArg   You tiptoe about the room, looking for someone to scare.~
OthersNoArg $n starts walking about the room on $s tiptoes, perhaps trying to be a ballerina?~
CharFound   You walk quietly up behind $N and yell, 'BOO!'.~
OthersFound $n tiptoes up beind $N and yells, 'BOO!'~
VictFound   $n taps you on your shoulder from behind and yells, 'BOO!'~
CharAuto    Ahhhhhh! Don't scare yourself like that!~
OthersAuto  $n starts screaming at $mself. The voices must have returned inside $s head.~
End

#SOCIAL
Name        scowl~
CharNoArg   You scowl angrily.~
OthersNoArg $n scowls angrily.~
CharFound   You scowl angrily at $M.~
OthersFound $n scowls angrily at $N.~
VictFound   $n scowls angrily at you.~
CharAuto    You scowl angrily at yourself.~
OthersAuto  $n scowls angrily at $mself.~
End

#SOCIAL
Name        scratch~
CharNoArg   You scratch yourself ... ahhhhhh!~
OthersNoArg $n scratches $mself .... ahhhhh!~
CharFound   You scratch $N where $E can't reach. You are a good friend.~
OthersFound $n scratches $N where $E can't reach. $n is a good friend.~
VictFound   $n scratches you where you can't reach. $n is a good friend~
End

#SOCIAL
Name        scream~
CharNoArg   ARRRRRRRRRRGH!!!!!~
OthersNoArg $n screams loudly!~
CharFound   ARRRRRRRRRRGH!!!!!  Yes, it MUST have been $S fault!!!~
OthersFound $n screams loudly at $N.  Better leave before $n blames you, too!!!~
VictFound   $n screams at you!  That's not nice!  *sniff*~
CharAuto    You scream at yourself.  Yes, that's ONE way of relieving tension!~
OthersAuto  $n screams loudly at $mself!  Is there a full moon up?~
End

#SOCIAL
Name        serenade~
CharNoArg   You raise your clear voice towards the sky.~
OthersNoArg $n has begun to sing.~
CharFound   You sing a ballad to $M.~
OthersFound $n sings a ballad to $N.~
VictFound   $n sings a ballad to you!  How sweet!~
CharAuto    You sing a little ditty to yourself.~
OthersAuto  $n sings a little ditty to $mself.~
End

#SOCIAL
Name        shake~
CharNoArg   You shake your head.~
OthersNoArg $n shakes $s head.~
CharFound   You shake your head in response to $N's question.~
OthersFound $n shakes $s head in $N's direction.~
VictFound   $n shakes $s head in response to your question.~
CharAuto    You are shaken by yourself.~
OthersAuto  $n shakes and quivers like a bowl full of jelly.~
End

#SOCIAL
Name        shame~
CharNoArg   You are ashamed of yourself.~
OthersNoArg $n is ashamed of $mself.~
CharFound   You point at $N and say, "shame! shame!"~
OthersFound $n points at $N and says, "shame! shame!"~
VictFound   $n points at you and says, "shame! shame!"~
End

#SOCIAL
Name        shhh~
CharNoArg   You frown and say, "Shhhhhh"~
OthersNoArg $n asks everyone to please be quiet.~
CharFound   You tell $N to be quiet.~
OthersFound $n attempts to make $N close $S mouth in the interest of peace.~
VictFound   $n has asked you to please be quiet.~
CharAuto    You remind yourself to shaddup.~
OthersAuto  $n oddly tells $mself to be quiet. Those darn voices again.~
End

#SOCIAL
Name        shiver~
CharNoArg   Brrrrrrrrr.~
OthersNoArg $n shivers uncomfortably.~
CharFound   You shiver at the thought of fighting $M.~
OthersFound $n shivers at the thought of fighting $N.~
VictFound   $n shivers at the suicidal thought of fighting you.~
CharAuto    You shiver to yourself?~
OthersAuto  $n scares $mself to shivers.~
End

#SOCIAL
Name        shoo~
CharNoArg   You shoo away the imaginary fairies.~
OthersNoArg $n starts shooing away imaginary fairies.. hmmmm.~
CharFound   You tell $N to shoo, g'way.~
OthersFound $n tells $N to shoo!~
VictFound   $n tells you to shoo, guess $e wants to be alone.~
CharAuto    You tell yourself to shoo, g'way!~
OthersAuto  $n tells $mself to go away.. odd..~
End

#SOCIAL
Name        shrug~
CharNoArg   You shrug.~
OthersNoArg $n shrugs helplessly.~
CharFound   You shrug in response to $S question.~
OthersFound $n shrugs in response to $N's question.~
VictFound   $n shrugs in response to your question.~
CharAuto    You shrug to yourself.~
OthersAuto  $n shrugs to $mself.  What a strange person.~
End

#SOCIAL
Name        shudder~
CharNoArg   Your body shivers in uncontrollable revulsion.~
OthersNoArg $n convulses as $e shudders in disgust.~
CharFound   Just looking at $N makes you want to wretch!~
OthersFound $n shudders in disgust at $N!~
VictFound   $n shudders in disgust at your wretched behavior!~
CharAuto    You disgust yourself.~
OthersAuto  $n shudders with repulsion as $e thinks of $s behavior.~
End

#SOCIAL
Name        sigh~
CharNoArg   You sigh.~
OthersNoArg $n sighs loudly.~
CharFound   You sigh as you think of $M.~
OthersFound $n sighs at the sight of $N.~
VictFound   $n sighs as $e thinks of you.  Touching, huh?~
CharAuto    You sigh at yourself.  You MUST be lonely.~
OthersAuto  $n sighs at $mself.  What a sorry sight.~
End

#SOCIAL
Name        silly~
CharNoArg   You announce that you are just a big old silly.~
OthersNoArg $n announces that $e is just a big old silly.~
CharFound   You think $N is a big old silly.~
OthersFound $n thinks $N is a big old silly.~
VictFound   $n thinks you are a big old silly.~
End

#SOCIAL
Name        sing~
CharNoArg   You sing a lovely song.~
OthersNoArg $n sings a lovely song.~
CharFound   You look at $N and begin to sing a lovely song to $M.~
OthersFound $n looks at $N and begins to sing a lovely song to $M.~
VictFound   $n looks at you and begins to sing a lovely song to you.~
CharAuto    You begin to sing a lovely song to yourself.  That should lift your spirits.~
OthersAuto  $n begins to sing a lovely song to $mself. Maybe $e needs an audience?~
End

#SOCIAL
Name        skiss~
CharNoArg   You make kissing noises.~
OthersNoArg $n makes kissing noises.~
CharFound   You gently take $N's face into your hands and kiss $M softly.~
OthersFound $n gently takes $N's face into $s hands and kisses $M softly.~
VictFound   $n gently takes your face into $s hands and kisses you softly.~
CharAuto    You blow kisses at yourself in a mirror.~
OthersAuto  $n blows kisses at $mself in a mirror.~
End

#SOCIAL
Name        slap~
CharNoArg   Slap whom?~
OthersNoArg $n raises $s hand spastically as if to slap an unseen annoyance.~
CharFound   You rear back and slap $M with all your might.~
OthersFound $n rears back and slaps $N for $S stupidity.~
VictFound   $n rears back and slaps you cruelly for your stupidity.  OUCH!~
CharAuto    You slap yourself.  You deserve it.~
OthersAuto  $n slaps $mself.  Why don't you join in?~
End

#SOCIAL
Name        slaughter~
CharNoArg   Wielding a massive cleaver, you prowl for unsuspecting meat.~
OthersNoArg $n grips a massive cleaver tightly, grinning mischievously.~
CharFound   Your cleaver swings carelessly as you chop $N into itsy bitsy pieces.~
OthersFound $n's cleaver cuts through the air, then through $N... repeatedly.~
VictFound   That's gonna leave a mark.~
CharAuto    Chop, chop, chop.  Ouch, ouch, ouch.~
OthersAuto  A-chopping $n will go...~
End

#SOCIAL
Name        sleeve~
CharNoArg   You look around for a sleeve to tug.~
OthersNoArg $n looks around for a sleeve to tug, maybe $e needs some attention?~
CharFound   You gently tug on $N's sleeve, trying to get $S attention.~
OthersFound $n gently tugs on $N's sleeve, trying to get their attention.~
VictFound   $n gently tugs on your sleeve, looking hopeful.~
CharAuto    You gently tug your own sleeve, perhaps you should concentrate?~
OthersAuto  $n tugs $s own sleeve, weird eh?~
End

#SOCIAL
Name        slime~
CharNoArg   Slime who?~
CharFound   You dump a bucket of slime all over $N. How cavalier!~
OthersFound $n dumps a bucket of slime all over $N. How cavalier!~
VictFound   $n dumps a bucket of slime all over you. Eeeeeeeew! How gross!~
CharAuto    You want to slime yourself? Don't be silly!~
End

#SOCIAL
Name        slobber~
CharNoArg   You slobber all over the floor.~
OthersNoArg $n slobbers all over the floor.~
CharFound   You slobber all over $M.~
OthersFound $n slobbers all over $N.~
VictFound   $n slobbers all over you.~
CharAuto    You slobber all down your front.~
OthersAuto  $n slobbers all over $mself.~
End

#SOCIAL
Name        slurp~
CharNoArg   Slurp! Slurp!~
OthersNoArg $n slurps $s drink noisily.~
CharFound   You slurp $N!~
OthersFound $n takes $N's face into $s hands and slurps $M! ~
VictFound   $n grabs ahold of your face and slurps you!~
CharAuto    That's really not possible.~
OthersAuto  $n tries to do something not physically possible.~
End

#SOCIAL
Name        smashey~
CharNoArg   You exclaim 'Smashey Smashey!'~
OthersNoArg $n exclaims 'Smashey Smashey!'~
CharFound   You smash $N.~
OthersFound $n smashes $N.~
VictFound   $n smashes you.~
CharAuto    But that would hurt!~
OthersAuto  $n considers hurting $mself.~
End

#SOCIAL
Name        smile~
CharNoArg   You smile happily.~
OthersNoArg $n smiles happily.~
CharFound   You smile at $M.~
OthersFound $n beams a smile at $N.~
VictFound   $n smiles at you.~
CharAuto    You smile at yourself.~
OthersAuto  $n smiles at $mself.~
End

#SOCIAL
Name        smirk~
CharNoArg   You smirk.~
OthersNoArg $n smirks.~
CharFound   You smirk at $S saying.~
OthersFound $n smirks at $N's saying.~
VictFound   $n smirks at your saying.~
CharAuto    You smirk at yourself.  Okay ...~
OthersAuto  $n smirks at $s own 'wisdom'.~
End

#SOCIAL
Name        smite~
CharNoArg   You are in the mood to smite someone.~
OthersNoArg $n is in a smiting mood. Runaway!!!~
CharFound   You attempt to smite $N!~
OthersFound $n attempts to SMITE $N with a single mighty blow!~
VictFound   $n smites you with a single blow!~
CharAuto    Smiting oneself is not permitted by the Gods.~
OthersAuto  $n tries to smite $mself. Fortunately, the Gods intervened just in time.~
End

#SOCIAL
Name        smooch~
CharNoArg   You are searching for someone to smooch.~
OthersNoArg $n is looking for someone to smooch.~
CharFound   You give $M a nice, wet smooch.~
OthersFound $n and $N are smooching in the corner.~
VictFound   $n smooches you passionately on the lips.~
CharAuto    You smooch yourself.~
OthersAuto  $n smooches $mself.  Yuck.~
End

#SOCIAL
Name        snack~
CharNoArg   You whip up a bunch of delicious snacks for everyone in the room.~
OthersNoArg $n whips up a bunch of delicious snacks for everyone in the room.~
CharFound   You whip up some delicious snacks for $N.~
OthersFound $n whips up some delicious snacks for $N.~
VictFound   $n whips up some delicious snacks for you.~
End

#SOCIAL
Name        snap~
CharNoArg   PRONTO ! You snap your fingers.~
OthersNoArg $n snaps $s fingers.~
CharFound   You snap back at $M.~
OthersFound $n snaps back at $N.~
VictFound   $n snaps back at you!~
CharAuto    You snap yourself to attention.~
OthersAuto  $n snaps $mself to attention.~
End

#SOCIAL
Name        snark~
CharNoArg   You try to snarl but just get snarky instead.~
OthersNoArg $n tries to snarl but just gets snarky instead.~
CharFound   You get very snarky with $N.~
OthersFound $n gets very snarky with $N.~
VictFound   $n gets very snarky with you.~
End

#SOCIAL
Name        snarl~
CharNoArg   You grizzle your teeth and look mean.~
OthersNoArg $n snarls angrily.~
CharFound   You snarl at $M.~
OthersFound $n snarls at $N.~
VictFound   $n snarls at you, for some reason.~
CharAuto    You snarl at yourself.~
OthersAuto  $n snarls at $mself.~
End

#SOCIAL
Name        sneer~
CharNoArg   You sneer in contempt.~
OthersNoArg $n sneers in contempt.~
CharFound   You sneer at $M in contempt.~
OthersFound $n sneers at $N in contempt.~
VictFound   $n sneers at you in contempt.~
CharAuto    You sneer at yourself in contempt.~
OthersAuto  $n sneers at $mself in contempt.~
End

#SOCIAL
Name        sneeze~
CharNoArg   Gesundheit!~
OthersNoArg $n sneezes.~
CharFound   You sneeze all over $N.~
OthersFound $n sneezes all over $N. Ewwww!~
VictFound   $n sneezes all over you. How rude!~
End

#SOCIAL
Name        sngrin~
CharNoArg   You grin mischievously.~
OthersNoArg $n grins as if $s thoughts are mischievous.~
CharFound   You give $N your award winning sneaky grin..~
OthersFound $n gives $N a very sneaky looking grin.~
VictFound   $n gives you a grin that makes you reach for your coin purse to see if it's there.~
CharAuto    You grin mischievously.~
OthersAuto  $n grins in a way that makes you reach for your coin purse to see if it's there.~
End

#SOCIAL
Name        snicker~
CharNoArg   You snicker softly.~
OthersNoArg $n snickers softly.~
CharFound   You snicker with $M about your shared secret.~
OthersFound $n snickers with $N about their shared secret.~
VictFound   $n snickers with you about your shared secret.~
CharAuto    You snicker at your own evil thoughts.~
OthersAuto  $n snickers at $s own evil thoughts.~
End

#SOCIAL
Name        sniff~
CharNoArg   You sniff sadly. *SNIFF*~
OthersNoArg $n sniffs sadly.~
CharFound   You sniff sadly at the way $E is treating you.~
OthersFound $n sniffs sadly at the way $N is treating $m.~
VictFound   $n sniffs sadly at the way you are treating $m.~
CharAuto    You sniff sadly at your lost opportunities.~
OthersAuto  $n sniffs sadly at $mself.  Something MUST be bothering $m.~
End

#SOCIAL
Name        snore~
CharNoArg   Zzzzzzzzzzzzzzzzz.~
OthersNoArg $n snores loudly.~
End

#SOCIAL
Name        snort~
CharNoArg   You snort in disgust.~
OthersNoArg $n snorts in disgust.~
CharFound   You snort at $M in disgust.~
OthersFound $n snorts at $N in disgust.~
VictFound   $n snorts at you in disgust.~
CharAuto    You snort at yourself in disgust.~
OthersAuto  $n snorts at $mself in disgust.~
End

#SOCIAL
Name        snowball~
CharNoArg   Whom do you want to throw a snowball at?~
CharFound   You throw a snowball in $N's face.~
OthersFound $n throws a snowball at $N.~
VictFound   $n throws a snowball at you.~
CharAuto    You throw a snowball at yourself.~
OthersAuto  $n throws a snowball at $mself.~
End

#SOCIAL
Name        snuggle~
CharNoArg   Who?~
CharFound   you snuggle $M.~
OthersFound $n snuggles up to $N.~
VictFound   $n snuggles up to you.~
CharAuto    You snuggle up, getting ready to sleep.~
OthersAuto  $n snuggles up, getting ready to sleep.~
End

#SOCIAL
Name        snuke~
CharNoArg   You attempt to smile, but have your fingers on the wrong keys.~
OthersNoArg $n tries to smile, but has $s fingers on the wrong keys.~
CharFound   You try to smile at $N, but have your fingers on the wrong keys.~
OthersFound $n attempts to smile at $N, but has $s fingers on the wrong keys!~
VictFound   $n attempts to smile at you, but has $s fingers on the wrong keys!~
CharAuto    You snuke yourself.~
OthersAuto  $n snukes $mself.~
End

#SOCIAL
Name        soap~
CharNoArg   You soap.~
CharFound   You wash $N's mouth out with soap.~
OthersFound $n washes $N's mouth out with soap.~
VictFound   $n washes your mouth out with soap.~
End

#SOCIAL
Name        spam~
CharNoArg   You gobble down a can of spam. Oink!~
OthersNoArg $n gobbles down a can of spam. Oink!~
CharFound   You hurl a can of Spam at $N. Hey! No spamming allowed here!~
OthersFound $n hurls a can of Spam at $N. Silly $n ... $e thinks that is spamming.~
VictFound   $n hurls a can of Spam at you. What a fool! I bet $e thinks $e is spamming you.~
CharAuto    You want to spam yourself? Forget it!~
End

#SOCIAL
Name        spank~
CharNoArg   Spank whom?~
CharFound   You spank $M playfully.~
OthersFound $n spanks $N playfully.~
VictFound   $n spanks you playfully.  OUCH!~
CharAuto    You spank yourself.  Kinky!~
OthersAuto  $n spanks $mself.  Kinky!~
End

#SOCIAL
Name        sparkle~
CharNoArg   Your eyes sparkle with amusement.~
OthersNoArg $n's eyes sparkle with amusement.~
CharFound   Your eyes sparkle with amusement over $N's actions.~
OthersFound $n's eyes sparkle with amusement over $N's actions.~
VictFound   $n's eyes sparkle with amusement over your actions.~
CharAuto    You find yourself to be quite amusing.~
OthersAuto  $n apparently believes $e is quite amusing.~
End

#SOCIAL
Name        spaz~
CharNoArg   You announce to the world that you are just a big spaz!~
OthersNoArg $n announces to the world that $e is just a big spaz!~
CharFound   You try to hug $N but spaz all over $M instead.~
OthersFound $n tries to hug $N but spazzes all over $M instead.~
VictFound   $n tries to hug you but spazzes all over you instead.~
End

#SOCIAL
Name        spit~
CharNoArg   You spit.~
OthersNoArg $n spits like a camel..get out of the way!!~
CharFound   You spit on $N. This is how the plague spreads.~
OthersFound $n has spit on $N. Obviously how the plague spread.~
VictFound   $n hocks a big glob of spit at you! Ewwww~
CharAuto    You spit on yourself.~
OthersAuto  $n spits into the wind. Unfortunately the wind is blowing towards $m.~
End

#SOCIAL
Name        splat~
CharNoArg   You run to the nearest wall and ... splat!~
OthersNoArg $n runs to the nearest wall and ... splat!~
CharFound   You run right into $N ... splat!~
OthersFound $n runs right into $N ... splat!~
VictFound   $n runs right into you ... Splat!~
CharAuto    $n runs right into you ... splat!~
End

#SOCIAL
Name        spum~
CharNoArg   You are so spammed you are now spummed.~
OthersNoArg $n is so spammed $e is now spummed.~
CharFound   You spam $N so much $E is now spummed.~
OthersFound $n spams $N so hard $E is now spummed.~
VictFound   $n spams you so hard you are now spummed.~
End

#SOCIAL
Name        squeak~
CharNoArg   You squeak like a mouse.~
OthersNoArg $n squeaks like a mouse.~
CharFound   You squeak at $M.~
OthersFound $n squeaks at $N.  Is $e a man or a mouse?~
VictFound   $n squeaks at you.  Is $e a man or a mouse?~
CharAuto    You squeak at yourself like a mouse.~
OthersAuto  $n squeaks at $mself like a mouse.~
End

#SOCIAL
Name        squeal~
CharNoArg   You squeal with delight.~
OthersNoArg $n squeals with delight.~
CharFound   You squeal at $M.~
OthersFound $n squeals at $N.  Wonder why?~
VictFound   $n squeals at you.  You must be doing something well.~
CharAuto    You squeal at yourself.~
OthersAuto  $n squeals at $mself.~
End

#SOCIAL
Name        squeeze~
CharNoArg   Where, what, how, whom?~
CharFound   You squeeze $M fondly.~
OthersFound $n squeezes $N fondly.~
VictFound   $n squeezes you fondly.~
CharAuto    You squeeze yourself - try to relax a little!~
OthersAuto  $n squeezes $mself.~
End

#SOCIAL
Name        squint~
CharNoArg   Bright light! Bright light!~
OthersNoArg $n hides $s wee pupils from Ra's menacing globe.~
CharFound   $N glows with energy!~
OthersFound $n hides $s wee pupils from $N's radiant aura.~
VictFound   $n hides $s wee pupils from your radiant aura.~
CharAuto    You attempt to invert your eyeballs.  That's GOTTA hurt.~
OthersAuto  $n tries to hide $s wee pupils from themself.  They glow with strangeness!~
End

#SOCIAL
Name        squirm~
CharNoArg   You squirm guiltily.~
OthersNoArg $n squirms guiltily.  Looks like $e did it.~
CharFound   You squirm in front of $M.~
OthersFound $n squirms in front of $N.~
VictFound   $n squirms in front of you.  You make $m nervous.~
CharAuto    You squirm and squirm and squirm....~
OthersAuto  $n squirms and squirms and squirm.....~
End

#SOCIAL
Name        squish~
CharNoArg   You squish your toes into the sand.~
OthersNoArg $n squishes $s toes into the sand.~
CharFound   You squish $M between your legs.~
OthersFound $n squishes $N between $s legs.~
VictFound   $n squishes you between $s legs.~
CharAuto    You squish yourself.~
OthersAuto  $n squishes $mself.  OUCH.~
End

#SOCIAL
Name        stamp~
CharNoArg   You look around for someone to stamp.~
OthersNoArg $n looks around for someone to stamp.~
CharFound   You give $N your certified sanity stamp of approval.~
OthersFound $n gives $N $s certified sanity stamp of approval.~
VictFound   $n gives you $s certified sanity stamp of approval.~
CharAuto    You give yourself your certified sanity stamp of approval.~
OthersAuto  $n gives $mself $s certified sanity stamp of approval.  ~
End

#SOCIAL
Name        stare~
CharNoArg   You stare at the sky.~
OthersNoArg $n stares at the sky.~
CharFound   You stare dreamily at $N, completely lost in $S eyes..~
OthersFound $n stares dreamily at $N.~
VictFound   $n stares dreamily at you, completely lost in your eyes.~
CharAuto    You stare dreamily at yourself - enough narcissism for now.~
OthersAuto  $n stares dreamily at $mself - NARCISSIST!~
End

#SOCIAL
Name        steam~
CharNoArg   You are so angry that two sharp blasts of steam come whistling out of your ears.~
OthersNoArg $n is so angry that two sharp blasts of steam come whistling out of $s ears.~
CharFound   You are so angry at $N that two sharp blasts of steam come whistling out of your ears.~
OthersFound $n is so angry at $N that two sharp blasts of steam come whistling out of $s ears!~
VictFound   $n is so angry at you that two sharp blasts of steam come whistling out of $s ears!~
End

#SOCIAL
Name        stink~
CharNoArg   You smell really bad. In fact ... you stink!~
OthersNoArg $n smells pretty bad. In fact ... $e stinks!~
CharFound   You think $N smells real bad. In fact ... you think $E stinks!~
OthersFound $n thinks $N smells real bad. $n thinks $E stinks!~
VictFound   $n thinks you smell real bad. In fact ... $e thinks you stink!~
End

#SOCIAL
Name        stomp~
CharNoArg   You stomp your feet and pout like the baby you are.~
OthersNoArg $n stomps $s feet and throws a tantrum like a baby.~
CharFound   You kick your feet on the ground for what $N did to you.~
OthersFound $n stomps $s feet and throws a tantrum at $N.~
VictFound   $n stomps $s feet like a baby.~
CharAuto    You stomp your feet in a virtual temper tantrum.~
OthersAuto  $n throws a tantrum. What a baby!~
End

#SOCIAL
Name        stone~
CharNoArg   You kick a stone at nothing in particular.~
CharFound   You begin to pelt $M with large stones.~
OthersFound $n attempts to stone $N to death! Eeeek!~
VictFound   $n throws several stones at you! Owwww. Are you gonna take that?~
CharAuto    The gods don't allow suicide.~
OthersAuto  $n picks up a rock and throws it straight up in the air. Watch out!~
End

#SOCIAL
Name        stress~
CharNoArg   You are under a lot of stress. Sleep!~
OthersNoArg $n is under a lot of stress.~
CharFound   $N is really stressing you out.~
OthersFound $N is causing $n a lot of stress.~
VictFound   $n screams, "You're stressing me!", and runs away pulling $s hair!~
CharAuto    You are stressing yourself out, stop that!~
OthersAuto  $n is stressing out. Run while you can!~
End

#SOCIAL
Name        stretch~
CharNoArg   You stretch and relax your sore muscles.~
OthersNoArg $n stretches luxuriously.  Makes you want to, doesn't it?~
CharFound   You stretch and relax your sore muscles.~
OthersFound $n stretches luxuriously.  Makes you want to, doesn't it?~
VictFound   $n stretches luxuriously.  Makes you want to, doesn't it?~
CharAuto    You stretch and relax your sore muscles.~
OthersAuto  $n stretches luxuriously.  Makes you want to, doesn't it?~
End

#SOCIAL
Name        strut~
CharNoArg   Strut your stuff.~
OthersNoArg $n struts, thinking $e's far too sexy for this mud.~
CharFound   You strut to get $S attention.~
OthersFound $n struts, hoping to get $N's attention.~
VictFound   $n struts, hoping to get your attention.~
CharAuto    You strut to yourself, lost in your own world.~
OthersAuto  $n struts to $mself, lost in $s own world.~
End

#SOCIAL
Name        stud~
CharNoArg   You strike a pose in your most studly style!~
OthersNoArg $n strikes a pose in $s most studly style!~
CharFound   You think $N is a supreme stud!~
OthersFound $n looks at $N and thinks, "Wow! What a stud!"~
VictFound   $n thinks you are a supreme stud!~
End

#SOCIAL
Name        stuff~
CharNoArg   You stuff your imaginary friend into an extradimensional portal.~
OthersNoArg $n stuffs $s imaginary friend into an extradimensional portal.~
CharFound   You stuff $N into an extradimensional portal...where $E belongs.~
OthersFound $n stuffs $N into an extradimensional portal...where $E belongs.~
VictFound   $n stuffs you into an extradimensional portal...where you belong.~
CharAuto    You attempt to dive head first into an extradimensional portal.~
OthersAuto  $n attempts to hide by diving head first into an extradimensional portal.~
End

#SOCIAL
Name        suffer~
CharNoArg   No xp again?  You suffer at the hands of fate.~
OthersNoArg $n is suffering.  Looks like $e can't seem to level.~
CharFound   You tell $M how you suffer whenever you're away from $M.~
OthersFound $n tells $N that $e suffers whenever they're apart.~
VictFound   $n tells you that $e suffers whenever you're apart.~
CharAuto    No xp again?  You suffer at the hands of fate.~
OthersAuto  $n is suffering.  Looks like $e can't seem to level.~
End

#SOCIAL
Name        sulk~
CharNoArg   You sulk.~
OthersNoArg $n sulks in the corner.~
CharFound   You sulk at $N.~
OthersFound $n sulks at $N.~
VictFound   $n looks at you and sulks.~
CharAuto    You need to improve your self image!~
OthersAuto  $n sulks about something.~
End

#SOCIAL
Name        sweetheart~
CharNoArg   You are looking for someone to call sweetheart.~
OthersNoArg $n is looking for someone to call sweetheart.~
CharFound   You croon to $N, "Let me call you sweetheart. I'm in love with you."~
VictFound   $n croons to you, "Let me call you sweetheart. I'm in love with you."~
End

#SOCIAL
Name        sweetie~
CharNoArg   You look for your sweetie but cannot find them.~
OthersNoArg $n looks for $s sweetie but cannot find them.~
CharFound   You spot $N and coo, "Hello Sweetie!"~
OthersFound $n spots $N and coos, "Hello Sweetie!"~
VictFound   $n spots you and coos, "Hello Sweetie!"~
End

#SOCIAL
Name        swoon~
CharNoArg   You swoon in ecstasy.~
OthersNoArg $n swoons in ecstasy.~
CharFound   You swoon in ecstasy at the thought of $M.~
OthersFound $n swoons in ecstasy at the thought of $N.~
VictFound   $n swoons in ecstasy as $e thinks of you.~
CharAuto    You swoon in ecstasy.~
OthersAuto  $n swoons in ecstasy.~
End

#SOCIAL
Name        tackle~
CharNoArg   You can't tackle the AIR!~
CharFound   You run over to $M and bring $M down!~
OthersFound $n runs over to $N and tackles $M to the ground!~
VictFound   $n runs over to you and tackles you to the ground!~
CharAuto    You wrap your arms around yourself, and throw yourself to the ground.~
OthersAuto  $n wraps $s arms around $mself and brings $mself down!?~
End

#SOCIAL
Name        tag~
CharNoArg   You shout, "Enough killing!  Let's play some tag!"~
OthersNoArg $n says, "Enough killing!  Let's play some tag!"~
CharFound   You slap $N on the back and scream, "You're it slowpoke!"~
OthersFound $n slaps $N on the back and screams, "You're it slowpoke!"~
VictFound   $n slaps you on the back and screams, "You're it slowpoke!"~
CharAuto    You dodge your left hand, but are led directly into your right!  Fool!~
OthersAuto  $n dodges $s left hand, but is led directly into $s right!  Fool!~
End

#SOCIAL
Name        tail~
CharNoArg   You wag your tail. Feeling happy?~
OthersNoArg $n wags $s tail. $n must be feeling happy.~
CharFound   You wag your tail at $N. You must like $M.~
OthersFound $n wags $s tail at $N. $e must like $M.~
VictFound   $n wags $s tail at you. $e must like you. You are such a lucky mudder. Aren't you?~
End

#SOCIAL
Name        tamale~
CharNoArg   You are a hot tamale. Arriba!~
OthersNoArg $n is one hot tamale. Arriba!~
CharFound   You look at $N and think, "Wow! What a hot tamale!"~
OthersFound $n looks at $N and thinks, "Wow! What a hot tamale!"~
VictFound   $n looks at you and thinks, "Wow! What a hot tamale!"~
End

#SOCIAL
Name        tango~
CharNoArg   You tip-toe around the room dancing with your shadow. Weirdo.~
OthersNoArg $n tip-toes around the room dancing with $s shadow. Give $m room!~
CharFound   You grab $N and dance a wild tango with $M.~
OthersFound $n grabs $N and dances a wild tango with $M.~
VictFound   $n grabs you and dances a wild tango with you. You feel very excited.~
CharAuto    My my, aren't we the contortionist. The dip must pose a significant problem.~
OthersAuto  $n takes a couple stuttering steps before falling over backwards.~
End

#SOCIAL
Name        tap~
CharNoArg   You tap your foot impatiently.~
OthersNoArg $n taps $s foot impatiently.~
CharFound   You tap your foot impatiently.  Will $E ever be ready?~
OthersFound $n taps $s foot impatiently as $e waits for $N.~
VictFound   $n taps $s foot impatiently as $e waits for you.~
CharAuto    You tap yourself on the head.  Ouch!~
OthersAuto  $n taps $mself on the head.~
End

#SOCIAL
Name        taunt~
CharNoArg   You peer around you, looking for someone to taunt.~
OthersNoArg $n wants to taunt someone.~
CharFound   You taunt $M. Hope $E doesn't make you do it a second time!~
OthersFound $n is taunting $N.~
VictFound   $n taunts you cruelly, and threatens to do it again! Watch out!~
CharAuto    You really can't taunt yourself.~
OthersAuto  $n attempts to taunt $mself. Perhaps $e needs help?~
End

#SOCIAL
Name        tbite~
CharNoArg   You quoth 'Dost thou bite thy thumb at me?'~
OthersNoArg $n quoths 'Dost thou bite thy thumb at me?'~
CharFound   You bite your thumb in $N's general direction.~
OthersFound $n bites $s thumb in $N's general direction.~
VictFound   $n bites $s thumb in your general direction!~
CharAuto    You bite your thumb at everyone in general.~
OthersAuto  $n bites $s thumb in your general direction!~
End

#SOCIAL
Name        teapot~
CharNoArg   Where do your arms go again?..~
OthersNoArg $n tries to remember the words to that song...~
CharFound   You place your arms in position and begin singing "I'm a little teapot, short and stout"~
OthersFound $n gets $s hands in position and begins doing the teapot dance.~
VictFound   $n puts $s hands in position and begins singing "I'm a little teapot, short and stout", to you.~
CharAuto    You begin doing the teapot dance for your own amusement...bored maybe?~
OthersAuto  $n begins singing and dancing to I'm a little teapot, better call the asylum.~
End

#SOCIAL
Name        tease~
CharNoArg   You look for someone to tease.~
OthersNoArg $n searches for a victim to tease.~
CharFound   You tease $M playfully.~
OthersFound $n is such a tease!~
VictFound   $n teases you playfully.~
CharAuto    You attempt to tease yourself. Pick on someone else!~
OthersAuto  $n tries to tease $mself. What a loon.~
End

#SOCIAL
Name        tee~
CharNoArg   You say "tee hee" showing off your cute dimples.~
OthersNoArg $n shows off $s cute dimples saying, "tee hee".~
CharFound   You show off your cute dimples to $N saying, "tee hee".~
OthersFound $n shows off $s cute dimples to $N saying, "tee hee".~
VictFound   $n shows off $s cute dimples to you saying, "tee hee".~
End

#SOCIAL
Name        temple~
CharNoArg   You want to do what?? And to whom??~
CharFound   You begin to rub $S temples, attempting to reduce $S stress.~
OthersFound $n begins to rub $N's temples... $E begins to relax.~
VictFound   $n begins to carefully massage your temples... Your stress melts away.~
CharAuto    You rub your temples, your patience wearing thin.~
OthersAuto  $n rubs $s temples, looking rather impatient.~
End

#SOCIAL
Name        tendril~
CharNoArg   You reach up and move a stray strand of hair from your eyes.~
OthersNoArg $n reaches up to remove a stray wisp of hair from $s eyes.~
CharFound   You gently move a stray wisp of hair from $N's eyes.~
OthersFound $n reaches up to move a stray strand of hair from $N's eyes.~
VictFound   $n gently pushes the hair from your eyes.~
CharAuto    You reach up and move a stray strand of hair from your eyes.~
OthersAuto  $n casually flips the hair from in front of $s eyes~
End

#SOCIAL
Name        thank~
CharNoArg   Thank you too.~
CharFound   You thank $N heartily.~
OthersFound $n thanks $N heartily.~
VictFound   $n thanks you heartily.~
CharAuto    You thank yourself since nobody else wants to !~
OthersAuto  $n thanks $mself since you won't.~
End

#SOCIAL
Name        thrash~
CharNoArg   You thrash around madly!~
OthersNoArg $n thrashes about madly! Make room!~
CharFound   You thrash into $N.~
OthersFound $n thrashes into $N. Better make some room!~
VictFound   $n thrashes into you. Two can play that game!~
CharAuto    You thrash yourself! Go with your bad self!~
OthersAuto  $n thrashes around on the floor. Hurm.~
End

#SOCIAL
Name        throttle~
CharNoArg   Whom do you want to throttle?~
CharFound   You throttle $M till $E is blue in the face.~
OthersFound $n throttles $N about the neck, until $E passes out.  THUNK!~
VictFound   $n throttles you about the neck until you pass out.  THUNK!~
CharAuto    That might hurt!  Better not do it!~
OthersAuto  $n is getting a crazy look in $s eye again.~
End

#SOCIAL
Name        throw~
CharNoArg   You feel Japanese and practice a judo throw.~
OthersNoArg $n is feeling Japanese and practices a judo throw.~
CharFound   You think you are Japanese and flip $N on $S back with a judo throw.~
OthersFound $n is feeling Japanese and flips $N onto $S back with a judo throw.~
VictFound   $n is feeling Japanese and flips you onto your back with a judo throw.~
End

#SOCIAL
Name        thumbsup~
CharNoArg   You give it a big thumbs up!~
OthersNoArg $n gives the idea a big thumbs up!~
CharFound   You give $N a big thumbs up.~
OthersFound $n gives $N a big thumbs up.~
VictFound   $n gives you a big thumbs up.~
CharAuto    You give yourself a big thumbs up.~
OthersAuto  $n gives $mself a big thumbs up.~
End

#SOCIAL
Name        thwap~
CharNoArg   You swing about in vain trying to thwap someone.~
OthersNoArg $n tries in vain to thwap someone who isn't here.~
CharFound   You THWAP $N for being a moron.~
OthersFound $n THWAPS $N for being a moron.~
VictFound   $n THWAPS you for being a moron. ~
CharAuto    You thwap yourself in the forehead. You loser.~
OthersAuto  $n thwaps $mself for being a moron.~
End

#SOCIAL
Name        tickle~
CharNoArg   Whom do you want to tickle?~
CharFound   You tickle $N.~
OthersFound $n tickles $N.~
VictFound   $n tickles you - hee hee hee.~
CharAuto    You tickle yourself, how funny!~
OthersAuto  $n tickles $mself.~
End

#SOCIAL
Name        tingle~
CharNoArg   You feel all warm and tingly!~
OthersNoArg $n smiles contentedly, feeling all warm and tingly!~
CharFound   You feel all tingly as you set your eyes upon $N!~
OthersFound $N tingles with delight as $E is graced with a smile from $n.~
VictFound   $n stares at you with a warm, tingly look on $s face! You slowly back away..~
CharAuto    You feel all warm and tingly!~
OthersAuto  $n smiles contentedly, feeling all warm and tingly!~
End

#SOCIAL
Name        tipcap~
CharNoArg   You tip your cap to everyone in the room.~
OthersNoArg $n tips $s cap to everyone in the room.~
CharFound   You tip your cap to $N.~
OthersFound $n tips $s cap to $N. My how chivalrous!~
VictFound   $n tips $s cap to you. My how chivalrous!~
CharAuto    You tip your cap to yourself, hoping someone may take the hint and acknowledge your presence.~
OthersAuto  $n tips $s cap to $mself, hoping in vain to have $s presence acknowledged.~
End

#SOCIAL
Name        tissue~
CharNoArg   You search for a tissue.~
OthersNoArg $n searches for a tissue.~
CharFound   You give $M a tissue. Awwww.~
OthersFound $n gallantly gives $N a tissue.~
VictFound   $n hands you a tissue in an attempt to ease your pain.~
CharAuto    You search for a tissue. Hope you find one quickly.~
OthersAuto  $n looks for a tissue. Poor baby :(.~
End

#SOCIAL
Name        tkiss~
CharNoArg   You will enjoy it more if you choose someone to kiss.~
CharFound   You give $M a soft, tender kiss.~
OthersFound $n gives $N a soft, tender kiss.~
VictFound   $n gives you a soft, tender kiss.~
CharAuto    You'd better not, people may start to talk!~
End

#SOCIAL
Name        toast~
CharNoArg   You raise your glass and propose a toast!~
OthersNoArg $n raises their glass and proposes a toast!~
CharFound   You raise your glass and toast $N's good fortune.~
OthersFound $n raises their glass and toasts $N's good fortune.~
VictFound   $n raises their glass and toasts your good fortune.~
CharAuto    You raise your glass and toast your good fortune.~
OthersAuto  $n toasts $mself; you suppress an urge to yawn.~
End

#SOCIAL
Name        toes~
CharNoArg   You wiggle your toes.~
OthersNoArg $n wiggles $s toes.~
CharFound   You tickle $N's toes.~
OthersFound $n tickles $N's toes.~
VictFound   $n tickles your toes.~
End

#SOCIAL
Name        tomato~
CharNoArg   You heft a large, rotten tomato and say "Ketchup anyone?"~
OthersNoArg $n hefts a rotten tomato and wonders if you would like to come a bit closer.~
CharFound   You whip a rotten tomato at $N!~
OthersFound $n whips a rotten tomato at $N!~
VictFound   $n whips a rotten tomato at you! SPLAT!!!~
CharAuto    You toss a tomato straight up in the air.  SPLAT!!  Boy it looks like you got tomato sauce all over your face.~
OthersAuto  Look out!! $n is the wild tomato tosser everyone warned you about.~
End

#SOCIAL
Name        tongue~
CharNoArg   You stick your tongue out. How childish!~
OthersNoArg $n sticks $s tongue out.  How juvenile of $m!~
CharFound   You stick out your tongue at $N.  How impressed $E is with you now!~
OthersFound $n sticks $s tongue out at $N, apparently thinking this will impress $M.  Whadda jerk!~
VictFound   $n sticks $s tongue out at you, apparently thinking it will impress you.  Whadda fool!~
CharAuto    You stick your tongue out. How childish!~
OthersAuto  $n sticks $s tongue out. How juvenile of $m!~
End

#SOCIAL
Name        torture~
CharNoArg   You have to torture someone!~
CharFound   You torture $M with rusty weapons, Mwaahhhhh!!~
OthersFound $n tortures $N with rusty weapons, $E must have been REAL bad!~
VictFound   $n tortures you with rusty weapons!  What did you DO!?!~
CharAuto    You torture yourself with rusty weapons.  Was it good for you?~
OthersAuto  $n tortures $mself with rusty weapons.  Looks like $e enjoys it!?~
End

#SOCIAL
Name        towel~
CharNoArg   You wield a sopping wet towel. Hmmm who will be your next victim?~
OthersNoArg $n wields a sopping wet towel. RUN!!!!~
CharFound   You snap $N with a wet towel. Ouch!~
OthersFound $n snaps $N with a wet towel. Be careful, you might be next!~
VictFound   $n snaps you with a wet towel.~
CharAuto    You grab a towel and dry yourself off.~
OthersAuto  $n grabs a towel and dries $mself off.~
End

#SOCIAL
Name        tralala~
CharNoArg   You try to sing. Best you can manage is 'tra la la'.~
OthersNoArg $n goes 'tra la la'~
CharFound   You croon to $N 'tra la la'.~
OthersFound $n croons to $N 'tra la la'.~
VictFound   $n croons to you 'tra la la'.~
End

#SOCIAL
Name        tsk~
CharNoArg   You tsk.~
OthersNoArg $n tsks.~
CharFound   You tsk at $N.~
OthersFound $n looks at $N and says 'tsk tsk tsk'.~
VictFound   $n looks at you, shakes $s head and says 'tsk tsk tsk'.~
CharAuto    You want to tsk yourself? I don't think so.~
End

#SOCIAL
Name        tuck~
CharNoArg   You search in vain for someone to tuck you in.~
OthersNoArg $n is looking for someone to tuck $m in.~
CharFound   You smile at $N and tuck $M into bed.~
OthersFound $n smiles at $N and tucks $M into bed.~
VictFound   $n tucks you into bed and smiles at you.~
CharAuto    You tuck yourself into a ball and roll across the room.~
OthersAuto  $n tucks $mself into a ball and rolls across the room.~
End

#SOCIAL
Name        tug~
CharNoArg   You look around for a sleeve to tug.~
OthersNoArg $n looks around for a sleeve to tug, maybe $e needs some attention?~
CharFound   You gently tug on $N's sleeve, trying to get $S attention.~
OthersFound $n gently tugs on $N's sleeve, trying to get their attention.~
VictFound   $n gently tugs on your sleeve, looking hopeful.~
CharAuto    You gently tug your own sleeve, perhaps you should concentrate?~
OthersAuto  $n tugs $s own sleeve, weird eh?~
End

#SOCIAL
Name        tummy~
CharNoArg   You rub your tummy and wish you'd bought a pie at the bakery.~
OthersNoArg $n rubs $s tummy and wishes $e'd bought a pie at the bakery.~
CharFound   You rub your tummy and ask $M for some food.~
OthersFound $n rubs $s tummy and asks $N for some food.~
VictFound   $n rubs $s tummy and asks you for some food.  Please?~
CharAuto    You rub your tummy and wish you'd bought a pie at the bakery.~
OthersAuto  $n rubs $s tummy and wishes $e'd bought a pie at the bakery.~
End

#SOCIAL
Name        turkey~
CharNoArg   You stand up full and gobble like a turkey.~
OthersNoArg $n rises up and gobbles like a turkey.~
CharFound   You look at $N and think what a turkey!~
OthersFound $n looks at $N and thinks, "What a turkey!"~
VictFound   $n looks at you and wishes you were a big turkey. Is Thanksgiving coming?~
End

#SOCIAL
Name        turtle~
CharNoArg   You pull your head back into your shell.~
OthersNoArg $n pulls $s head back into $s shell.~
CharFound   You pull your head back into your shell in an effort to hide from $N.~
OthersFound $n pulls $s head back into $s head in an effort to hide from $N.~
VictFound   $n pulls $s head back into $s shell in an effort to hide from you.~
CharAuto    You want to turn into a turtle? Try building a cocoon!~
OthersAuto  $n wants to be a turtle. Perhaps $e should build a cocoon!~
End

#SOCIAL
Name        twiddle~
CharNoArg   You patiently twiddle your thumbs.~
OthersNoArg $n patiently twiddles $s thumbs.~
CharFound   You twiddle $S ears.~
OthersFound $n twiddles $N's ears.~
VictFound   $n twiddles your ears.~
CharAuto    You twiddle your ears like Dumbo.~
OthersAuto  $n twiddles $s own ears like Dumbo.~
End

#SOCIAL
Name        twirl~
CharNoArg   You twirl in a graceful pirouette.~
OthersNoArg $n twirls in a graceful pirouette.~
CharFound   You spin $M on one finger.~
OthersFound $n spins $N on $s finger.~
VictFound   $n spins you around on $s finger.~
CharAuto    You spin yourself around and around and around...~
OthersAuto  $n spins $mself around and around and around...~
End

#SOCIAL
Name        twitch~
CharNoArg   You twitch nervously.~
OthersNoArg $n twitches nervously.~
CharFound   Your left eye begins to twitch uncontrollably.~
OthersFound $n begins to twitch uncontrollably.~
VictFound   $n twitches repeatedly.~
CharAuto    You twitch.~
OthersAuto  $n is twitching uncontrollably.~
End

#SOCIAL
Name        type~
CharNoArg   You throw up yor handz in dizgust at yur losy typing skils.~
OthersNoArg $n couldn't type a period if there was only one key on the keyboard.~
CharFound   You throw up yor handz in dizgust at yur losy typing skils.~
OthersFound $n couldn't type a period if there was only one key on the keyboard.~
VictFound   $n couldn't type a period if there was only one key on the keyboard.~
CharAuto    You throw up yor handz in dizgust at yur losy typing skils.~
OthersAuto  $n couldn't type a period if there was only one key on the keyboard.~
End

#SOCIAL
Name        ugh~
CharNoArg   You make a face and say, Ugh!~
OthersNoArg $n makes a face and says, Ugh!~
CharFound   You take one look at $N and run away screaming "Uggggggh!"~
OthersFound $n looks at $N and screams, "Ugggggggghhhhh!  in pure terror! ~
VictFound   $n covers $s eyes and runs off screaming, "UGGGGGH!", after looking at you.~
CharAuto    You ugh at yourself in contempt.~
OthersAuto  $n catches a glimpse of $self in a mirror and screams, "UGGGGGGGGH"!~
End

#SOCIAL
Name        uplift~
CharNoArg   You summon a mystical hand to provide aid.~
OthersNoArg $n tries to summon a mystical hand to do $s bidding.~
CharFound   You summon a healing hand for $N, but it backfires! A demon hand appears and begins to thrash $M about the head repeatedly!~
OthersFound $N is slapped from afar by a strange mystical hand.~
VictFound   A mystical hand conjured by $n appears in front of you and *THWAPS* you hard!~
CharAuto    You attempt to summon a mystical hand to do your bidding.~
OthersAuto  You watch in horror as the mystical hand is summoned by $n.  The spell backfires and the hand begins to beat $m senseless!~
End

#SOCIAL
Name        vamp~
CharNoArg   You strike your best vamping pose.~
OthersNoArg $n strikes $s best vamping pose.~
CharFound   You circle $N vamping $M till $E can't stand no more.~
OthersFound $n circles $N vamping $M till $E can't stand no more.~
VictFound   $n circles around you vamping you till you can't stand it no more.~
End

#SOCIAL
Name        vbite~
CharNoArg   You bare your fangs.~
OthersNoArg $n bares $s fangs and peers around nonchalantly.~
CharFound   You sensually brush $N's neck with your lips before sucking $S blood!~
OthersFound $n softly brushes $N's neck with $s lips, and then BITES!~
VictFound   $n sensually brushes your neck with $s lips before sucking your blood!~
CharAuto    Now, if you could bite your own neck..I'd worry.~
OthersAuto  $n is in danger of hurting $mself.~
End

#SOCIAL
Name        vblood~
CharNoArg   You get a strange look in your eyes as you long for blood.~
OthersNoArg $n gets a strange look in $s eyes as $e longs for blood.~
CharFound   You thirst for $N's blood.~
OthersFound $n stares hungrily at $N's throat.~
VictFound   $n stares hungrily at your jugular.~
CharAuto    You are so desperate for blood that you begin to gnaw on your own wrist.~
OthersAuto  $n is so desperate for blood $e tries to feed on $mself.~
End

#SOCIAL
Name        victory~
CharNoArg   You raise your hand in the air to your adoring fans, you anticipate victory!~
OthersNoArg $n raises $s hand in the air to $s adoring fans, $e anticipates victory!~
CharFound   You raise your hand to your adoring fan, $N, letting $M know that you anticipate victory.~
OthersFound $n raises $s hand to $s adoring fan, $N, letting $M know that $e anticipate victory.~
VictFound   $n raises $s hand to you, $s loyal fan, letting you know that $e anticipates victory.~
CharAuto    Yay, you are your own hero!~
OthersAuto  $n definitely has a narcissistic side.~
End

#SOCIAL
Name        village~
CharNoArg   You know there's a village idiot in every village. Why can't you find it here?? You boggle.~
OthersNoArg $n searches for the town imbecile in vain. You notice the "Village Idiot" sign attached to $s armor.~
CharFound   You dub $N the new Village idiot.~
OthersFound $n proudly introduces $N, the Village Idiot!~
VictFound   $n has dubbed thee Village Idiot! Congratulations!~
CharAuto    Ack! You realize you're the village idiot they all talk about!~
OthersAuto  $n prances around proclaiming $mself village idiot!~
End

#SOCIAL
Name        violin~
CharNoArg   You whip out your violin and play a sweet song.~
OthersNoArg $n tries to play the violin, but $s fingers are too fat.~
CharFound   You play the world's saddest song on the world's tiniest violin just for $N.~
OthersFound $n's tiny violin shows that $e is unsympathetic to $N's plight.~
VictFound   $n's tiny violin shows that $e is unsympathetic to your plight.~
CharAuto    You play your violin for yourself.~
OthersAuto  $n plays softly to $mself on a violin.~
End

#SOCIAL
Name        waddle~
CharNoArg   You waddle around, imitating a penguin.~
OthersNoArg $n waddles around imitating a penguin.  Maybe $e will squawk next!~
CharFound   You waddle at $N, like a penguin!~
OthersFound $n waddles at $N like a penguin! Maybe $e will squawk next!~
VictFound   $n waddles at you like a penguin! Maybe $e will squawk next!~
CharAuto    You waddle at yourself like a penguin! Feeling alright?~
OthersAuto  $n waddles at $mself like a penguin! Wonder if $e is feeling alright.~
End

#SOCIAL
Name        waggle~
CharNoArg   You waggle your eyebrows mischievously.~
OthersNoArg $n waggles $s eyebrows mischievously.~
CharFound   You waggle your eyebrows mischievously at $N.~
OthersFound $n waggles $s eyebrows mischievously at $N.~
VictFound   $n waggles $s eyebrows mischievously at you.~
CharAuto    You waggle your eyes mischievously.~
OthersAuto  $n waggles $s eyebrows mischievously. You don't want to know.~
End

#SOCIAL
Name        wave~
CharNoArg   You wave.~
OthersNoArg $n waves happily.~
CharFound   You wave goodbye to $N.~
OthersFound $n waves goodbye to $N.~
VictFound   $n waves goodbye to you.  Have a good journey.~
CharAuto    Are you going on adventures as well?~
OthersAuto  $n waves goodbye to $mself.~
End

#SOCIAL
Name        weak~
CharNoArg   You feel so weak, a newbie could kill you!~
OthersNoArg $n feels so weak, a newbie could kill $m!~
CharFound   You inform $N that you are feeling weak today.~
OthersFound $n informs $N that $e is feeling weak today.~
VictFound   $n informs you that $e is feeling weak today.~
CharAuto    You are so weak, you can't even hurt yourself.~
OthersAuto  $n tries to hurt $mself, but $e is too weak to do so.~
End

#SOCIAL
Name        wedgie~
CharNoArg   You give the air a wedgie??~
OthersNoArg $n gives the air a wedgie? What a weirdo!!~
CharFound   You give $N a monster wedgie! OWWWW!!!~
OthersFound $n gives $N a wedgie, run or you might be next!~
VictFound   Your leggings violently go into your butt as $n gives you a monster wedgie! OWWWW!!!~
CharAuto    You give yourself a wedgie??--Kinky!!~
OthersAuto  $n pulls $s leggings up violently! --Kinky!!~
End

#SOCIAL
Name        weep~
CharNoArg   You weep uncontrollably. You are unconsolable.~
OthersNoArg $n begins to weep uncontrollably breaking down into a huge sobbing fit.~
CharFound   You begin to weep uncontrollably. What has $N done to make you so unconsolable?~
OthersFound $n begins to weep uncontrollably. What has $N done to make $m so unconsolable?~
VictFound   $n begins to weep uncontrollably. What have you done to $m?~
End

#SOCIAL
Name        welcome~
CharNoArg   You try to make everyone feel welcome.~
OthersNoArg $n welcomes you.~
CharFound   You tell $M how truly welcome $E is.~
OthersFound $n makes $N feel very welcome.~
VictFound   $n makes you feel very welcome.~
CharAuto    You make yourself feel very welcome.  How nice of you!~
OthersAuto  $n welcomes $mself.  Hmmm.  Perhaps $e invited $mself too.~
End

#SOCIAL
Name        what~
CharNoArg   You ask, "what?"~
OthersNoArg $n asks, "what?"~
CharFound   You ask $N, "what?"~
OthersFound $n asks $N, "what?"~
VictFound   $n asks you, "what?"~
End

#SOCIAL
Name        whee~
CharNoArg   You jump up into the air and shout, "Wheeeee!"~
OthersNoArg $n jumps up into the air and hollers, "Wheeeee!"~
CharFound   You are so happy to see $N you jump into the air and holler, "Wheeeeeee!"~
OthersFound $n is so happy to see $N that $e jumps into the air and hollers, "Wheeeeee!"~
VictFound   $n is so happy to see you that $e jumps into the air and hollers, "Wheeeeee!"~
End

#SOCIAL
Name        when~
CharNoArg   You ask, "when?"~
OthersNoArg $n asks, "when?"~
CharFound   You ask $N, "when?"~
OthersFound $n asks $N, "when?"~
VictFound   $n asks you, "when?"~
End

#SOCIAL
Name        whimper~
CharNoArg   You whimper loud enough for the entire room to hear.~
OthersNoArg $n whimpers loudly.~
CharFound   You whimper in fear of $N.~
OthersFound $n whimpers in fear of $N.~
VictFound   $n whimpers in fear of you.~
CharAuto    You whimper quietly to yourself.~
OthersAuto  $n whimpers quietly in the corner.~
End

#SOCIAL
Name        whine~
CharNoArg   You whine like the great whiners of the century.~
OthersNoArg $n whines 'I want to be an immortal already.  I need more hitpoints..I...'~
CharFound   You whine to $M like the great whiners of the century.~
OthersFound $n whines to $N 'I want to be an immortal already.  I need more hp...I..'~
VictFound   $n whines to you 'I want to be an immortal already.  I need more hp...I...'~
CharAuto    You whine like the great whiners of the century.~
OthersAuto  $n whines 'I want to be an immortal already.  I need more hitpoints..I...'~
End

#SOCIAL
Name        whistle~
CharNoArg   You whistle appreciatively.~
OthersNoArg $n whistles appreciatively.~
CharFound   You whistle at the sight of $M.~
OthersFound $n whistles at the sight of $N.~
VictFound   $n whistles at the sight of you.~
CharAuto    You whistle a little tune to yourself.~
OthersAuto  $n whistles a little tune to $mself.~
End

#SOCIAL
Name        why~
CharNoArg   You look up at the heavens and cry out, "why? why? why?"~
OthersNoArg $n looks up at the heavens and cries out, "why? why? why?"~
CharFound   You ask $N, "why? why? why?"~
OthersFound $n asks $N, "why? why? why?"~
VictFound   $n asks you, "why? why? why?"~
CharAuto    You look up at the heavens and ask, "why me?"~
OthersAuto  $n looks up at the heavens and asks, "why me?"~
End

#SOCIAL
Name        wibble~
CharNoArg   You WiBblE!~
OthersNoArg $n WiBbLeS!~
CharFound   You WiBbLe $N!~
OthersFound $n WiBblEs $N!~
VictFound   $n WiBblEs you!~
CharAuto    You WiBbLe!~
OthersAuto  $n WiBbLeS!~
End

#SOCIAL
Name        wicked~
CharNoArg   Your face lights up as you exclaim, "Wicked!"~
OthersNoArg $n's face lights up as $e exclaims, "Wicked!"~
CharFound   You look at $N and exclaim, "Wicked!"~
OthersFound $n looks at $N and exclaims, "Wicked!"~
VictFound   $n looks at you and exclaims, "Wicked!"~
End

#SOCIAL
Name        wiggle~
CharNoArg   You wiggle your bottom.~
OthersNoArg $n wiggles $s bottom.~
CharFound   You wiggle your bottom toward $M.~
OthersFound $n wiggles $s bottom toward $N.~
VictFound   $n wiggles $s bottom towards you.~
CharAuto    You wiggle about like a fish.~
OthersAuto  $n wiggles about like a fish.~
End

#SOCIAL
Name        wince~
CharNoArg   You wince.  Ouch!~
OthersNoArg $n winces.  Ouch!~
CharFound   You wince at $M.~
OthersFound $n winces at $N.~
VictFound   $n winces at you.~
CharAuto    You wince at yourself.  Ouch!~
OthersAuto  $n winces at $mself.  Ouch!~
End

#SOCIAL
Name        wink~
CharNoArg   You wink suggestively.~
OthersNoArg $n winks suggestively.~
CharFound   You wink suggestively at $N.~
OthersFound $n winks at $N.~
VictFound   $n winks suggestively at you.~
CharAuto    You wink at yourself ?? - what are you up to ?~
OthersAuto  $n winks at $mself - something strange is going on...~
End

#SOCIAL
Name        witch~
CharNoArg   You announce to all that you are indeed a witch!~
OthersNoArg $n announces that $e is a witch. You wonder where that frog came from.~
CharFound   You whisper to $N that you are a witch. $N is afraid you will turn $M into a frog.~
OthersFound $n informs $N that $e is a witch and is trying to remember the spell to turn $N into a frog.~
VictFound   $n informs you $e is a witch and is trying to remember the spell to turn you into a frog.~
CharAuto    You think you are a witch? Hmmm ... you could be right.~
OthersAuto  $n thinks $e is a witch and $e probably is right.~
End

#SOCIAL
Name        women~
CharNoArg   You wonder why women go to the bathroom together.~
OthersNoArg $n ponders why women do the things they do.~
CharFound   You shrug in response to $N and say "women, can't live with em, can't shoot em and get away with it"~
OthersFound $n tries to explain the mystery of women and the way they think to $N, with no apparant luck.~
VictFound   $n says "Bah..women, I don't understand them," to you.~
CharAuto    You wish you could understand women, why do they do that??~
OthersAuto  $n tries to figure out women, get back to $m in a couple hundred years.~
End

#SOCIAL
Name        woohoo~
CharNoArg   You grin and shout 'WooHoo!!!'~
OthersNoArg $n grins and shouts 'WooHoo!!!'.~
CharFound   You grin at $N and shout 'WooHoo!!!'.~
OthersFound $n grins at $N and shouts 'WooHoo!!!'.~
VictFound   $n grins at you and shouts 'WooHoo!!!'.~
CharAuto    You shout 'WooHoo!!!'.~
OthersAuto  $n shouts 'WooHoo!!!'.~
End

#SOCIAL
Name        worship~
CharNoArg   You worship the powers that be.~
OthersNoArg $n worships the powers that be.~
CharFound   You drop to your knees in homage of $M.~
OthersFound $n prostrates $mself before $N.~
VictFound   $n believes you are all powerful.~
CharAuto    You worship yourself.~
OthersAuto  $n worships $mself - ah, the conceitedness of it all.~
End

#SOCIAL
Name        wow~
CharNoArg   Wow what?~
OthersNoArg $n says, "wow!"~
CharFound   You look at $N and say, "wow!"~
OthersFound $n looks at $N and says, "wow!"~
VictFound   $n looks at you and says, "wow!"~
CharAuto    You look at yourself in amazement.~
OthersAuto  $n seems to be amazed.~
End

#SOCIAL
Name        wretch~
CharNoArg   You feel so wretched.~
OthersNoArg A feeling of revulsion and depression emanates from $n.~
CharFound   You point out how sickly and miserable $N appears.~
OthersFound $n declares, "Look out! $N looks so sick I think $E is going to evacuate the contents of $S stomach!"~
VictFound   $n notices that you are shaking, shivering and your gorge is rising.~
CharAuto    You feel sick to your stomach.~
OthersAuto  Run! You can hear $N preparing to empty $S stomach.~
End

#SOCIAL
Name        wriggle~
CharNoArg   You wriggle your toes for all to see. Oooh! You sexy thing!~
OthersNoArg $n wriggles $s toes for all to see. Oooh! What a sexy thing!~
CharFound   You wriggle your sexy toes for $N. Ooooh! You sexy thing!~
OthersFound $n wriggles $s toes for $N. Oooh! What a sexy thing $e is!~
VictFound   $n wriggles $s toes for you. Oooh. What a sexy thing $e is!~
End

#SOCIAL
Name        yabba~
CharNoArg   You jump into the air and holler, "Yabba Dabba Doo!"~
OthersNoArg $n jumps up and hollers, "Yabba Dabba Doo!"~
CharFound   You look at $N and holler, "Yabba Dabba Doo!"~
OthersFound $n looks at $N and hollers, "Yabba Dabba Doo!"~
VictFound   $n looks at you and hollers, "Yabba Dabba Doo!"~
End

#SOCIAL
Name        yah~
CharNoArg   You jump into the air and holler, "Yah Hoo!"~
OthersNoArg $n jumps into the air and hollers, "Yah Hoo!"~
CharFound   You look at $N and holler, "Yah Hoo!"~
OthersFound $n looks at $N and hollers, "Yah Hoo!"~
VictFound   $n looks at you and hollers, "Yah Hoo!"~
End

#SOCIAL
Name        yarr~
CharNoArg   You make like a pirate and say "Yarr!"~
OthersNoArg $n makes like a pirate and says "Yarr!"~
CharFound   You wave your saber at $N and say "I be sendin' ya to the murky deep!"~
OthersFound $n waves $s saber at $N and says "I be sendin' ya to the murky deep!"~
VictFound   $n waves $s saber at you and says "I be sendin' ya to the murky deep!"~
CharAuto    You start to look for buried treasure.~
OthersAuto  $n starts to look for buried treasure.~
End

#SOCIAL
Name        yawn~
CharNoArg   You must be tired.~
OthersNoArg $n yawns.~
CharFound   You yawn widely in the middle of $S sentence.~
OthersFound $n yawns widely while $N prattles on.~
VictFound   $n yawns widely in the middle of your sentence.~
CharAuto    Even boring yourself now, eh?~
OthersAuto  $n yawns, apparently boring even $mself.~
End

#SOCIAL
Name        yee~
CharNoArg   You toss your hat in the air and holler, "Yee Haw!"~
OthersNoArg $n tosses $s hat in the air and hollers, "Yee Haw!"~
CharFound   You look at $N and holler, "Yee Haw!"~
OthersFound $n looks at $N and hollers, "Yee Haw!"~
VictFound   $n looks at you and hollers, "Yee Haw!"~
End

#SOCIAL
Name        yoga~
CharNoArg   You begin to do some yogic meditations.~
OthersNoArg $n begins to do some yogic meditations. Ooooo.~
CharFound   You begin to show $N some of your yogic flying.~
OthersFound $n begins showing $N some yogic flying techniques.~
VictFound   $n begins showing you some yogic flying techniques.~
CharAuto    You jump up in the air and begin yogic flying!~
OthersAuto  $n jumps up in the air and begins to yoga fly around the room.~
End

#SOCIAL
Name        yoyo~
CharNoArg   You whip out a yoyo and begin to play with it.~
OthersNoArg $n whips out a yoyo and begins to play with it.~
CharFound   You look at $N and think, "What a yoyo!"~
OthersFound $n looks at $N and thinks, "What a yoyo!"~
VictFound   $n looks at you and thinks, "What a yoyo!"~
CharAuto    You wish you were a yoyo? Get a grip!~
OthersAuto  $n wishes $e was a yoyo. $n is a nutbar!~
End

#SOCIAL
Name        yummy~
CharNoArg   Your rub your tummy and say "Yummy!".~
OthersNoArg $n rubs $s tummy and exclaims, "Yummy!"~
CharFound   You look at $N and exclaim, "Yummy!"~
OthersFound $n looks at $N and exclaims, "Yummy!"~
VictFound   $n looks at you and exclaims, "Yummy!"~
CharAuto    You think you are yummy? How conceited!~
OthersAuto  $n thinks $e is yummy. How conceited!~
End

#SOCIAL
Name        zombie~
CharNoArg   You shuffle through the room, a dazed look on your face, gurgling randomly.  Maybe it's time to log off...~
OthersNoArg $n shuffles through the room, a dazed look on $s face, gurgling randomly. Maybe it's time to logoff...~
CharFound   You eye $N hungrily, uttering the word, 'Brraaaaaiiiinnnssss...'~
OthersFound $n eyes $N hungrily, and utters the word 'Braaaaiiiinnnnnns...' Hrm, maybe it's time to get outta here.~
VictFound   $n eyes you hungrily, and utters the word 'Braaaaiiiinnnnnsss...' Better leave before $e turns on you...~
CharAuto    You look into the mirror and see a hallow eyed, slack jawed, drooling, sleep deprived creature staring back at you.~
OthersAuto  $n wanders around the room, a slack jawed, drooling, sleep deprived creature, mumbling something about brains...~
End

#SOCIAL
Name        zuh~
CharNoArg   You look around, completely confused.~
OthersNoArg $n looks around, a bewildered expression on $s face.~
CharFound   $N's actions have you at a complete loss.~
OthersFound $n looks at $N and inquires 'Zuh?'~
VictFound   $n looks at you, dazed, and says 'Zuh?'~
CharAuto    You've confounded yourself again!~
OthersAuto  $n asks 'Zuh?' of no one in particular. Must be those voices again.~
End

#END
--]]

-- EOF