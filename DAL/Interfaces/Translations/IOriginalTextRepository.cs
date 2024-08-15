using System;
using Base.Interfaces;

namespace DAL.Interfaces.Translations;

public interface IOriginalTextRepository : IEntityRepository<DTO.Translations.OriginalText>
{
    Guid CreateTextForTranslation(Guid languageId, string translation);
}