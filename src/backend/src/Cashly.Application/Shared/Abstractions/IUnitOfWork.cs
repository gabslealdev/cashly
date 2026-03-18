namespace Cashly.Application.Shared.Abstractions
{
    public interface IUnitOfWork
    {
        Task commitAsync();
    }
}
