using Quiz.Core.Abstractions;

namespace Quiz.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}