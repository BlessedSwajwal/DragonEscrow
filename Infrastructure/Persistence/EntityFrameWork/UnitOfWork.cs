using Application.Common.Repositories;
using Application.Common.Services;
using Infrastructure.Persistence.EntityFrameWork.Repositories;


namespace Infrastructure.Persistence.EntityFrameWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly IConsumerRepository _consumerRepository;
    private readonly IProviderRepository _providerRepository;
    private readonly DragonEscrowDbContext _dbContext;
    private bool _disposed = false;

    public UnitOfWork(DragonEscrowDbContext dbContext)
    {
        _dbContext = dbContext;
        _consumerRepository = new EConsumerRepository(_dbContext);
        _providerRepository = new EProviderRepository(_dbContext);
    }

    public IConsumerRepository ConsumerRepository => _consumerRepository;

    public IProviderRepository ProviderRepository => _providerRepository;


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
        await _dbContext.SaveChangesAsync();
    }
}
