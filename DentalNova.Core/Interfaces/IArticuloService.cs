using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Interfaces
{
    public interface IArticuloService
    {
        // CRUD de Admin
        Task<PagedResultDto<ArticuloDto>> ObtenerArticulosAdminAsync(ArticuloFilterDto filtro);
        Task<ArticuloDto> ObtenerArticuloPorIdAsync(int id);
        Task CrearArticuloAsync(ArticuloDtoIn dto);
        Task ActualizarArticuloAsync(int id, ArticuloDtoIn dto);
        Task EliminarArticuloAsync(int id);
    }
}
