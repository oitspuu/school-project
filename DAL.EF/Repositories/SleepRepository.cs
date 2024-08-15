using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class SleepRepository : BaseEntityRepository<Domain.SleepDuration, DAL.DTO.SleepDuration, AppDbContext>, ISleepRepository
{
    public SleepRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<SleepDuration, DTO.SleepDuration>(mapper))
    {
        
    }
}