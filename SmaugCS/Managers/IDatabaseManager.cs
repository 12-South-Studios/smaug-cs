using System.Collections.Generic;
using Realm.Library.Patterns.Repository;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Organizations;
using SmaugCS.Data.Templates;
using SmaugCS.Language;

namespace SmaugCS.Managers
{
    public interface IDatabaseManager
    {
        ITemplateRepository<RoomTemplate> ROOMS { get; }
        IRepository<long, AreaData> AREAS { get; }
        ITemplateRepository<ObjectTemplate> OBJECT_INDEXES { get; }
        ITemplateRepository<MobTemplate> MOBILE_INDEXES { get; }
        IInstanceRepository<CharacterInstance> CHARACTERS { get; }
        IInstanceRepository<ObjectInstance> OBJECTS { get; }
        void Initialize(bool fCopyOver);

        IEnumerable<LiquidData> LIQUIDS { get; }
        IEnumerable<SkillData> HERBS { get; }
        IEnumerable<SkillData> SKILLS { get; }
        IEnumerable<SpecialFunction> SPEC_FUNS { get; }
        IEnumerable<CommandData> COMMANDS { get; }
        IEnumerable<LanguageData> LANGUAGES { get; }
        IEnumerable<RaceData> RACES { get; }
        IEnumerable<ClassData> CLASSES { get; }
        IEnumerable<DeityData> DEITIES { get; }
        IEnumerable<SocialData> SOCIALS { get; }
        IEnumerable<ClanData> CLANS { get; }
        IEnumerable<CouncilData> COUNCILS { get; } 
    }
}
