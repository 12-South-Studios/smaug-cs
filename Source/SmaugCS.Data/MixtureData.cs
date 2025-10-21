
using System.Collections.Generic;

namespace SmaugCS.Data;

public class MixtureData
{
    public string Name { get; set; }
    public IEnumerable<int> Data { get; private set; } = [];
    public bool Object { get; set; }
}