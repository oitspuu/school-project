using System;
using System.Collections.Generic;
using Base.Interfaces;

namespace BLL.DTO.Translations;

public class OriginalText : Base.Translation.BaseText, IDomainEntityId
{
    public Guid LanguageId { get; set; }
    
    public IEnumerable<Language>? Languages { get; set; }
    public IEnumerable<Translation>? Translations { get; set; }
}