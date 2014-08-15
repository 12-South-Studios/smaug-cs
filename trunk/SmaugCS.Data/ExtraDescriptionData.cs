
namespace SmaugCS.Data
{
    public class ExtraDescriptionData
    {
        public static ExtraDescriptionData Create()
        {
            return new ExtraDescriptionData();
        }

        public string Keyword { get; set; }
        public string Description { get; set; }

        private ExtraDescriptionData()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proxy"></param>
        /* public void SaveFUSS(TextWriterProxy proxy)
         {
             proxy.Write("#EXDESC\n");
             proxy.Write("ExDescKey   {0}~\n", Keyword);
             if (!Description.IsNullOrEmpty())
                 proxy.Write("ExDesc       {0}~\n", Description);
             proxy.Write("#ENDEXDESC\n\n");
         }*/
    }
}
