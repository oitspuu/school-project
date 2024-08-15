using System.Threading.Tasks;
using Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Base;

public abstract class BaseBll<TAppDbContext> : IBll
    where TAppDbContext : DbContext
{
    protected readonly IUnitOfWork UoW;

    protected BaseBll(IUnitOfWork uoW)
    {
        UoW = uoW;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await UoW.SaveChangesAsync();
    }

    public void Clear()
    {
        UoW.Clear();
    }
}