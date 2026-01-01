using DentalNova.Core.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DentalNova.Core.Repository.Entities.Enumerables;

namespace DentalNova.Core.Repository.Interfaces
{
    public interface IHorarioOdontologoRepository
    {

        // --- Consultas Específicas ---
        // Obtiene todos los horarios de un odontólogo específico (incluyendo inactivos para gestión)
        Task<IEnumerable<HorarioOdontologo>> ObtenerPorOdontologoIdAsync(int odontologoId);

        // Obtiene un horario específico con su relación de Odontólogo cargada
        Task<HorarioOdontologo> ObtenerPorIdAsync(int id);

        // Para encontrar odontólogos disponibles en un momento dado
        Task<List<HorarioOdontologo>> ObtenerHorariosDisponiblesAsync(DiaSemana dia, TimeSpan horaInicio, TimeSpan horaFin);

        // --- CRUD Estándar (Espejo de TratamientoRepository) ---
        IQueryable<HorarioOdontologo> ObtenerQueryableParaFiltro();
        Task AgregarAsync(HorarioOdontologo horario);
        Task ActualizarAsync(HorarioOdontologo horario);
        Task EliminarAsync(int id);

        // --- Validaciones ---
        // Verifica si ya existe un horario que choque con el rango propuesto
        Task<bool> ExisteSolapamientoAsync(int odontologoId, DiaSemana dia, TimeSpan horaInicio, TimeSpan horaFin, int? idExcluir = null);
    }
}
