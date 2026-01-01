using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Interfaces
{
    public interface IRecordatorioBL
    {
        Task<PaginatedList<RecordatorioDto>> ObtenerListaPaginadaAsync(RecordatorioFilterDto filtro);
        Task<RecordatorioDto> ObtenerPorIdAsync(int id);
        Task<IEnumerable<RecordatorioDto>> ObtenerPendientesAsync();
        Task CrearRecordatorioAsync(RecordatorioDtoIn dto);
        Task ActualizarRecordatorioAsync(int id, RecordatorioDtoIn dto);
        Task EliminarRecordatorioAsync(int id);
        Task MarcarComoEnviadoAsync(int id);
    }
}
