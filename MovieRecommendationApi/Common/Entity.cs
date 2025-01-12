namespace MovieRecommendationApi.Common;

public abstract class Entity : Entity<Guid>
{
    protected Entity() : base(Guid.NewGuid())
    {
    }
}
public abstract class Entity<TKey>
{
    public TKey Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; protected set; }
    protected Entity(TKey id)
    {
        Id = id;
        CreatedAt = DateTime.UtcNow;
    }
}
