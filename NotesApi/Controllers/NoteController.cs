using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Application.CQRS.Note;
using NotesApi.Application.DTOs.Note;
using NotesApi.Domain.ValueObject;
using NotesApi.Infrastacture.Models;
using System.Net;

namespace NotesApi.Controllers
{
    public class NoteController : BaseController
    {
        public NoteController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        public async Task<ActionResult> Create([FromBody] CreateNoteDto model) =>
            await ExecuteCommand(async () => await _mediator.Send(new AddNoteCommand(model)));

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Modify([FromBody] UpdateNoteDto model, [FromRoute] int id) =>
            await ExecuteCommand(async () => await _mediator.Send(new UpdateNoteCommand(model, id)));

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> Delete([FromRoute] int id) =>
            await ExecuteCommand(async () => await _mediator.Send(new DeleteNoteCommand(id)));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Response<NoteDto>),(int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetById([FromRoute] int id) =>
            await ExecuteQuery(async () => await _mediator.Send(new GetNoteByIdQuery(id)));

        [HttpGet("list")]
        [ProducesResponseType(typeof(Response<List<NoteDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetById([FromQuery] TagTypes[] types) =>
          await ExecuteQuery(async () => await _mediator.Send(new GetNotesQuery(types)));
    }
}
