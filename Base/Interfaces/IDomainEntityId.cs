using System;

namespace Base.Interfaces;

public interface IDomainEntityId : IDomainEntityId<Guid>
{
    
}
public interface IDomainEntityIdInt : IDomainEntityId<int>
{
    
}

public interface IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
}