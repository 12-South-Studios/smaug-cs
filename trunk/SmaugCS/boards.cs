using System.Collections.Generic;
using System.IO;
using System.Linq;
using Realm.Library.Common;
using Realm.Library.Common.Extensions;
using SmaugCS.Constants;
using SmaugCS.Enums;
using SmaugCS.Objects;

namespace SmaugCS
{
    public static class boards
    {
        public static void delete_project(ProjectData project)
        {
            project.Logs.Clear();
            db.PROJECTS.Remove(project);
        }

        public static void write_boards_txt()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Board) +
                          SystemConstants.GetSystemFile(SystemFileTypes.Boards);

            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                foreach (BoardData board in db.BOARDS)
                    board.Save(proxy);
            }
        }

        public static bool is_note_to(CharacterInstance ch, NoteData note)
        {
            if (ch.Name.EqualsIgnoreCase(note.Sender))
                return true;
            if ("all".IsEqual(note.RecipientList))
                return true;
            if (ch.IsHero() && "immortal".IsEqual(note.RecipientList))
                return true;
            if (ch.Name.IsEqual(note.RecipientList))
                return true;

            return false;
        }

        public static void note_attach(CharacterInstance ch)
        {
            NoteData note = new NoteData { Sender = ch.Name };

            if (ch.NoteList == null)
                ch.NoteList = new List<NoteData>();
            ch.NoteList.Add(note);
        }

        public static ObjectInstance find_quill(CharacterInstance ch)
        {
            return ch.Carrying.FirstOrDefault(quill => quill.ItemType == ItemTypes.Pen && handler.can_see_obj(ch, quill));
        }

        public static void load_boards()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Board) +
                          SystemConstants.GetSystemFile(SystemFileTypes.Boards);
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<TextSection> sections = proxy.ReadSections(new[] { "#BOARD" }, new[] { "*" }, new[] { "END" }, null);
                foreach (TextSection section in sections)
                {
                    BoardData newBoard = new BoardData();
                    newBoard.Load(section);
                    newBoard.LoadNotes();
                    db.BOARDS.Add(newBoard);
                }
            }
        }

        public static void mail_count(CharacterInstance ch)
        {
            int counterTo = 0;
            int counterFrom = 0;

            foreach (NoteData note in db.BOARDS
                .Where(board => board.Type == BoardTypes.Mail && board.CanRead(ch))
                .SelectMany(board => board.NoteList))
            {
                if (ch.Name.IsEqual(note.RecipientList))
                    ++counterTo;
                else if (ch.Name.EqualsIgnoreCase(note.Sender))
                    ++counterFrom;
            }

            if (counterTo > 0)
                color.ch_printf(ch, "You have {0} mail message{1}waiting for you.\r\n", counterTo, counterTo > 1 ? "s " : " ");
            if (counterFrom > 0)
                color.ch_printf(ch, "You have {0} mail message{1}written by you.\r\n", counterFrom, counterFrom > 1 ? "s " : " ");
        }
    }
}
