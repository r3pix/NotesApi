using NotesApi.Application.CQRS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.IntegrationTest
{
    public class UserIntegrationTests
    {
        [Fact]
        public async Task WhenRegistratingUser_ItShouldAddUser()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("User_Integration_1");
            var request = new RegisterUserCommand(new Application.DTOs.User.RegisterUserDto { Email = "ala@com.pl", Password = "Pass123$" });
            var client = application.CreateClient();
            //Act
            var response = await client.PostAsJsonAsync("/api/user/register", request);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WhenRegistratingUserWithBadPassword_ItShouldReturnError()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("User_Integration_2");
            var request = new RegisterUserCommand(new Application.DTOs.User.RegisterUserDto { Email = "ala@com.pl", Password = "" });
            var client = application.CreateClient();
            //Act
            var response = await client.PostAsJsonAsync("/api/user/register", request);

            //Assert
            Assert.True(!response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task WhenLogginWithGoodUser_ItShouldReturnToken()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("User_Integration_3");
            var request = new LoginUserQuery(new Application.DTOs.User.LoginUserDto() { Email = "user@com.pl", Password = "Pass123$" });
            var client = application.CreateClient();
            //Act
            var response = await client.PostAsJsonAsync("/api/user/login", request);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task WhenLogginWithBadUser_ItShouldReturnToken()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("User_Integration_4");
            var request = new LoginUserQuery(new Application.DTOs.User.LoginUserDto() { Email = "user@com.pl", Password = "" });
            var client = application.CreateClient();
            //Act
            var response = await client.PostAsJsonAsync("/api/user/login", request);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
