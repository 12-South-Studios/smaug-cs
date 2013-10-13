using System;

namespace SmaugCS.Objects
{
    public class SpellFunction
    {
        public Func<int, int, CharacterInstance, object, int> Value { get; set; }
    }
}
