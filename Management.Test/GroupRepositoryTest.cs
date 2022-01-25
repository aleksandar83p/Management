using Management.Data;
using Management.Data.DTO;
using Management.Data.Models;
using Management.Services.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Management.Test
{
    public class GroupRepositoryTest
    {
        private static DbContextOptions<AppDbContext> _DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "GroupRepositoryTest")
            .Options;

        AppDbContext _Context;
        GroupRepository _GroupRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _Context = new AppDbContext(_DbContextOptions);
            _Context.Database.EnsureCreated();

            SeedDatabase();

            _GroupRepository = new GroupRepository(_Context);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _Context.Database.EnsureDeleted();
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

        [Test, Order(1)]
        public async Task GetGroupsAsync_Test()
        {
            var result = await _GroupRepository.GetGroupsAsync();

            Assert.That(result.Count, Is.EqualTo(5));
            Assert.That(result[0].Name, Is.EqualTo("CEO"));
        }

        [Test, Order(2)]
        public async Task GetGroupByIdAsync_WithResponse_Test()
        {
            var result = await _GroupRepository.GetGroupByIdAsync(2);
            Assert.That(result.Id, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Vice President"));
        }

        [Test, Order(3)]
        public async Task GetGroupByIdAsync_WithoutResponse_Test()
        {
            var result = await _GroupRepository.GetGroupByIdAsync(99);
            
            Assert.That(result, Is.Null);
        }

        [Test, Order(4)]
        public async Task AddGroupAsync_Test()
        {
            var newGroup = new GroupDTO()
            {
                Name = "Employee"
            };

            await _GroupRepository.AddGroupAsync(newGroup);

            var result = await _GroupRepository.GetGroupsAsync();

            Assert.That(result.Count, Is.EqualTo(6));
            Assert.That(result[5].Name, Is.EqualTo("Employee"));
        }

        [Test, Order(5)]
        public async Task UpdateGroupByIdAsync_Test()
        {
            var newGroup = new Group()
            {
                Id = 1,
                Name = "Aliens"
            };

            await _GroupRepository.UpdateGroupByIdAsync(1, newGroup);
            var result = await _GroupRepository.GetGroupsAsync();

            Assert.That(result[0].Name, Is.EqualTo("Aliens"));
        }

        [Test, Order(6)]
        public async Task DeleteGroupAsync_Test()
        {
            int groupId = 5;

            var groupsBefore = await _GroupRepository.GetGroupByIdAsync(groupId);
            Assert.That(groupsBefore, Is.Not.Null);
            Assert.That(groupsBefore.Name, Is.EqualTo("Associate"));

            await _GroupRepository.DeleteGroupAsync(groupId);

            var groupsAfter = await _GroupRepository.GetGroupByIdAsync(groupId);
            Assert.That(groupsAfter, Is.Null);           
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _Context.Database.EnsureDeleted();
        }
    }
}