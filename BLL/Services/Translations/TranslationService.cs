using AutoMapper;
using Base;
using BLL.Interfaces.Translations;
using DAL;
using DAL.DTO.Translations;
using DAL.Interfaces.Translations;

namespace BLL.Services.Translations;

public class TranslationService : BaseEntityService<DAL.DTO.Translations.Translation, BLL.DTO.Translations.Translation, ITranslationRepository>,
    ITranslationService
{
    public TranslationService(IAppUnitOfWork unitOfWork, ITranslationRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<Translation, DTO.Translations.Translation>(mapper))
    {
        
    }
}