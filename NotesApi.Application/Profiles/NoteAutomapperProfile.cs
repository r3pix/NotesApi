using AutoMapper;
using NotesApi.Application.CQRS.Note;
using NotesApi.Application.DTOs.Note;
using NotesApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Profiles
{
    public class NoteAutomapperProfile : Profile
    {
        public NoteAutomapperProfile()
        {
            CreateMap<AddNoteCommand, Note>().ReverseMap();
            CreateMap<UpdateNoteCommand, Note>().ReverseMap();
            CreateMap<Note, NoteDto>();
        }
    }
}
