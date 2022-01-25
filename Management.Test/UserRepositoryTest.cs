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
    public class UserRepositoryTest
    {
        private static DbContextOptions<AppDbContext> _DbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "UserRepositoryTest")
            .Options;

        AppDbContext _Context;
        UserRepository _UserRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _Context = new AppDbContext(_DbContextOptions);
            _Context.Database.EnsureCreated();

            SeedDatabase();

            _UserRepository = new UserRepository(_Context);
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

        [Test, Order(1)]
        public async Task GetUsersAsync_Test()
        {
            var result = await _UserRepository.GetUsersAsync();

            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Name, Is.EqualTo("Leonardo"));
            Assert.That(result[2].Email, Is.EqualTo("Doni@Doni.com"));
        }

        [Test, Order(2)]
        public async Task GetUserByIdAsync_WithResponse_Test()
        {
            var result = await _UserRepository.GetUserByIdAsync(2);
            Assert.That(result.Id, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("Rafaelo"));
        }

        [Test, Order(3)]
        public async Task GetUserByIdAsync_WithoutResponse_Test()
        {
            var result = await _UserRepository.GetUserByIdAsync(99);

            Assert.That(result, Is.Null);
        }

        [Test, Order(4)]
        public async Task AddUserAsync_Test()
        {
            var newGroup = new UserDTO()
            {
                Name = "Mikelandjelo",
                Email = "mike@mike.com",
                Password = "sadsadada",
                GroupId = 3
            };

            await _UserRepository.AddUserAsync(newGroup);

            var result = await _UserRepository.GetUsersAsync();

            Assert.That(result.Count, Is.EqualTo(4));
            Assert.That(result[3].Name, Is.EqualTo("Mikelandjelo"));
        }

        [Test, Order(5)]
        public async Task UpdateUserByIdAsync_Test()
        {
            var newUser = new UserDTOUpdate()
            {
                Id = 1,
                Name = "Splinter",
                Email = "splinet@pacov.com",
                Password = "sadmaio90q23",
                GroupId = 1
            };

            await _UserRepository.UpdateUserByIdAsync(1, newUser);
            var result = await _UserRepository.GetUsersAsync();

            Assert.That(result[0].Name, Is.EqualTo("Splinter"));
        }

        [Test, Order(6)]
        public async Task DeleteUserAsync_Test()
        {
            int userId = 2;

            var usersBefore = await _UserRepository.GetUserByIdAsync(userId);
            Assert.That(usersBefore, Is.Not.Null);
            Assert.That(usersBefore.Name, Is.EqualTo("Rafaelo"));

            await _UserRepository.DeleteUserAsync(userId);

            var usersAfter = await _UserRepository.GetUserByIdAsync(userId);
            Assert.That(usersAfter, Is.Null);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            _Context.Database.EnsureDeleted();
        }
    }
}
