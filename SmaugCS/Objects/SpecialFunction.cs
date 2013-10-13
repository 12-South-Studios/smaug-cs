using System;

namespace SmaugCS.Objects
{
    public class SpecialFunction
    {
        public string Name { get; set; }
        public Func<CharacterInstance, bool> Value { get; set; }
    }
}
