
namespace SmaugCS.Data
{
    public class CommandData
    {
        public string Name { get; set; }
        public DoFunction DoFunction { get; set; }
        public string FunctionName { get; set; }
        public int Flags { get; set; }
        public int Position { get; set; }
        public int Level { get; set; }
        public int Log { get; set; }
        public UseHistory userec { get; set; }
        public int lag_count { get; set; }


        public int GetModifiedPosition()
        {
            int originalPosition = Position;
            if (originalPosition < 100)
            {
                switch (originalPosition)
                {
                    case 5:
                        originalPosition = 6;
                        break;
                    case 6:
                        originalPosition = 8;
                        break;
                    case 7:
                        originalPosition = 9;
                        break;
                    case 8:
                        originalPosition = 12;
                        break;
                    case 9:
                        originalPosition = 13;
                        break;
                    case 10:
                        originalPosition = 14;
                        break;
                    case 11:
                        originalPosition = 15;
                        break;
                } 
            }

            return originalPosition >= 100 ? originalPosition - 100 : originalPosition;
        }
    }
}
