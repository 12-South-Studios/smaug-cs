using SmaugCS.Constants.Enums;

namespace SmaugCS.Data.Interfaces
{
    public interface IVerifiable
    {
        bool IsNpc();
        bool IsFloating();
        bool IsAffected(AffectedByTypes affectedBy);
        bool IsImmortal(int level = 51);
        bool IsHero(int level = 50);
    }
}
