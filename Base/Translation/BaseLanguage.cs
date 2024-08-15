using System.ComponentModel.DataAnnotations;

namespace Base.Translation;

public class BaseLanguage : BaseEntity
{
    [MaxLength(8)]
    public string LanguageIdentifier { get; set; } = default!;
    [MaxLength(256)]
    public string LanguageName { get; set; } = default!;
}