using AutoMapper;
using Base;
using BLL.Interfaces.Translations;
using DAL;
using DAL.DTO.Translations;
using DAL.Interfaces.Translations;

namespace BLL.Services.Translations;

public class LanguageService : BaseEntityService<DAL.DTO.Translations.Language, BLL.DTO.Translations.Language, ILanguageRepository>,
    ILanguageService
{
    public LanguageService(IAppUnitOfWork unitOfWork, ILanguageRepository repository, IMapper mapper) : 
        base(unitOfWork, repository, new DalBllMapper<Language, DTO.Translations.Language>(mapper))
    {
        
    }
}