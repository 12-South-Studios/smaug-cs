using System;
using SmaugCS.Enums;

namespace SmaugCS.Objects
{
    public class SpellFunction
    {
        public Func<int, int, CharacterInstance, object, ReturnTypes> Value { get; set; }
    }
}
