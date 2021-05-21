using SmaugCS.Common.Enumerations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public long BoardObjectId { get; set; }
        public bool Saved { get; set; }

        public ReadOnlyCollection<NoteData> Notes { get; }

        public BoardData(int id, BoardTypes type)
        {
            Id = id;
            Type = type;
            Saved = false;
            Notes = new ReadOnlyCollection<NoteData>(new List<NoteData>());
        }

        public void AddNotes(IEnumerable<NoteData> notes) => Notes.ToList().AddRange(notes);

        public void RemoveNote(NoteData note)
        {
            if (note == null) return;

            Notes.ToList().Remove(note);
        }
    }
}
