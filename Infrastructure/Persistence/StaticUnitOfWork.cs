using Application.Common.Repositories;
using Application.Common.Services;

namespace Infrastructure.Persistence;

public class StaticUnitOfWork : IUnitOfWork
{
    private readonly IConsumerRepository _userRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IOrderRepository _orderRepository;

    public StaticUnitOfWork()
    {
        _userRepository = new ConsumerRepository();
        _providerRepository = new ProviderRepository();
        _orderRepository = new OrderRepository();
    }

    public IConsumerRepository ConsumerRepository => _userRepository;

    public IProviderRepository ProviderRepository => _providerRepository;

    public IOrderRepository OrderRepository => _orderRepository;

    private void Dispose(bool disposing)
    {

    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }
}
