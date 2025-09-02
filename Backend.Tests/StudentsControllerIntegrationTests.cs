using AutoMapper;
using Backend.Contexts;
using Backend.Controllers;
using Backend.DTOs.StudentDtos;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests
{

    public class StudentsControllerIntegrationTests : IntegrationTestBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RepositorioEstudiantes _repoEstudiantes;
        private readonly RepositorioPasantias _repoPasantias;
        private readonly ServicioEstudiantes _servicioEstudiantes;
        private readonly StudentsController _controller;

        public StudentsControllerIntegrationTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mapper = GetMapper();
            _repoEstudiantes = new RepositorioEstudiantes(_dbContext);
            _repoPasantias = new RepositorioPasantias(_dbContext);
            var validationService = new EstudianteValidationService(_repoEstudiantes, _repoPasantias);
            _servicioEstudiantes = new ServicioEstudiantes(_repoEstudiantes, _mapper, validationService);
            _controller = new StudentsController(_servicioEstudiantes);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsOkWithExpectedData()
        {
            // Arrange
            var estudiante = new Estudiante { IdEstudiante = 1, Nombre = "Juan", Apellido = "Perez", Carrera = "Ing", AreaTrabajo = "IT", Documento = "43850228"  };
            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.SaveChanges();

            var filtro = new StudentBusquedaAvanzadaDto { Documento = "43850228" };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var estudiantes = Assert.IsAssignableFrom<IEnumerable<StudentDto>>(okResult.Value);
            Assert.Single(estudiantes);
            Assert.Equal("Juan", estudiantes.First().Nombre);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsOkWithEmptyList()
        {
            // Arrange
            var filtro = new StudentBusquedaAvanzadaDto { Documento = "NoExiste" };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var estudiantes = Assert.IsAssignableFrom<IEnumerable<StudentDto>>(okResult.Value);
            Assert.Empty(estudiantes);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithListOfStudents()
        {
            // Arrange
            var student1 = new Estudiante { IdEstudiante = 1, Nombre = "Juan", Apellido = "Perez", Documento = "12345678" };
            var student2 = new Estudiante { IdEstudiante = 2, Nombre = "Maria", Apellido = "Gomez", Documento = "87654321" };
            _dbContext.Estudiantes.AddRange(student1, student2);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var students = Assert.IsAssignableFrom<IEnumerable<StudentDto>>(okResult.Value);
            Assert.Equal(2, students.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkWithStudent()
        {
            // Arrange
            var student = new Estudiante { IdEstudiante = 1, Nombre = "Juan", Apellido = "Perez" };
            _dbContext.Estudiantes.Add(student);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var studentDto = Assert.IsType<StudentDto>(okResult.Value);
            Assert.Equal(1, studentDto.IdEstudiante);
            Assert.Equal("Juan", studentDto.Nombre);
        }

        [Fact]
        public async Task Create_ReturnsCreatedWithStudent()
        {
            // Arrange
            var createDto = new StudentCreateDto { Nombre = "Juan", Apellido = "Perez", Carrera = "Ing" };

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var studentDto = Assert.IsType<StudentDto>(createdResult.Value);
            Assert.Equal("Juan", studentDto.Nombre);
            Assert.Equal("Perez", studentDto.Apellido);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedStudent()
        {
            // Arrange
            var student = new Estudiante { IdEstudiante = 1, Nombre = "Juan", Apellido = "Perez" };
            _dbContext.Estudiantes.Add(student);
            _dbContext.SaveChanges();

            var updateDto = new StudentUpdateDto { IdEstudiante = 1, Nombre = "Juan Updated", Apellido = "Perez Updated" };

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedStudent = Assert.IsType<StudentDto>(okResult.Value);
            Assert.Equal(1, updatedStudent.IdEstudiante);
            Assert.Equal("Juan Updated", updatedStudent.Nombre);
            Assert.Equal("Perez Updated", updatedStudent.Apellido);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var student = new Estudiante { IdEstudiante = 1, Nombre = "Juan", Apellido = "Perez" };
            _dbContext.Estudiantes.Add(student);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
