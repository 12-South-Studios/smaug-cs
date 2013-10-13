namespace SmaugCS.Objects
{
    public class EditorData
    {
        public int NumberOfLines { get; set; }
        public int OnLine { get; set; }
        public int Size { get; set; }
        public char[,] Text { get; set; }

        public EditorData()
        {
            Text = new char[49,81];
        }
    }
}
