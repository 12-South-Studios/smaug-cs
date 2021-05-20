namespace Realm.Standard.Patterns.Decorator
{
    public abstract class Decorator : IComponent
    {
        protected Decorator(IComponent component)
        {
            Component = component;
        }

        public IComponent Component { get; }
    }
}
