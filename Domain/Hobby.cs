using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Base;
using Base.Interfaces;
using Domain.Translations;

namespace Domain;

public class Hobby : BaseEntity, IDomainEntityId
{
    [MaxLength(128), Display(ResourceType = typeof(Resources.Domain.Hobby), Name = nameof(HobbyName))] 
    public string HobbyName { get; set; } = default!;
    
    public Guid? OriginalTextId { get; set; }
    public OriginalText? NameTranslations { get; set; }
    
    [Display(ResourceType = typeof(Resources.Domain.Hobby), Name = nameof(UserHobbies))]
    public ICollection<UserHobby>? UserHobbies { get; set; }
}