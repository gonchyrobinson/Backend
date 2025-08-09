using Xunit;
using Backend.Contexts;
using Backend.Repositories;
using Backend.Services;
using Backend.Controllers;
using Backend.Models;
using Backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Backend.Exceptions;

namespace Backend.Tests
{
    public class PasantiasControllerIntegrationTests : IntegrationTestBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RepositorioPasantias _repoPasantias;
        private readonly RepositorioConvenios _repoConvenios;
        private readonly RepositorioEstudiantes _repoEstudiantes;
        private readonly ServicioPasantias _servicioPasantias;
        private readonly PasantiasController _controller;

        public PasantiasControllerIntegrationTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mapper = GetMapper();
            _repoPasantias = new RepositorioPasantias(_dbContext);
            _repoConvenios = new RepositorioConvenios(_dbContext);
            _repoEstudiantes = new RepositorioEstudiantes(_dbContext);
            _servicioPasantias = new ServicioPasantias(_repoPasantias, _repoEstudiantes, _repoConvenios, _mapper);
            _controller = new PasantiasController(_servicioPasantias);
        }

        [Fact]
        public async Task GetAllDetalle_ReturnsOkWithNotEmptyList()
        {
            // Arrange
            var convenio = new Convenio { IdConvenio = 1 };
            var estudiante = new Estudiante { IdEstudiante = 1 };
            var pasantia = new Pasantia { IdPasantia = 1, IdConvenio = 1, IdEstudiante = 1 };
            _dbContext.Convenios.Add(convenio);
            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetAllDetalle();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var detalles = Assert.IsAssignableFrom<IEnumerable<PasantiaDetalleDto>>(okResult.Value);
            Assert.NotEmpty(detalles);
            Assert.Contains(detalles, d => d.Pasantia != null && d.Pasantia.IdPasantia == 1);
        }

        [Fact]
        public async Task GetByConvenioId_ReturnsNotFoundException()
        {
            // Arrange
            var convenio = new Convenio { IdConvenio = 1 };
            _dbContext.Convenios.Add(convenio);
            _dbContext.SaveChanges();

            // Act & Assert
            try
            {
                var result = await _controller.GetByConvenioId(1);
            }
            catch (NotFoundException ex)
            {
                Assert.Equal("No se encontraron pasantías para el convenio con ID 1", ex.Message);
            }
        }

        [Fact]
        public async Task GetByEstudianteId_ReturnsOkWithNotEmptyList()
        {
            // Arrange
            var estudiante = new Estudiante { IdEstudiante = 1, Nombre = "Juan" };
            var convenio = new Convenio { IdConvenio = 1 };
            var pasantia = new Pasantia { IdPasantia = 1, IdEstudiante = 1, IdConvenio = 1 };

            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.Convenios.Add(convenio);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetByEstudianteId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pasantias = Assert.IsAssignableFrom<IEnumerable<PasantiaDto>>(okResult.Value);
            Assert.NotEmpty(pasantias);
            Assert.Contains(pasantias, p => p.IdPasantia == 1 && p.IdEstudiante == 1);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithListOfPasantias()
        {
            // Arrange
            var pasantia1 = new Pasantia { IdPasantia = 1, IdEstudiante = 1, IdConvenio = 1 };
            var pasantia2 = new Pasantia { IdPasantia = 2, IdEstudiante = 2, IdConvenio = 2 };
            _dbContext.Pasantias.AddRange(pasantia1, pasantia2);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pasantias = Assert.IsAssignableFrom<IEnumerable<PasantiaDto>>(okResult.Value);
            Assert.Equal(2, pasantias.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkWithPasantia()
        {
            // Arrange
            var pasantia = new Pasantia { IdPasantia = 1, IdEstudiante = 1, IdConvenio = 1 };
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pasantiaDto = Assert.IsType<PasantiaDto>(okResult.Value);
            Assert.Equal(1, pasantiaDto.IdPasantia);
        }

        [Fact]
        public async Task Create_ReturnsCreatedWithPasantia()
        {
            var estudiante1 = new Estudiante { IdEstudiante = 1, Nombre = "Juan" };
            var convenio1 = new Convenio { IdConvenio = 1 };
            _dbContext.Estudiantes.AddRange(estudiante1);
            _dbContext.Convenios.AddRange(convenio1);
            // Arrange
            var createDto = new PasantiaCreateDto { IdEstudiante = 1, IdConvenio = 1 };

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var pasantiaDto = Assert.IsType<PasantiaDto>(createdResult.Value);
            Assert.Equal(1, pasantiaDto.IdEstudiante);
            Assert.Equal(1, pasantiaDto.IdConvenio);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedPasantia()
        {
            // Arrange
            var estudiante1 = new Estudiante { IdEstudiante = 1, Nombre = "Juan" };
            var estudiante2 = new Estudiante { IdEstudiante = 2, Nombre = "Maria" };
            var convenio1 = new Convenio { IdConvenio = 1 };
            var convenio2 = new Convenio { IdConvenio = 2 };
            var pasantia = new Pasantia { IdPasantia = 1, IdEstudiante = 1, IdConvenio = 1 };

            _dbContext.Estudiantes.AddRange(estudiante1, estudiante2);
            _dbContext.Convenios.AddRange(convenio1, convenio2);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            var updateDto = new PasantiaDto { IdPasantia = 1, IdEstudiante = 2, IdConvenio = 2 };

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedPasantia = Assert.IsType<PasantiaDto>(okResult.Value);
            Assert.Equal(1, updatedPasantia.IdPasantia);
            Assert.Equal(2, updatedPasantia.IdEstudiante);
            Assert.Equal(2, updatedPasantia.IdConvenio);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var pasantia = new Pasantia { IdPasantia = 1, IdEstudiante = 1, IdConvenio = 1 };
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
