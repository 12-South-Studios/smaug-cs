using System.IO;
using Realm.Library.Common;
using SmaugCS.Constants.Enums;
using SmaugCS.Data;
using SmaugCS.Data.Instances;
using SmaugCS.Loaders.Loaders;

namespace SmaugCS.Loaders
{
    public class CommentLoader : BaseLoader
    {
        #region Overrides of ListLoader

        public CommentLoader(ILuaManager luaManager) : base(luaManager)
        {
        }

        public override string Filename => string.Empty;

        public CharacterInstance Character { get; set; }

        public override void Save()
        {
            if (Character == null)
                return;

            using (var proxy = new TextWriterProxy(new StreamWriter(Filename)))
            {
                /*foreach (NoteData note in Character.NoteList)
                {
                    proxy.Write("#COMMENT\n");
                    proxy.Write("sender  {0}~\n", note.Sender);
                    proxy.Write("date    {0}~\n", note.DateSent);
                    proxy.Write("to      {0}~\n", note.RecipientList);
                    proxy.Write("subject {0}~\n", note.Subject);
                    proxy.Write("text\n{0}~\n", note.Text);
                }*/
            }
        }

        protected override string AppSettingName
        {
            get { throw new System.NotImplementedException(); }
        }

        protected override SystemDirectoryTypes SystemDirectory
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void Load()
        {
            if (Character == null)
                return;

            using (var proxy = new TextReaderProxy(new StreamReader(Filename)))
            {
                /*List<TextSection> sections = proxy.ReadSections(new[] { "#COMMENT" }, new[] { "*" }, null, null);
                if (sections.Count > 0 && Character.Comments == null)
                    Character.Comments = new List<NoteData>();

                foreach (TextSection section in sections)
                {
                    bool inText = false;
                    NoteData newNote = new NoteData();
                    foreach (string line in section.Lines)
                    {
                        if (inText)
                        {
                            if (line.EndsWith("~"))
                                inText = false;
                            newNote.Text += line.TrimHash();
                        }

                        Tuple<string, string> tuple = line.FirstArgument();
                        switch (tuple.Item1.ToLower())
                        {
                            case "sender":
                                newNote.Sender = tuple.Item2.TrimHash();
                                break;
                            case "date":
                                newNote.DateSent = tuple.Item2.TrimHash();
                                break;
                            case "to":
                                newNote.RecipientList = tuple.Item2.TrimHash();
                                break;
                            case "subject":
                                newNote.Subject = tuple.Item2.TrimHash();
                                break;
                            case "text":
                                inText = true;
                                break;
                        }
                    }

                    Character.Comments.Add(newNote);
                }*/
            }
        }

        #endregion
    }
}
