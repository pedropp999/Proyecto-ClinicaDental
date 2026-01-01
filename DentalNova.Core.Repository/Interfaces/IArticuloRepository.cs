using DentalNova.Core.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Repository.Interfaces
{
    public interface IArticuloRepository
    {
        // Existente
        Task<IEnumerable<Articulo>> ObtenerTodosActivosAsync();

        // --- CRUD Admin ---
        IQueryable<Articulo> ObtenerQueryableParaFiltro();
        Task<Articulo> ObtenerPorIdAsync(int id);
        Task AgregarAsync(Articulo articulo);
        Task ActualizarAsync(Articulo articulo);
        Task EliminarAsync(int id);

        // Validación
        Task<bool> ExisteCodigoAsync(string codigo, int? idExcluir = null);
    }
}
