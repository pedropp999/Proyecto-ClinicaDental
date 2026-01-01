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
    public class RecordatoriosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecordatoriosController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<RecordatorioDto>>> GetRecordatorios([FromQuery] RecordatorioFilterDto filtro)
        {
            var pagedList = await _unitOfWork.Recordatorio.ObtenerListaPaginadaAsync(filtro);

            var response = new PagedResultDto<RecordatorioDto>
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

        [HttpGet("pendientes")]
        public async Task<ActionResult<IEnumerable<RecordatorioDto>>> GetRecordatoriosPendientes()
        {
            var pendientes = await _unitOfWork.Recordatorio.ObtenerPendientesAsync();
            return Ok(pendientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecordatorioDto>> GetRecordatorio(int id)
        {
            var dto = await _unitOfWork.Recordatorio.ObtenerPorIdAsync(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecordatorio(RecordatorioDtoIn dto)
        {
            try
            {
                await _unitOfWork.Recordatorio.CrearRecordatorioAsync(dto);
                return Ok(new { Mensaje = "Recordatorio creado exitosamente." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecordatorio(int id, RecordatorioDtoIn dto)
        {
            if (id != dto.Id) return BadRequest("El ID no coincide.");
            try
            {
                await _unitOfWork.Recordatorio.ActualizarRecordatorioAsync(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpPatch("{id}/marcar-enviado")]
        public async Task<IActionResult> MarcarComoEnviado(int id)
        {
            await _unitOfWork.Recordatorio.MarcarComoEnviadoAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecordatorio(int id)
        {
            await _unitOfWork.Recordatorio.EliminarRecordatorioAsync(id);
            return NoContent();
        }
    }
}
