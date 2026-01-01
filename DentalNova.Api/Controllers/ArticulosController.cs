using DentalNova.Business.Rules;
using DentalNova.Core.Dtos;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalNova.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ArticulosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Inyectamos la "Unidad de Trabajo" principal
        public ArticulosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtiene el catálogo de todos los artículos activos en inventario.
        /// </summary>
        /// <returns>Una lista de artículos con sus detalles.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArticuloDto>), 200)] // 200 OK
        public async Task<IActionResult> ObtenerCatalogo()
        {
            // 1. Llama a la capa de lógica de negocio (ArticuloBL)
            var catalogo = await _unitOfWork.Articulo.ObtenerCatalogoAsync();

            // 2. Devuelve los DTOs como JSON con un código 200 OK
            return Ok(catalogo);
        }

        [HttpGet("admin")]
        //[Authorize(Roles = "Administrador")]
        public async Task<ActionResult<PagedResultDto<ArticuloDto>>> GetArticulosAdmin([FromQuery] ArticuloFilterDto filtro)
        {
            var pagedList = await _unitOfWork.Articulo.ObtenerListaPaginadaAsync(filtro);

            var response = new PagedResultDto<ArticuloDto>
            {
                Items = pagedList,
                TotalCount = pagedList.TotalCount,
                TotalPages = pagedList.TotalPages,
                PageIndex = pagedList.PageIndex,
                HasNextPage = pagedList.HasNextPage,
                HasPreviousPage = pagedList.HasPreviousPage
            };
            return Ok(response);
        }

        [HttpGet("admin/{id}")]
        //[Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ArticuloDto>> GetArticuloAdmin(int id)
        {
            var dto = await _unitOfWork.Articulo.ObtenerPorIdAdminAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        //[Authorize(Roles = "Administrador")]
        public async Task<ActionResult> CreateArticulo(ArticuloDtoIn dto)
        {
            try
            {
                await _unitOfWork.Articulo.CrearArticuloAdminAsync(dto);
                return Ok(new { Mensaje = "Artículo creado exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UpdateArticulo(int id, ArticuloDtoIn dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide.");
            try
            {
                await _unitOfWork.Articulo.ActualizarArticuloAdminAsync(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteArticulo(int id)
        {
            await _unitOfWork.Articulo.EliminarArticuloAsync(id);
            return NoContent();
        }
    }
}
