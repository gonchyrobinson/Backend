using Backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public virtual async Task<ActionResult<TDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public virtual async Task<ActionResult<TDto>> Create(TCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = GetIdFromDto(result) }, result);
        }

        [HttpPut]
        [Authorize]
        public virtual async Task<ActionResult<TDto>> Update(TDto dto)
        {
            var result = await _service.UpdateAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound($"Entidad con ID {id} no encontrada");
            return NoContent();
        }

        protected abstract int GetIdFromDto(TDto dto);
    }
}