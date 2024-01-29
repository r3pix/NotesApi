using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Infrastacture.Exceptions;
using NotesApi.Infrastacture.Models;
using NotesApi.Persistence;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.Application.CQRS.User
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, Response<string>>
    {
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly NotesApiDbContext _notesApiDbContext;

        public LoginUserQueryHandler(IPasswordHasher<Domain.Entities.User> passwordHasher, IConfiguration configuration, NotesApiDbContext notesApiDbContext)
        {
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _notesApiDbContext = notesApiDbContext;
        }

        public async Task<Response<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _notesApiDbContext.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user is null)
                throw new NotFoundException("User credentials are wrong!");

            var passwordVerifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (passwordVerifyResult == PasswordVerificationResult.Failed)
                throw new Exception("User credentials are wrong!");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTConfiguration:SecretKey").Get<string>());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)

                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JWTConfiguration:Issuer"],
                Audience = _configuration["JWTConfiguration:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Response<string>(tokenHandler.WriteToken(token));
        }
    }
}
