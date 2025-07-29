using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Backend.Mappings;
using Backend.Exceptions;
using Xunit;

namespace Backend.Tests
{
    public class StudentServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly StudentRepository _repository;
        private readonly IMapper _mapper;
        private readonly StudentService _service;

        public StudentServiceTests()
        {
            // Configurar base de datos en memoria para tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new StudentRepository(_context);

            // Configurar AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _service = new StudentService(_repository, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateStudent()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Name = "John Doe",
                Email = "john@example.com",
                Career = "Computer Science"
            };

            // Act
            var result = await _service.CreateAsync(studentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal("john@example.com", result.Email);
            Assert.Equal("Computer Science", result.Career);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnStudent()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                Career = "Engineering"
            };
            var created = await _service.CreateAsync(studentDto);

            // Act
            var result = await _service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(999));
        }
    }
}