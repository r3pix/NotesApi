using AutoMapper;
using Castle.Core.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NotesApi.Application.CQRS.User;
using NotesApi.Application.DTOs.User;
using NotesApi.Application.Profiles;
using NotesApi.Domain.Entities;
using NotesApi.Infrastacture.Exceptions;
using NotesApi.Infrastacture.Interfaces;
using NotesApi.Persistence;
using NotesApi.Persistence.Seeders;

namespace NotesApi.UnitTests
{
    public class UserTests
    {
        [Fact]
        public async Task WhenAddingUser_ItShoudAddEntity()
        {
            //arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_User_Add").Options;
            var profile = new UserAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
            var _config = builder.Build();
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Email).Returns("system");
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            var passwordHasher = new PasswordHasher<User>();
            //act
            await UserSeeder.SeedRoles(dbContext);

            var dto = new RegisterUserDto
            {
                Email = "admin@com.pl",
                Password = "Test",
            };

            var commandHandler = new RegisterUserCommandHandler(mapper, passwordHasher, dbContext);
            await commandHandler.Handle(new RegisterUserCommand(dto), CancellationToken.None);
            //assert

            Assert.True(await dbContext.Users.AnyAsync());
        }

        [Fact]
        public async Task WhenLogging_ItShouldReturnToken()
        {
            //arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_User_Login").Options;
            var profile = new UserAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
            var _config = builder.Build();
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Email).Returns("system");
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            var passwordHasher = new PasswordHasher<User>();
                
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);

            //act
            var dto = new LoginUserDto()
            {
                Email = "user@com.pl",
                Password = "Pass123$"
            };

            var commandHandler = new LoginUserQueryHandler(passwordHasher, _config, dbContext);
            var result = await commandHandler.Handle(new LoginUserQuery(dto), CancellationToken.None);
            //assert

            Assert.NotNull(result);
        }

        [Fact]
        public async Task WhenLoggingWithNonExisting_ItShouldthrowError()
        {
            //arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_User_Throw").Options;
            var profile = new UserAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
            var _config = builder.Build();
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Email).Returns("system");
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            var passwordHasher = new PasswordHasher<User>();
            //act

            var dto = new LoginUserDto()
            {
                Email = "admiaan@com.pl",
                Password = "Pass123$"
            };

            var commandHandler = new LoginUserQueryHandler(passwordHasher, _config, dbContext);
            //assert

            await Assert.ThrowsAsync<NotFoundException>(async () => await commandHandler.Handle(new LoginUserQuery(dto), CancellationToken.None));
        }
        
    }
}