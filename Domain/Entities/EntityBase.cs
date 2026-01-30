namespace Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get;  protected set; } = DateTime.UtcNow;
    public bool Active { get; protected set; } = true;
    public string? Description { get; protected set; }
    //public bool IsDeleted { get; protected set; } = false;
    public virtual void ChangeStatus(){ 
        Active = !Active;
    }
    // public void Delete(){
    //     IsDeleted = true;
    // }
}
