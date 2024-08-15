using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base;
using DAL.Interfaces.Translations;
using Domain.Translations;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories.Translations;

public class LanguageRepository : BaseEntityRepository<Domain.Translations.Language, DAL.DTO.Translations.Language, AppDbContext>,
    ILanguageRepository
{
    public LanguageRepository(AppDbContext dbContext, IMapper mapper) : base(dbContext,
        new DomainDalMapper<Language, DTO.Translations.Language>(mapper))
    {
        
    }

    public async Task<Guid?> GetLanguageIdAsync(string language)
    {
        var id = await RepoDbSet.Where(l => l.LanguageIdentifier == language)
            .Select(id => id.Id).FirstOrDefaultAsync();
        return id;
    }
}