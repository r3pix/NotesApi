using NotesApi.Application.CQRS.Note;
using NotesApi.Application.CQRS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace NotesApi.IntegrationTest
{
    public class NotesIntegrationTests
    {
        private string token => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJ1c2VyQGNvbS5wbCIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzA2NDc3MTc3LCJleHAiOjE3MzgwOTk1NzcsImlhdCI6MTcwNjQ3NzE3NywiaXNzIjoiTm90ZXNBcGkiLCJhdWQiOiJOb3Rlc0FwaSJ9.b3CludJfmfGRIXNYBW2F-SPuKo1GCdA9qbMkAmkKMnE";

        [Fact]
        public async Task WhenSendUnauthorizedRequest_ItShouldThrowError()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("Note_Integration_1");
            var request = new Application.DTOs.Note.CreateNoteDto{ Content = "tak "};
            var client = application.CreateClient();
            //Act
            var response = await client.PostAsJsonAsync("/api/note", request);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task WhenPostRequest_ItShouldAdd()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("Note_Integration_2");
            var request = new Application.DTOs.Note.CreateNoteDto { Content = "tak " };
            var client = application.CreateClient();
            //Act
            var headers = client.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer" ,token);
            var response = await client.PostAsJsonAsync("/api/note", request);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task WhenBadPostRequest_ItShouldThrowError()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("Note_Integration_3");
            var request = new AddNoteCommand(new Application.DTOs.Note.CreateNoteDto { Content = "" });
            var client = application.CreateClient();
            //Act
            var headers = client.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.PostAsJsonAsync("/api/note", request);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task WhenUpdatingWithBadData_ItShouldThrowError()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("Note_Integration_4");
            var request = new Application.DTOs.Note.UpdateNoteDto { Content = "" };
            var client = application.CreateClient();
            //Act
            var headers = client.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.PutAsJsonAsync("/api/note/1", request);

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task WhenGetWithoutId_ItShouldThrowError()
        {
            //Arrange
            var application = new NotesApiApplicationFactory("Note_Integration_5");
            var client = application.CreateClient();
            //Act
            var headers = client.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync("/api/note");

            //Assert
            Assert.False(response.IsSuccessStatusCode);
        }

    }
}
