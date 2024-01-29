using AutoMapper;
using NotesApi.Application.CQRS.User;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Profiles
{
    public class UserAutomapperProfile : Profile
    {
        public UserAutomapperProfile()
        {
            CreateMap<RegisterUserCommand, User>().ReverseMap();
        }
    }
}
