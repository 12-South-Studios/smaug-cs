using System.Collections.Generic;
using SmaugCS.Common;
using SmaugCS.Enums;
using SmaugCS.Interfaces;

// ReSharper disable CheckNamespace
namespace SmaugCS.Objects
// ReSharper restore CheckNamespace
{
    public class ObjectTemplate : Template, IHasExtraFlags
    {
        public List<ExtraDescriptionData> ExtraDescriptions { get; set; }
        public List<AffectData> Affects { get; set; }
        public ExtendedBitvector ExtraFlags { get; set; }
        public string ShortDescription { get; set; }
        public string ActionDescription { get; set; }
        public int[] Value { get; set; }
        public int serial { get; set; }
        public int Cost { get; set; }
        public int Rent { get; set; }
        public int magic_flags { get; set; }
        public int WearFlags { get; set; }
        public int count { get; set; }
        public int Weight { get; set; }
        public int Layers { get; set; }
        public int Level { get; set; }
        public ItemTypes Type { get; set; }

        public ObjectTemplate()
        {
            Value = new int[6];
            ExtraDescriptions = new List<ExtraDescriptionData>();
            Affects = new List<AffectData>();
        }
    }
}
