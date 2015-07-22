namespace Infrastructure.Model.Domain
{
    public abstract class Entity
    {
        public virtual int Id { get; set; }

        public virtual bool IsTransient()
        {
            return Id == default(int);
        }
    }
}
