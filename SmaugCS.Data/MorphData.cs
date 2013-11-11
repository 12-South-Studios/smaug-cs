using SmaugCS.Common;

namespace SmaugCS.Data
{
    public class MorphData
    {
        public string blood { get; set; }
        public string damroll { get; set; }
        public string deity { get; set; }
        public string Description { get; set; }
        public string help { get; set; }
        public string hit { get; set; }
        public string hitroll { get; set; }
        public string key_words { get; set; }
        public string LongDescription { get; set; }
        public string mana { get; set; }
        public string morph_other { get; set; }
        public string morph_self { get; set; }
        public string move { get; set; }
        public string name { get; set; }
        public string ShortDescription { get; set; }
        public string no_skills { get; set; }
        public string skills { get; set; }
        public string unmorph_other { get; set; }
        public string unmorph_self { get; set; }
        public ExtendedBitvector affected_by { get; set; }
        public int Class { get; set; }
        public int Position { get; set; }
        public ExtendedBitvector no_affected_by { get; set; }
        public int no_immune { get; set; }
        public int no_resistant { get; set; }
        public int no_suscept { get; set; }
        public int immune { get; set; }
        public int resistant { get; set; }
        public int suscept { get; set; }
        public int[] obj { get; set; }
        public int race { get; set; }
        public int timer { get; set; }
        public int used { get; set; }
        public int vnum { get; set; }
        public short ac { get; set; }
        public short bloodused { get; set; }
        public short cha { get; set; }
        public short con { get; set; }
        public short dayfrom { get; set; }
        public short dayto { get; set; }
        public short dex { get; set; }
        public short dodge { get; set; }
        public short favourused { get; set; }
        public short gloryused { get; set; }
        public short hpused { get; set; }
        public short inte { get; set; }
        public short lck { get; set; }
        public short level { get; set; }
        public short manaused { get; set; }
        public short moveused { get; set; }
        public short parry { get; set; }
        public short pkill { get; set; }
        public short saving_breath { get; set; }
        public short saving_para_petri { get; set; }
        public short saving_poison_death { get; set; }
        public short saving_spell_staff { get; set; }
        public short saving_wand { get; set; }
        public short sex { get; set; }
        public short str { get; set; }
        public short timefrom { get; set; }
        public short timeto { get; set; }
        public short tumble { get; set; }
        public short wis { get; set; }
        public bool no_cast { get; set; }
        public bool[] objuse { get; set; }

        public MorphData()
        {
            obj = new int[3];
            objuse = new bool[3];
        }
    }
}
