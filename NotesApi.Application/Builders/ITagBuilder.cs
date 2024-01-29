using NotesApi.Application.CQRS.Note;
using NotesApi.Domain.Entities;

namespace NotesApi.Infrastacture.Builders
{
    public interface ITagBuilder
    {
        Task<Note> BuildNoteWithTags(AddNoteCommand command);
        Task BuildNoteWithTagsOnUpdate(Note note);
    }
}