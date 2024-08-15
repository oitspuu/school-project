using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Services;
using DAL;
using DAL.EF;
using DAL.EF.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.BLL;

public class TestUserCourseService
{
    private readonly IAppUnitOfWork _uoW;
    private readonly UserCourseService _service;
    
    public TestUserCourseService()
    {
        
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        var ctx = new AppDbContext(optionsBuilder.Options);

        // reset db
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();

        var config = new MapperConfiguration(cfg 
            => cfg.CreateMap<Domain.UserCourse, global::DAL.DTO.UserCourse>().ReverseMap());
        var mapper = config.CreateMapper();

        var testRepo =
            new UserCourseRepository(
                ctx,
                mapper
            );
        
        config = new MapperConfiguration(cfg 
            => cfg.CreateMap<UserCourse, global::DAL.DTO.UserCourse>().ReverseMap());
        mapper = config.CreateMapper();
        _uoW = new AppUOW(ctx, mapper);
        _service = new UserCourseService(_uoW, testRepo, mapper);
    }
    
    [Fact]
    public async Task TestAdd()
    {
        // arrange
        _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _uoW.SaveChangesAsync();

        // act
        var data = await _service.GetAllAsync();

        // assert
        Assert.Single(data);
    }

    [Fact]
    public async Task TestUpdate()
    {
        var entity = new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };
        _service.Add(entity);
        await _uoW.SaveChangesAsync();
        entity.HomeworkTime = TimeSpan.MaxValue;
        var result =  _service.Update(entity);
        await _uoW.SaveChangesAsync();
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestFirstOrDefault()
    {
        var guid = Guid.NewGuid();
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _service.Add(entity);
        _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _uoW.SaveChangesAsync();
        var result = _service.FirstOrDefault(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestFirstOrDefaultAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _service.Add(entity);
        _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _uoW.SaveChangesAsync();
        var result = await _service.FirstOrDefaultAsync(guid);
        Assert.Equivalent(entity, result);
    }
    
    [Fact]
    public async Task TestGetAll()
    {
        _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        await _uoW.SaveChangesAsync();

        var count = _service.GetAll().Count();
        Assert.Equivalent(count, 3); 
    }
    
    [Fact]
    public async Task TestGetAllAsync()
    {
        var one = _service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        var two =_service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        var three =_service.Add(new UserCourse()
        {
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        });
        var array = new ArrayList() { one, two, three };
        await _uoW.SaveChangesAsync();

        var list = (await _service.GetAllAsync()).ToArray();
        Assert.Equivalent(list, array); 
    }

    [Fact]
    public async Task TestExists()
    {
        
        var guid = Guid.NewGuid();
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();
        
        Assert.True(_service.Exists(guid));
    }
    
    [Fact]
    public async Task TestExistsAsync()
    {
        var guid = Guid.NewGuid();
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();
        
        Assert.True(await _service.ExistsAsync(guid));
    }
    
    [Fact]
    public async Task TestRemove()
    {
        var guid = Guid.NewGuid();
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
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
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
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
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
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
        var entity = new UserCourse()
        {
            Id = guid,
            AppUserId = Guid.NewGuid(),
            CourseId = Guid.NewGuid(),
            HomeworkTime = TimeSpan.Zero
        };

        _service.Add(entity);
        await _uoW.SaveChangesAsync();

        var result = _service.Remove(entity.Id);
        await _uoW.SaveChangesAsync();

        Assert.Equal(1, result);
    }


}