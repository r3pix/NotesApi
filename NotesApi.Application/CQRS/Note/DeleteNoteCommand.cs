using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class DeleteNoteCommand : IRequest
    {
        public DeleteNoteCommand(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
