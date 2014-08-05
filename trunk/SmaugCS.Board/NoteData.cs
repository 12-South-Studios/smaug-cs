using System;
using SmaugCS.Common;

namespace SmaugCS.Board
{
    public class NoteData
    {
        public int ID { get; private set; }
        public string Sender { get; set; }
        public string DateSent { get; set; }
        public string RecipientList { get; set; }
        public string Subject { get; set; }
        public int Voting { get; set; }
        public string YesVotes { get; set; }
        public string NoVotes { get; set; }
        public string Abstentions { get; set; }
        public string Text { get; set; }

        public NoteData(int id)
        {
            ID = id;
        }

        public static NoteData Translate(System.Data.DataRow row)
        {
            NoteData note = new NoteData(Convert.ToInt32(row["NoteId"]))
            {
                Sender = row.GetDataValue("Sender", string.Empty),
                DateSent = row.GetDataValue("DateSent", DateTime.Now).ToString(),
                RecipientList = row.GetDataValue("RecipientList", string.Empty),
                Subject = row.GetDataValue("Subject", string.Empty),
                Voting = row.GetDataValue("Voting", 1),
                Text = row.GetDataValue("Text", string.Empty)
            };
            return note;
        }
    }
}
