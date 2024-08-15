using System;
using System.Linq;
using AutoMapper;
using Base;
using DAL.Interfaces.Translations;
using Domain.Translations;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Repositories.Translations;

public class TranslationRepository : BaseEntityRepository<Domain.Translations.Translation, DAL.DTO.Translations.Translation, AppDbContext>,
    ITranslationRepository
{
    private readonly AppDbContext _context;

    public TranslationRepository(AppDbContext context, IMapper mapper) : base(context, new DomainDalMapper<Translation, DTO.Translations.Translation>(mapper))
    {
        _context = context;
    }

    public string? FindTranslation(Guid original, Guid languageId)
    {
        return RepoDbSet.Where(t => t.LanguageId == languageId && t.TextId == original).AsNoTracking()
            .Select(t => t.Translation).FirstOrDefault();
    }

    public DTO.Translations.Translation? FindTranslationEntity(Guid original, Guid languageId)
    {
        return DalMapper.Map(RepoDbSet.AsNoTracking()
            .FirstOrDefault(t => t.LanguageId == languageId && t.TextId == original));
    }

    public Guid CreateTranslation(Guid languageId, Guid textId, string translation)
    {
        var text = new Translation()
        {
            Id = Guid.NewGuid(),
            LanguageId = languageId,
            TextId = textId,
            Translation = translation
        };
        RepoDbSet.Add(text);
        return textId;
    }
}