using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Application.CQRS.User;
using NotesApi.Application.DTOs.User;
using NotesApi.Infrastacture.Models;
using System.Net;

namespace NotesApi.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto model) =>
            await ExecuteCommand(async () => await _mediator.Send(new RegisterUserCommand(model)));

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Response<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> LoginUser([FromBody] LoginUserDto model) =>
            await ExecuteQuery(async () => await _mediator.Send(new LoginUserQuery(model)));

    }
}
