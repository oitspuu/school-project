using System;
using Base;
using Base.Interfaces;

namespace BLL.DTO;

public class School : BaseEntity, IDomainEntityId
{
    public string Name { get; set; } = default!;
    public Guid? OriginalTextId { get; set; }
}