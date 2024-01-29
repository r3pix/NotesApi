using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NotesApi.Application.DTOs.Note;
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
    public class GetNotesQueryHandler : IRequestHandler<GetNotesQuery, Response<List<NoteDto>>>
    {
        private readonly NotesApiDbContext _notesApiDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetNotesQueryHandler(NotesApiDbContext notesApiDbContext, ICurrentUserService currentUserService, IMapper mapper)
        {
            _notesApiDbContext = notesApiDbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Response<List<NoteDto>>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
        {
            var query = _notesApiDbContext.Notes
               .Include(x => x.Tags)
               .Where(x => x.UserId == _currentUserService.Id.Value);

            if (request.Types.Any())
                query = query.Where(x => x.Tags.Any(y => request.Types.Contains(y.Type)));

            var notes = await query.ToListAsync(cancellationToken);

            return new Response<List<NoteDto>>(_mapper.Map<List<NoteDto>>(notes)); 
        }
    }
}
