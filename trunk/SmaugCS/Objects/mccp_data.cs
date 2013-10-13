using ZLibNet;

namespace SmaugCS.Objects
{
    public class mccp_data
    {
        public GZipStream out_compress { get; set; }
        public string out_compress_buf { get; set; }
    }
}
