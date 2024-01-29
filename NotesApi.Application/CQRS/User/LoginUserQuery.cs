using MediatR;
using NotesApi.Application.DTOs.User;
using NotesApi.Infrastacture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.User
{
    public class LoginUserQuery : IRequest<Response<string>>
    {
        public LoginUserQuery(LoginUserDto model)
        {
            Email = model.Email;
            Password = model.Password;
        }

        public string Email { get; init; }
        public string Password { get; init; }
    }
}
