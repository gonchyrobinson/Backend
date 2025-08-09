using Xunit;
using AutoMapper;
using Backend.Contexts;
using Backend.Models;
using Backend.DTOs;
using Backend.Repositories;
using Backend.Services;

using Microsoft.AspNetCore.Mvc;
using Backend.Controllers;



namespace Backend.Tests
{
    public class ConveniosControllerIntegrationTests : IntegrationTestBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RepositorioConvenios _repoConvenios;
        private readonly RepositorioEmpresas _repoEmpresas;
    private readonly RepositorioPasantias _repoPasantias;
    private readonly ServicioConvenios _servicioConvenios;
    private readonly ConveniosController _controller;

        public ConveniosControllerIntegrationTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mapper = GetMapper();
            _repoConvenios = new RepositorioConvenios(_dbContext);
            _repoEmpresas = new RepositorioEmpresas(_dbContext);
            _repoPasantias = new RepositorioPasantias(_dbContext);
            _servicioConvenios = new ServicioConvenios(_repoConvenios, _repoEmpresas, _repoPasantias, _mapper);
            _controller = new ConveniosController(_servicioConvenios);
        }

                [Fact]
                public async Task Create_ReturnsCreatedWithData()
                {
                    // Arrange
                    var createDto = new ConvenioCreateDto {
                        Expediente = "EXP-123",
                        FechaFirma = System.DateOnly.FromDateTime(System.DateTime.Today),
                        FechaCaducidad = System.DateOnly.FromDateTime(System.DateTime.Today.AddYears(1))
                    };

                    // Act
                    var actionResult = await _controller.Create(createDto);

                    // Assert
                    var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
                    var dto = Assert.IsAssignableFrom<ConvenioDto>(createdResult.Value);
                    Assert.Equal("EXP-123", dto.Expediente);
                    Assert.NotEqual(0, dto.IdConvenio);
                }

                [Fact]
                public async Task Update_ReturnsOkWithUpdatedData()
                {
                    // Arrange
                    var convenio = new Convenio { IdConvenio = 10, Expediente = "EXP-OLD" };
                    _dbContext.Convenios.Add(convenio);
                    _dbContext.SaveChanges();

                    var updateDto = new ConvenioDto { IdConvenio = 10, Expediente = "EXP-NEW" };

                    // Act
                    var result = await _controller.Update(updateDto);

                    // Assert
                    var okResult = Assert.IsType<OkObjectResult>(result.Result);
                    var dto = Assert.IsType<ConvenioDto>(okResult.Value);
                    Assert.Equal(10, dto.IdConvenio);
                    Assert.Equal("EXP-NEW", dto.Expediente);
                }

                [Fact]
                public async Task Delete_ReturnsNoContent_WhenExists()
                {
                    // Arrange
                    var convenio = new Convenio { IdConvenio = 20 };
                    _dbContext.Convenios.Add(convenio);
                    _dbContext.SaveChanges();

                    // Act
                    var result = await _controller.Delete(20);

                    // Assert
                    Assert.IsType<NoContentResult>(result);
                }
        [Fact]
        public async Task ListarConveniosConEmpresa_ReturnsOkWithExpectedData()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa Test" };
            var convenio = new Convenio
            {
                IdConvenio = 1,
                IdEmpresa = 1,
                FechaFirma = System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(-10)),
                FechaCaducidad = System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(10))
            };

            _dbContext.Empresas.Add(empresa);
            _dbContext.Convenios.Add(convenio);
            _dbContext.SaveChanges();

            var filtro = new ConvenioEmpresaFiltroDto
            {
                NombreEmpresa = "Empresa Test",
                FechaFirmaDesde = System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(-15)),
                FechaFirmaHasta = System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(-5)),
                FechaCaducidadDesde = System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(5)),
                FechaCaducidadHasta = System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(15))
            };

            // Act
            var result = await _controller.ListarConveniosConEmpresa(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var convenios = Assert.IsAssignableFrom<IEnumerable<ConvenioEmpresaDto>>(okResult.Value);
            Assert.Single(convenios);
            Assert.Equal("Empresa Test", convenios.First().NombreEmpresa);
        }

        [Fact]
        public async Task AsignarEmpresa_ReturnsOk()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa Test" };
            var convenio = new Convenio { IdConvenio = 1 };
            _dbContext.Empresas.Add(empresa);
            _dbContext.Convenios.Add(convenio);
            _dbContext.SaveChanges();

            var dto = new AsignarEmpresaDto { ConvenioId = 1, EmpresaId = 1 };

            // Act
            var result = await _controller.AsignarEmpresa(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async Task CaducarConvenio_ReturnsOk()
        {
            // Arrange
            var convenio = new Convenio { IdConvenio = 2 };
            _dbContext.Convenios.Add(convenio);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.CaducarConvenio(2, System.DateOnly.FromDateTime(System.DateTime.Today.AddDays(1)));

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
