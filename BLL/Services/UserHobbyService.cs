using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Base.Helpers;
using BLL.DTO.Translations;
using BLL.Interfaces;
using DAL;
using DAL.DTO;
using DAL.Interfaces;
using Hobby = BLL.DTO.Hobby;

namespace BLL.Services;

public class UserHobbyService : BaseEntityService<DAL.DTO.UserHobby, BLL.DTO.UserHobby, IUserHobbyRepository>, IUserHobbyService
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserHobbyService(IAppUnitOfWork unitOfWork, IUserHobbyRepository repository, IMapper mapper) :
        base(unitOfWork, repository, new DalBllMapper<UserHobby, DTO.UserHobby>(mapper))
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<DTO.UserHobby?> AddUserHobby(DTO.UserHobby entity)
    {
        entity.Id = Guid.NewGuid();
        entity.HobbyId = Guid.NewGuid();
        
        var hobbyMapper = new DalBllMapper<Hobby, DAL.DTO.Hobby>(_mapper);
        var hobby = new DTO.Hobby()
        {
            HobbyName = entity.HobbyName,
            Id = entity.HobbyId
        };
        if (entity.Language != null)
        {
            var langId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(entity.Language);
            if (langId != null)
            {
                var text = new OriginalText()
                {
                    Id = Guid.NewGuid(),
                    OriginalText = entity.HobbyName,
                    LanguageId = (Guid)langId
                };
                var textMapper = new DalBllMapper<OriginalText, DAL.DTO.Translations.OriginalText>(_mapper);
                _unitOfWork.TextRepository.Add(textMapper.Map(text));

                hobby.OriginalTextId = text.Id;
                
                _unitOfWork.HobbyRepository.Add(hobbyMapper.Map(hobby));

                var translation = new Translation()
                {
                    Id = Guid.NewGuid(),
                    LanguageId = (Guid) langId,
                    TextId = text.Id,
                    Translation = entity.HobbyName
                };
                var translationMapper = new DalBllMapper<Translation, DAL.DTO.Translations.Translation>(_mapper);
                _unitOfWork.TranslationRepository.Add(translationMapper.Map(translation));
            }
        }
        else
        {
            _unitOfWork.HobbyRepository.Add(hobbyMapper.Map(hobby));
        }
        Repository.Add(Mapper.Map(entity));

        return entity;
    }

    public async Task<DTO.UserHobby?> UpdateUserHobby(DTO.UserHobby entity)
    {
        Guid? id = null;
        if (entity.Language != null)
        {
            id = await SetTranslation(entity.OriginalTextId, entity.Language, entity.HobbyName);
        }
        var original = await Repository.FirstOrDefaultAsync(entity.Id, entity.AppUserId);
        if (original == null) return null;
        
        var originalHobby = await _unitOfWork.HobbyRepository.FirstOrDefaultAsync(original.HobbyId);
        if (originalHobby == null) return null;
        
        if (id != null)
        {
            originalHobby.OriginalTextId = id;
            entity.OriginalTextId = id;
        }
        
        originalHobby.HobbyName = entity.HobbyName;
        _unitOfWork.HobbyRepository.Update(originalHobby);
        
        original.TimeSpent = entity.TimeSpent;
        Repository.Update(original);
        
        return entity;
    }

    public async Task<DTO.UserHobby?> GetUserHobbyAsync(Guid appUser, Guid userHobby, string language)
    {
        var thisUserHobby = Mapper.Map(await Repository.FirstOrDefaultAsync(userHobby, appUser));
        if (thisUserHobby == null) return null;

        thisUserHobby = await AddInformation(thisUserHobby, language);

        return thisUserHobby;
    }

    public async Task<ICollection<DTO.UserHobby>> GetAllUserHobbiesAsync(Guid appUser, string language)
    {
        var hobbies = new List<DTO.UserHobby>();
        var userHobbies = (await Repository.GetAllAsync(appUser))
            .Select(u => Mapper.Map(u)).OfType<BLL.DTO.UserHobby>().ToList();
        
        foreach (var userHobby in userHobbies)
        {
            var hobby = await AddInformation(userHobby, language);
            hobbies.Add(hobby);
        }
        return hobbies;
    }

    public async Task<bool> AddTime(Guid appUser, Guid userHobby, TimeSpan time)
    {
        var hobby = await Repository.FirstOrDefaultAsync(userHobby, appUser);
        if (hobby == null) return false;

        hobby.TimeSpent += time;

        var updated = Repository.Update(hobby);
        return updated != null;
    }

    private async Task<DTO.UserHobby> AddInformation(DTO.UserHobby userHobby, string language)
    {
        var hobby = await _unitOfWork.HobbyRepository.FirstOrDefaultAsync(userHobby.HobbyId);
        if (hobby == null) return userHobby;

        userHobby.HobbyName = hobby.HobbyName;
        userHobby.OriginalTextId = hobby.OriginalTextId;
        
        if (language is not (Constants.English or Constants.Estonian) || userHobby.OriginalTextId is null) return userHobby;

        userHobby.Language = language;

        userHobby.HobbyName = await GetTranslation((Guid) userHobby.OriginalTextId, language) ?? hobby.HobbyName;

        return userHobby;
    }
    
    private async Task<string?> GetTranslation(Guid textId, string language)
    {
        var languageId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(language);
        return languageId == null ? null : _unitOfWork.TranslationRepository.FindTranslation(textId, (Guid) languageId);
    }

    private async Task<Guid?> SetTranslation(Guid? textId, string language, string translation)
    {
        var languageId = await _unitOfWork.LanguageRepository.GetLanguageIdAsync(language);
        if (languageId == null) return null;

        if (textId == null)
        {
            return CreateNewTranslation((Guid)languageId, translation);
        }
        var original = await _unitOfWork.TextRepository.FirstOrDefaultAsync((Guid) textId);
        
        if (original == null)
        {
            return CreateNewTranslation((Guid)languageId, translation);
        }

        if (original.LanguageId == (Guid)languageId)
        {
            original.OriginalText = translation;
        }
        
        var text = _unitOfWork.TranslationRepository.FindTranslationEntity(original.Id, (Guid) languageId);
        UoW.Clear();
        if (text == null)
        {
            var newText = new DAL.DTO.Translations.Translation()
            {
                Id = Guid.NewGuid(),
                LanguageId = (Guid)languageId,
                TextId = original.Id,
                Translation = translation
            };
            _unitOfWork.TranslationRepository.Add(newText);
        }
        else
        {
            text.Translation = translation;
            _unitOfWork.TranslationRepository.Update(text);
        }

        _unitOfWork.TextRepository.Update(original);
        return original.Id;
    }

    private Guid CreateNewTranslation(Guid languageId, string translation)
    {
        var id = _unitOfWork.TextRepository.CreateTextForTranslation(languageId, translation);
        _unitOfWork.TranslationRepository.CreateTranslation(languageId, id, translation);
        return id;
    }
}