using System.IO;
using Realm.Library.Common;
using SmaugCS.Data;
using SmaugCS.Data;

namespace SmaugCS.Loaders
{
    public class HelpAreaLoader : AreaLoader
    {
        public HelpAreaLoader(string areaName, bool bootDb)
            : base(areaName, bootDb)
        {
        }

        #region Overrides of AreaLoader

        public override AreaData LoadArea(AreaData area)
        {
            using (TextReaderProxy proxy = new TextReaderProxy(new StreamReader(FilePath)))
            {
                do
                {
                    HelpData help = new HelpData
                                        {
                                            Level = proxy.ReadNumber(),
                                            Keyword = proxy.ReadString()
                                        };
                    if (help.Keyword.StartsWith("$"))
                        continue;

                    help.Text = proxy.ReadString();
                    if (help.Keyword.Equals("greeting"))
                        db.HelpGreeting = help.Text;

                    db.add_help(help);

                } while (!proxy.EndOfStream);

                return null;
            }
        }

        #endregion
    }
}
