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
    public class RecordatorioRepository : IRecordatorioRepository
    {
        private readonly ApplicationDbContext _context;

        public RecordatorioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Recordatorio> ObtenerQueryableParaFiltro()
        {
            return _context.Recordatorios
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                .AsNoTracking();
        }

        public async Task<Recordatorio> ObtenerPorIdAsync(int id)
        {
            return await _context.Recordatorios
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Recordatorio>> ObtenerPendientesAsync()
        {
            return await _context.Recordatorios
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                .Where(r => !r.Enviado)
                .ToListAsync();
        }

        public async Task AgregarAsync(Recordatorio recordatorio)
        {
            await _context.Recordatorios.AddAsync(recordatorio);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Recordatorio recordatorio)
        {
            _context.Recordatorios.Update(recordatorio);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var recordatorio = await _context.Recordatorios.FindAsync(id);
            if (recordatorio != null)
            {
                _context.Recordatorios.Remove(recordatorio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarcarComoEnviadoAsync(int id)
        {
            var recordatorio = await _context.Recordatorios.FindAsync(id);
            if (recordatorio != null)
            {
                recordatorio.Enviado = true;
                recordatorio.FechaEnvio = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}
