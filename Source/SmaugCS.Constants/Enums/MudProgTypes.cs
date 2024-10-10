using Library.Common.Attributes;

namespace SmaugCS.Constants.Enums;

public enum MudProgTypes
{
    Error = -1,

    [Name("in_file_prog")]
    InFile = -2,

    None = 0,

    [Name("act_prog")]
    Act = 1,

    [Name("speech_prog")]
    Speech,

    [Name("rand_prog")]
    Random,

    [Name("fight_prod")]
    Fight,

    [Name("death_prog")]
    Death,

    [Name("hitprcnt_prog")]
    HitPercent,

    [Name("entry_prog")]
    Entry,

    [Name("greet_prog")]
    Greet,

    [Name("all_greet_prog")]
    GreetAll,

    [Name("give_prog")]
    Give,

    [Name("bribe_prog")]
    Bribe,

    [Name("hour_prog")]
    Hour,

    [Name("time_prog")]
    Time,

    [Name("wear_prog")]
    Wear,

    [Name("remove_prog")]
    Remove,

    [Name("sac_prog")]
    Sacrifice,

    [Name("look_prog")]
    Look,

    [Name("exa_prog")]
    Examine,

    [Name("zap_prog")]
    Zap,

    [Name("get_prog")]
    Get,

    [Name("drop_prog")]
    Drop,

    [Name("damage_prog")]
    Damage,

    [Name("repair_prog")]
    Repair,

    [Name("randiw_prog")]
    RANDIW_PROG,

    [Name("speechiw_prog")]
    SPEECHIW_PROG,

    [Name("pull_prog")]
    Pull,

    [Name("push_prog")]
    Push,

    [Name("sleep_prog")]
    Sleep,

    [Name("rest_prog")]
    Rest,

    [Name("leave_prog")]
    Leave,

    [Name("script_prog")]
    Script,

    [Name("use_prog")]
    Use,

    [Name("sell_prog")]
    Sell,

    [Name("tell_prog")]
    Tell,

    [Name("command_prog")]
    Command
}