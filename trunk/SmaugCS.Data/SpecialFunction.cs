using System;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class SpecialFunction
    {
        public string Name { get; set; }
        public Func<CharacterInstance, bool> Value { get; set; }
    }
}
