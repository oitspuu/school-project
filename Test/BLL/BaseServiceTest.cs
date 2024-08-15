using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Test.DAL;
using Xunit;

namespace Test.BLL;

public class BaseServiceTest
{
    private readonly TestUoW _uoW;
    private readonly TestEntityService _service;
    
    public BaseServiceTest()
    {
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var ctx = new TestDbContext(optionsBuilder.Options);

        // reset db
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg => cfg.CreateMap<TestEntity, TestEntity>());
        var mapper = config.CreateMapper();

        var testEntityRepository =
            new TestEntityRepository(
                ctx,
                new DomainDalMapper<TestEntity,TestEntity>(mapper)
            );
        _uoW = new TestUoW(ctx, new DomainDalMapper<TestEntity, TestEntity>(mapper));
        _service = new TestEntityService(_uoW, testEntityRepository,
            new DomainDalMapper<TestEntity, TestEntity>(mapper));

    }
    
     [Fact]
    public async Task TestAdd()
    {
        // arrange
        _service.Add(new TestEntity() {Name = "Foo"});
        await _uoW.SaveChangesAsync();

        // act
        var data = await _service.GetAllAsync();

        // assert
        Assert.Single(data);
    }

    [Fact]
    public async Task TestUpdate()
    {
        var entity = new TestEntity() { Name = "Foo" };
        _service.Add(entity);
        await _uoW.SaveChangesAsync();
        entity.Name = "sadd";
        var result =  _service.Update(entity);
        await _uoW.SaveChangesAsync();
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestFirstOrDefault()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        _service.Add(new TestEntity(){Name = "name"});
        await _uoW.SaveChangesAsync();
        var result = _service.FirstOrDefault(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestFirstOrDefaultAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        _service.Add(new TestEntity(){Name = "name"});
        await _uoW.SaveChangesAsync();
        var result = await _service.FirstOrDefaultAsync(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestGetAll()
    {
        _service.Add(new TestEntity(){Name = "name"});
        _service.Add(new TestEntity(){Name = "asdaa"});
        _service.Add(new TestEntity(){Name = "nadd"});
        await _uoW.SaveChangesAsync();

        var count = _service.GetAll().Count();
        Assert.Equivalent(count, 3); 
    }
    
    [Fact]
    public async Task TestGetAllAsync()
    {
        var one = _service.Add(new TestEntity(){Name = "name"});
        var two =_service.Add(new TestEntity(){Name = "asdaa"});
        var three =_service.Add(new TestEntity(){Name = "nadd"});
        var array = new ArrayList() { one, two, three };
        await _uoW.SaveChangesAsync();

        var list = (await _service.GetAllAsync()).ToArray();
        Assert.Equivalent(list, array); 
    }

    [Fact]
    public async Task TestExists()
    {
        
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();
        
        Assert.True(_service.Exists(guid));
    }
    
    [Fact]
    public async Task TestExistsAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();
        
        Assert.True(await _service.ExistsAsync(guid));
    }
    
    [Fact]
    public async Task TestRemove()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();

        var result = _service.Remove(entity);
        await _uoW.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task TestRemoveAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();

        var result = await _service.RemoveAsync(entity);
        await _uoW.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task TestRemoveByIdAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();

        var result = await _service.RemoveAsync(entity.Id);
        await _uoW.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task TestRemoveById()
    {
        var guid = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = guid,
            Name = "Foo"
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();

        var result = _service.Remove(entity.Id);
        await _uoW.SaveChangesAsync();

        Assert.Equal(1, result);
    }


}