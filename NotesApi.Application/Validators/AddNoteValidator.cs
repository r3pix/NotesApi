using FluentValidation;
using NotesApi.Application.DTOs.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Validators
{
    public class AddNoteValidator : AbstractValidator<CreateNoteDto>
    {
        public AddNoteValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }
}
