using MediatR;
using NotesApi.Application.DTOs.Note;
using NotesApi.Domain.ValueObject;
using NotesApi.Infrastacture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class GetNotesQuery : IRequest<Response<List<NoteDto>>>
    {
        public GetNotesQuery(TagTypes[] types)
        {
            Types = types;
        }

        public TagTypes[] Types { get; set; }
    }
}
