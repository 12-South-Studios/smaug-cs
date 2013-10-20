namespace Realm.Library.Patterns.Decorator
{
    /// <summary>
    /// Abstract class that defines a decorator component
    /// </summary>
    public abstract class Decorator : IComponent
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="component"></param>
        protected Decorator(IComponent component)
        {
            Component = component;
        }

        /// <summary>
        /// Gets a component reference
        /// </summary>
        public IComponent Component { get; private set; }
    }
}