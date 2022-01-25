using Management.Controllers;
using Management.Data;
using Management.Data.DTO;
using Management.Data.Models;
using Management.Services.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management.Test
{
    class UsersControllerTest
    {
        private static DbContextOptions<AppDbContext> _DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "GroupDbControllerTest")
           .Options;

        AppDbContext _Context;
        UserRepository _UserRepository;
        UsersController _UserController;

        [OneTimeSetUp]
        public void Setup()
        {
            _Context = new AppDbContext(_DbContextOptions);
            _Context.Database.EnsureCreated();

            SeedDatabase();

            _UserRepository = new UserRepository(_Context);
            _UserController = new UsersController(_UserRepository);

        }

        [Test, Order(1)]
        public async Task HTTPGET_GetAllUsersAsync_ReturnOK_Test()
        {
            IActionResult actionResult = await _UserController.GetAllUsersAsync();

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var actionResultData = (actionResult as OkObjectResult).Value as List<User>;

            Assert.That(actionResultData.FirstOrDefault().Name, Is.EqualTo("Leonardo"));
            Assert.That(actionResultData.FirstOrDefault().Id, Is.EqualTo(1));
            Assert.That(actionResultData.Count(), Is.EqualTo(3));
        }

        [Test, Order(2)]
        public async Task HTTPGET_GetUserByIdAsync_ReturnsOK_Test()
        {
            int userId = 1;
            IActionResult actionResult = await _UserController.GetUserByIdAsync(userId);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var userData = (actionResult as OkObjectResult).Value as User;
            Assert.That(userData.Id, Is.EqualTo(1));
            Assert.That(userData.Name, Is.EqualTo("Leonardo"));
        }

        [Test, Order(3)]
        public async Task HTTPGET_GetUserByIdAsync_ReturnsNotFound_Test()
        {
            int userId = 99;
            IActionResult actionResult = await _UserController.GetUserByIdAsync(userId);

            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test, Order(4)]
        public async Task HTTPPOST_CreateUserAsync_ReturnsCreated_Test()
        {
            var newUser = new UserDTO()
            {
                Name = "Mikelandjelo",
                Email = "mike@mike.com",
                Password = "sadjkbsajdasd",
                GroupId = 1
            };

            IActionResult actionResult = await _UserController.CreateUserAsync(newUser);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());

        }

        [Test, Order(5)]
        public async Task HTTPPUT_UpdateUserAsync_ReturnsNotFound_Test()
        {
            int userId = 99;
            var newUser = new UserDTOUpdate()
            {
                Id= 99,
                Name = "Mikelandjelo",
                Email = "mike@mike.com",
                Password = "sadjkbsajdasd",
                GroupId = 1
            };

            IActionResult actionResult = await _UserController.UpdateUserAsync(userId, newUser);
            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test, Order(6)]
        public async Task HTTPPUT_UpdateUserAsync_ReturnsBadRequest_Test()
        {
            int userId = 50;
            var newUser = new UserDTOUpdate()
            {
                Id = 1,
                Name = "Mikelandjelo",
                Email = "mike@mike.com",
                Password = "sadjkbsajdasd",
                GroupId = 1
            };

            IActionResult actionResult = await _UserController.UpdateUserAsync(userId, newUser);            
            Assert.That(userId, Is.Not.EqualTo(newUser.Id));
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(7)]
        public async Task HTTPPUT_UpdateUserAsync_ReturnsOk_Test()
        {
            int userId = 1;
            var newUser = new UserDTOUpdate()
            {
                Id = 1,
                Name = "Mikelandjelo",
                Email = "mike@mike.com",
                Password = "sadjkbsajdasd",
                GroupId = 1
            };

            IActionResult actionResult = await _UserController.UpdateUserAsync(userId, newUser);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
        }

        [Test, Order(8)]
        public async Task HTTPDELETE_DeleteUserAsync_ReturnsOK_Test()
        {
            int userId = 2;

            IActionResult actionResult = await _UserController.DeleteUserAsync(userId);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
        }

        [Test, Order(9)]
        public async Task HTTPDELETE_DeleteGroupAsync_ReturnsBadRequest_Test()
        {
            int userId = 50;

            IActionResult actionResult = await _UserController.DeleteUserAsync(userId);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        private void SeedDatabase()
        {
            var groups = new List<Group>
            {
                new Group()
                {
                    Id = 1,
                    Name = "CEO"
                },
                new Group()
                {
                    Id = 2,
                    Name = "Vice President"
                },
                new Group()
                {
                    Id = 3,
                    Name = "Director"
                },
                new Group()
                {
                    Id = 4,
                    Name = "Manager"
                },
                new Group()
                {
                    Id = 5,
                    Name = "Associate"
                }

            };

            var users = new List<User>()
            {
                new User
                {
                    Id = 1,
                    Name = "Leonardo",
                    Password = "LeoBlue",
                    Email = "Leo@leo.com",
                    GroupId = groups[0].Id
                },
                new User
                {
                    Id = 2,
                    Name = "Rafaelo",
                    Password = "RafRed",
                    Email = "Raf@raf.com",
                    GroupId = groups[1].Id
                },
                 new User
                {
                    Id = 3,
                    Name = "Donatelo",
                    Password = "Doni",
                    Email = "Doni@Doni.com",
                    GroupId = groups[2].Id
                },
            };

            _Context.Groups.AddRange(groups);
            _Context.Users.AddRange(users);
            _Context.SaveChanges();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _Context.Database.EnsureDeleted();
        }
    }
}
