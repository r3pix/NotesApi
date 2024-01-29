using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotesApi.Domain.Consts;
using NotesApi.Infrastacture.Builders;
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
    public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand>
    {
        private readonly NotesApiDbContext _notesApiDbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITagBuilder _tagBuilder;

        public UpdateNoteCommandHandler(NotesApiDbContext notesApiDbContext, IMapper mapper, ICurrentUserService currentUserService, ITagBuilder tagBuilder)
        {
            _notesApiDbContext = notesApiDbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _tagBuilder = tagBuilder;
        }

        public async Task Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
        {
            var noteToUpdate = await _notesApiDbContext.Notes
                .Include(x => x.Tags)
                .Where(x => x.UserId == _currentUserService.Id.Value)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (noteToUpdate == null)
                throw new NotFoundException(ExceptionDescriptions.NoteNotFoundMessage);

            _mapper.Map(request, noteToUpdate);
            await _tagBuilder.BuildNoteWithTagsOnUpdate(noteToUpdate);

            await _notesApiDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
