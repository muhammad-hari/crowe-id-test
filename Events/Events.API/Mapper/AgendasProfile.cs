using AutoMapper;
using Events.Data.Entities;
using Events.Domain.Models;

namespace Events.API.Mapper
{
    public class AgendasProfile : Profile
    {
        public AgendasProfile()
        {
            CreateMap<AgendasModel, Agenda>();
            CreateMap<Agenda, AgendasModel>();
        }
    }
}
