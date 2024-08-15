using System.Threading.Tasks;
using Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Base;

public abstract class UnitOfWork<TAppDbContext> : IUnitOfWork
where TAppDbContext : DbContext
{
    protected readonly TAppDbContext Context;
    
    protected UnitOfWork(TAppDbContext context)
    {
        Context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await Context.SaveChangesAsync();
    }

    public void Clear()
    {
        Context.ChangeTracker.Clear();
    }
}