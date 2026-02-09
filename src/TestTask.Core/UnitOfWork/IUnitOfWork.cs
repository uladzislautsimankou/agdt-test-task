namespace TestTask.Core.UnitOfWork;

public interface IUnitOfWork
{
    void CommitChanges();

    Task CommitChangesAsync();
}
