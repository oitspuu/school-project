using System;
using Base.Interfaces;

namespace Test.DAL;

public class TestEntity : IDomainEntityId
{
    public Guid Id {
        get;
        set;
    }

    public string Name { get; set; } = default!;
}