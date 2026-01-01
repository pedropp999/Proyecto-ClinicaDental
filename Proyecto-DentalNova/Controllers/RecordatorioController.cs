using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_DentalNova.Models.RecordatorioViewModel;

namespace Proyecto_DentalNova.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class RecordatorioController : Controller
    {
        private readonly IRecordatorioService _recordatorioService;

        public RecordatorioController(IRecordatorioService recordatorioService)
        {
            _recordatorioService = recordatorioService;
        }

        // --- GET: Index ---
        [HttpGet]
        public async Task<IActionResult> Index([Bind(Prefix = "Filtro")] RecordatorioFilterViewModel filtro)
        {
            // 1. Mapear VM Filtro -> DTO Filtro
            var filtroDto = new RecordatorioFilterDto
            {
                Page = filtro.Page,
                PageSize = filtro.PageSize,
                Id = filtro.Id,
                CitaId = filtro.CitaId,
                Enviado = filtro.Enviado,
                FechaEnvioDesde = filtro.FechaEnvioDesde,
                FechaEnvioHasta = filtro.FechaEnvioHasta,
                MensajeLike = filtro.MensajeLike
            };

            // 2. Llamar API
            var apiResult = await _recordatorioService.ObtenerRecordatoriosAsync(filtroDto);

            // 3. Crear lista paginada
            var pagedResults = PaginatedList<RecordatorioDto>.Create(
                apiResult.Items,
                apiResult.TotalCount,
                apiResult.PageIndex,
                filtro.PageSize);

            var vm = new RecordatorioIndexViewModel
            {
                Filtro = filtro,
                Resultados = pagedResults
            };

            return View(vm);
        }

        // --- GET: Pendientes ---
        [HttpGet]
        public async Task<IActionResult> Pendientes()
        {
            var pendientes = await _recordatorioService.ObtenerRecordatoriosPendientesAsync();
            return View(pendientes);
        }

        // --- GET: Details ---
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dto = await _recordatorioService.ObtenerRecordatorioPorIdAsync(id.Value);
                return View(dto);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- GET: Create ---
        public IActionResult Create()
        {
            var vm = new RecordatorioVM
            {
                Recordatorio = new RecordatorioDtoIn
                {
                    FechaEnvio = DateTime.Now
                }
            };
            return View(vm);
        }

        // --- POST: Create ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecordatorioVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _recordatorioService.CrearRecordatorioAsync(vm.Recordatorio);
                    TempData["MensajeExito"] = "Recordatorio creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            TempData["MensajeError"] = "No se pudo crear el recordatorio. Por favor, revise los errores.";
            return View(vm);
        }

        // --- GET: Edit ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dtoOut = await _recordatorioService.ObtenerRecordatorioPorIdAsync(id.Value);

                // Mapear Salida -> Entrada para el formulario
                var dtoIn = new RecordatorioDtoIn
                {
                    Id = dtoOut.Id,
                    CitaId = dtoOut.CitaId,
                    FechaEnvio = dtoOut.FechaEnvio,
                    Mensaje = dtoOut.Mensaje,
                    Enviado = dtoOut.Enviado
                };

                var vm = new RecordatorioVM { Recordatorio = dtoIn };
                return View(vm);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- POST: Edit ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecordatorioVM vm)
        {
            if (id != vm.Recordatorio.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _recordatorioService.ActualizarRecordatorioAsync(id, vm.Recordatorio);
                    TempData["MensajeExito"] = "Recordatorio actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            TempData["MensajeError"] = "No se pudo actualizar el recordatorio. Por favor, revise los errores.";
            return View(vm);
        }

        // --- GET: Delete ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dto = await _recordatorioService.ObtenerRecordatorioPorIdAsync(id.Value);
                return View(dto);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- POST: Delete ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _recordatorioService.EliminarRecordatorioAsync(id);
                TempData["MensajeExito"] = "Recordatorio eliminado correctamente.";
            }
            catch (HttpRequestException ex)
            {
                TempData["MensajeError"] = "Error al eliminar: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Recordatorio/MarcarEnviado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarEnviado(int id)
        {
            try
            {
                await _recordatorioService.MarcarComoEnviadoAsync(id);
                TempData["MensajeExito"] = "Recordatorio marcado como enviado.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "No se pudo marcar el recordatorio: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
