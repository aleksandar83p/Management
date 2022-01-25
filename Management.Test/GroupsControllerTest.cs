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
    public class GroupsControllerTest
    {
        private static DbContextOptions<AppDbContext> _DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseInMemoryDatabase(databaseName: "GroupDbControllerTest")
           .Options;

        AppDbContext _Context;
        GroupRepository _GroupRepository;
        GroupsController _GroupsController;

        [OneTimeSetUp]
        public void Setup()
        {
            _Context = new AppDbContext(_DbContextOptions);
            _Context.Database.EnsureCreated();

            SeedDatabase();

            _GroupRepository = new GroupRepository(_Context);
            _GroupsController = new GroupsController(_GroupRepository);

        }

        [Test, Order(1)]
        public async Task HTTPGET_GetAllGroupsAsync_ReturnOK_Test()
        {
            IActionResult actionResult = await _GroupsController.GetAllGroupsAsync();

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var actionResultData = (actionResult as OkObjectResult).Value as List<Group>;

            Assert.That(actionResultData.FirstOrDefault().Name, Is.EqualTo("CEO"));
            Assert.That(actionResultData.FirstOrDefault().Id, Is.EqualTo(1));
            Assert.That(actionResultData.Count(), Is.EqualTo(5));
        }

        [Test, Order(2)]
        public async Task HTTPGET_GetGroupByIdAsync_ReturnsOK_Test()
        {
            int groupId = 1;
            IActionResult actionResult = await _GroupsController.GetGroupByIdAsync(groupId);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());

            var groupData = (actionResult as OkObjectResult).Value as Group;
            Assert.That(groupData.Id, Is.EqualTo(1));
            Assert.That(groupData.Name, Is.EqualTo("CEO"));

        }

        [Test, Order(3)]
        public async Task HTTPGET_GetGroupByIdAsync_ReturnsNotFound_Test()
        {
            int groupId = 99;
            IActionResult actionResult = await _GroupsController.GetGroupByIdAsync(groupId);

            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test, Order(4)]
        public async Task HTTPPOST_CreateGroupAsync_ReturnsCreated_Test()
        {
            var newGroup = new GroupDTO()
            {
                Name = "Employee"
            };

            IActionResult actionResult = await _GroupsController.CreateGroupAsync(newGroup);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());

        }

        [Test, Order(5)]
        public async Task HTTPPUT_UpdateGroupAsync_ReturnsNotFound_Test()
        {
            int groupId = 99;
            var newGroup = new Group()
            {
                Id = 99,
                Name = "BlaBLA"
            };

            IActionResult actionResult = await _GroupsController.UpdateGroupAsync(groupId, newGroup);
            Assert.That(actionResult, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test, Order(6)]
        public async Task HTTPPUT_UpdateGroupAsync_ReturnsBadRequest_Test()
        {
            int groupId = 50;
            var newGroup = new Group()
            {
                Id = 99,
                Name = "BlaBLA"
            };

            IActionResult actionResult = await _GroupsController.UpdateGroupAsync(groupId, newGroup);
            Assert.That(groupId, Is.Not.EqualTo(newGroup.Id));
            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test, Order(7)]
        public async Task HTTPPUT_UpdateGroupAsync_ReturnsOk_Test()
        {
            int groupId = 1;
            var newGroup = new Group()
            {
                Id = 1,
                Name = "BlaBLA"
            };

            IActionResult actionResult = await _GroupsController.UpdateGroupAsync(groupId, newGroup);
            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());         
        }

        [Test, Order(8)]
        public async Task HTTPDELETE_DeleteGroupAsync_ReturnsOK_Test()
        {
            int groupId = 2;

            IActionResult actionResult = await _GroupsController.DeleteGroupAsync(groupId);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>());
        }

        [Test, Order(9)]
        public async Task HTTPDELETE_DeleteGroupAsync_ReturnsBadRequest_Test()
        {
            int groupId = 50;

            IActionResult actionResult = await _GroupsController.DeleteGroupAsync(groupId);

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

            _Context.Groups.AddRange(groups);
            _Context.SaveChanges();
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _Context.Database.EnsureDeleted();
        }
    }
}
