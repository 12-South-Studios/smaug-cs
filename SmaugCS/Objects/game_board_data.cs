namespace SmaugCS.Objects
{
    public class game_board_data
    {
        public string player1 { get; set; }
        public string player2 { get; set; }
        public int[,] board { get; set; }
        public int turn { get; set; }
        public int type { get; set; }

        public game_board_data()
        {
            board = new int[8, 8];
        }
    }
}
