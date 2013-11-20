using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Enums;

namespace SmaugCS.Constants.Constants
{
    public static class SystemConstants
    {
        private static readonly Dictionary<SystemDirectoryTypes, string> SystemDirectories = new Dictionary<SystemDirectoryTypes, string>();

        public static string GetSystemDirectory(string directory)
        {
            SystemDirectoryTypes dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(directory);
            return GetSystemDirectory(dirType);
        }
        public static string GetSystemDirectory(SystemDirectoryTypes directory)
        {
            return SystemDirectories.ContainsKey(directory) ? SystemDirectories[directory] : string.Empty;
        }

        private static readonly Dictionary<SystemFileTypes, KeyValuePair<string, bool>> SystemFiles = new Dictionary<SystemFileTypes, KeyValuePair<string, bool>>();

        public static string GetSystemFile(string file)
        {
            SystemFileTypes fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(file);
            return GetSystemFile(fileType);
        }
        public static string GetSystemFile(SystemFileTypes file)
        {
            if (SystemFiles.ContainsKey(file))
            {
                KeyValuePair<string, bool> kvp = SystemFiles[file];
                return kvp.Value
                    ? GetSystemDirectory(SystemDirectoryTypes.System) + kvp.Key
                    : kvp.Key;
            }
            return string.Empty;
        }


        public static void LoadSystemDirectories(string path)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                while (!proxy.EndOfStream)
                {
                    string line = proxy.ReadLine().TrimEnd(new[] { '~' });
                    string[] words = line.Split(new[] { ',' });

                    SystemDirectoryTypes dirType = EnumerationExtensions.GetEnum<SystemDirectoryTypes>(words[0]);
                    SystemDirectories.Add(dirType, words[1]);
                }
            }
        }

        private static readonly string[] BooleanConstants = new[] { "true", "false", "1", "0", "yes", "no" };
        public static void LoadSystemFiles(string path)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                while (!proxy.EndOfStream)
                {
                    string line = proxy.ReadLine().TrimEnd(new[] { '~' });
                    string[] words = line.Split(new[] { ',' });

                    SystemFileTypes fileType = EnumerationExtensions.GetEnum<SystemFileTypes>(words[0]);
                    bool useSystemDirectory = Convert.ToBoolean(BooleanConstants.ContainsIgnoreCase(words[2]));

                    SystemFiles.Add(fileType, new KeyValuePair<string, bool>(words[1], useSystemDirectory));
                }
            }
        }
    }
}
