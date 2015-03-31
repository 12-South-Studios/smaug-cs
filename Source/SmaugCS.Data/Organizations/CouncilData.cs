using System.Xml.Serialization;

namespace SmaugCS.Data.Organizations
{
    [XmlRoot("Council")]
    public class CouncilData : OrganizationData
    {
        public string NumberOne { get; set; }
        public string Powers { get; set; }
        public int Members { get; set; }
        public int Meeting { get; set; }

        public CouncilData(long id, string name) : base(id, name)
        {
        }

        /*public override void Save()
        {
            if (string.IsNullOrWhiteSpace(Filename))
            {
                LogManager.Instance.Bug("Council {0} has no filename", Name);
                return;
            }

            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council) + Filename;
            using (TextWriterProxy proxy = new TextWriterProxy(new StreamWriter(path)))
            {
                proxy.Write("#COUNCIL\n");
                proxy.Write("Name         {0}~\n", Name);
                proxy.Write("Filename     {0}~\n", Filename);
                proxy.Write("Description  {0}~\n", Description);
                proxy.Write("Head         {0}~\n", Leader);
                if (NumberOne != null)
                    proxy.Write("Head2        {0}~\n", NumberOne);
                proxy.Write("Members       {0}\n", Members);
                proxy.Write("Board         {0}\n", Board);
                proxy.Write("Meeting       {0}\n", Meeting);
                if (!string.IsNullOrWhiteSpace(Powers))
                    proxy.Write("Powers        {0}\n", Powers);
                proxy.Write("End\n\n");
                proxy.Write("#END\n");
            }
        }

        public override void Load()
        {
            string path = SystemConstants.GetSystemDirectory(SystemDirectoryTypes.Council) + Filename;
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(path)))
            {
                List<string> lines = proxy.ReadIntoList();
                foreach (string line in lines.Where(x => !x.StartsWith("*")))
                {
                    Tuple<string, string> tuple = line.FirstArgument();
                    switch (tuple.Item1.ToLower())
                    {
                        case "board":
                            Board = tuple.Item2.ToInt32();
                            break;
                        case "description":
                            Description = tuple.Item2.TrimHash();
                            break;
                        case "end":
                            return;
                        case "filename":
                            Filename = tuple.Item2.TrimHash();
                            break;
                        case "head":
                            Leader = tuple.Item2.TrimHash();
                            break;
                        case "head2":
                            NumberOne = tuple.Item2.TrimHash();
                            break;
                        case "members":
                            Members = tuple.Item2.ToInt32();
                            break;
                        case "meeting":
                            Meeting = tuple.Item2.ToInt32();
                            break;
                        case "name":
                            Name = tuple.Item2.TrimHash();
                            break;
                        case "powers":
                            Powers = tuple.Item2.TrimHash();
                            break;
                        default:
                            LogManager.Instance.Bug("Unknown clan parameter {0} for {1}", tuple.Item1, path);
                            break;
                    }
                }
            }
        }*/
    }
}
