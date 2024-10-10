namespace SmaugCS.Data.Interfaces;

public interface IHasExtraDescriptions
{
    void AddExtraDescription(string keywords, string description);
    bool DeleteExtraDescription(string keyword);
    ExtraDescriptionData GetExtraDescription(string keyword);
}