using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SmaugCS.Common
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DirectoryProxy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual IEnumerable<string> GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }
    }
}
