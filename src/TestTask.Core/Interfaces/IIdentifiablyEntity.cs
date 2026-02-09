namespace TestTask.Core.Interfaces;

public interface IIdentifiablyEntity<TId>
{
    TId Id { get; set; }
}
