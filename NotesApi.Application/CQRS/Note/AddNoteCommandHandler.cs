using AutoMapper;
using MediatR;
using NotesApi.Infrastacture.Builders;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class AddNoteCommandHandler : IRequestHandler<AddNoteCommand>
    {
        private readonly NotesApiDbContext _notesApiDbContext;
        private readonly ITagBuilder _tagBuilder;

        public AddNoteCommandHandler(NotesApiDbContext notesApiDbContext, ITagBuilder tagBuilder)
        {
            _notesApiDbContext = notesApiDbContext;
            _tagBuilder = tagBuilder;
        }

        public async Task Handle(AddNoteCommand request, CancellationToken cancellationToken)
        {
            var result = await _tagBuilder.BuildNoteWithTags(request);
            await _notesApiDbContext.Notes.AddAsync(result, cancellationToken);
            await _notesApiDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
