using AutoMapper;

namespace DTO;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BLL.DTO.Identity.RefreshToken, DTO.v1_0.Identity.LogoutInfo>().ReverseMap();
        
        CreateMap<BLL.DTO.UserCourse, DTO.v1_0.UserCourse>().ReverseMap();
        CreateMap<BLL.DTO.UserCourse, DTO.v1_0.UserCourseBasic>().ReverseMap();
        CreateMap<BLL.DTO.UserCourse, DTO.v1_0.UserCourseAddTime>().ReverseMap();
        CreateMap<BLL.DTO.UserCourse, DTO.v1_0.UserCourseCreate>().ReverseMap();
        
        CreateMap<BLL.DTO.UserWork, DTO.v1_0.UserWork>().ReverseMap();
        CreateMap<BLL.DTO.UserWork, DTO.v1_0.UserWorkCreate>().ReverseMap();
        
        CreateMap<BLL.DTO.UserHobby, DTO.v1_0.UserHobby>().ReverseMap();
        CreateMap<BLL.DTO.UserHobby, DTO.v1_0.UserHobbyAddTime>().ReverseMap();
        CreateMap<BLL.DTO.UserHobby, DTO.v1_0.UserHobbyCreate>().ReverseMap();
        
        CreateMap<BLL.DTO.WorkHours, DTO.v1_0.WorkHours>().ReverseMap();
        CreateMap<BLL.DTO.WorkHours, DTO.v1_0.WorkHoursCreate>().ReverseMap();

        CreateMap<BLL.DTO.SleepDuration, DTO.v1_0.Sleep>().ReverseMap();
        CreateMap<BLL.DTO.SleepDuration, DTO.v1_0.SleepCreate>().ReverseMap();

    }
}