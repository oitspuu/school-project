using System;
using Base.Interfaces;
using BLL;
using DAL;
using Test.DAL;

namespace Test.BLL;

public class TestEntityService: BaseEntityService<TestEntity, TestEntity, TestEntityRepository, Guid>,
    IEntityService<TestEntity>
{
    public TestEntityService(TestUoW uoW, TestEntityRepository repository, IDalMapper<TestEntity, TestEntity> mapper) 
        : base(uoW, repository, mapper)
    {
        
    }
}