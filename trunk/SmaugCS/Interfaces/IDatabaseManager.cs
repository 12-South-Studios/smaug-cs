﻿using Realm.Library.Common;
using SmaugCS.Data;
using SmaugCS.Data.Organizations;
using SmaugCS.Language;
using SmaugCS.Logging;
using SmaugCS.Managers;
using SmaugCS.Repositories;

namespace SmaugCS.Interfaces
{
    public interface IDatabaseManager
    {
        ILogManager LogManager { get; }

        void AddToRepository<T>(T obj) where T : Entity;
        long GenerateNewId<T>() where T : Entity;

        T GetEntity<T>(int id) where T : class;
        T GetEntity<T>(string name) where T : class;
        T GetEntity<T>(long id) where T : class;

        GenericRepository<T> GetRepository<T>(RepositoryTypes type) where T : class;

        RoomRepository ROOMS { get; }
        AreaRepository AREAS { get; }
        ObjectRepository OBJECT_INDEXES { get; }
        MobileRepository MOBILE_INDEXES { get; }
        CharacterRepository CHARACTERS { get; }
        ObjInstanceRepository OBJECTS { get; }
        GenericRepository<LiquidData> LIQUIDS { get; }
        GenericRepository<SkillData> SKILLS { get; }
        GenericRepository<HerbData> HERBS { get; }
        GenericRepository<SpecialFunction> SPEC_FUNS { get; }
        GenericRepository<CommandData> COMMANDS { get; }
        GenericRepository<LanguageData> LANGUAGES { get; }
        GenericRepository<RaceData> RACES { get; }
        GenericRepository<ClassData> CLASSES { get; }
        GenericRepository<DeityData> DEITIES { get; }
        GenericRepository<SocialData> SOCIALS { get; }
        GenericRepository<ClanData> CLANS { get; }
        GenericRepository<CouncilData> COUNCILS { get; }
        GenericRepository<PlaneData> PLANES { get; }
        GenericRepository<MorphData> MORPHS { get; }
        GenericRepository<HintData> HINTS { get; }
    }
}
