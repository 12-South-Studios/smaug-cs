namespace SmaugCS.Objects
{
    public class map_index_data
    {
        public int vnum { get; set; }
        public int[,] map_of_vnums { get; set; }

        public map_index_data()
        {
            map_of_vnums = new int[49,81];
        }
    }
}
