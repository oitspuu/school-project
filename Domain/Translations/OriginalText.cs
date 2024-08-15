using System;
using System.Collections.Generic;
using Base.Interfaces;

namespace Domain.Translations;

public class OriginalText : Base.Translation.BaseText, IDomainEntityId
{
    public Guid LanguageId { get; set; }
    public Language? Language { get; set; }
    
    public ICollection<Translation>? Translations { get; set; }
}