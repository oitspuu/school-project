using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Base;

public class BaseEntityRepository<TDomainEntity, TDalEntity, TDbContext> : 
    BaseEntityRepository<Guid, Guid, TDomainEntity, TDalEntity, TDbContext>, IEntityRepository<TDalEntity>
    where TDomainEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IDalMapper<TDomainEntity, TDalEntity> dalMapper) : base(dbContext, dalMapper)
    {
        
    }
    
}

public class BaseEntityRepositoryInt<TDomainEntity, TDalEntity, TDbContext> : 
    BaseEntityRepository<int, Guid, TDomainEntity, TDalEntity, TDbContext>, IEntityRepositoryInt<TDalEntity>
    where TDomainEntity : class, IDomainEntityIdInt
    where TDalEntity : class, IDomainEntityIdInt
    where TDbContext : DbContext
{
    public BaseEntityRepositoryInt(TDbContext dbContext, IDalMapper<TDomainEntity, TDalEntity> dalMapper) : base(dbContext, dalMapper)
    {
        
    }
    
}

public class BaseEntityRepository <TEntityKey, TKey, TDomainEntity, TDalEntity, TDbContext>
    where TEntityKey : IEquatable<TEntityKey>
    where TKey : IEquatable<TKey>
    where TDomainEntity : class, IDomainEntityId<TEntityKey>
    where TDalEntity : class, IDomainEntityId<TEntityKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;
    protected readonly IDalMapper<TDomainEntity, TDalEntity> DalMapper;

    public BaseEntityRepository(TDbContext repoDbContext, IDalMapper<TDomainEntity, TDalEntity> dalMapper)
    {
        RepoDbContext = repoDbContext;
        RepoDbSet = repoDbContext.Set<TDomainEntity>();
        DalMapper = dalMapper;
    }

    protected virtual IQueryable<TDomainEntity> CreateQuery(TKey? userId = default, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (userId != null && !userId.Equals(default) && typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)))
        {
            query = query.Include("AppUser")
                .Where(entity => ((IDomainAppUserId<TKey>)entity).AppUserId.Equals(userId));
        }

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
    
    public virtual TDalEntity? Add(TDalEntity entity)
    {
        return DalMapper.Map(RepoDbSet.Add(DalMapper.Map(entity)).Entity);
    }

    public virtual TDalEntity? Update(TDalEntity entity)
    {
        var thing = DalMapper.Map(entity);
        if (thing == null) return null;
        try
        {
            thing = (RepoDbSet.Update(thing)).Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return DalMapper.Map(thing);
    }

    public virtual int Remove(TDalEntity entity, TKey? userId = default)
    {

        var findFirst = FirstOrDefault(entity.Id, userId);
        if (findFirst == null) return 0;
        var mapped = DalMapper.Map(findFirst);
        if (mapped == null) return 0;
        RepoDbSet.Entry(mapped).State = EntityState.Deleted;
        //.Remove(mapped);
        return 1;
    }

    public virtual async Task<int> RemoveAsync(TDalEntity entity, TKey? userId = default)
    {
        var findFirst = await FirstOrDefaultAsync(entity.Id, userId);
        if (findFirst == null) return 0;
        var mapped = DalMapper.Map(findFirst);
        if (mapped == null) return 0;
        RepoDbSet.Entry(mapped).State = EntityState.Deleted;
        return 1;
    }

    public virtual int Remove(TEntityKey entityId, TKey? userId = default)
    {
        var findFirst = FirstOrDefault(entityId, userId);
        if (findFirst == null) return 0;
        var mapped = DalMapper.Map(findFirst);
        if (mapped == null) return 0;
        RepoDbSet.Entry(mapped).State = EntityState.Deleted;
        return 1;
    }

    public virtual async Task<int> RemoveAsync(TEntityKey entityId, TKey? userId = default)
    {
        var findFirst =await FirstOrDefaultAsync(entityId, userId);
        if (findFirst == null) return 0;
        var mapped = DalMapper.Map(findFirst);
        if (mapped == null) return 0;
        RepoDbSet.Entry(mapped).State = EntityState.Deleted;
        return 1;
    }

    public virtual TDalEntity? FirstOrDefault(TEntityKey entityId, TKey? userId = default, bool noTracking = true)
    {
        return DalMapper.Map(CreateQuery(userId, noTracking).FirstOrDefault(e => e.Id.Equals(entityId)));
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TEntityKey entityId, TKey? userId = default, bool noTracking = true)
    {
        return DalMapper.Map(await CreateQuery(userId, noTracking).FirstOrDefaultAsync(e => e.Id.Equals(entityId)));
    }

    public virtual IEnumerable<TDalEntity> GetAll(TKey? userId = default, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking).ToList().Select(id => DalMapper.Map(id)).OfType<TDalEntity>();
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(TKey? userId = default, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking).ToListAsync())
            .Select(id => DalMapper.Map(id)).OfType<TDalEntity>();
    }

    public virtual bool Exists(TEntityKey entityId, TKey? userId = default)
    {
        return CreateQuery(userId).Any(e => e.Id.Equals(entityId));
    }

    public virtual async Task<bool> ExistsAsync(TEntityKey entityId, TKey? userId = default)
    {
        return await CreateQuery(userId).AnyAsync(e => e.Id.Equals(entityId));
    }
}