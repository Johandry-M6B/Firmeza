namespace Domain.Exceptions;

public class EntityNotFoundException : DomainException
{
    public int EntityName { get; }
    
    
    public EntityNotFoundException(string entityName, object id)
        : base($"Cannot found the entity '{entityName}' with ID:{id}",
            "ENTITY_NOT-FOUND")
    {
        EntityName = (int)id;
        AddErrorDetail("Code", id);
    }

   
}