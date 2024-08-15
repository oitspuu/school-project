using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.DAL;

public class BaseRepositoryTest
{
    private readonly TestDbContext _ctx;
    private readonly TestEntityRepository _testEntityRepository;
    
    public BaseRepositoryTest()
    {
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new TestDbContext(optionsBuilder.Options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg => cfg.CreateMap<TestEntity, TestEntity>());
        var mapper = config.CreateMapper();

        _testEntityRepository =
            new TestEntityRepository(
                _ctx,
                new DomainDalMapper<TestEntity,TestEntity>(mapper)
            );
    }
    
    [Fact]
    public async Task TestAdd()
    {
        // arrange
        _testEntityRepository.Add(new TestEntity() {Name = "Foo"});
        await _ctx.SaveChangesAsync();

        // act
        var data = await _testEntityRepository.GetAllAsync();

        // assert
        Assert.Single(data);
    }

    [Fact]
    public async Task TestUpdate()
    {
        var entity = new TestEntity() { Name = "Foo" };
        _testEntityRepository.Add(entity);
        _ctx.SaveChanges();
        entity.Name = "sadd";
        var result =  _testEntityRepository.Update(entity);
        await _ctx.SaveChangesAsync();
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

        _testEntityRepository.Add(entity);
        _testEntityRepository.Add(new TestEntity(){Name = "name"});
        await _ctx.SaveChangesAsync();
        var result = _testEntityRepository.FirstOrDefault(guid);
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

        _testEntityRepository.Add(entity);
        _testEntityRepository.Add(new TestEntity(){Name = "name"});
        await _ctx.SaveChangesAsync();
        var result = await _testEntityRepository.FirstOrDefaultAsync(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestGetAll()
    {
        _testEntityRepository.Add(new TestEntity(){Name = "name"});
        _testEntityRepository.Add(new TestEntity(){Name = "asdaa"});
        _testEntityRepository.Add(new TestEntity(){Name = "nadd"});
        await _ctx.SaveChangesAsync();

        var count = _testEntityRepository.GetAll().Count();
        Assert.Equivalent(count, 3); 
    }
    
    [Fact]
    public async Task TestGetAllAsync()
    {
        var one = _testEntityRepository.Add(new TestEntity(){Name = "name"});
        var two =_testEntityRepository.Add(new TestEntity(){Name = "asdaa"});
        var three =_testEntityRepository.Add(new TestEntity(){Name = "nadd"});
        var array = new ArrayList() { one, two, three };
        await _ctx.SaveChangesAsync();

        var list = (await _testEntityRepository.GetAllAsync()).ToArray();
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

        _testEntityRepository.Add(entity);
        await _ctx.SaveChangesAsync();
        
        Assert.True(_testEntityRepository.Exists(guid));
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

        _testEntityRepository.Add(entity);
        await _ctx.SaveChangesAsync();
        
        Assert.True(await _testEntityRepository.ExistsAsync(guid));
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

        _testEntityRepository.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = _testEntityRepository.Remove(entity);
        await _ctx.SaveChangesAsync();

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

        _testEntityRepository.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = await _testEntityRepository.RemoveAsync(entity);
        await _ctx.SaveChangesAsync();

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

        _testEntityRepository.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = await _testEntityRepository.RemoveAsync(entity.Id);
        await _ctx.SaveChangesAsync();

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

        _testEntityRepository.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = _testEntityRepository.Remove(entity.Id);
        await _ctx.SaveChangesAsync();

        Assert.Equal(1, result);
    }

}