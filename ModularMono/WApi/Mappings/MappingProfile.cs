using AutoMapper;
using Domain.Entities;
using WApi.DTOs;

namespace WApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDto, User>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLowerInvariant()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                    string.IsNullOrWhiteSpace(src.PhoneNumber) ? null : src.PhoneNumber.Trim()))
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<User, UserResponseDto>();
        }
    }
}
