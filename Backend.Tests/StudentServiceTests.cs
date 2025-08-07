using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Backend.Contexts;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;
using Backend.Interfaces;
using Backend.Services;
using Backend.Mappings;
using Backend.Exceptions;
using Xunit;

namespace Backend.Tests
{
    public class StudentServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly RepositorioEstudiantes _repository;
        private readonly IRepositorioPasantias _repoPasantias;
        private readonly IMapper _mapper;
        private readonly ServicioEstudiantes _service;

        public StudentServiceTests()
        {
            // Configurar base de datos en memoria para tests
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new RepositorioEstudiantes(_context);
            _repoPasantias = Moq.Mock.Of<IRepositorioPasantias>();

            // Configurar AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            _service = new ServicioEstudiantes(_repository, _repoPasantias, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateStudent()
        {
            // Arrange

            var studentCreateDto = new StudentCreateDto
            {
                Nombre = "John",
                Apellido = "Doe",
                Email = "john@example.com",
                Carrera = "Computer Science",
                Documento = "12345678",
                Domicilio = "Calle Principal 123",
                Libreta = "2023-001",
                AreaTrabajo = "Desarrollo Web"
            };

            // Act
            var result = await _service.CreateAsync(studentCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John", result.Nombre);
            Assert.Equal("Doe", result.Apellido);
            Assert.Equal("john@example.com", result.Email);
            Assert.Equal("Computer Science", result.Carrera);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnStudent()
        {
            // Arrange
            var studentCreateDto = new StudentCreateDto
            {
                Nombre = "Jane",
                Apellido = "Smith",
                Email = "jane@example.com",
                Carrera = "Engineering"
            };

            var createdStudent = await _service.CreateAsync(studentCreateDto);

            // Act
            var result = await _service.GetByIdAsync(createdStudent.IdEstudiante);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.Nombre);
            Assert.Equal("Smith", result.Apellido);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(999));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStudent()
        {
            // Arrange
            var studentCreateDto = new StudentCreateDto
            {
                Nombre = "Original",
                Apellido = "Name",
                Email = "original@example.com",
                Carrera = "Original Career"
            };

            var createdStudent = await _service.CreateAsync(studentCreateDto);

            var updateDto = new StudentDto
            {
                IdEstudiante = createdStudent.IdEstudiante,
                Nombre = "Updated",
                Apellido = "Name",
                Email = "updated@example.com",
                Carrera = "Updated Career"
            };

            // Act
            var result = await _service.UpdateAsync(updateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.Nombre);
            Assert.Equal("updated@example.com", result.Email);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteStudent()
        {
            // Arrange
            var studentCreateDto = new StudentCreateDto
            {
                Nombre = "ToDelete",
                Apellido = "Student",
                Email = "delete@example.com",
                Carrera = "Test Career"
            };

            var createdStudent = await _service.CreateAsync(studentCreateDto);

            // Act
            await _service.DeleteAsync(createdStudent.IdEstudiante);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(createdStudent.IdEstudiante));
        }
    }
}