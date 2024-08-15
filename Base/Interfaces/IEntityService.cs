using System;

namespace Base.Interfaces;


public interface IEntityService<TEntity> : IEntityRepository<TEntity>, IEntityService<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}

public interface IEntityService<TEntity, TKey> : IEntityRepository<TEntity, TKey, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{ 
   
}