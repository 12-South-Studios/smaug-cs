using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Config;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants.Constants
{
    /// <summary>
    /// Static class that contains system constants for the MUD
    /// </summary>
    public static class SystemConstants
    {
        private static readonly Dictionary<SystemDirectoryTypes, string> SystemDirectories =
            new Dictionary<SystemDirectoryTypes, string>();

        private static readonly Dictionary<SystemFileTypes, KeyValuePair<string, bool>> SystemFiles =
            new Dictionary<SystemFileTypes, KeyValuePair<string, bool>>();

        private static readonly string[] BooleanConstants = {"true", "false", "1", "0", "yes", "no"};

        public static string GetSystemDirectory(string directory)
        {
            var dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(directory);
            return GetSystemDirectory(dirType);
        }

        public static string GetSystemDirectory(SystemDirectoryTypes directory)
        {
            return SystemDirectories.ContainsKey(directory) ? SystemDirectories[directory] : string.Empty;
        }

        public static string GetSystemFile(string file)
        {
            var fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(file);
            return GetSystemFile(fileType);
        }

        public static string GetSystemFile(SystemFileTypes file)
        {
            if (SystemFiles.ContainsKey(file))
            {
                var kvp = SystemFiles[file];
                return kvp.Value
                    ? GetSystemDirectory(SystemDirectoryTypes.System) + kvp.Key
                    : kvp.Key;
            }
            return string.Empty;
        }

        [Obsolete("No longer loaded from a data file, now stored in app.Config")]
        public static void LoadSystemDirectoriesFromDataFile(string path)
        {
            using (var proxy = new TextReaderProxy(new StreamReader(path + "\\SystemDirectories.txt")))
            {
                while (!proxy.EndOfStream)
                {
                    var line = proxy.ReadLine().TrimEnd('~');
                    var words = line.Split(',');

                    var dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(words[0]);
                    SystemDirectories.Add(dirType, path + "\\" + words[1]);
                }
            }
        }

        [Obsolete("No longer loaded from a data file, now stored in app.Config")]
        public static void LoadSystemFilesFromDataFile(string path)
        {
            using (var proxy = new TextReaderProxy(new StreamReader(path + "\\SystemFiles.txt")))
            {
                while (!proxy.EndOfStream)
                {
                    var line = proxy.ReadLine().TrimEnd('~');
                    var words = line.Split(',');

                    var fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(words[0]);
                    var useSystemDirectory = Convert.ToBoolean(BooleanConstants.ContainsIgnoreCase(words[2]));

                    SystemFiles.Add(fileType, new KeyValuePair<string, bool>(words[1], useSystemDirectory));
                }
            }
        }

        public static int LoadSystemDirectoriesFromConfig(string path)
        {
            var section = (SystemDataConfigurationSection) ConfigurationManager.GetSection("SystemDataSection");
            var collection = section.SystemDirectories;

            foreach (SystemDirectoryElement element in collection)
            {
                var dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(element.Name);
                SystemDirectories.Add(dirType, path + "\\" + element.Path);
            }

            return SystemDirectories.Count;
        }

        public static int LoadSystemFilesFromConfig()
        {
            var section = (SystemDataConfigurationSection)ConfigurationManager.GetSection("SystemDataSection");
            var collection = section.SystemFiles;

            foreach (SystemFileElement element in collection)
            {
                var fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(element.Name);
                SystemFiles.Add(fileType, new KeyValuePair<string, bool>(element.Filename, element.UseSystemFolder));
            }

            return SystemFiles.Count;
        }
    }
}
