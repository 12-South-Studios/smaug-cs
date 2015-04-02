using System;
using System.Collections.Generic;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Instances
{
    public class PlayerInstance : CharacterInstance
    {
        public DescriptorData Descriptor { get; set; }
        public PlayerData PlayerData { get; set; }
        public EditorData CurrentEditor { get; set; }
        public CharacterSubStates SubState { get; set; }
        public override int Trust { get; set; }
        public int PlayedDuration { get { return (int)(DateTime.Now.ToFileTimeUtc() - LoggedOn.ToFileTimeUtc()) / 3600; } }
        public DateTime LoggedOn { get { return Descriptor.User.ConnectedOn; } }
        public int TotalPlayedTime { get; set; }
        public DateTime save_time { get; set; }
        public Dictionary<ATTypes, char> Colors { get; private set; }
        public CharacterInstance ReplyTo { get; set; }
        public CharacterInstance RetellTo { get; set; }

        public PlayerInstance(int id, string name) : base(id, name)
        {
            Colors = new Dictionary<ATTypes, char>();
        }

        public override bool IsImmortal(int level = 51)
        {
            return Trust >= level;
        }

        public override bool IsHero(int hero = 50)
        {
            return Trust >= hero;
        }

        public int tempnum { get; set; }
    }
}
