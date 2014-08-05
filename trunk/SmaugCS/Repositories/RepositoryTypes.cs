using SmaugCS.Attributes;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Language;

namespace SmaugCS.Repositories
{
    public enum RepositoryTypes
    {
        [TypeMap(Repository = typeof(RoomRepository), Object = typeof(RoomTemplate))]
        Rooms,

        [TypeMap(Repository = typeof(ObjectRepository), Object = typeof(ObjectTemplate))]
        ObjectTemplates,

        [TypeMap(Object = typeof(ObjectInstance))]
        ObjectInstances,

        [TypeMap(Repository = typeof(MobileRepository), Object = typeof(MobTemplate))]
        MobileTemplates,

        MobileInstances,

        [TypeMap(Repository = typeof(AreaRepository), Object = typeof(AreaData))]
        Areas,

        [TypeMap(Repository = typeof(CharacterRepository), Object = typeof(CharacterInstance))]
        Characters,

        [TypeMap(Repository = typeof(GenericRepository<LiquidData>), Object = typeof(LiquidData))]
        Liquids,

        [TypeMap(Repository = typeof(GenericRepository<HerbData>), Object = typeof(HerbData))]
        Herbs,

        [TypeMap(Repository = typeof(GenericRepository<SkillData>), Object = typeof(SkillData))]
        Skills,

        [TypeMap(Repository = typeof(GenericRepository<SpecialFunction>), Object = typeof(SpecialFunction))]
        SpecFuns,

        [TypeMap(Repository = typeof(GenericRepository<CommandData>), Object = typeof(CommandData))]
        Commands,

        [TypeMap(Repository = typeof(GenericRepository<SocialData>), Object = typeof(SocialData))]
        Socials,

        [TypeMap(Repository = typeof(GenericRepository<RaceData>), Object = typeof(RaceData))]
        Races,

        [TypeMap(Repository = typeof(GenericRepository<ClassData>), Object = typeof(ClassData))]
        Classes,

        [TypeMap(Repository = typeof(GenericRepository<DeityData>), Object = typeof(DeityData))]
        Deities,

        [TypeMap(Repository = typeof(GenericRepository<LanguageData>), Object = typeof(LanguageData))]
        Languages,

        [TypeMap(Repository = typeof(GenericRepository<ClanData>), Object = typeof(ClanData))]
        Clans,

        [TypeMap(Repository = typeof(GenericRepository<CouncilData>), Object = typeof(CouncilData))]
        Councils,

        [TypeMap(Repository = typeof(GenericRepository<HintData>), Object = typeof(HintData))]
        Hints,

        [TypeMap(Repository = typeof(GenericRepository<MixtureData>), Object = typeof(MixtureData))]
        Mixtures,

        [TypeMap(Repository = typeof(GenericRepository<PlaneData>), Object = typeof(PlaneData))]
        Planes,

        [TypeMap(Repository = typeof(GenericRepository<MorphData>), Object = typeof(MorphData))]
        Morphs
    }
}
