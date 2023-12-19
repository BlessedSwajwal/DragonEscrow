using Application.Common.Repositories;

namespace Application.Common.Services;

public interface IUnitOfWork : IDisposable
{
    public IConsumerRepository ConsumerRepository { get; }
    public IProviderRepository ProviderRepository { get; }
    Task SaveAsync();
}
