
namespace SmaugCS.Data.Interfaces
{
    public interface IPersistable
    {
        string Filename { get; set; }

        void Save();
        void Load();
    }
}
