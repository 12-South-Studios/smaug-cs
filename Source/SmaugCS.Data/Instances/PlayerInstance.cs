using SmaugCS.Constants.Enums;
using System;
using System.Collections.Generic;

namespace SmaugCS.Data.Instances;

public class PlayerInstance(int id, string name) : CharacterInstance(id, name)
{
    public DescriptorData Descriptor { get; set; }
    public PlayerData PlayerData { get; set; }
    public EditorData CurrentEditor { get; set; }
    public CharacterSubStates SubState { get; set; }
    public override int Trust { get; set; }
    public int PlayedDuration => (int)(DateTime.Now.ToFileTimeUtc() - LoggedOn.ToFileTimeUtc()) / 3600;
    public DateTime LoggedOn => Descriptor.User.ConnectedOn;
    public int TotalPlayedTime { get; set; }
    public DateTime save_time { get; set; }
    public Dictionary<ATTypes, char> Colors { get; private set; } = new();
    public CharacterInstance ReplyTo { get; set; }
    public CharacterInstance RetellTo { get; set; }

    public override bool IsImmortal(int level = 51) => Trust >= level;

    public override bool IsHero(int hero = 50) => Trust >= hero;

    public int tempnum { get; set; }
}