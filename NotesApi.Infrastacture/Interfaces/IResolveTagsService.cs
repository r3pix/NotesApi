using NotesApi.Domain.ValueObject;

namespace NotesApi.Infrastacture.Interfaces
{
    public interface IResolveTagsService
    {
        IEnumerable<TagTypes> ResolveTags(string content);
    }
}