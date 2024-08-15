using System;
using System.ComponentModel.DataAnnotations;

namespace Base;

public class BaseRefreshToken : BaseRefreshToken<Guid>
{
    
}

public class BaseRefreshToken<TKey> : BaseEntity
    where TKey : IEquatable<TKey>
{
    [MaxLength(64)]
    public string RefreshToken { get; set; } = Guid.NewGuid().ToString();
    public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
    
    [MaxLength(64)]
    public string? PreviousRefreshToken { get; set; }
    public DateTime PreviousExpires { get; set; } = DateTime.UtcNow.AddDays(7);
}