using SmaugCS.Common;

namespace SmaugCS.Data;

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
    public int[] obj { get; set; } = new int[3];
    public int race { get; set; }
    public int timer { get; set; }
    public int used { get; set; }
    public int vnum { get; set; }
    public int ac { get; set; }
    public int bloodused { get; set; }
    public int cha { get; set; }
    public int con { get; set; }
    public int dayfrom { get; set; }
    public int dayto { get; set; }
    public int dex { get; set; }
    public int DodgeChances { get; set; }
    public int favourused { get; set; }
    public int gloryused { get; set; }
    public int hpused { get; set; }
    public int inte { get; set; }
    public int lck { get; set; }
    public int level { get; set; }
    public int manaused { get; set; }
    public int moveused { get; set; }
    public int ParryChances { get; set; }
    public int pkill { get; set; }
    public int saving_breath { get; set; }
    public int saving_para_petri { get; set; }
    public int saving_poison_death { get; set; }
    public int saving_spell_staff { get; set; }
    public int saving_wand { get; set; }
    public int sex { get; set; }
    public int str { get; set; }
    public int timefrom { get; set; }
    public int timeto { get; set; }
    public int TumbleChances { get; set; }
    public int wis { get; set; }
    public bool no_cast { get; set; }
    public bool[] objuse { get; set; } = new bool[3];
}