using DentalNova.Core.Dtos;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DentalNova.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administrador")]
    public class CompraArticulosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompraArticulosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<CompraArticuloDto>>> GetCompraArticulos([FromQuery] CompraArticuloFilterDto filtro)
        {
            var pagedList = await _unitOfWork.CompraArticulo.ObtenerListaPaginadaAsync(filtro);

            var response = new PagedResultDto<CompraArticuloDto>
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

        [HttpGet("{id}")]
        public async Task<ActionResult<CompraArticuloDto>> GetCompraArticulo(int id)
        {
            var dto = await _unitOfWork.CompraArticulo.ObtenerPorIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCompraArticulo(CompraArticuloDtoIn dto)
        {
            try
            {
                await _unitOfWork.CompraArticulo.CrearCompraArticuloAsync(dto);
                return Ok(new { Mensaje = "Compra de artículo registrada exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompraArticulo(int id, CompraArticuloDtoIn dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide.");
            try
            {
                await _unitOfWork.CompraArticulo.ActualizarCompraArticuloAsync(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompraArticulo(int id)
        {
            await _unitOfWork.CompraArticulo.EliminarCompraArticuloAsync(id);
            return NoContent();
        }
    }
}
