using System;
using Base.Interfaces;

namespace DAL.Interfaces.Translations;

public interface ITranslationRepository : IEntityRepository<DTO.Translations.Translation>
{
    string? FindTranslation(Guid original, Guid languageId);
    DTO.Translations.Translation? FindTranslationEntity(Guid original, Guid languageId);
    Guid CreateTranslation(Guid languageId,  Guid textId, string translation);
}