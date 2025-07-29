using Microsoft.AspNetCore.Mvc;
using Backend.Interfaces;
using Backend.Exceptions;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<TEntity, TDto> : ControllerBase 
        where TEntity : class 
        where TDto : class
    {
        protected readonly IService<TEntity, TDto> _service;

        protected BaseController(IService<TEntity, TDto> service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TDto>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public virtual async Task<ActionResult<TDto>> Create(TDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = GetIdFromDto(result) }, result);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TDto>> Update(int id, TDto dto)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound($"Entidad con ID {id} no encontrada");
            
            return NoContent();
        }

        protected virtual int GetIdFromDto(TDto dto)
        {
            // Esta implementación debe ser sobrescrita en controladores específicos
            // para obtener el ID del DTO
            return 0;
        }
    }
}