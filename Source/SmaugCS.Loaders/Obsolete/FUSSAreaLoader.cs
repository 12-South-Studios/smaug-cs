using SmaugCS.Data;
using SmaugCS.Logging;

namespace SmaugCS.Loaders.Obsolete
{
    // ReSharper disable InconsistentNaming
    public class FUSSAreaLoader : AreaLoader
    // ReSharper restore InconsistentNaming
    {
        public FUSSAreaLoader(string areaName, bool bootDb)
            : base(areaName, bootDb)
        {
        }

        #region Overrides of AreaLoader

        public override AreaData LoadArea(ILogManager logManager, AreaData area)
        {
            /*using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(FilePath)))
            {
                string word = string.Empty;
                AreaData newArea = area;

                do
                {
                    char c = proxy.ReadNextLetter();
                    if (c == '*')
                    {
                        proxy.ReadToEndOfLine();
                        continue;
                    }

                    if (c != '#')
                    {
                        LogManager.Instance.Bug("LoadFUSSArea: # not found. Invalid format for Area File {0}", AreaName);
                        if (BootDb)
                            throw new InitializationException("Invalid format for Area File {0}", AreaName);
                        break;
                    }

                    word = proxy.EndOfStream ? "ENDAREA" : proxy.ReadNextWord();

                    switch (word.ToUpper())
                    {
                        case "AREADATA":
                            if (newArea == null)
                                area = CreateArea();
                            ReadAreaData(proxy, area);
                            break;
                        case "MOBILE":
                            // fread_fuss_mobile(proxy, area);
                            break;
                        case "OBJECT":
                            // fread_fuss_object(proxy, area);
                            break;
                        case "ROOM":
                            // fread_fuss_room(proxy, area);
                            break;
                        case "ENDAREA":
                            break;
                        default:
                            LogManager.Instance.Bug("Bad section header {0} in Area file {1}", word, AreaName);
                            proxy.ReadToEnd();
                            break;
                    }

                } while (!proxy.EndOfStream
                         && !word.Equals("ENDAREA", StringComparison.OrdinalIgnoreCase));

                return newArea;
            }*/
            return null;
        }

        #endregion

        /*private static ExtraDescriptionData ReadExtraDescription(TextReaderProxy proxy)
        {
            string word;
            ExtraDescriptionData ed = new ExtraDescriptionData();

            do
            {
                word = proxy.EndOfStream ? "#ENDEXDESC" : proxy.ReadNextWord();
                switch (word.ToLower())
                {
                    case "exdesckey":
                        ed.Keyword = proxy.ReadString();
                        break;
                    case "exdesc":
                        ed.Description = proxy.ReadString();
                        break;
                    case "#endexdesc":
                        if (string.IsNullOrEmpty(ed.Keyword))
                        {
                            LogManager.Instance.Bug("Missing Keyword");
                            return null;
                        }
                        break;
                }

            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#ENDEXDESC"));

            return ed;
        }

        private static AffectData ReadAffect(TextReaderProxy proxy, string word)
        {
            AffectData af = new AffectData();

            if (word.EqualsIgnoreCase("affect"))
            {
                af.Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>(proxy.ReadNumber());
            }
            else
            {
                string skillName = proxy.ReadNextWord();
                int sn = RepositoryManager.Instance.LookupSkill(skillName);
                if (sn < 0)
                    LogManager.Instance.Bug("Unknown skill {0}", skillName);
                else
                    af.Type = Realm.Library.Common.EnumerationExtensions.GetEnum<AffectedByTypes>(sn);
            }

            af.Duration = proxy.ReadNumber();
            int afMod = proxy.ReadNumber();
            af.Location = Realm.Library.Common.EnumerationExtensions.GetEnum<ApplyTypes>(proxy.ReadNumber());
            af.BitVector = proxy.ReadBitvector();

            if (af.Location == ApplyTypes.WeaponSpell
                || af.Location == ApplyTypes.WearSpell
                || af.Location == ApplyTypes.StripSN
                || af.Location == ApplyTypes.RemoveSpell
                || af.Location == ApplyTypes.RecurringSpell)
                af.Modifier = magic.slot_lookup(afMod);
            else
                af.Modifier = afMod;

            return af;
        }

        private void ReadExit(TextReaderProxy proxy, RoomTemplate room)
        {
            string word;
            ExitData exit = null;

            do
            {
                word = proxy.EndOfStream ? "#ENDEXIT" : proxy.ReadNextWord();
                switch (word.ToLower())
                {
                    case "#endexit":
                        return;
                    case "desc":
                        exit.Description = proxy.ReadString();
                        break;
                    case "distance":
                        exit.Distance = proxy.ReadNumber();
                        break;
                    case "direction":
                        int door = (int)build.get_dir(proxy.ReadFlagString());
                        if (door < 0 || door > (int)DirectionTypes.Somewhere)
                        {
                            LogManager.Instance.Bug("Vnum {0} has bad door number {1}", room.Vnum, door);
                            if (BootDb)
                                return;
                        }
                        exit = db.make_exit(room, null, door);
                        break;
                    case "flags":
                        string exitflags = proxy.ReadFlagString();

                        foreach (char c in exitflags)
                        {
                            int value = FlagLookup.get_exflag(c.ToString(CultureInfo.InvariantCulture));
                            if (value < 0 || value > 31)
                                LogManager.Instance.Bug("Unknown exit flag {0}", c);
                            else
                                exit.Flags.SetBit(1 << value);
                        }
                        break;
                    case "key":
                        exit.Key = proxy.ReadNumber();
                        break;
                    case "keywords":
                        exit.Keywords = proxy.ReadString();
                        break;
                    case "pull":
                        exit.PullType = Realm.Library.Common.EnumerationExtensions.GetEnum<DirectionPullTypes>(proxy.ReadNumber());
                        exit.Pull = proxy.ReadNumber();
                        break;
                    case "toroom":
                        //exit.vnum = proxy.ReadNumber();
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#ENDEXIT"));

            if (exit != null)
                handler.extract_exit(room, exit);
        }

        private static void ReadAreaData(TextReaderProxy proxy, AreaData area)
        {
            string word;

            do
            {
                word = proxy.EndOfStream ? "#ENDAREADATA" : proxy.ReadNextWord();
                switch (word.ToLower())
                {
                    case "#endareadata":
                        area.Age = area.ResetFrequency;
                        return;
                    case "author":
                        area.Author = proxy.ReadString().TrimHash();
                        break;
                    case "credits":
                        area.Credits = proxy.ReadString().TrimHash();
                        break;
                    case "economy":
                        area.HighEconomy = proxy.ReadNumber();
                        area.LowEconomy = proxy.ReadNumber();
                        break;
                    case "flags":
                        string flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_areaflag(c.ToString(CultureInfo.InvariantCulture));
                            if (value < 0 || value > 31)
                                LogManager.Instance.Bug("Unknown flag {0}", c);
                            else
                                area.Flags.SetBit(1 << value);
                        }
                        break;
                    case "name":
                        // area.Name = proxy.ReadString();
                        break;
                    case "ranges":
                        string[] words = proxy.ReadString().Split(new[] { ' ' });
                        area.LowSoftRange = words[0].ToInt32();
                        area.HighSoftRange = words[1].ToInt32();
                        area.LowHardRange = words[2].ToInt32();
                        area.HighHardRange = words[3].ToInt32();
                        break;
                    case "resetmsg":
                        area.ResetMessage = proxy.ReadString().TrimHash();
                        break;
                    case "resetfreq":
                        area.ResetFrequency = proxy.ReadNumber();
                        break;
                    case "spelllimit":
                        area.SpellLimit = proxy.ReadNumber();
                        break;
                    case "version":
                        area.Version = proxy.ReadNumber();
                        break;
                    case "weatherx":
                        area.WeatherX = proxy.ReadNumber();
                        break;
                    case "weathery":
                        area.WeatherY = proxy.ReadNumber();
                        break;
                    default:
                        LogManager.Instance.Log("Area {0} found no matching value {1}", area.Filename, word);
                        proxy.ReadToEndOfLine();
                        break;
                }

            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#ENDAREADATA"));
        }

        private static MudProgData ReadMudProg(TextReaderProxy proxy, Template index)
        {
            string word;
            MudProgData prog = new MudProgData();

            do
            {
                word = proxy.EndOfStream ? "#ENDEXIT" : proxy.ReadNextWord();
                switch (word.ToLower())
                {
                    case "#endprog":
                        break;
                    case "arglist":
                        prog.ArgList = proxy.ReadString();
                        prog.IsFileProg = false;

                        if (prog.Type == MudProgTypes.InFile)
                        {
                            if (index is RoomTemplate)
                                db.rprog_file_read((RoomTemplate)index, prog.ArgList);
                            else if (index is ObjectTemplate)
                                db.oprog_file_read((ObjectTemplate)index, prog.ArgList);
                            else if (index is MobTemplate)
                                db.mprog_file_read((MobTemplate)index, prog.ArgList);
                        };
                        break;
                    case "comlist":
                        prog.Script = proxy.ReadString();
                        break;
                    case "progtype":
                        prog.Type =
                            Realm.Library.Common.EnumerationExtensions.GetEnum<MudProgTypes>(db.mprog_name_to_type(proxy.ReadFlagString()));
                        //index.ProgTypes.SetBit((int)prog.Type);
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#endprog"));

            return prog;
        }

        private void ReadMobile(TextReaderProxy proxy, AreaData area)
        {
            string word;
            MobTemplate mob = null;

            do
            {
                word = proxy.EndOfStream ? "#ENDMOBILE" : proxy.ReadNextWord();
                string flags;
                string[] words;

                switch (word.ToLower())
                {
                    case "#endmobile":
                        RepositoryManager.Instance.MOBILE_INDEXES.CastAs<Repository<long, MobTemplate>>().Add(mob.Vnum, mob);
                        break;
                    case "#mudprog":
                        MudProgData prog = ReadMudProg(proxy, mob);
                        if (prog != null)
                            mob.MudProgs.Add(prog);
                        break;
                    case "actflags":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_actflag(c.ToString(CultureInfo.InvariantCulture));
                            //if (value < 0 || value >= ExtendedBitvector.MAX_BITS)
                            //    LogManager.Instance.Bug("Unknown flag {0}", c);
                            //else
                            //    mob.Act.SetBit(value);
                        }
                        break;
                    case "affected":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_aflag(c.ToString(CultureInfo.InvariantCulture));
                            //if (value < 0 || value >= ExtendedBitvector.MAX_BITS)
                            //    LogManager.Instance.Bug("Unknown flag {0}", c);
                            // else
                            //     mob.AffectedBy.SetBit(value);
                        }
                        break;
                    case "attacks":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_attackflag(c.ToString(CultureInfo.InvariantCulture));
                            //if (value < 0 || value >= ExtendedBitvector.MAX_BITS)
                            //    LogManager.Instance.Bug("Unknown flag {0}", c);
                            //else
                            //    mob.Attacks.SetBit(value);
                        }
                        break;
                    case "attribs":
                        words = proxy.ReadString().Split(new[] { ' ' });

                        mob.Statistics[StatisticTypes.Strength] = words[0].ToInt32();
                        mob.Statistics[StatisticTypes.Intelligence] = words[1].ToInt32();
                        mob.Statistics[StatisticTypes.Wisdom] = words[2].ToInt32();
                        mob.Statistics[StatisticTypes.Dexterity] = words[3].ToInt32();
                        mob.Statistics[StatisticTypes.Constitution] = words[4].ToInt32();
                        mob.Statistics[StatisticTypes.Charisma] = words[5].ToInt32();
                        mob.Statistics[StatisticTypes.Luck] = words[6].ToInt32();
                        break;
                    case "bodyparts":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_partflag(c.ToString(CultureInfo.InvariantCulture));
                            if (value < 0 || value > 31)
                                LogManager.Instance.Bug("Unknown flag {0}", c);
                            else
                                mob.ExtraFlags.SetBit(1 << value);
                        }
                        break;
                    case "class":
                        int npcClass = build.get_npc_class(proxy.ReadString().TrimHash());
                        if (npcClass < 0 || npcClass >= LookupManager.Instance.GetLookups("NPCClasses").Count())
                        {
                            LogManager.Instance.Bug("Vnum {0} has invalid class {1}", mob.Vnum, npcClass);
                            npcClass = build.get_npc_class("warrior");
                        }
                        //mob.Class = npcClass;
                        break;
                    case "defenses":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_defenseflag(c.ToString(CultureInfo.InvariantCulture));
                            //if (value < 0 || value >= ExtendedBitvector.MAX_BITS)
                            //    LogManager.Instance.Bug("Unknown flag {0}", c);
                            // else
                            //    mob.Defenses.SetBit(value);
                        }
                        break;
                    case "defpos":
                        break;
                    case "desc":
                        mob.Description = proxy.ReadString().TrimHash();
                        break;
                    case "gender":
                        break;
                    case "immune":
                        break;
                    case "keywords":
                        //mob.Name = proxy.ReadString().TrimHash();
                        break;
                    case "long":
                        mob.LongDescription = proxy.ReadString().TrimHash();
                        break;
                    case "position":
                        break;
                    case "race":
                        break;
                    case "repairdata":
                        break;
                    case "resist":
                        break;
                    case "saves":
                        break;
                    case "short":
                        mob.ShortDescription = proxy.ReadString().TrimHash();
                        break;
                    case "shopdata":
                        break;
                    case "speaks":
                        break;
                    case "speaking":
                        break;
                    case "specfun":
                        break;
                    case "stats1":
                        break;
                    case "stats2":
                        break;
                    case "stats3":
                        break;
                    case "stats4":
                        break;
                    case "suscept":
                        break;
                    case "vnum":
                        break;
                }

            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#endmobile"));
        }

        private void ReadRoom(TextReaderProxy proxy, AreaData area)
        {
            string word;
            RoomTemplate room = null;

            do
            {
                word = proxy.EndOfStream ? "#ENDROOM" : proxy.ReadNextWord();
                switch (word.ToLower())
                {
                    case "#endroom":
                        RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Add(room.Vnum, room);
                        return;
                    case "#exit":
                        ReadExit(proxy, room);
                        break;
                    case "#exadesc":
                        ExtraDescriptionData ed = ReadExtraDescription(proxy);
                        if (ed != null)
                            room.ExtraDescriptions.Add(ed);
                        break;
                    case "#mudprog":
                        MudProgData prog = ReadMudProg(proxy, room);
                        if (prog != null)
                            room.MudProgs.Add(prog);
                        break;
                    case "affect":
                    case "affectdata":
                        AffectData af = ReadAffect(proxy, word);
                        if (af != null)
                            room.PermanentAffects.Add(af);
                        break;
                    case "desc":
                        room.Description = proxy.ReadString();
                        break;
                    case "flags":
                        string flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_rflag(c.ToString(CultureInfo.InvariantCulture));
                           // if (value < 0 || value > ExtendedBitvector.MAX_BITS)
                            //    LogManager.Instance.Bug("Unknown flag {0}", c);
                           // else
                                room.Flags.SetBit(value);
                        }
                        break;
                    case "name":
                        //room.Name = proxy.ReadString();
                        break;
                    case "reset":
                        // TODO load room reset
                        break;
                    case "sector":
                        int sector = FlagLookup.get_secflag(proxy.ReadFlagString());
                        if (sector < 0 || sector > EnumerationFunctions.MaximumEnumValue<SectorTypes>())
                        {
                            LogManager.Instance.Bug("Room {0} has bad sector type {1}", room.Vnum, sector);
                            sector = 1;
                        }

                        room.SectorType = Realm.Library.Common.EnumerationExtensions.GetEnum<SectorTypes>(sector);
                        break;
                    case "stats":
                        string[] words = proxy.ReadString().Split(new[] { ' ' });
                        room.TeleportDelay = words[0].ToInt32();
                        room.TeleportToVnum = words[1].ToInt32();
                        room.Tunnel = words[2].ToInt32();
                        break;
                    case "vnum":
                        //bool tmpBootDb = DatabaseManager.BootDb;
                        int vnum = proxy.ReadNumber();
                        if (RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(vnum) != null)
                        {
                            //if (tmpBootDb)
                            //{
                            //    LogManager.Instance.Bug("Vnum {0} duplicated", vnum);
                            //    DatabaseManager.BootDb = false;
                            //}
                            //else
                            //{
                                room = RepositoryManager.Instance.ROOMS.CastAs<Repository<long, RoomTemplate>>().Get(vnum);
                                LogManager.Instance.Log(LogTypes.Build,
                                               GameManager.Instance.SystemData.GetMinimumLevel(PlayerPermissionTypes.BuildLevel),
                                               "Cleaning room {0}", vnum);
                                handler.clean_room(room);
                            //}
                        }
                        else
                        {
                            room = new RoomTemplate(0, "");
                        }

                        //room.Vnum = vnum;
                        room.Area = area;
                       // DatabaseManager.BootDb = tmpBootDb;

                        //if (DatabaseManager.BootDb)
                        //{
                        //    if (area.LowRoomNumber == 0)
                       //         area.LowRoomNumber = vnum;
                       //     if (area.HighRoomNumber > 0)
                       //         area.HighRoomNumber = vnum;
                       // }
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#endroom"));
        }

        private void ReadObject(TextReaderProxy proxy, AreaData area)
        {
            string word;
            ObjectTemplate obj = null;

            do
            {
                word = proxy.EndOfStream ? "#ENDOBJECT" : proxy.ReadNextWord();
                string[] words;
                string flags;
                switch (word.ToLower())
                {
                    case "#endobject":
                        RepositoryManager.Instance.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Add(obj.Vnum, obj);
                        return;
                    case "#exdesc":
                        ExtraDescriptionData ed = ReadExtraDescription(proxy);
                        if (ed != null)
                            obj.ExtraDescriptions.Add(ed);
                        break;
                    case "#mudprog":
                        MudProgData prog = ReadMudProg(proxy, obj);
                        if (prog != null)
                            obj.MudProgs.Add(prog);
                        break;
                    case "action":
                        obj.Action = proxy.ReadString().TrimHash();
                        break;
                    case "affect":
                    case "affectdata":
                        AffectData af = ReadAffect(proxy, word);
                        if (af != null)
                            obj.Affects.Add(af);
                        break;
                    case "flags":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_oflag(c.ToString(CultureInfo.InvariantCulture));
                            //if (value < 0 || value >= ExtendedBitvector.MAX_BITS)
                            //    LogManager.Instance.Bug("Unknown flag {0}", c);
                           // else
                                obj.ExtraFlags.SetBit(value);
                        }
                        break;
                    case "keywords":
                        // obj.Name = proxy.ReadString().TrimHash();
                        break;
                    case "long":
                        obj.Description = proxy.ReadString().TrimHash();
                        break;
                    case "short":
                        obj.ShortDescription = proxy.ReadString().TrimHash();
                        break;
                    case "spells":
                        if (obj.Type == ItemTypes.Pill
                            || obj.Type == ItemTypes.Potion
                            || obj.Type == ItemTypes.Scroll)
                        {
                            obj.Value.ToList()[1] = RepositoryManager.Instance.LookupSkill(proxy.ReadNextWord());
                            obj.Value.ToList()[2] = RepositoryManager.Instance.LookupSkill(proxy.ReadNextWord());
                            obj.Value.ToList()[3] = RepositoryManager.Instance.LookupSkill(proxy.ReadNextWord());
                            break;
                        }
                        if (obj.Type == ItemTypes.Staff
                            || obj.Type == ItemTypes.Wand)
                        {
                            obj.Value.ToList()[3] = RepositoryManager.Instance.LookupSkill(proxy.ReadNextWord());
                            break;
                        }
                        if (obj.Type == ItemTypes.Salve)
                        {
                            obj.Value.ToList()[4] = RepositoryManager.Instance.LookupSkill(proxy.ReadNextWord());
                            obj.Value.ToList()[5] = RepositoryManager.Instance.LookupSkill(proxy.ReadNextWord());
                        }
                        break;
                    case "stats":
                        words = proxy.ReadString().Split(new[] { ' ' });
                        obj.Weight = words[0].ToInt32();
                        obj.Cost = words[1].ToInt32();
                        obj.Rent = words[2].ToInt32();
                        obj.Level = words[3].ToInt32();
                        obj.Layers = words[4].ToInt32();
                        break;
                    case "type":
                        int otype = FlagLookup.get_otype(proxy.ReadFlagString());
                        if (otype < 0)
                        {
                            LogManager.Instance.Bug("Vnum {0} object has invalid type {1}", obj.Vnum, otype);
                            otype = FlagLookup.get_otype("trash");
                        }
                        obj.Type = Realm.Library.Common.EnumerationExtensions.GetEnum<ItemTypes>(otype);
                        break;
                    case "values":
                        words = proxy.ReadString().Split(new[] { ' ' });
                        obj.Value.ToList()[0] = words[0].ToInt32();
                        obj.Value.ToList()[1] = words[1].ToInt32();
                        obj.Value.ToList()[2] = words[2].ToInt32();
                        obj.Value.ToList()[3] = words[3].ToInt32();
                        obj.Value.ToList()[4] = words[4].ToInt32();
                        obj.Value.ToList()[5] = words[5].ToInt32();
                        break;
                    case "vnum":
                       // bool tmpBootDb = DatabaseManager.BootDb;
                        int vnum = proxy.ReadNumber();
                        if (RepositoryManager.Instance.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Get(vnum) != null)
                        {
                            //if (tmpBootDb)
                            //{
                           //     LogManager.Instance.Bug("Vnum {0} duplicated", vnum);
                                //DatabaseManager.BootDb = false;
                            //}
                            //else
                            //{
                                obj = RepositoryManager.Instance.OBJECT_INDEXES.CastAs<Repository<long, ObjectTemplate>>().Get(vnum);
                                LogManager.Instance.Log(LogTypes.Build,
                                               GameManager.Instance.SystemData.GetMinimumLevel(PlayerPermissionTypes.BuildLevel),
                                               "Cleaning object {0}", vnum);
                                handler.clean_obj(obj);
                            //}
                        }
                        else
                        {
                            obj = new ObjectTemplate(0, "");
                        }

                        //obj.Vnum = vnum;
                        //DatabaseManager.BootDb = tmpBootDb;

                        //if (DatabaseManager.BootDb)
                        //{
                         //   if (area.LowObjectNumber == 0)
                           //     area.LowObjectNumber = vnum;
                           // if (area.HighObjectNumber > 0)
                            //    area.HighObjectNumber = vnum;
                        //}
                        break;
                    case "wflags":
                        flags = proxy.ReadFlagString();

                        foreach (char c in flags)
                        {
                            int value = FlagLookup.get_wflag(c.ToString(CultureInfo.InvariantCulture));
                            if (value < 0 || value > 31)
                                LogManager.Instance.Bug("Unknown flag {0}", c);
                            //else
                            //    obj.WearFlags.SetBit(1 << value);
                        }
                        break;
                }
            } while (!proxy.EndOfStream && !word.EqualsIgnoreCase("#endobject"));
        }*/
    }
}