using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class WorkHoursRepository : BaseEntityRepository<Domain.WorkHours, DAL.DTO.WorkHours, AppDbContext>, IWorkHoursRepository
{
    public WorkHoursRepository(AppDbContext context, IMapper mapper): base(context, new DomainDalMapper<WorkHours, DTO.WorkHours>(mapper))
    {
        
    }
}