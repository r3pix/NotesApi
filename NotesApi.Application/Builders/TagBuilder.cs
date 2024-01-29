using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NotesApi.Application.CQRS.Note;
using NotesApi.Domain.Entities;
using NotesApi.Infrastacture.Interfaces;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Infrastacture.Builders
{
    public class TagBuilder : ITagBuilder
    {
        private readonly NotesApiDbContext _notesApiDbContext;
        private readonly IResolveTagsService _resolveTagsService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public TagBuilder(NotesApiDbContext notesApiDbContext, IResolveTagsService resolveTagsService, IMapper mapper, ICurrentUserService currentUserService)
        {
            _notesApiDbContext = notesApiDbContext;
            _resolveTagsService = resolveTagsService;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Note> BuildNoteWithTags(AddNoteCommand command)
        {
            var resultNote = _mapper.Map<Note>(command);
            resultNote.Tags = new List<Tag>();
            resultNote.Tags.AddRange(await ResolveTags(resultNote.Content));
            resultNote.UserId = _currentUserService.Id.Value;
            return resultNote;
        }

        public async Task BuildNoteWithTagsOnUpdate(Note note)
        {
            note.Tags.RemoveAll(x => true);
            note.Tags.AddRange(await ResolveTags(note.Content));
        }

        private async Task<ICollection<Tag>> ResolveTags(string content)
        {
            var tagTypes = _resolveTagsService.ResolveTags(content);

            var tags = await _notesApiDbContext.Tags.Where(x => tagTypes.Contains(x.Type)).ToListAsync();

            return tags;
        } 
    }
}
