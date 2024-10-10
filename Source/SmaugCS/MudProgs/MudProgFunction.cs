using SmaugCS.Constants.Enums;
using System;

namespace SmaugCS.MudProgs;

public class MudProgFunction
{
    public MudProgTypes Type { get; set; }
    public Func<object[], bool> Function { get; set; }
}