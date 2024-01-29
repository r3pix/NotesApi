using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using NotesApi.Application.Profiles;
using NotesApi.Domain.Entities;
using NotesApi.Infrastacture.Interfaces;
using NotesApi.Persistence.Seeders;
using NotesApi.Persistence;
using NotesApi.Application.DTOs.Note;
using NotesApi.Application.CQRS.Note;
using NotesApi.Infrastacture.Builders;
using NotesApi.Infrastacture.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotesApi.Application.CQRS.User;
using NotesApi.Infrastacture.Exceptions;
using NotesApi.Domain.ValueObject;

namespace NotesApi.UnitTests
{
    public class NotesTest
    {
        [Fact]
        public async Task WhenAddingNote_ItShouldAddEntityAndAssignTags()
        {
            //arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_Add").Options;
            var profile = new NoteAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
            var _config = builder.Build();
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Email).Returns("system");
            currentUser.Setup(x => x.Id).Returns(1);
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            var passwordHasher = new PasswordHasher<User>();
            var resolveTagsService = new ResolveTagsService();
            var tagBuilder = new Infrastacture.Builders.TagBuilder(dbContext, resolveTagsService, mapper, currentUser.Object);
            //act
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var dto = new CreateNoteDto
            {
                Content = "Ala ma kota"
            };

            var commandHandler = new AddNoteCommandHandler(dbContext, tagBuilder);
            await commandHandler.Handle(new AddNoteCommand(dto), CancellationToken.None);
            //assert

            Assert.True(await dbContext.Notes.AnyAsync());
            Assert.True((await dbContext.Notes.Include(x => x.Tags).FirstOrDefaultAsync()).Tags.Any());
        }

        [Fact]
        public async Task WhenUpdatingNote_ItShouldUpdateContentAndTags()
        {

            //arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_Update").Options;
            var profile = new NoteAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
            var _config = builder.Build();
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Email).Returns("system");
            currentUser.Setup(x => x.Id).Returns(1);
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            var passwordHasher = new PasswordHasher<User>();
            var resolveTagsService = new ResolveTagsService();
            var tagBuilder = new Infrastacture.Builders.TagBuilder(dbContext, resolveTagsService, mapper, currentUser.Object);
            //act
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var dto = new CreateNoteDto
            {
                Content = "Ala ma kota"
            };

            var commandHandler = new AddNoteCommandHandler(dbContext, tagBuilder);
            await commandHandler.Handle(new AddNoteCommand(dto), CancellationToken.None);

            var note = await dbContext.Notes.FirstOrDefaultAsync();

            var updateDto = new UpdateNoteDto()
            {
                Content = "123 123 123"
            };
        
            var updateCommandHandler = new UpdateNoteCommandHandler(dbContext, mapper, currentUser.Object, tagBuilder);
            await updateCommandHandler.Handle(new UpdateNoteCommand(updateDto, note.Id), CancellationToken.None);

            //assert
            Assert.True(await dbContext.Notes.AnyAsync());
            Assert.True((await dbContext.Notes.Include(x => x.Tags).FirstOrDefaultAsync()).Tags.Any(x => x.Type == Domain.ValueObject.TagTypes.Phone));
            Assert.True((await dbContext.Notes.FirstOrDefaultAsync()).Content == updateDto.Content);
        }

        [Fact]
        public async Task WhenUpdatingNotExistingNote_ItShouldUpdateContentAndTags()
        {

            //arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_Update").Options;
            var profile = new NoteAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var builder = new ConfigurationBuilder().AddJsonFile($"testSettings.json", optional: false);
            var _config = builder.Build();
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Email).Returns("system");
            currentUser.Setup(x => x.Id).Returns(1);
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            var passwordHasher = new PasswordHasher<User>();
            var resolveTagsService = new ResolveTagsService();
            var tagBuilder = new Infrastacture.Builders.TagBuilder(dbContext, resolveTagsService, mapper, currentUser.Object);
            //act
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var dto = new UpdateNoteDto()
            {
                Content = "123 123 123"
            };

            var commandHandler = new UpdateNoteCommandHandler(dbContext, mapper, currentUser.Object, tagBuilder);

            //assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await commandHandler.Handle(new UpdateNoteCommand(dto, 213), CancellationToken.None));
        }

        [Fact]
        public async Task WhenGettingNotes_ShouldReturnCorrectNotes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_GetCorrectNotes").Options;
            var tagProfile = new TagAutomapperProfile();
            var profile = new NoteAutomapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(profile);
                cfg.AddProfile(tagProfile);
            });
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(1);
            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var notes = new[]
            {
                new Note { Content = "Note 1", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Email, Name = "Email" } } },
                new Note { Content = "Note 2", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Phone, Name = "Phone" } } },
                new Note { Content = "Note 3", UserId = 2, Tags = new List<Tag> { new Tag { Type = TagTypes.None, Name = "None" } } }
            };

            dbContext.Notes.AddRange(notes);
            await dbContext.SaveChangesAsync();
            
            var query = new GetNotesQuery(new[] { TagTypes.Email });
            var handler = new GetNotesQueryHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Result.Count);
            Assert.Equal("Note 1", result.Result.First().Content);
        }

        [Fact]
        public async Task WhenGetingNotesEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_GetEmptyList").Options;
            var profile = new NoteAutomapperProfile();
            var tagProfile = new TagAutomapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(profile);
                cfg.AddProfile(tagProfile);
            });
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(2);

            var dbContext = new NotesApiDbContext(options, currentUser.Object);

            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var notes = new[]
            {
                new Note { Content = "Note 1", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Phone, Name = "Phone" } } },
                new Note { Content = "Note 2", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Email, Name = "Email" } } }
            };

            dbContext.Notes.AddRange(notes);
            await dbContext.SaveChangesAsync();


            var query = new GetNotesQuery(new[] { Domain.ValueObject.TagTypes.Email });
            var handler = new GetNotesQueryHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Result); 
        }

        [Fact]
        public async Task WhenGetingNotesWithNoTagSpecified_ShouldReturnAll()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_GetAllNotes").Options;
            var profile = new NoteAutomapperProfile();
            var tagProfile = new TagAutomapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(profile);
                cfg.AddProfile(tagProfile);
            });
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(1);
            var dbContext = new NotesApiDbContext(options, currentUser.Object);

            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var notes = new[]
             {
                new Note { Content = "Note 1", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Email, Name = "Email" } } },
                new Note { Content = "Note 2", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Phone, Name = "Phone" } } },
                new Note { Content = "Note 3", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.None, Name = "None" } } }
            };

            dbContext.Notes.AddRange(notes);
            await dbContext.SaveChangesAsync();
            

            var query = new GetNotesQuery(new TagTypes[] { });
            var handler = new GetNotesQueryHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Result.Count); 
        }


        [Fact]
        public async Task WhenGettingById_ShouldCorrectNote()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_GetById").Options;
            var profile = new NoteAutomapperProfile();
            var tagProfile = new TagAutomapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(profile);
                cfg.AddProfile(tagProfile);
            });
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(1);

            var dbContext = new NotesApiDbContext(options, currentUser.Object);
                
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var notes = new[]
            {
                new Note { Content = "Note 1", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Email, Name = "Email" } } },
                new Note { Content = "Note 2", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Phone, Name = "Phone" } } },
                new Note { Content = "Note 3", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.None, Name = "None" } } }
            };

            dbContext.Notes.AddRange(notes);
            await dbContext.SaveChangesAsync();
              
            var query = new GetNoteByIdQuery(2);
            var handler = new GetNoteByIdQueryHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object, mapper);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Note 2", result.Result.Content);
        }

        [Fact]
        public async Task WhenGettingIncorrectNote_ShouldThrowException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_Throw").Options;
            var profile = new NoteAutomapperProfile();
            var tagProfile = new TagAutomapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(profile);
                cfg.AddProfile(tagProfile);
            });
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(1);

            var dbContext = new NotesApiDbContext(options, currentUser.Object);
                
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);
                
            var query = new GetNoteByIdQuery(1);
            var handler = new GetNoteByIdQueryHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object, mapper);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task WhenDeletingNote_ItSouldDeleteNote()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_Delete").Options;
            var profile = new NoteAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(1);

            var dbContext = new NotesApiDbContext(options, currentUser.Object);

            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var notes = new[]
             {
                new Note { Content = "Note 1", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Email, Name = "Email" } } },
                new Note { Content = "Note 2", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.Phone, Name = "Phone" } } },
                new Note { Content = "Note 3", UserId = 1, Tags = new List<Tag> { new Tag { Type = TagTypes.None, Name = "None" } } }
            };

            dbContext.Notes.AddRange(notes);
            await dbContext.SaveChangesAsync();
            
            var command = new DeleteNoteCommand(2);
            var handler = new DeleteNoteCommandHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(await dbContext.Notes.FirstOrDefaultAsync(x=>x.Id == 2)); 
        }

        [Fact]
        public async Task WhenDeletingIncorrectNote_ItSouldThrowError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<NotesApiDbContext>().UseInMemoryDatabase(databaseName: "Test_Note_Delete_Throw").Options;
            var profile = new NoteAutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            var mapper = new Mapper(configuration);
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.Id).Returns(1);

            var dbContext = new NotesApiDbContext(options, currentUser.Object);
            
            await UserSeeder.SeedRoles(dbContext);
            await UserSeeder.Seed(dbContext);
            await TagSeeder.Seed(dbContext);

            var command = new DeleteNoteCommand(1);
            var handler = new DeleteNoteCommandHandler(new NotesApiDbContext(options, currentUser.Object), currentUser.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(command, CancellationToken.None));
        }
    }
}
