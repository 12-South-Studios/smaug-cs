using SmaugCS.Constants.Enums;

namespace SmaugCS.Data
{
    public class MudProgData
    {
        public MudProgTypes Type { get; set; }
        public bool triggered { get; set; }
        public int resetdelay { get; set; }
        public string arglist { get; set; }
        public string comlist { get; set; }
        public bool IsFileProg { get; set; }

        /*public bool Save(TextWriterProxy proxy)
        {
            if (arglist.IsNullOrEmpty())
                return false;

            proxy.Write("#MUDPROG\n");
            proxy.Write("ProgType  {0}~\n", BuilderConstants.mprog_type_to_name(Type));
            proxy.Write("Arglist   {0}~\n", arglist);

            if (!comlist.IsNullOrEmpty() && !IsFileProg)
                proxy.Write("Comlist   {0}~\n", comlist);

            proxy.Write("#ENDPROG\n\n");
            return true;
        }*/
    }
}
