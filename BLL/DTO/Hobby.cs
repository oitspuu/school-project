using System;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class Hobby : BaseEntity, IDomainEntityId
{
    public string HobbyName { get; set; } = default!;
    public Guid? OriginalTextId { get; set; }
}