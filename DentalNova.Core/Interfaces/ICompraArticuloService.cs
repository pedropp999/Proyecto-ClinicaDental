using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Interfaces
{
    public interface ICompraArticuloService
    {
        Task<PagedResultDto<CompraArticuloDto>> ObtenerCompraArticulosAsync(CompraArticuloFilterDto filtro);
        Task<CompraArticuloDto> ObtenerCompraArticuloPorIdAsync(int id);
        Task CrearCompraArticuloAsync(CompraArticuloDtoIn dto);
        Task ActualizarCompraArticuloAsync(int id, CompraArticuloDtoIn dto);
        Task EliminarCompraArticuloAsync(int id);
        Task<IEnumerable<ArticuloDto>> ObtenerArticulosActivosAsync();
    }
}
