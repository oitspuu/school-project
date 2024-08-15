using System;
using Base.Interfaces;

namespace DAL.DTO.Translations;

public class OriginalText : Base.Translation.BaseText, IDomainEntityId
{
    public Guid LanguageId { get; set; }
}