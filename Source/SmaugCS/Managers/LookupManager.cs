﻿using System;
using System.Collections.Generic;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Interfaces;
using SmaugCS.Lookup;

namespace SmaugCS.Managers;

public sealed class LookupManager : ILookupManager
{
  private static Dictionary<string, List<string>> _lookupTable;

  public LookupManager()
  {
    _lookupTable = new Dictionary<string, List<string>>();
    CommandLookup = new CommandLookupTable();
    SkillLookup = new SkillLookupTable();
    SpellLookup = new SpellLookupTable();
    StatModLookup = new Dictionary<string, List<StatModLookup>>();
  }

  public LookupBase<CommandData, DoFunction> CommandLookup { get; }
  public LookupBase<SkillData, DoFunction> SkillLookup { get; }
  public LookupBase<SkillData, SpellFunction> SpellLookup { get; }
  public Dictionary<string, List<StatModLookup>> StatModLookup { get; }

  public ResistanceTypes GetResistanceType(SpellDamageTypes type)
  {
    DamageResistanceAttribute attrib = type.GetAttribute<DamageResistanceAttribute>();
    return attrib?.ResistanceType ?? ResistanceTypes.Unknown;
  }

  public void AddLookup(string table, string entry)
  {
    if (!_lookupTable.ContainsKey(table.ToLower()))
      _lookupTable[table.ToLower()] = [];

    List<string> lookups = _lookupTable[table.ToLower()];
    if (!lookups.Contains(entry))
      lookups.Add(entry);
  }

  public void RemoveLookup(string table, string entry)
  {
    if (_lookupTable.ContainsKey(table.ToLower()) && _lookupTable[table.ToLower()].Contains(entry))
      _lookupTable[table.ToLower()].Remove(entry);
  }

  public bool HasLookup(string table, string entry)
  {
    return _lookupTable.ContainsKey(table.ToLower()) && _lookupTable[table.ToLower()].Contains(entry);
  }

  public IEnumerable<string> GetLookups(string table)
  {
    return _lookupTable.ContainsKey(table.ToLower()) ? _lookupTable[table.ToLower()] : new List<string>();
  }

  public string GetLookup(string table, int index)
  {
    IEnumerable<string> lookups = GetLookups(table);
    if (lookups.Any() && lookups.Count() > index)
      return lookups.ElementAt(index);
    return string.Empty;
  }

  public object GetStatMod(string category, int level, Enum name)
  {
    if (!StatModLookup.TryGetValue(category, out List<StatModLookup> value)) return null;

    List<StatModLookup> lookups = value;
    if (level < 0 || level >= lookups.Count)
      throw new Exception();

    return lookups[level].GetLookup(name.ToString());
  }
}