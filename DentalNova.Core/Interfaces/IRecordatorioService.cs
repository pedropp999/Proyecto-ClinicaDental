using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Interfaces
{
    public interface IRecordatorioService
    {
        Task<PagedResultDto<RecordatorioDto>> ObtenerRecordatoriosAsync(RecordatorioFilterDto filtro);
        Task<RecordatorioDto> ObtenerRecordatorioPorIdAsync(int id);
        Task<IEnumerable<RecordatorioDto>> ObtenerRecordatoriosPendientesAsync();
        Task CrearRecordatorioAsync(RecordatorioDtoIn dto);
        Task ActualizarRecordatorioAsync(int id, RecordatorioDtoIn dto);
        Task EliminarRecordatorioAsync(int id);
        Task MarcarComoEnviadoAsync(int id);
    }
}
