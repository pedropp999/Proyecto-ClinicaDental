using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Interfaces
{
    public interface IArticuloBL
    {
        // Existente (Catálogo público)
        Task<IEnumerable<ArticuloDto>> ObtenerCatalogoAsync();

        // --- NUEVOS (Gestión Admin) ---
        Task<PaginatedList<ArticuloDto>> ObtenerListaPaginadaAsync(ArticuloFilterDto filtro);
        Task<ArticuloDto> ObtenerPorIdAdminAsync(int id);
        Task CrearArticuloAdminAsync(ArticuloDtoIn dto);
        Task ActualizarArticuloAdminAsync(int id, ArticuloDtoIn dto);
        Task EliminarArticuloAsync(int id);
    }
}
