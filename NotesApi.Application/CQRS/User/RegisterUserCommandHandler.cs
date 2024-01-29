using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.User
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;
        private readonly NotesApiDbContext _notesApiDbContext;

        public RegisterUserCommandHandler(IMapper mapper, IPasswordHasher<Domain.Entities.User> passwordHasher, NotesApiDbContext notesApiDbContext)
        {
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _notesApiDbContext = notesApiDbContext;
        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.User>(request);
            var role = await _notesApiDbContext.Roles.FirstOrDefaultAsync(x => x.Name == "User", cancellationToken);
            entity.PasswordHash = _passwordHasher.HashPassword(entity, request.Password);
            entity.RoleId = role.Id;
            await _notesApiDbContext.Users.AddAsync(entity, cancellationToken);

            await _notesApiDbContext.SaveChangesAsyncWithoutUser();
        }
    }
}
