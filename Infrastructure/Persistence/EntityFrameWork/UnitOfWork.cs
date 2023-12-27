using Application.Common.Repositories;
using Application.Common.Services;
using Domain.Common;
using Infrastructure.Persistence.EntityFrameWork.Repositories;
using MediatR;


namespace Infrastructure.Persistence.EntityFrameWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IConsumerRepository _consumerRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IBidRepository _bidRepository;
    private readonly DragonEscrowDbContext _dbContext;
    private readonly IPublisher _publisher;
    private bool _disposed = false;

    public UnitOfWork(DragonEscrowDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _consumerRepository = new EConsumerRepository(_dbContext);
        _providerRepository = new EProviderRepository(_dbContext);
        _orderRepository = new EOrderRepository(_dbContext);
        _bidRepository = new EBidRepository(_dbContext);
        _publisher = publisher;
    }

    public IConsumerRepository ConsumerRepository => _consumerRepository;

    public IProviderRepository ProviderRepository => _providerRepository;

    public IOrderRepository OrderRepository => _orderRepository;

    public IBidRepository BidRepository => _bidRepository;

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (!this._disposed)
            {
                _dbContext.Dispose();
            }
        }
        this._disposed = true;
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task SaveAsync()
    {
        await PublishDomainEvents();
        await _dbContext.SaveChangesAsync();
    }

    private async Task PublishDomainEvents()
    {
        if (_dbContext is null) return;

        var entitiesWithDomainEvents = _dbContext.ChangeTracker.Entries<IHasDomainEvents>()
                                    .Where(entries => entries.Entity.DomainEvents.Any())
                                    .Select(entry => entry.Entity).ToList();

        //Get domain events
        var domainEvents = entitiesWithDomainEvents.SelectMany(en => en.DomainEvents).ToList();

        //Clear domain events
        foreach (var entity in entitiesWithDomainEvents)
        {
            entity.ClearDomainEvents();
        }

        //Publish the domain events
        foreach (var domainevent in domainEvents)
        {
            await _publisher.Publish(domainevent);
        }

        return;
    }
}
