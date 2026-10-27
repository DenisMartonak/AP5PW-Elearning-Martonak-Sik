using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PW.Elearning.Domain.Entities;
using PW.Elearning.Domain.Repositories;
using PW.Elearning.Infrastructure.Persistence;

namespace PW.Elearning.Infrastructure.Repositories;

public class Repository<T>(ElearningDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly ElearningDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity) => DbSet.Update(entity);

    public void Remove(T entity) => DbSet.Remove(entity);
}
