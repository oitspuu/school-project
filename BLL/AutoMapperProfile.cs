using AutoMapper;

namespace BLL;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BLL.DTO.Identity.AppRole, DAL.DTO.Identity.AppRole>().ReverseMap();
        CreateMap<BLL.DTO.Identity.AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
        CreateMap<BLL.DTO.Identity.RefreshToken, DAL.DTO.Identity.RefreshToken>().ReverseMap();
        CreateMap<BLL.DTO.Translations.Language, DAL.DTO.Translations.Language>().ReverseMap();
        CreateMap<BLL.DTO.Translations.Translation, DAL.DTO.Translations.Translation>().ReverseMap();
        CreateMap<BLL.DTO.Translations.OriginalText, DAL.DTO.Translations.OriginalText>().ReverseMap();
        CreateMap<BLL.DTO.Course, DAL.DTO.Course>().ReverseMap();
        CreateMap<BLL.DTO.UserCourse, DAL.DTO.UserCourse>().ReverseMap();
        CreateMap<BLL.DTO.Hobby, DAL.DTO.Hobby>().ReverseMap();
        CreateMap<BLL.DTO.UserHobby, DAL.DTO.UserHobby>().ReverseMap();
        CreateMap<BLL.DTO.School, DAL.DTO.School>().ReverseMap();
        CreateMap<BLL.DTO.SleepDuration, DAL.DTO.SleepDuration>().ReverseMap();
        CreateMap<BLL.DTO.UserWork, DAL.DTO.UserWork>().ReverseMap();
        CreateMap<BLL.DTO.Work, DAL.DTO.Work>().ReverseMap();
        CreateMap<BLL.DTO.WorkHours, DAL.DTO.WorkHours>().ReverseMap();
    }
}