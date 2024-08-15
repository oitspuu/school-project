using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Interfaces;
using DAL;
using DAL.DTO.Translations;

namespace BLL;

public class BaseEntityService<TDalEntity, TBllEntity, TRepository> :
    BaseEntityService<TDalEntity, TBllEntity, TRepository, Guid>,
    IEntityService<TBllEntity>
    where TBllEntity : class, IDomainEntityId
    where TRepository : IEntityRepository<TDalEntity, Guid, Guid>
    where TDalEntity : class, IDomainEntityId<Guid>
{
    public BaseEntityService(IAppUnitOfWork uoW, TRepository repository, IDalMapper<TDalEntity, TBllEntity> Mapper) : base(uoW,
        repository, Mapper)
    {
    }
}

public class BaseEntityService<TDalEntity, TBllEntity, TRepository, TKey> : IEntityService<TBllEntity, TKey>
    where TRepository : IEntityRepository<TDalEntity, TKey, TKey>
    where TKey : IEquatable<TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
{
    protected readonly IUnitOfWork UoW;
    protected readonly TRepository Repository;
    protected readonly IDalMapper<TDalEntity, TBllEntity> Mapper;

    public BaseEntityService(IUnitOfWork uoW, TRepository repository, IDalMapper<TDalEntity, TBllEntity> mapper)
    {
        UoW = uoW;
        Repository = repository;
        Mapper = mapper;
    }

    public TBllEntity? Add(TBllEntity entity)
    {
        return Mapper.Map(Repository.Add(Mapper.Map(entity)));
    }

    public TBllEntity? Update(TBllEntity entity)
    {
        return Mapper.Map(Repository.Update(Mapper.Map(entity)));
    }

    public int Remove(TBllEntity entity, TKey? userId = default)
    {
        return Repository.Remove(Mapper.Map(entity));
    }

    public async Task<int> RemoveAsync(TBllEntity entity, TKey? userId = default)
    {
        return await Repository.RemoveAsync(Mapper.Map(entity));
    }

    public int Remove(TKey entityId, TKey? userId = default)
    {
        return Repository.Remove(entityId, userId);
    }

    public async Task<int> RemoveAsync(TKey entityId, TKey? userId = default)
    {
        return await Repository.RemoveAsync(entityId, userId);    }

    public TBllEntity? FirstOrDefault(TKey entityId, TKey? userId = default, bool noTracking = true)
    {
        return Mapper.Map(Repository.FirstOrDefault(entityId, userId, noTracking));
    }

    public async Task<TBllEntity?> FirstOrDefaultAsync(TKey entityId, TKey? userId = default, bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(entityId, userId, noTracking));
    }

    public IEnumerable<TBllEntity> GetAll(TKey? userId = default, bool noTracking = true)
    {
        return Repository.GetAll(userId, noTracking).Select(each => Mapper.Map(each)).OfType<TBllEntity>();
    }

    public async Task<IEnumerable<TBllEntity>> GetAllAsync(TKey? userId = default, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(userId, noTracking))
            .Select(each => Mapper.Map(each)).OfType<TBllEntity>();
    }

    public bool Exists(TKey entityId, TKey? userId = default)
    {
        return Repository.Exists(entityId, userId);
    }

    public async Task<bool> ExistsAsync(TKey entityId, TKey? userId = default)
    {
        return await Repository.ExistsAsync(entityId, userId);
    }

}