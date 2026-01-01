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
    public class CompraArticuloRepository : ICompraArticuloRepository
    {
        private readonly ApplicationDbContext _context;

        public CompraArticuloRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<CompraArticulo> ObtenerQueryableParaFiltro()
        {
            return _context.CompraArticulos
                .Include(c => c.Articulo)
                .AsNoTracking();
        }

        public async Task<CompraArticulo> ObtenerPorIdAsync(int id)
        {
            return await _context.CompraArticulos
                .Include(c => c.Articulo)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AgregarAsync(CompraArticulo compraArticulo)
        {
            await _context.CompraArticulos.AddAsync(compraArticulo);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(CompraArticulo compraArticulo)
        {
            _context.CompraArticulos.Update(compraArticulo);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var compra = await _context.CompraArticulos.FindAsync(id);
            if (compra != null)
            {
                _context.CompraArticulos.Remove(compra);
                await _context.SaveChangesAsync();
            }
        }
    }
}
