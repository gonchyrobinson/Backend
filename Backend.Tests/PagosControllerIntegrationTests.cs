using AutoMapper;
using Backend.Contexts;
using Backend.Controllers;
using Backend.DTOs.PagosDtos;
using Backend.Exceptions;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Backend.Tests
{
    public class PagosControllerIntegrationTests : IntegrationTestBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RepositorioPagos _repoPagos;
        private readonly RepositorioPasantias _repoPasantias;
        private readonly ServicioPagos _servicioPagos;
        private readonly PagosController _controller;

        public PagosControllerIntegrationTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mapper = GetMapper();
            _repoPagos = new RepositorioPagos(_dbContext);
            _repoPasantias = new RepositorioPasantias(_dbContext);
            _servicioPagos = new ServicioPagos(_repoPagos, _repoPasantias, _mapper);
            _controller = new PagosController(_servicioPagos);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithListOfPagos()
        {
            // Arrange
            var pago1 = new Pago { IdPago = 1, Monto = 1000 };
            var pago2 = new Pago { IdPago = 2, Monto = 2000 };
            _dbContext.Pagos.AddRange(pago1, pago2);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagos = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            Assert.Equal(2, pagos.Count());
        }

        [Fact]
        public async Task GetById_ReturnsOkWithPago()
        {
            // Arrange
            var pago = new Pago { IdPago = 1, Monto = 1000 };
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagoDto = Assert.IsType<PagosDto>(okResult.Value);
            Assert.Equal(1, pagoDto.IdPago);
            Assert.Equal(1000, pagoDto.Monto);
        }

        [Fact]
        public async Task Create_ReturnsCreatedWithPago()
        {
            // Arrange
            var createDto = new CreatePagosDto { Monto = 1500 };

            // Act
            var result = await _controller.Create(createDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var pagoDto = Assert.IsType<PagosDto>(createdResult.Value);
            Assert.Equal(1500, pagoDto.Monto);
        }

        [Fact]
        public async Task Update_ReturnsOkWithUpdatedPago()
        {
            // Arrange
            var pago = new Pago { IdPago = 1, Monto = 1000 };
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            var updateDto = new PagosUpdateDto { IdPago = 1, Monto = 2000 };

            // Act
            var result = await _controller.Update(updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedPago = Assert.IsType<PagosDto>(okResult.Value);
            Assert.Equal(1, updatedPago.IdPago);
            Assert.Equal(2000, updatedPago.Monto);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var pago = new Pago { IdPago = 1, Monto = 1000 };
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();
            
            // Desacoplar la entidad del contexto para evitar conflictos de tracking
            _dbContext.Entry(pago).State = EntityState.Detached;

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetByPasantiaId_ReturnsOkWithPago()
        {
            // Arrange
            var pasantia = new Pasantia { IdPasantia = 1 };
            var pago = new Pago { IdPago = 1, IdPasantia = 1, Monto = 1000 };
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetByPasantiaId(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagosDto = Assert.IsAssignableFrom<List<PagosDto>>(okResult.Value);
            Assert.Single(pagosDto);
            Assert.Equal(1, pagosDto[0].IdPasantia);
            Assert.Equal(1000, pagosDto[0].Monto);
        }

        [Fact]
        public async Task GetByPasantiaId_ReturnsEmptyList_WhenNoPagos()
        {
            // Arrange
            var pasantia = new Pasantia { IdPasantia = 2 };
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetByPasantiaId(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagosDto = Assert.IsAssignableFrom<List<PagosDto>>(okResult.Value);
            Assert.Empty(pagosDto);
        }

        [Fact]
        public async Task MarcarComoPagado_SetsPagadoAndFechaPago()
        {
            // Arrange
            var pago = new Pago { IdPago = 1, Monto = 1000, Pagado = false, FechaPago = null };
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();
            var dto = new MarcarPagoDto { IdPago = 1 };

            // Act
            var result = await _controller.MarcarComoPagado(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagoDto = Assert.IsType<PagosDto>(okResult.Value);
            Assert.True(pagoDto.Pagado);
            Assert.NotNull(pagoDto.FechaPago);
        }

        [Fact]
        public async Task GetPagosPorVencer_ReturnsPagosDueWithinSpecifiedDays()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var pago1 = new Pago { IdPago = 1, Monto = 1000, FechaVencimiento = today.AddDays(5), Pagado = false };
            var pago2 = new Pago { IdPago = 2, Monto = 2000, FechaVencimiento = today.AddDays(15), Pagado = false };
            var pago3 = new Pago { IdPago = 3, Monto = 3000, FechaVencimiento = today.AddDays(25), Pagado = false };
            var pago4 = new Pago { IdPago = 4, Monto = 4000, FechaVencimiento = today.AddDays(35), Pagado = false };
            
            _dbContext.Pagos.AddRange(pago1, pago2, pago3, pago4);
            _dbContext.SaveChanges();

            // Act - Buscar pagos que vencen en los próximos 25 días (para incluir el Pago 3)
            var result = await _controller.GetPagosPorVencer(25);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagosDto = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            var pagosList = pagosDto.ToList();
            
            // Debería retornar solo los pagos 1, 2 y 3 (que vencen en 5, 15 y 25 días)
            Assert.Equal(3, pagosList.Count);
            Assert.Contains(pagosList, p => p.IdPago == 1);
            Assert.Contains(pagosList, p => p.IdPago == 2);
            Assert.Contains(pagosList, p => p.IdPago == 3);
            Assert.DoesNotContain(pagosList, p => p.IdPago == 4); // Este vence en 35 días
        }

        [Fact]
        public async Task GetPagosPorVencer_ReturnsEmptyList_WhenNoPagosDueWithinDays()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var pago1 = new Pago { IdPago = 1, Monto = 1000, FechaVencimiento = today.AddDays(25), Pagado = false };
            var pago2 = new Pago { IdPago = 2, Monto = 2000, FechaVencimiento = today.AddDays(30), Pagado = false };
            
            _dbContext.Pagos.AddRange(pago1, pago2);
            _dbContext.SaveChanges();

            // Act - Buscar pagos que vencen en los próximos 10 días
            var result = await _controller.GetPagosPorVencer(10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagosDto = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            Assert.Empty(pagosDto);
        }

        [Fact]
        public async Task GetPagosPorVencer_ReturnsOnlyUnpaidPagos()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Today);
            var pago1 = new Pago { IdPago = 1, Monto = 1000, FechaVencimiento = today.AddDays(5), Pagado = false };
            var pago2 = new Pago { IdPago = 2, Monto = 2000, FechaVencimiento = today.AddDays(10), Pagado = true }; // Ya pagado
            var pago3 = new Pago { IdPago = 3, Monto = 3000, FechaVencimiento = today.AddDays(15), Pagado = false };
            
            _dbContext.Pagos.AddRange(pago1, pago2, pago3);
            _dbContext.SaveChanges();

            // Act - Buscar pagos que vencen en los próximos 20 días
            var result = await _controller.GetPagosPorVencer(20);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagosDto = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            var pagosList = pagosDto.ToList();
            
            // Debería retornar solo los pagos no pagados (1 y 3)
            Assert.Equal(2, pagosList.Count);
            Assert.Contains(pagosList, p => p.IdPago == 1);
            Assert.DoesNotContain(pagosList, p => p.IdPago == 2); // Ya pagado
            Assert.Contains(pagosList, p => p.IdPago == 3);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsOkWithExpectedData()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa Test" };
            var estudiante = new Estudiante { IdEstudiante = 1, Documento = "12345678" };
            var convenio = new Convenio { IdConvenio = 1, IdEmpresa = 1 };
            var pasantia = new Pasantia { IdPasantia = 1, IdConvenio = 1, IdEstudiante = 1 };
            var pago = new Pago { IdPago = 1, IdPasantia = 1, FechaVencimiento = DateOnly.FromDateTime(DateTime.Today.AddDays(30)), Pagado = false };

            _dbContext.Empresas.Add(empresa);
            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.Convenios.Add(convenio);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            var filtro = new PagosBusquedaAvanzadaDto
            {
                IdEmpresa = 1,
                Estudiante = "12345678",
                EstadoPago = false,
                FechaVencimiento = DateTime.Today.AddDays(30).ToString("yyyy-MM-dd")
            };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagos = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            var pagosList = pagos.ToList();
            Assert.Single(pagosList);
            Assert.Equal(1, pagosList.First().IdPago);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsEmptyList_WhenNoMatches()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa Test" };
            var estudiante = new Estudiante { IdEstudiante = 1, Documento = "12345678" };
            var convenio = new Convenio { IdConvenio = 1, IdEmpresa = 1 };
            var pasantia = new Pasantia { IdPasantia = 1, IdConvenio = 1, IdEstudiante = 1 };
            var pago = new Pago { IdPago = 1, IdPasantia = 1, FechaVencimiento = DateOnly.FromDateTime(DateTime.Today.AddDays(30)), Pagado = false };

            _dbContext.Empresas.Add(empresa);
            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.Convenios.Add(convenio);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            var filtro = new PagosBusquedaAvanzadaDto
            {
                IdEmpresa = 999, // Empresa que no existe
                Estudiante = "99999999", // Estudiante que no existe
                EstadoPago = true, // Estado diferente
                FechaVencimiento = DateTime.Today.AddDays(999).ToString("yyyy-MM-dd") // Fecha que no existe
            };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagos = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            Assert.Empty(pagos);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsAllPagos_WhenNoFilters()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa Test" };
            var estudiante = new Estudiante { IdEstudiante = 1, Documento = "12345678" };
            var convenio = new Convenio { IdConvenio = 1, IdEmpresa = 1 };
            var pasantia = new Pasantia { IdPasantia = 1, IdConvenio = 1, IdEstudiante = 1 };
            var pago1 = new Pago { IdPago = 1, IdPasantia = 1, FechaVencimiento = DateOnly.FromDateTime(DateTime.Today.AddDays(30)), Pagado = false };
            var pago2 = new Pago { IdPago = 2, IdPasantia = 1, FechaVencimiento = DateOnly.FromDateTime(DateTime.Today.AddDays(60)), Pagado = true };

            _dbContext.Empresas.Add(empresa);
            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.Convenios.Add(convenio);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.Pagos.AddRange(pago1, pago2);
            _dbContext.SaveChanges();

            var filtro = new PagosBusquedaAvanzadaDto(); // Sin filtros

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagos = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            var pagosList = pagos.ToList();
            Assert.Equal(2, pagosList.Count);
        }

        [Fact]
        public async Task GetSugerenciasEmpresas_ReturnsOkWithExpectedData()
        {
            // Arrange
            var empresa = new Empresa { IdEmpresa = 1, Nombre = "Empresa Test" };
            var convenio = new Convenio { IdConvenio = 1, IdEmpresa = 1 };
            var pasantia = new Pasantia { IdPasantia = 1, IdConvenio = 1 };
            var pago = new Pago { IdPago = 1, IdPasantia = 1 };

            _dbContext.Empresas.Add(empresa);
            _dbContext.Convenios.Add(convenio);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetSugerenciasEmpresas();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var empresas = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            var empresasList = empresas.ToList();
            Assert.Single(empresasList);
        }

        [Fact]
        public async Task GetSugerenciasEstudiantes_ReturnsOkWithExpectedData()
        {
            // Arrange
            var estudiante = new Estudiante { IdEstudiante = 1, Documento = "12345678" };
            var pasantia = new Pasantia { IdPasantia = 1, IdEstudiante = 1 };
            var pago = new Pago { IdPago = 1, IdPasantia = 1 };

            _dbContext.Estudiantes.Add(estudiante);
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.Pagos.Add(pago);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetSugerenciasEstudiantes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var estudiantes = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            var estudiantesList = estudiantes.ToList();
            Assert.Single(estudiantesList);
        }

        [Fact]
        public async Task BuscarAvanzado_ReturnsOk_WhenInvalidDateFormat()
        {
            // Arrange
            var filtro = new PagosBusquedaAvanzadaDto
            {
                FechaVencimiento = "invalid-date-format"
            };

            // Act
            var result = await _controller.BuscarAvanzado(filtro);

            // Assert
            // El método maneja la fecha inválida de forma segura y retorna Ok con lista vacía
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagos = Assert.IsAssignableFrom<IEnumerable<PagosDto>>(okResult.Value);
            Assert.Empty(pagos);
        }
    }
}
