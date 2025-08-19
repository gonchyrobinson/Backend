using AutoMapper;
using Backend.Contexts;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests
{
    public class EmpresasControllerIntegrationTests : IntegrationTestBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RepositorioEmpresas _repoEmpresas;
        private readonly RepositorioConvenios _repoConvenios;
        private readonly ServicioEmpresas _servicioEmpresas;
        private readonly EmpresasController _controller;

        public EmpresasControllerIntegrationTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mapper = GetMapper();
            _repoEmpresas = new RepositorioEmpresas(_dbContext);
            _repoConvenios = new RepositorioConvenios(_dbContext);
            _servicioEmpresas = new ServicioEmpresas(_repoEmpresas, _repoConvenios, _mapper);
            _controller = new EmpresasController(_servicioEmpresas);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsOkWithExpectedData()
        {
            // Arrange
            var empresa = new Empresa
            {
                IdEmpresa = 1,
                Nombre = "Acme Corp",
                FechaInicio = DateOnly.FromDateTime(System.DateTime.Today),
                FechaFin = DateOnly.FromDateTime(System.DateTime.Today.AddYears(1)),
                TipoContrato = "Temporal",
                Encargado = "Juan Perez",
                Celular = "123456789",
                CorreoElectronico = "acme@empresa.com",
                Eliminado = false,
                FechaEliminacion = null
            };
            _dbContext.Empresas.Add(empresa);
            _dbContext.SaveChanges();

            var filtro = new EmpresaBusquedaAvanzadaDto { Nombre = "Acme" };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var empresas = Assert.IsAssignableFrom<IEnumerable<EmpresaDto>>(okResult.Value);
            Assert.Single(empresas);
            Assert.Equal("Acme Corp", empresas.First().Nombre);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsOkWithEmptyList()
        {
            // Arrange
            var filtro = new EmpresaBusquedaAvanzadaDto { Nombre = "NoExiste" };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var empresas = Assert.IsAssignableFrom<IEnumerable<EmpresaDto>>(okResult.Value);
            Assert.Empty(empresas);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithListOfEmpresas()
        {
            // Arrange
            var empresa1 = new Empresa { IdEmpresa = 1, Nombre = "Empresa 1" };
            var empresa2 = new Empresa { IdEmpresa = 2, Nombre = "Empresa 2" };
            _dbContext.Empresas.AddRange(empresa1, empresa2);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var empresas = Assert.IsAssignableFrom<IEnumerable<EmpresaDto>>(okResult.Value);
            Assert.Equal(2, empresas.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkWithEmpresa()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa 1" };
            _dbContext.Empresas.Add(empresa);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var empresaDto = Assert.IsType<EmpresaDto>(okResult.Value);
            Assert.Equal(1, empresaDto.IdEmpresa);
            Assert.Equal("Empresa 1", empresaDto.Nombre);
        }

        [Fact]
        public async Task Create_ReturnsCreatedWithEmpresa()
        {
            // Arrange
            var createDto = new EmpresaCreateDto { Nombre = "Empresa Nueva", TipoContrato = "temporal" };

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var empresaDto = Assert.IsType<EmpresaDto>(createdResult.Value);
            Assert.Equal("Empresa Nueva", empresaDto.Nombre);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedEmpresa()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa 1" };
            _dbContext.Empresas.Add(empresa);
            _dbContext.SaveChanges();

            var updateDto = new EmpresaDto { IdEmpresa = 1, Nombre = "Empresa Actualizada" };

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedEmpresa = Assert.IsType<EmpresaDto>(okResult.Value);
            Assert.Equal(1, updatedEmpresa.IdEmpresa);
            Assert.Equal("Empresa Actualizada", updatedEmpresa.Nombre);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa 1" };
            _dbContext.Empresas.Add(empresa);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
