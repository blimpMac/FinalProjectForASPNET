using AutoMapper;
using Midterm_Project.DTO;
using Midterm_Project.Models;

namespace Midterm_Project.Profiles
{
    public class CookBook_RecipeDBProfile : Profile
    {
        public CookBook_RecipeDBProfile()
        {
            CreateMap<RecipesTb, CookBook_RecipeReadDTO>();
            CreateMap<CookBook_RecipeCreateDTO, RecipesTb>();
            CreateMap<CookBook_RecipeUpdateDTO, RecipesTb>();
        }
    }
}