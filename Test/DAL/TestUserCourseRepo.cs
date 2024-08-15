using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.EF;
using DAL.EF.Repositories;
using Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.DAL;

public class TestUserCourseRepo
{
    private readonly AppDbContext _ctx;
    private readonly UserCourseRepository _testRepo;
    
    public TestUserCourseRepo()
    {
        
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new AppDbContext(optionsBuilder.Options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg 
            => cfg.CreateMap<UserCourse, global::DAL.DTO.UserCourse>().ReverseMap());
        var mapper = config.CreateMapper();

        _testRepo =
            new UserCourseRepository(
                _ctx,
                mapper
            );
    }
    
     
    [Fact]
    public async Task TestAdd()
    {
        // arrange
        _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _ctx.SaveChangesAsync();

        // act
        var data = await _testRepo.GetAllAsync();

        // assert
        Assert.Single(data);
    }

    [Fact]
    public async Task TestUpdate()
    {
        var entity = new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };
        _testRepo.Add(entity);
        _ctx.SaveChanges();
        entity.HomeworkTime = TimeSpan.MaxValue;
        var result =  _testRepo.Update(entity);
        await _ctx.SaveChangesAsync();
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestFirstOrDefault()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _ctx.SaveChangesAsync();
        var result = _testRepo.FirstOrDefault(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestFirstOrDefaultAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _ctx.SaveChangesAsync();
        var result = await _testRepo.FirstOrDefaultAsync(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestGetAll()
    {
        _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _ctx.SaveChangesAsync();

        var count = _testRepo.GetAll().Count();
        Assert.Equivalent(count, 3); 
    }
    
    [Fact]
    public async Task TestGetAllAsync()
    {
        var one = _testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        var two =_testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        var three =_testRepo.Add(new global::DAL.DTO.UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        var array = new ArrayList() { one, two, three };
        await _ctx.SaveChangesAsync();

        var list = (await _testRepo.GetAllAsync()).ToArray();
        Assert.Equivalent(list, array); 
    }

    [Fact]
    public async Task TestExists()
    {
        
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        await _ctx.SaveChangesAsync();
        
        Assert.True(_testRepo.Exists(guid));
    }
    
    [Fact]
    public async Task TestExistsAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        await _ctx.SaveChangesAsync();
        
        Assert.True(await _testRepo.ExistsAsync(guid));
    }
    
    [Fact]
    public async Task TestRemove()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = _testRepo.Remove(entity);
        await _ctx.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task TestRemoveAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = await _testRepo.RemoveAsync(entity);
        await _ctx.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task TestRemoveByIdAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = await _testRepo.RemoveAsync(entity.Id);
        await _ctx.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task TestRemoveById()
    {
        var guid = Guid.NewGuid();
        var entity = new global::DAL.DTO.UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _testRepo.Add(entity);
        await _ctx.SaveChangesAsync();

        var result = _testRepo.Remove(entity.Id);
        await _ctx.SaveChangesAsync();

        Assert.Equal(1, result);
    }
    
}