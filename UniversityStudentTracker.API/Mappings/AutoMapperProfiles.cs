using AutoMapper;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.StudySession;

namespace UniversityStudentTracker.API.Mappings;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<StudySession, StudySessionDto>().ReverseMap();
        CreateMap<AddStudySessionDto, StudySession>().ReverseMap();
    }
}