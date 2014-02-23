using System.Collections.Generic;
using Realm.Library.Patterns.Repository;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Data.Interfaces;
using SmaugCS.Data.Organizations;
using SmaugCS.Data.Templates;
using SmaugCS.Language;
using SmaugCS.Logging;

namespace SmaugCS.Managers
{
    public interface IDatabaseManager
    {
        void Initialize(ILogManager logManager);
        void InitializeDatabase(bool copyOver);

        ITemplateRepository<RoomTemplate> ROOMS { get; }
        IRepository<long, AreaData> AREAS { get; }
        ITemplateRepository<ObjectTemplate> OBJECT_INDEXES { get; }
        
        ITemplateRepository<MobTemplate> MOBILE_INDEXES { get; }
        MobTemplate GetMobTemplate(int vnum);
        
        IInstanceRepository<CharacterInstance> CHARACTERS { get; }
        IInstanceRepository<ObjectInstance> OBJECTS { get; }
        
        IEnumerable<LiquidData> LIQUIDS { get; }
        void AddLiquid(LiquidData liquid);
        LiquidData GetLiquid(int vnum);
        LiquidData GetLiquid(string str);

        IEnumerable<SkillData> HERBS { get; }
        void AddHerb(SkillData herb);

        IEnumerable<SkillData> SKILLS { get; }
        SkillData GetSkill(int skillNumber);
        SkillData GetSkill(string name);
        IEnumerable<SkillData> GetSkills(SkillTypes type);
        int LookupSkill(string name);
        int AddSkill(string name);
        void AddSkill(SkillData skill);

        IEnumerable<SpecialFunction> SPEC_FUNS { get; }
        void AddSpecFun(SpecialFunction specfun);
        SpecialFunction GetSpecFun(string name);
        
        IEnumerable<CommandData> COMMANDS { get; }
        CommandData GetCommand(string command);
        void AddCommand(CommandData command);

        IEnumerable<LanguageData> LANGUAGES { get; }
        void AddLanguage(LanguageData lang);

        IEnumerable<RaceData> RACES { get; }
        void AddRace(RaceData race);

        IEnumerable<ClassData> CLASSES { get; }
        void AddClass(ClassData cls);
        ClassData GetClass(ClassTypes type);
        ClassData GetClass(string name);
        ClassData GetClass(int id);

        IEnumerable<DeityData> DEITIES { get; }
        void AddDeity(DeityData deity);

        IEnumerable<SocialData> SOCIALS { get; }
        SocialData GetSocial(string command);
        void AddSocial(SocialData social);

        IEnumerable<ClanData> CLANS { get; }
        void AddClan(ClanData clan);

        IEnumerable<CouncilData> COUNCILS { get; }
        void AddCouncil(CouncilData council);

    }
}
