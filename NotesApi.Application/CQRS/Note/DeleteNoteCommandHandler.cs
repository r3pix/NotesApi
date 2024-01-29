using MediatR;
using Microsoft.EntityFrameworkCore;
using NotesApi.Domain.Consts;
using NotesApi.Infrastacture.Exceptions;
using NotesApi.Infrastacture.Interfaces;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand>
    {
        private readonly NotesApiDbContext _notesApiDbContext;
        private readonly ICurrentUserService _currentUserService;

        public DeleteNoteCommandHandler(NotesApiDbContext notesApiDbContext, ICurrentUserService currentUserService)
        {
            _notesApiDbContext = notesApiDbContext;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
            var noteToRemove = await _notesApiDbContext.Notes
                .Include(x => x.Tags)
                .Where(x => x.UserId == _currentUserService.Id.Value)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (noteToRemove == null)
                throw new NotFoundException(ExceptionDescriptions.NoteNotFoundMessage);

            _notesApiDbContext.Notes.Remove(noteToRemove);
            await _notesApiDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
