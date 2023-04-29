using Xunit;
using Moq;
using BookstoreAPI.Controllers;
using BookstoreAPI.Services;
using BookstoreAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BookstoreAPI;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace BookstoreTests
{
    public class AuthorControllerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly string _jwtToken;

        public AuthorControllerTests(TestFixture fixture)
        {
            _fixture = fixture;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
            
            var testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseEnvironment("Test")
                .UseConfiguration(_configuration)
                .UseStartup<Startup>());

            _client = testServer.CreateClient();
        }

        [Fact]
        public async Task CreateAuthorTest()
        {
            Console.Write("test");
        }
    }
}

