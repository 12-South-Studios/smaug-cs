using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using SmaugCS.Common;
using SmaugCS.Common.Enumerations;

namespace SmaugCS.Board
{
    public class BoardData
    {
        public int Id { get; private set; }
        public BoardTypes Type { get; private set; }
        public string Name { get; set; }
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
        public int MinimumReadLevel { get; set; }
        public int MinimumPostLevel { get; set; }
        public int MinimumRemoveLevel { get; set; }
        public int MaximumPosts { get; set; }
        public int BoardObjectId { get; set; }

        public ReadOnlyCollection<NoteData> Notes { get; private set; }

        public BoardData(int id, BoardTypes type)
        {
            Id = id;
            Type = type;
            Notes = new ReadOnlyCollection<NoteData>(new List<NoteData>());
        }

        public void AddNotes(IEnumerable<NoteData> notes)
        {
            Notes.ToList().AddRange(notes);
        }
       /* public void Save(TextWriterProxy proxy)
        {
            proxy.Write("#BOARD\n");
            proxy.Write("Filename          {0}~\n", NoteFile);
            proxy.Write("Vnum              {0}\n", BoardObjectId);
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
                //LogManager.Instance.Bug("Note were null");
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
        */

        public static BoardData Translate(DataRow dataRow)
        {
            BoardData board = new BoardData(Convert.ToInt32(dataRow["BoardId"]),
                Realm.Library.Common.EnumerationExtensions.GetEnumByName<BoardTypes>(dataRow["BoardTypeName"].ToString()))
                {
                    Name = dataRow.GetDataValue("Name", string.Empty),
                    ReadGroup = dataRow.GetDataValue("ReadGroup", string.Empty),
                    PostGroup = dataRow.GetDataValue("PostGroup", string.Empty),
                    ExtraReaders = dataRow.GetDataValue("ExtraReaders", string.Empty),
                    ExtraRemovers = dataRow.GetDataValue("ExtraRemovers", string.Empty),
                    OTakeMessage = dataRow.GetDataValue("OTakeMessage", string.Empty),
                    OPostMessage = dataRow.GetDataValue("OPostMessage", string.Empty),
                    ORemoveMessage = dataRow.GetDataValue("ORemoveMessage", string.Empty),
                    OCopyMessage = dataRow.GetDataValue("OCopyMessage", string.Empty),
                    PostMessage = dataRow.GetDataValue("PostMessage", string.Empty),
                    OReadMessage = dataRow.GetDataValue("OReadMessage", string.Empty),
                    OListMessage = dataRow.GetDataValue("OListMessage", string.Empty),
                    MinimumReadLevel = dataRow.GetDataValue("MinimumReadLevel", 0),
                    MinimumPostLevel = dataRow.GetDataValue("MinimumPostLevel", 0),
                    MinimumRemoveLevel = dataRow.GetDataValue("MinimumRemoveLevel", 0),
                    MaximumPosts = dataRow.GetDataValue("MaximumPosts", 0),
                    BoardObjectId = dataRow.GetDataValue("BoardObjectId", 0)
                };
            return board;
        }
    }
}
