namespace RecipeFinder.Domain.Entities.Common
{
    public abstract class Entity<TId>
    {
        public TId Id { get; protected set; } = default!;

        protected Entity(TId id)
        {
            Id = id;
        }
        protected Entity() { } // Para EF Core
    }
}
