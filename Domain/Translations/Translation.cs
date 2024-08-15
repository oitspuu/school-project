using System;
using Base.Interfaces;

namespace Domain.Translations;

public class Translation : Base.Translation.BaseTranslation, IDomainEntityId
{
    public Guid LanguageId { get; set; }
    public Language? Language { get; set; }
    
    public Guid TextId { get; set; }
    public OriginalText? Text { get; set; }
}