using Microsoft.EntityFrameworkCore;
using Takenlijst.DataAccess;
using Takenlijst.Models;

namespace Takenlijst.Repositories;

public interface IRepository<T> where T : Basis
{
    Task<List<T>> HaalAllesOp();
    Task<T> HaalOp(Guid code);
    Task<T> VoegToe(T entity);
    Task<T> Wijzig(T entity);
    Task<T> Verwijder(Guid code);
}

public class Repository<T> : IRepository<T> where T : Basis
{
    private readonly TakenDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(TakenDbContext dbContext, DbSet<T> dbSet)
    {
        _dbContext = dbContext;
        _dbSet = dbSet;
    }

    public async Task<List<T>> HaalAllesOp()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> HaalOp(Guid code)
    {
        return await _dbSet.SingleOrDefaultAsync(x => x.Code == code);
    }

    public async Task<T> VoegToe(T entity)
    {
        _dbSet.Add(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> Wijzig(T entity)
    {
        _dbSet.Update(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<T> Verwijder(Guid code)
    {
        var teVerwijderen = _dbSet.SingleOrDefault(x => x.Code == code);

        if (teVerwijderen != null)
        {
            _dbSet.Remove(teVerwijderen);
            await _dbContext.SaveChangesAsync();
        }

        return teVerwijderen;
    }
}