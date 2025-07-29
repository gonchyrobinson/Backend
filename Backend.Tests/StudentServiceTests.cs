using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Contexts;
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
                Nombre = "John Doe",
                Email = "john@example.com",
                Carrera = "Computer Science"
            };

            // Act
            var result = await _service.CreateAsync(studentDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Nombre);
            Assert.Equal("john@example.com", result.Email);
            Assert.Equal("Computer Science", result.Carrera);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnStudent()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Nombre = "Jane Doe",
                Email = "jane@example.com",
                Carrera = "Engineering"
            };
            var created = await _service.CreateAsync(studentDto);

            // Act
            var result = await _service.GetByIdAsync(created.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.Nombre);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(999));
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStudents()
        {
            // Arrange
            var student1 = new StudentDto { Nombre = "Student 1", Email = "student1@test.com", Carrera = "CS" };
            var student2 = new StudentDto { Nombre = "Student 2", Email = "student2@test.com", Carrera = "Engineering" };
            
            await _service.CreateAsync(student1);
            await _service.CreateAsync(student2);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            var students = result.ToList();
            Assert.Equal(2, students.Count);
        }

        [Fact]
        public async Task UpdateAsync_WithValidId_ShouldUpdateStudent()
        {
            // Arrange
            var originalStudent = new StudentDto
            {
                Nombre = "Original Name",
                Email = "original@test.com",
                Carrera = "Original Career"
            };
            var created = await _service.CreateAsync(originalStudent);

            var updatedStudent = new StudentDto
            {
                Nombre = "Updated Name",
                Email = "updated@test.com",
                Carrera = "Updated Career"
            };

            // Act
            var result = await _service.UpdateAsync(created.Id, updatedStudent);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Name", result.Nombre);
            Assert.Equal("updated@test.com", result.Email);
            Assert.Equal("Updated Career", result.Carrera);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Nombre = "Test Student",
                Email = "test@test.com",
                Carrera = "Test Career"
            };

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(999, studentDto));
        }

        [Fact]
        public async Task DeleteAsync_WithValidId_ShouldDeleteStudent()
        {
            // Arrange
            var studentDto = new StudentDto
            {
                Nombre = "Student to Delete",
                Email = "delete@test.com",
                Carrera = "Delete Career"
            };
            var created = await _service.CreateAsync(studentDto);

            // Act
            var result = await _service.DeleteAsync(created.Id);

            // Assert
            Assert.True(result);
            
            // Verify student is deleted
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(created.Id));
        }

        [Fact]
        public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
        {
            // Act
            var result = await _service.DeleteAsync(999);

            // Assert
            Assert.False(result);
        }
    }
}