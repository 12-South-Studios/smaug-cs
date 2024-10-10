using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SmaugCS.Common;

[ExcludeFromCodeCoverage]
public class DirectoryProxy
{
    public virtual IEnumerable<string> GetFiles(string path) => Directory.GetFiles(path);
}