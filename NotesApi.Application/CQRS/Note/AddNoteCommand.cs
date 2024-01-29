using MediatR;
using NotesApi.Application.DTOs.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class AddNoteCommand : IRequest
    {
        public AddNoteCommand(CreateNoteDto model)
        {
            Content = model.Content;
        }

        public string Content { get; set; }
    }
}
