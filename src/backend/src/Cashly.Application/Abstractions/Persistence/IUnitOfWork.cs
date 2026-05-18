namespace Cashly.Application.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
