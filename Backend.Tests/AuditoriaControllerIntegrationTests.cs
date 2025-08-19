using AutoMapper;
using Backend.Contexts;
using Backend.Controllers;
using Backend.DTOs;
using Backend.Exceptions;
using Backend.Models;
using Backend.Repositories;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Backend.Tests
{
    public class AuditoriaControllerIntegrationTests : IntegrationTestBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly RepositorioAuditoria _repoAuditoria;
        private readonly ServicioAuditoria _servicioAuditoria;
        private readonly AuditoriaController _controller;

        public AuditoriaControllerIntegrationTests()
        {
            _dbContext = GetInMemoryDbContext();
            _mapper = GetMapper();
            _repoAuditoria = new RepositorioAuditoria(_dbContext);
            _servicioAuditoria = new ServicioAuditoria(_repoAuditoria, _mapper);
            _controller = new AuditoriaController(_servicioAuditoria);
        }

        [Fact]
        public async Task Buscar_ReturnsOkWithExpectedLogs()
        {
            // Arrange
            var Usuario1 = new Usuario { IdUsuario = 1, NombreUsuario = "Admin" };
            var log1 = new Auditoria { IdAuditoria = 1, IdUsuario = 1, TipoOperacion = "Insert", FechaOperacion = System.DateTime.Now.AddDays(-1) };
            var log2 = new Auditoria { IdAuditoria = 2, IdUsuario = null, TipoOperacion = "Update", FechaOperacion = System.DateTime.Now };
            _dbContext.Usuarios.Add(Usuario1);
            _dbContext.Auditoria.AddRange(log1, log2);
            _dbContext.SaveChanges();

            var filtro = new AuditoriaBuscarDto { UsuarioNombre = "Admin" };

            // Act
            var result = await _controller.Buscar(filtro);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var logs = Assert.IsAssignableFrom<IEnumerable<AuditoriaDto>>(okResult.Value);
            Assert.Single(logs);
            Assert.Equal("Admin", logs.First().UsuarioNombre);
        }

        [Fact]
        public async Task Buscar_ReturnsNotFoundException()
        {
            // Arrange
            var filtro = new AuditoriaBuscarDto { UsuarioNombre = "NonExistentUser" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(async () =>
            {
                await _controller.Buscar(filtro);
            });

            Assert.Equal("No se encontraron registros de auditoria con los filtros especificados.", exception.Message);
        }
    }
}