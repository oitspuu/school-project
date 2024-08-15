using System;
using AutoMapper;
using Base;
using DAL.Interfaces.Translations;
using Domain.Translations;

namespace DAL.EF.Repositories.Translations;

public class OriginalTextRepository : BaseEntityRepository<Domain.Translations.OriginalText, DAL.DTO.Translations.OriginalText, AppDbContext>,
    IOriginalTextRepository
{
    public OriginalTextRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<OriginalText, DTO.Translations.OriginalText>(mapper))
    {
        
    }

    public Guid CreateTextForTranslation(Guid languageId, string translation)
    {
        var original = new OriginalText()
        {
            Id = Guid.NewGuid(),
            LanguageId = languageId,
            OriginalText = translation
        };
        RepoDbSet.Add(original);
        return original.Id;
    }
}