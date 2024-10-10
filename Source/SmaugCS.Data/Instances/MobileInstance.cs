using Library.Common.Objects;
using SmaugCS.Data.Templates;

namespace SmaugCS.Data.Instances;

public class MobileInstance(int id, string name) : CharacterInstance(id, name)
{
    public HuntHateFearData CurrentHunting { get; set; }
    public HuntHateFearData CurrentFearing { get; set; }
    public HuntHateFearData CurrentHating { get; set; }
    public SpecialFunction SpecialFunction { get; set; }
    public string SpecialFunctionName { get; set; }

    public override int Trust
    {
        get => Level;
        set => Trust = value;
    }

    public MobileTemplate MobIndex => Parent.CastAs<MobileTemplate>();
}