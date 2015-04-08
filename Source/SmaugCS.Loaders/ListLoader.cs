
namespace SmaugCS.Loaders
{
    public abstract class ListLoader
    {
        public abstract string Filename { get; }
        public abstract void Save();
        public abstract void Load();
    }
}
