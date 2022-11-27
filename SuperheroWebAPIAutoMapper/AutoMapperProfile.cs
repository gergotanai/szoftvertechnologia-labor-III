using AutoMapper;
using SuperheroWebAPIAutoMapper.Dtos;
using SuperheroWebAPIAutoMapper.Models;

namespace SuperheroWebAPIAutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Superhero, SuperheroDto>();
            CreateMap<SuperheroDto, Superhero>();
        }
    }
}
