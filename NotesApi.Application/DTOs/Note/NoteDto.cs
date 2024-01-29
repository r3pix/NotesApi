using NotesApi.Application.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.DTOs.Note
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}
