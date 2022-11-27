using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SuperheroWebAPIAutoMapper.Dtos;
using SuperheroWebAPIAutoMapper.Models;
using SuperheroWebAPIAutoMapper.Repositories;

namespace SuperheroWebAPIAutoMapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperheroController : ControllerBase
    {

        private readonly IMapper _mapper;

        public SuperheroController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<Superhero>> GetHeroes()
        {
            return Ok(_mapper.Map<List<SuperheroDto>>(SuperheroRepository.Superheros));
        }

        [HttpPost]
        public ActionResult<List<Superhero>> AddHero(SuperheroDto superheroDto)
        {
            var superhero = _mapper.Map<Superhero>(superheroDto);
            SuperheroRepository.Superheros.Add(superhero);
            return Ok(_mapper.Map<List<SuperheroDto>>(SuperheroRepository.Superheros));
        }
    }
}
