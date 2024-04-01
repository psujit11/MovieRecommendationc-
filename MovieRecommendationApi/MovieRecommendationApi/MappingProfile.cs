using AutoMapper;
using MovieRecommendationApi.Dtos;
using MovieRecommendationApi.Models;

namespace MovieRecommendationApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, ReadMovieDto>().ReverseMap();
            CreateMap<CreateMovieDto, Movie>().ReverseMap();
            CreateMap<UpdateMovieDto, Movie>().ReverseMap();
        }
    }

}
