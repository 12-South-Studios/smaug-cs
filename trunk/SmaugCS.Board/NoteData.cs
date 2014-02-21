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
            throw new System.NotImplementedException();
        }
    }
}
