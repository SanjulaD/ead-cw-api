using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Break;
using UniversityStudentTracker.API.Models.DTO.StudySession;
using UniversityStudentTracker.API.Models.DTO.User;

namespace UniversityStudentTracker.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<StudySession, StudySessionDto>().ReverseMap();
        CreateMap<AddStudySessionDto, StudySession>().ReverseMap();

        CreateMap<Break, BreakDto>().ReverseMap();
        CreateMap<AddBreakDto, Break>().ReverseMap();

        CreateMap<IdentityUser, UserDto>().ReverseMap();
    }
}