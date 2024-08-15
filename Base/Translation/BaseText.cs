using System.ComponentModel.DataAnnotations;

namespace Base.Translation;

public class BaseText : BaseEntity
{
    public string OriginalText { get; set; } = default!;
}