using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Translations;

namespace Domain;

public class School : BaseEntity, IDomainEntityId
{
    [MaxLength(128), Display(ResourceType = typeof(Resources.Domain.School), Name = nameof(Name))] 
    public string Name { get; set; } = default!;
    
    public Guid? OriginalTextId { get; set; }
    public OriginalText? NameTranslations { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.School), Name = nameof(Courses))]
    public ICollection<Course>? Courses { get; set; }
}