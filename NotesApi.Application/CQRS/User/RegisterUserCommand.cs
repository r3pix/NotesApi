using MediatR;
using NotesApi.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.User
{
    public class RegisterUserCommand : IRequest
    {
        public RegisterUserCommand(RegisterUserDto model)
        {
            Email = model.Email;
            Password = model.Password;
        }

        public string Email { get; init; }

        public string Password { get; init; }
    }
}
