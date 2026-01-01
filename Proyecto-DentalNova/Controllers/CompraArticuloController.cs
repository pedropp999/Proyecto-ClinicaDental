using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_DentalNova.Models.CompraArticuloViewModel;

namespace Proyecto_DentalNova.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class CompraArticuloController : Controller
    {
        private readonly ICompraArticuloService _compraArticuloService;

        public CompraArticuloController(ICompraArticuloService compraArticuloService)
        {
            _compraArticuloService = compraArticuloService;
        }

        // --- GET: Index ---
        [HttpGet]
        public async Task<IActionResult> Index([Bind(Prefix = "Filtro")] CompraArticuloFilterViewModel filtro)
        {
            // 1. Mapear VM Filtro -> DTO Filtro
            var filtroDto = new CompraArticuloFilterDto
            {
                Page = filtro.Page,
                PageSize = filtro.PageSize,
                Id = filtro.Id,
                ArticuloId = filtro.ArticuloId,
                FechaDesde = filtro.FechaDesde,
                FechaHasta = filtro.FechaHasta,
                MetodoPago = filtro.MetodoPago,
                ProveedorLike = filtro.ProveedorLike,
                MontoMin = filtro.MontoMin,
                MontoMax = filtro.MontoMax
            };

            // 2. Llamar API
            var apiResult = await _compraArticuloService.ObtenerCompraArticulosAsync(filtroDto);

            // 3. Crear lista paginada
            var pagedResults = PaginatedList<CompraArticuloDto>.Create(
                apiResult.Items,
                apiResult.TotalCount,
                apiResult.PageIndex,
                filtro.PageSize);

            var vm = new CompraArticuloIndexViewModel
            {
                Filtro = filtro,
                Resultados = pagedResults
            };

            return View(vm);
        }

        // --- GET: Details ---
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dto = await _compraArticuloService.ObtenerCompraArticuloPorIdAsync(id.Value);
                return View(dto);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- GET: Create ---
        public async Task<IActionResult> Create()
        {
            var articulosActivos = await _compraArticuloService.ObtenerArticulosActivosAsync();
            var vm = new CompraArticuloVM
            {
                ArticulosDisponibles = articulosActivos,
                CompraArticulo = new CompraArticuloDtoIn
                {
                    FechaCompra = DateTime.Now
                }
            };
            return View(vm);
        }

        // --- POST: Create ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraArticuloVM vm)
        {
            // Calcular el subtotal
            vm.CompraArticulo.Subtotal = vm.CompraArticulo.Cantidad * vm.CompraArticulo.PrecioUnitario;

            if (ModelState.IsValid)
            {
                try
                {
                    await _compraArticuloService.CrearCompraArticuloAsync(vm.CompraArticulo);
                    TempData["MensajeExito"] = "Compra de artículo registrada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Recargar artículos disponibles en caso de error
            vm.ArticulosDisponibles = await _compraArticuloService.ObtenerArticulosActivosAsync();
            TempData["MensajeError"] = "No se pudo crear la compra. Por favor, revise los errores.";
            return View(vm);
        }

        // --- GET: Edit ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dtoOut = await _compraArticuloService.ObtenerCompraArticuloPorIdAsync(id.Value);
                var articulosActivos = await _compraArticuloService.ObtenerArticulosActivosAsync();

                // Mapear Salida -> Entrada para el formulario
                var dtoIn = new CompraArticuloDtoIn
                {
                    Id = dtoOut.Id,
                    ArticuloId = dtoOut.ArticuloId,
                    Cantidad = dtoOut.Cantidad,
                    PrecioUnitario = dtoOut.PrecioUnitario,
                    Subtotal = dtoOut.Subtotal,
                    FechaCompra = dtoOut.FechaCompra,
                    MetodoPago = dtoOut.MetodoPago,
                    Proveedor = dtoOut.Proveedor
                };

                var vm = new CompraArticuloVM
                {
                    CompraArticulo = dtoIn,
                    ArticulosDisponibles = articulosActivos
                };
                return View(vm);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- POST: Edit ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CompraArticuloVM vm)
        {
            if (id != vm.CompraArticulo.Id) return BadRequest();

            // Calcular el subtotal
            vm.CompraArticulo.Subtotal = vm.CompraArticulo.Cantidad * vm.CompraArticulo.PrecioUnitario;

            if (ModelState.IsValid)
            {
                try
                {
                    await _compraArticuloService.ActualizarCompraArticuloAsync(id, vm.CompraArticulo);
                    TempData["MensajeExito"] = "Compra de artículo actualizada correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            // Recargar artículos disponibles en caso de error
            vm.ArticulosDisponibles = await _compraArticuloService.ObtenerArticulosActivosAsync();
            TempData["MensajeError"] = "No se pudo actualizar la compra. Por favor, revise los errores.";
            return View(vm);
        }

        // --- GET: Delete ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dto = await _compraArticuloService.ObtenerCompraArticuloPorIdAsync(id.Value);
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
                await _compraArticuloService.EliminarCompraArticuloAsync(id);
                TempData["MensajeExito"] = "Compra de artículo eliminada correctamente.";
            }
            catch (HttpRequestException ex)
            {
                TempData["MensajeError"] = "Error al eliminar: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
