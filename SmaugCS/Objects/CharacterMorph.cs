using SmaugCS.Common;

namespace SmaugCS.Objects
{
    public class CharacterMorph
    {
        public MorphData Morph { get; set; }

        public ExtendedBitvector AffectedBy { get; set; }
        public ExtendedBitvector NotAffectedBy { get; set; }

        public int no_immune { get; set; }
        public int no_resistant { get; set; }
        public int no_suscept { get; set; }
        public int immune { get; set; }
        public int resistant { get; set; }
        public int suscept { get; set; }
        public int timer { get; set; }

        public short ac { get; set; }
        public short blood { get; set; }
        public short cha { get; set; }
        public short con { get; set; }
        public short damroll { get; set; }
        public short dex { get; set; }
        public short dodge { get; set; }
        public short hit { get; set; }
        public short hitroll { get; set; }
        public short inte { get; set; }
        public short lck { get; set; }
        public short mana { get; set; }
        public short move { get; set; }
        public short parry { get; set; }
        public short saving_breath { get; set; }
        public short saving_para_petri { get; set; }
        public short saving_poison_death { get; set; }
        public short saving_spell_staff { get; set; }
        public short saving_wand { get; set; }
        public short str { get; set; }
        public short tumble { get; set; }
        public short wis { get; set; }
    }
}
