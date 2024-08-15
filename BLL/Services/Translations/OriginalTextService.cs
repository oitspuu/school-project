using AutoMapper;
using Base;
using BLL.Interfaces.Translations;
using DAL;
using DAL.DTO.Translations;
using DAL.Interfaces.Translations;

namespace BLL.Services.Translations;

public class OriginalTextService : BaseEntityService<DAL.DTO.Translations.OriginalText, BLL.DTO.Translations.OriginalText, IOriginalTextRepository>,
    IOriginalTextService
{
    public OriginalTextService(IAppUnitOfWork unitOfWork, IOriginalTextRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<OriginalText, DTO.Translations.OriginalText>(mapper))
    {
        
    }
}