
// ReSharper disable CheckNamespace
namespace SmaugCS.Data
// ReSharper restore CheckNamespace
{
    public interface IPersistable
    {
        string Filename { get; set; }

        void Save();
        void Load();
    }
}
