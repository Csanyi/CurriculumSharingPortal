using AutoMapper;
using CurriculumSharingPortal.Persistence;
using CurriculumSharingPortal.DTO;

namespace CurriculumSharingPortal.WebApi.MappingConfigurations
{
	public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectDto>();
        }
    }

    public class CurriculumProfile : Profile
    {
        public CurriculumProfile()
        {
            CreateMap<Curriculum, CurriculumDto>();
        }
    }

	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<User, UserDto>();
		}
	}

	public class ReviewProfile : Profile
	{
		public ReviewProfile()
		{
			CreateMap<Review, ReviewDto>();
		}
	}

	public class SubjectDtoProfile : Profile
    {
        public SubjectDtoProfile()
        {
            CreateMap<SubjectDto, Subject>();
        }
    }

    public class CurriculumDtoProfile : Profile
    {
        public CurriculumDtoProfile()
        {
            CreateMap<CurriculumDto, Curriculum>();
        }
    }

	public class UserDtoProfile : Profile
	{
		public UserDtoProfile()
		{
			CreateMap<UserDto, User>();
		}
	}

	public class ReviewDtoProfile : Profile
	{
		public ReviewDtoProfile()
		{
			CreateMap<ReviewDto, Review>();
		}
	}
}
