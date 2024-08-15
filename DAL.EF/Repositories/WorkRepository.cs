using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class WorkRepository : BaseEntityRepository<Domain.Work, DAL.DTO.Work, AppDbContext>, IWorkRepository
{
    public WorkRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<Work, DTO.Work>(mapper))
    {
        
    }
}