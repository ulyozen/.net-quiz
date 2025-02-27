namespace Quiz.Core.Entities;

public abstract class Entity
{
    public string Id { get; protected set; }

    protected Entity(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Id cannot be null or empty.", nameof(id));
        }
        
        Id = id;
    }
}