using DentalNova.Core.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Repository.Interfaces
{
    public interface IRecordatorioRepository
    {
        // --- CRUD Admin ---
        IQueryable<Recordatorio> ObtenerQueryableParaFiltro();
        Task<Recordatorio> ObtenerPorIdAsync(int id);
        Task<IEnumerable<Recordatorio>> ObtenerPendientesAsync();
        Task AgregarAsync(Recordatorio recordatorio);
        Task ActualizarAsync(Recordatorio recordatorio);
        Task EliminarAsync(int id);
        Task MarcarComoEnviadoAsync(int id);
    }
}
