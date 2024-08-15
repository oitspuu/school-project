using AutoMapper;
using Base;
using DAL.Interfaces;
using Domain;

namespace DAL.EF.Repositories;

public class SchoolRepository : BaseEntityRepository<Domain.School, DAL.DTO.School, AppDbContext>, ISchoolRepository
{
    public SchoolRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<School, DTO.School>(mapper))
    {
        
    }
}