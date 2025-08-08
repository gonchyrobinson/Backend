using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TDto, TCreateDto> : ControllerBase 
        where TEntity : class 
        where TDto : class
        where TCreateDto : class
    {
        protected readonly IService<TEntity, TDto, TCreateDto> _service;

        protected BaseController(IService<TEntity, TDto, TCreateDto> service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                return Ok(result);
            }
            catch (Exceptions.AppException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Loguear la excepción si es necesario
                return StatusCode(500, "Ocurrió un error inesperado. Por favor contacte al administrador.");
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public virtual async Task<ActionResult<TDto>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (Exceptions.AppException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Loguear la excepción si es necesario
                return StatusCode(500, "Ocurrió un error inesperado. Por favor contacte al administrador.");
            }
        }

        [HttpPost]
        [Authorize]
        public virtual async Task<ActionResult<TDto>> Create(TCreateDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = GetIdFromDto(result) }, result);
            }
            catch (Exceptions.AppException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Loguear la excepción si es necesario
                return StatusCode(500, "Ocurrió un error inesperado. Por favor contacte al administrador.");
            }
        }

        [HttpPut]
        [Authorize]
        public virtual async Task<ActionResult<TDto>> Update(TDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(dto);
                return Ok(result);
            }
            catch (Exceptions.AppException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Loguear la excepción si es necesario
                return StatusCode(500, "Ocurrió un error inesperado. Por favor contacte al administrador.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public virtual async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);
                if (!result)
                    return NotFound($"Entidad con ID {id} no encontrada");
                return NoContent();
            }
            catch (Exceptions.AppException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                // Loguear la excepción si es necesario
                return StatusCode(500, "Ocurrió un error inesperado. Por favor contacte al administrador.");
            }
        }

        protected abstract int GetIdFromDto(TDto dto);
    }
}