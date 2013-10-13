using System.Collections.Generic;
using System.IO;

namespace SmaugCS.Common
{
    public class DirectoryProxy
    {
        public DirectoryProxy() { }

        public virtual IEnumerable<string> GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }
    }
}
