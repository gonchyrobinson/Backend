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

            var updateDto = new PagosDto { IdPago = 1, Monto = 2000 };

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
            var pagoDto = Assert.IsType<PagosDto>(okResult.Value);
            Assert.Equal(1, pagoDto.IdPasantia);
            Assert.Equal(1000, pagoDto.Monto);
        }

        [Fact]
        public async Task GetByPasantiaId_ReturnsNotFoundException()
        {
            // Arrange
            var pasantia = new Pasantia { IdPasantia = 2 };
            _dbContext.Pasantias.Add(pasantia);
            _dbContext.SaveChanges();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _controller.GetByPasantiaId(2);
            });

            Assert.Equal("No se encontró un pago para la pasantía con ID 2", exception.Message);
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
    }
}
