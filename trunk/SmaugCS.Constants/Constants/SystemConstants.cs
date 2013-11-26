using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Config;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants.Constants
{
    public static class SystemConstants
    {
        private static readonly Dictionary<SystemDirectoryTypes, string> SystemDirectories =
            new Dictionary<SystemDirectoryTypes, string>();

        private static readonly Dictionary<SystemFileTypes, KeyValuePair<string, bool>> SystemFiles =
            new Dictionary<SystemFileTypes, KeyValuePair<string, bool>>();

        private static readonly string[] BooleanConstants = new[] {"true", "false", "1", "0", "yes", "no"};

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetSystemDirectory(string directory)
        {
            var dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(directory);
            return GetSystemDirectory(dirType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static string GetSystemDirectory(SystemDirectoryTypes directory)
        {
            return SystemDirectories.ContainsKey(directory) ? SystemDirectories[directory] : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetSystemFile(string file)
        {
            var fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(file);
            return GetSystemFile(fileType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [Obsolete]
        public static void LoadSystemDirectoriesFromDataFile(string path)
        {
            using (var proxy = new TextReaderProxy(new StreamReader(path + "\\SystemDirectories.txt")))
            {
                while (!proxy.EndOfStream)
                {
                    var line = proxy.ReadLine().TrimEnd(new[] { '~' });
                    var words = line.Split(new[] { ',' });

                    var dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(words[0]);
                    SystemDirectories.Add(dirType, path + "\\" + words[1]);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        [Obsolete]
        public static void LoadSystemFilesFromDataFile(string path)
        {
            using (var proxy = new TextReaderProxy(new StreamReader(path + "\\SystemFiles.txt")))
            {
                while (!proxy.EndOfStream)
                {
                    var line = proxy.ReadLine().TrimEnd(new[] { '~' });
                    var words = line.Split(new[] { ',' });

                    var fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(words[0]);
                    var useSystemDirectory = Convert.ToBoolean(BooleanConstants.ContainsIgnoreCase(words[2]));

                    SystemFiles.Add(fileType, new KeyValuePair<string, bool>(words[1], useSystemDirectory));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public static int LoadSystemFilesFromConfig(string path)
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
