using System;
using Base.Interfaces;

namespace BLL.DTO.Translations;

public class Translation : Base.Translation.BaseTranslation, IDomainEntityId
{
    public Guid LanguageId { get; set; }
    
    public Guid TextId { get; set; }

}