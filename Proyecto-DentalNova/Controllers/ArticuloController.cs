using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_DentalNova.Models.ArticuloViewModel;

namespace Proyecto_DentalNova.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class ArticuloController : Controller
    {
        private readonly IArticuloService _articuloService;

        public ArticuloController(IArticuloService articuloService)
        {
            _articuloService = articuloService;
        }

        // --- GET: Index ---
        [HttpGet]
        public async Task<IActionResult> Index([Bind(Prefix = "Filtro")] ArticuloFilterViewModel filtro)
        {
            // 1. Mapear VM Filtro -> DTO Filtro
            var filtroDto = new ArticuloFilterDto
            {
                Page = filtro.Page,
                PageSize = filtro.PageSize,
                Id = filtro.Id,
                Categoria = filtro.Categoria,
                NombreLike = filtro.NombreLike,
                CodigoLike = filtro.CodigoLike,
                Reutilizable = filtro.Reutilizable,
                StockMin = filtro.StockMin,
                StockMax = filtro.StockMax,
                Activo = filtro.Activo
            };

            // 2. Llamar API
            var apiResult = await _articuloService.ObtenerArticulosAdminAsync(filtroDto);

            // 3. Crear lista paginada
            var pagedResults = PaginatedList<ArticuloDto>.Create(
                apiResult.Items,
                apiResult.TotalCount,
                apiResult.PageIndex,
                filtro.PageSize);

            var vm = new ArticuloIndexViewModel
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
                var dto = await _articuloService.ObtenerArticuloPorIdAsync(id.Value);
                return View(dto);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- GET: Create ---
        public IActionResult Create()
        {
            var vm = new ArticuloVM();
            return View(vm);
        }

        // --- POST: Create ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticuloVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _articuloService.CrearArticuloAsync(vm.Articulo);
                    TempData["MensajeExito"] = "Artículo creado exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            TempData["MensajeError"] = "No se pudo crear el artículo. Por favor, revise los errores.";
            return View(vm);
        }

        // --- GET: Edit ---
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dtoOut = await _articuloService.ObtenerArticuloPorIdAsync(id.Value);

                // Mapear Salida -> Entrada para el formulario
                var dtoIn = new ArticuloDtoIn
                {
                    Id = dtoOut.Id,
                    Categoria = dtoOut.Categoria,
                    Nombre = dtoOut.Nombre,
                    Descripcion = dtoOut.Descripcion,
                    Codigo = dtoOut.Codigo,
                    Reutilizable = dtoOut.Reutilizable,
                    Stock = dtoOut.Stock,
                    Activo = dtoOut.Activo
                };

                var vm = new ArticuloVM { Articulo = dtoIn };
                return View(vm);
            }
            catch (HttpRequestException) { return NotFound(); }
        }

        // --- POST: Edit ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ArticuloVM vm)
        {
            if (id != vm.Articulo.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _articuloService.ActualizarArticuloAsync(id, vm.Articulo);
                    TempData["MensajeExito"] = "Artículo actualizado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (HttpRequestException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            TempData["MensajeError"] = "No se pudo actualizar el artículo. Por favor, revise los errores.";
            return View(vm);
        }

        // --- GET: Delete ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var dto = await _articuloService.ObtenerArticuloPorIdAsync(id.Value);
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
                await _articuloService.EliminarArticuloAsync(id);
                TempData["MensajeExito"] = "Artículo eliminado correctamente.";
            }
            catch (HttpRequestException ex)
            {
                TempData["MensajeError"] = "Error al eliminar: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Articulo/ToggleActivo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActivo(int id)
        {
            try
            {
                // 1. Obtener el artículo actual
                var currentDto = await _articuloService.ObtenerArticuloPorIdAsync(id);

                // 2. Validación de seguridad
                if (currentDto == null)
                {
                    TempData["MensajeError"] = "El artículo solicitado no existe.";
                    return RedirectToAction(nameof(Index));
                }

                // 3. Crear el DTO con el estado INVERTIDO
                var updateDto = new ArticuloDtoIn
                {
                    Id = currentDto.Id,
                    Categoria = currentDto.Categoria,
                    Nombre = currentDto.Nombre,
                    Descripcion = currentDto.Descripcion,
                    Codigo = currentDto.Codigo,
                    Reutilizable = currentDto.Reutilizable,
                    Stock = currentDto.Stock,
                    Activo = !currentDto.Activo
                };

                // 4. Llamar a la API
                await _articuloService.ActualizarArticuloAsync(id, updateDto);

                TempData["MensajeExito"] = $"El artículo se ha {(updateDto.Activo ? "activado" : "desactivado")} correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "No se pudo cambiar el estado: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
