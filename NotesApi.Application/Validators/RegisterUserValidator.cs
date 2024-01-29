using FluentValidation;
using NotesApi.Application.CQRS.User;
using NotesApi.Application.DTOs.User;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserValidator(NotesApiDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().Custom((value, context) =>
            {
                if (dbContext.Users.Any(x => x.Email == value))
                    context.AddFailure("This email is already taken");
            });

            RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
        }
    }
}
