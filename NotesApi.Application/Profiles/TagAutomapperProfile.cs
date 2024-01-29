using AutoMapper;
using NotesApi.Application.DTOs.Tag;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Profiles
{
    public class TagAutomapperProfile : Profile
    {
        public TagAutomapperProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}
