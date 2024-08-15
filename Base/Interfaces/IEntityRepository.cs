using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Interfaces;

public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, Guid, Guid>
    where TEntity : class
{
    
}

public interface IEntityRepositoryInt<TEntity> : IEntityRepository<TEntity, Guid, int>
    where TEntity : class
{
    
}

public interface IEntityRepository<TEntity, TKey, TEntityKey>
    where TEntity : class
    where TKey : IEquatable<TKey>
    where TEntityKey : IEquatable<TEntityKey>
{
    TEntity? Add(TEntity entity);
    TEntity? Update(TEntity entity);
    
    int Remove(TEntity entity, TKey? userId = default);
    Task<int> RemoveAsync(TEntity entity, TKey? userId = default);
    
    int Remove(TEntityKey entityId, TKey? userId = default);
    Task<int> RemoveAsync(TEntityKey entityId, TKey? userId = default);

    TEntity? FirstOrDefault(TEntityKey entityId, TKey? userId = default, bool noTracking = true);
    Task<TEntity?> FirstOrDefaultAsync(TEntityKey entityId, TKey? userId = default, bool noTracking = true);
    
    IEnumerable<TEntity> GetAll(TKey? userId = default, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(TKey? userId = default, bool noTracking = true);

    bool Exists(TEntityKey entityId, TKey? userId = default);
    Task<bool> ExistsAsync(TEntityKey entityId, TKey? userId = default);
    
}