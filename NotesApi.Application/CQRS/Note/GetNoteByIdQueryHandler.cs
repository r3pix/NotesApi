using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotesApi.Application.DTOs.Note;
using NotesApi.Domain.Consts;
using NotesApi.Infrastacture.Exceptions;
using NotesApi.Infrastacture.Interfaces;
using NotesApi.Infrastacture.Models;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.Note
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery, Response<NoteDto>>
    {
        private readonly NotesApiDbContext _notesApiDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetNoteByIdQueryHandler(NotesApiDbContext notesApiDbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _notesApiDbContext = notesApiDbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Response<NoteDto>> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
        {
            var note = await _notesApiDbContext.Notes
                .Include(x => x.Tags)
                .Where(x => x.UserId == _currentUserService.Id.Value)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (note == null)
                throw new NotFoundException(ExceptionDescriptions.NoteNotFoundMessage);

            return new Response<NoteDto>(_mapper.Map<NoteDto>(note));
        }
    }
}
