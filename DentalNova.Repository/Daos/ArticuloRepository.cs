using DentalNova.Core.Repository.Entities;
using DentalNova.Core.Repository.Interfaces;
using DentalNova.Repository.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Repository.Daos
{
    public class ArticuloRepository : IArticuloRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticuloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articulo>> ObtenerTodosActivosAsync()
        {
            // Llama al DbSet "Articulos"
            // y filtra por la propiedad "Activo"
            return await _context.Articulos
                                 .Where(a => a.Activo)
                                 .ToListAsync();
        }

        public IQueryable<Articulo> ObtenerQueryableParaFiltro()
        {
            return _context.Articulos.AsNoTracking();
        }

        public async Task<Articulo> ObtenerPorIdAsync(int id)
        {
            return await _context.Articulos.FindAsync(id);
        }

        public async Task AgregarAsync(Articulo articulo)
        {
            await _context.Articulos.AddAsync(articulo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Articulo articulo)
        {
            _context.Articulos.Update(articulo);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExisteCodigoAsync(string codigo, int? idExcluir = null)
        {
            var query = _context.Articulos.AsNoTracking();
            if (idExcluir.HasValue)
            {
                query = query.Where(a => a.Id != idExcluir.Value);
            }
            return await query.AnyAsync(a => a.Codigo == codigo);
        }
    }
}
