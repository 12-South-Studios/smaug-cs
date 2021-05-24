using Realm.Library.Common;
using Realm.Library.Common.Extensions;

namespace SmaugCS
{
    public class LoginMessageData
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public int Type { get; set; }

        public void Save(TextWriterProxy proxy)
        {
            proxy.Write("#LOGINMSG\n");
            proxy.Write("Name  {0}~\n", Name);
            if (!Text.IsNullOrEmpty())
                proxy.Write("Text  {0}~\n", Text);
            proxy.Write("Type  {0}\n", Type);
            proxy.Write("End\n");
        }
    }
}
