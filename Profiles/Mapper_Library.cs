using AutoMapper;
using FinalProject.Models;
using FinalProject.DTOs;

namespace FinalProject.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookTbl, BookDTO>();
            CreateMap<BorrowerTbl, BorrowerDTO>();
            CreateMap<ReturneeTbl, ReturneeDTO>();
            CreateMap<LibraryTransactionVw, LibraryTransactionDTO>()
             .ForMember(dest => dest.BorrowedOn, opt => opt.MapFrom(src => src.BorrowedOn.ToDateTime(TimeOnly.MinValue)))
             .ForMember(dest => dest.ReturnedOn, opt => opt.MapFrom(src => src.ReturnedOn.HasValue ? src.ReturnedOn.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null));
        }
    }
}
