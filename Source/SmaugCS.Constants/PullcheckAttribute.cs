using System;

namespace SmaugCS.Constants;

[AttributeUsage(AttributeTargets.Field)]
public sealed class PullcheckAttribute : Attribute
{
    public string ToChar { get; set; }
    public string ToRoom { get; set; }
    public string DestRoom { get; set; }
    public string ObjMsg { get; set; }
    public string DestObj { get; set; }
}