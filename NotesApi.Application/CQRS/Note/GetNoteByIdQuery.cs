using MediatR;
using NotesApi.Application.DTOs.Note;
using NotesApi.Infrastacture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class GetNoteByIdQuery : IRequest<Response<NoteDto>>
    {
        public GetNoteByIdQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
