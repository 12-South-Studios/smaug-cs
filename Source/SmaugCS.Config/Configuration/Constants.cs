namespace SmaugCS.Config.Configuration;

public class Constants
{
    public int MinNameLength { get; set; }
    public int MaxNameLength { get; set; }
    public int MaximumLayers { get; set; }
    public int MaximumWearLocations { get; set; }
    public int MaximumExperienceValue { get; set; }
    public int MinimumExperienceValue { get; set; }
    public int MaximumLevel { get; set; }
    public int MaximumBufferLines { get; set; }
    public int MaximumConditionValue { get; set; }
    public int MudProgMaxIfArgs { get; set; }
    public double AffectDurationConversionValue { get; set; }
    public int LogDumpFrequencyMS { get; set; }
    public int BanExpireFrequencyMS { get; set; }
    public int WeatherHeight { get; set; }
    public int WeatherWidth { get; set; }
    public int InitWeaponCondition { get; set; }
    public int MinimumAuctionLevel { get; set; }
    public int AuctionPulseSeconds { get; set; }
    public int MinimumDevotionLevel { get; set; }
    public int MaximumAuctionBid { get; set; }
    public int DefaultMaximumHealth { get; set; }
    public int DefaultMaximumMana { get; set; }
    public int DefaultMaximumMovement { get; set; }

    public string AppPath { get; set; }
    public int ReadAllMail { get; set; }
    public int ReadMailFree { get; set; }
    public int WriteMailFree { get; set; }
    public int TakeOthersMail { get; set; }
    public int Muse { get; set; }
    public int Think { get; set; }
    public int Builder { get; set; }
    public int Log { get; set; }
    public int ProtoFlag { get; set; }
    public int OverridePrivate { get; set; }
    public int MSetPlayer { get; set; }
    public int StunPlayerVsPlayer { get; set; }
    public int StunRegular { get; set; }
    public int GougePlayerVsPlayer { get; set; }
    public int GougeNonTank { get; set; }
    public int BashPlayerVsPlayer { get; set; }
    public int BashNonTank { get; set; }
    // damagemod
    // parrymod
    // tumblemod
    public int ForcePlayer { get; set; }
    public int SaveFlags { get; set; }
    public int SaveFrequency { get; set; }
    public int BestowDiff { get; set; }
    public int BanSiteLevel { get; set; }
    public int BanRaceLevel { get; set; }
    public int BanClassLevel { get; set; }
    public bool MorphOpt { get; set; }
    public bool PetSave { get; set; }
    public bool PKLoot { get; set; }
}