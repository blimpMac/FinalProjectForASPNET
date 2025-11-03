using AutoMapper;
using FinalProject.DTOs;
using FinalProject.Models;

namespace FinalProject.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookTbl, BookDTO_ReadDTO>();
            CreateMap<BookDTO_CreateDTO, BookTbl>()
                .ForMember(dest => dest.Isbnnumber, opt => opt.MapFrom(src => src.ISBNNumber));
            CreateMap<BookDTO_UpdateDTO, BookTbl>();
            CreateMap<BookTbl, BookDTO_DeleteDTO>()
                .ForMember(dest => dest.ISBNNumber, opt => opt.MapFrom(src => src.Isbnnumber));
            CreateMap<BorrowerTbl, Borrower_ReadDTO>();
            CreateMap<Borrower_CreateDTO, BorrowerTbl>()
                .ForMember(dest => dest.BookIsbn, opt => opt.MapFrom(src => src.BookISBN));
            CreateMap<Borrower_UpdateDTO, BorrowerTbl>();
            CreateMap<ReturneeTbl, Returnee_ReadDTO>();
            CreateMap<Returnee_CreateDTO, ReturneeTbl>();
            CreateMap<Returnee_UpdateDTO, ReturneeTbl>();
            CreateMap<LibraryTransactionVw, LibraryTransaction_ReadDTO>()
                .ForMember(dest => dest.BorrowedOn, opt => opt.MapFrom(src => src.BorrowedOn.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.ReturnedOn, opt => opt.MapFrom(src => src.ReturnedOn.HasValue ? src.ReturnedOn.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null));
        }
    }
}