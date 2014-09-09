using System;
using SmaugCS.Constants.Enums;
using SmaugCS.Data.Instances;

namespace SmaugCS.Data
{
    public class SpellFunction
    {
        public Func<int, int, CharacterInstance, object, ReturnTypes> Value { get; set; }
    }
}
