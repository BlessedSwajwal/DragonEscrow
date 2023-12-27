using Application.Common.Repositories;

namespace Application.Common.Services;

public interface IUnitOfWork : IDisposable
{
    public IConsumerRepository ConsumerRepository { get; }
    public IProviderRepository ProviderRepository { get; }
    public IOrderRepository OrderRepository { get; }
    public IBidRepository BidRepository { get; }
    Task SaveAsync();
}
