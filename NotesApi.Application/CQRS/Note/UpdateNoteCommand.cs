using MediatR;
using NotesApi.Application.DTOs.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class UpdateNoteCommand : IRequest
    {
        public UpdateNoteCommand(UpdateNoteDto model, int id)
        {
            Content = model.Content;
            Id = id;
        }
        public int Id { get; init; }
        public string Content { get; init; }
    }
}
