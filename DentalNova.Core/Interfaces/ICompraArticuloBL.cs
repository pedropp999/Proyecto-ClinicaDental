using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Interfaces
{
    public interface ICompraArticuloBL
    {
        Task<PaginatedList<CompraArticuloDto>> ObtenerListaPaginadaAsync(CompraArticuloFilterDto filtro);
        Task<CompraArticuloDto> ObtenerPorIdAsync(int id);
        Task CrearCompraArticuloAsync(CompraArticuloDtoIn dto);
        Task ActualizarCompraArticuloAsync(int id, CompraArticuloDtoIn dto);
        Task EliminarCompraArticuloAsync(int id);
    }
}
