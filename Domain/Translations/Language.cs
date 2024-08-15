using System.Collections.Generic;
using Base.Interfaces;
using Base.Translation;

namespace Domain.Translations;

public class Language : BaseLanguage, IDomainEntityId
{
    public ICollection<Translation>? Translations { get; set; }
    public ICollection<OriginalText>? OriginalTexts { get; set; }
}