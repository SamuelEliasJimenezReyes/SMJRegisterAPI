namespace SMJRegisterAPI.Common.Entities;

public abstract class BaseEntity
{
    public int ID { get; set; }
    public bool IsDeleted { get; set; }
}