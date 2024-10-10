using Library.Common.Objects;
using SmaugCS.Common;
using SmaugCS.Data.Instances;
using System;

namespace SmaugCS.Data;

public class SpecialFunction(long id, string name) : Entity(id, name)
{
    public Func<MobileInstance, IManager, bool> Value { get; set; }
}