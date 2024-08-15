using AutoMapper;

namespace DAL.EF;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Domain.Identity.AppRole, DAL.DTO.Identity.AppRole>().ReverseMap();
        CreateMap<Domain.Identity.AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<Domain.Identity.RefreshToken, DAL.DTO.Identity.RefreshToken>().ReverseMap();
        CreateMap<Domain.Translations.Language, DAL.DTO.Translations.Language>().ReverseMap();
        CreateMap<Domain.Translations.Translation, DAL.DTO.Translations.Translation>().ReverseMap();
        CreateMap<Domain.Translations.OriginalText, DAL.DTO.Translations.OriginalText>().ReverseMap();
        CreateMap<Domain.Course, DAL.DTO.Course>().ReverseMap();
        CreateMap<Domain.UserCourse, DAL.DTO.UserCourse>().ReverseMap();
        CreateMap<Domain.Hobby, DAL.DTO.Hobby>().ReverseMap();
        CreateMap<Domain.UserHobby, DAL.DTO.UserHobby>().ReverseMap();
        CreateMap<Domain.School, DAL.DTO.School>().ReverseMap();
        CreateMap<Domain.SleepDuration, DAL.DTO.SleepDuration>().ReverseMap();
        CreateMap<Domain.UserWork, DAL.DTO.UserWork>().ReverseMap();
        CreateMap<Domain.Work, DAL.DTO.Work>().ReverseMap();
        CreateMap<Domain.WorkHours, DAL.DTO.WorkHours>().ReverseMap();
    }
}