using System;
using System.Collections.Generic;
using System.IO;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants.Constants;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class BoardData
    {
        public List<NoteData> NoteList { get; set; }
        public string NoteFile { get; set; }
        public string ReadGroup { get; set; }
        public string PostGroup { get; set; }
        public string ExtraReaders { get; set; }
        public string ExtraRemovers { get; set; }
        public string OTakeMessage { get; set; }
        public string OPostMessage { get; set; }
        public string ORemoveMessage { get; set; }
        public string OCopyMessage { get; set; }
        public string OListMessage { get; set; }
        public string PostMessage { get; set; }
        public string OReadMessage { get; set; }
        public int BoardObjectVnum { get; set; }
        public int NumberOfPosts { get { return NoteList.Count; } }
        public int MinimumReadLevel { get; set; }
        public int MinimumPostLevel { get; set; }
        public int MinimumRemoveLevel { get; set; }
        public int MaximumPosts { get; set; }
        public BoardTypes Type { get; set; }

        public BoardData()
        {
            NoteList = new List<NoteData>();
        }

        public void Load(TextSection section)
        {
            foreach (string line in section.Lines)
            {
                string firstWord = line.FirstWord();
                string remainder = line.RemoveWord(1).TrimEnd('~');

                switch (firstWord.ToUpper())
                {
                    case "END":
                        return;
                    case "EXTRA_READERS":
                        ExtraReaders = remainder;
                        break;
                    case "EXTRA_REMOVERS":
                        ExtraRemovers = remainder;
                        break;
                    case "FILENAME":
                        NoteFile = remainder;
                        break;
                    case "MIN_READ_LEVEL":
                        MinimumReadLevel = remainder.ToInt32();
                        break;
                    case "MIN_POST_LEVEL":
                        MinimumPostLevel = remainder.ToInt32();
                        break;
                    case "MIN_REMOVE_LEVEL":
                        MinimumRemoveLevel = remainder.ToInt32();
                        break;
                    case "MAX_POSTS":
                        MaximumPosts = remainder.ToInt32();
                        break;
                    case "OTAKEMESSG":
                        OTakeMessage = remainder;
                        break;
                    case "OCOPYMESSG":
                        OCopyMessage = remainder;
                        break;
                    case "OREADMESSG":
                        OReadMessage = remainder;
                        break;
                    case "OREMOVEMESSG":
                        ORemoveMessage = remainder;
                        break;
                    case "OLISTMESSG":
                        OListMessage = remainder;
                        break;
                    case "OPOSTMESSG":
                        OPostMessage = remainder;
                        break;
                    case "POST_GROUP":
                        PostGroup = remainder;
                        break;
                    case "POSTMESSG":
                        PostMessage = remainder;
                        break;
                    case "READ_GROUP":
                        ReadGroup = remainder;
                        break;
                    case "TYPE":
                        Type = EnumerationExtensions.GetEnum<BoardTypes>(remainder.ToInt32());
                        break;
                    case "VNUM":
                        BoardObjectVnum = remainder.ToInt32();
                        break;
                    default:
                        //LogManager.Bug("Unknown Value {0}", line);
                        continue;
                }
            }
        }

        /*public void LoadNotes()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Board) + NoteFile;
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<TextSection> sections = proxy.ReadSections(new[] { "#NOTE" }, new[] { "*" }, new[] { "#END" }, "END");
                foreach (TextSection section in sections)
                {
                    NoteData newNote = new NoteData();
                    newNote.Load(section);
                    NoteList.Add(newNote);
                }
            }
        }*/

        public void Save(TextWriterProxy proxy)
        {
            proxy.Write("#BOARD\n");
            proxy.Write("Filename          {0}~\n", NoteFile);
            proxy.Write("Vnum              {0}\n", BoardObjectVnum);
            proxy.Write("Min_read_level    {0}\n", MinimumReadLevel);
            proxy.Write("Min_post_level    {0}\n", MinimumPostLevel);
            proxy.Write("Min_remove_level  {0}\n", MinimumRemoveLevel);
            proxy.Write("Max_posts         {0}\n", MaximumPosts);
            proxy.Write("Type              {0}\n", (int)Type);
            proxy.Write("Read_group        {0}~\n", ReadGroup);
            proxy.Write("Post_group        {0}~\n", PostGroup);
            proxy.Write("Extra_readers     {0}~\n", ExtraReaders);
            proxy.Write("Extra_removers    {0}~\n", ExtraRemovers);

            if (!string.IsNullOrWhiteSpace(OCopyMessage))
                proxy.Write("OCopymessg	   {0}~\n", OCopyMessage);
            if (!string.IsNullOrWhiteSpace(OListMessage))
                proxy.Write("OListmessg    {0}~\n", OListMessage);
            if (!string.IsNullOrWhiteSpace(OPostMessage))
                proxy.Write("OPostmessg    {0}~\n", OPostMessage);
            if (!string.IsNullOrWhiteSpace(OReadMessage))
                proxy.Write("OReadmessg    {0}~\n", OReadMessage);
            if (!string.IsNullOrWhiteSpace(OTakeMessage))
                proxy.Write("OTakemessg    {0}~\n", OTakeMessage);
            if (!string.IsNullOrWhiteSpace(PostMessage))
                proxy.Write("Postmessg     {0}~\n", PostMessage);

            proxy.Write("END\n");
        }

        public void SaveNotes()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Board) + NoteFile;
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                foreach (NoteData note in NoteList)
                {
                    proxy.Write("#NOTE\n");
                    proxy.Write("Sender      {0}~\n", note.Sender);
                    proxy.Write("Date        {0}~\n", note.DateSent);
                    proxy.Write("To          {0}~\n", note.RecipientList);
                    proxy.Write("Subject     {0}~\n", note.Subject);
                    proxy.Write("Voting      {0}~\n", note.Voting);
                    proxy.Write("YesVotes    {0}~\n", note.YesVotes);
                    proxy.Write("NoVotes     {0}~\n", note.NoVotes);
                    proxy.Write("Abstentions {0}~\n", note.Abstentions);
                    proxy.Write("Text\n{0}~\n\n", note.Text);
                    proxy.Write("#END\n");
                }
                proxy.Write("End\n");
            }
        }

        public void RemoveNote(NoteData note)
        {
            if (note == null)
            {
                //LogManager.Bug("Note were null");
                return;
            }

            NoteList.Remove(note);
            SaveNotes();
        }

        public bool CanPost(CharacterInstance ch)
        {
            if (ch.Trust >= MinimumPostLevel)
                return true;

            if (!string.IsNullOrWhiteSpace(PostGroup))
            {
                if (ch.PlayerData.Clan != null
                    && ch.PlayerData.Clan.Name.Equals(PostGroup, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (ch.PlayerData.Council != null
                    && ch.PlayerData.Council.Name.Equals(PostGroup, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        public bool CanRemove(CharacterInstance ch)
        {
            if (ch.Trust >= MinimumRemoveLevel)
                return true;
            if (!string.IsNullOrWhiteSpace(ExtraRemovers))
            {
                if (ch.Name.IsEqual(ExtraRemovers))
                    return true;
            }
            return false;
        }

        public bool CanRead(CharacterInstance ch)
        {
            if (ch.Trust >= MinimumReadLevel)
                return true;

            // Your trust wasn't high enough, so check if a read_group or extra readers have been set up. 
            if (!string.IsNullOrWhiteSpace(ReadGroup))
            {
                if (ch.PlayerData.Clan != null
                    && ch.PlayerData.Clan.Name.Equals(ReadGroup, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (ch.PlayerData.Council != null
                    && ch.PlayerData.Council.Name.Equals(ReadGroup, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            if (!string.IsNullOrWhiteSpace(ExtraReaders))
            {
                if (ch.Name.IsEqual(ExtraReaders))
                    return true;
            }
            return false;
        }
    }
}
