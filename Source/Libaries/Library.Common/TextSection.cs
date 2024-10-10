using System.Collections.Generic;

namespace Library.Common;

public class TextSection
{
    public string Header { get; set; }
    public List<string> Lines { get; set; } = [];
    public string Footer { get; set; }
}