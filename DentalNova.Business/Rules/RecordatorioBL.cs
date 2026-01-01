using DentalNova.Business.Helpers;
using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using DentalNova.Core.Repository.Entities;
using DentalNova.Core.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Business.Rules
{
    public class RecordatorioBL : IRecordatorioBL
    {
        private readonly IRepository _repository;

        public RecordatorioBL(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<RecordatorioDto>> ObtenerListaPaginadaAsync(RecordatorioFilterDto filtro)
        {
            var query = _repository.Recordatorio.ObtenerQueryableParaFiltro();

            if (filtro.Id.HasValue) query = query.Where(r => r.Id == filtro.Id.Value);
            if (filtro.CitaId.HasValue) query = query.Where(r => r.Cita.Id == filtro.CitaId.Value);
            if (filtro.Enviado.HasValue) query = query.Where(r => r.Enviado == filtro.Enviado.Value);
            if (filtro.FechaEnvioDesde.HasValue) query = query.Where(r => r.FechaEnvio >= filtro.FechaEnvioDesde.Value);
            if (filtro.FechaEnvioHasta.HasValue) query = query.Where(r => r.FechaEnvio <= filtro.FechaEnvioHasta.Value);
            if (!string.IsNullOrWhiteSpace(filtro.MensajeLike)) query = query.Where(r => r.Mensaje.Contains(filtro.MensajeLike));

            query = query.OrderByDescending(r => r.FechaEnvio);

            var pagedEntities = await PaginatedList<Recordatorio>.CreateAsync(query, filtro.Page, filtro.PageSize);
            var dtos = pagedEntities.Select(r => r.ToDto()).ToList();

            return PaginatedList<RecordatorioDto>.Create(dtos, pagedEntities.TotalCount, pagedEntities.PageIndex, filtro.PageSize);
        }

        public async Task<RecordatorioDto> ObtenerPorIdAsync(int id)
        {
            var recordatorio = await _repository.Recordatorio.ObtenerPorIdAsync(id);
            return recordatorio?.ToDto();
        }

        public async Task<IEnumerable<RecordatorioDto>> ObtenerPendientesAsync()
        {
            var recordatorios = await _repository.Recordatorio.ObtenerPendientesAsync();
            return recordatorios.Select(r => r.ToDto());
        }

        public async Task CrearRecordatorioAsync(RecordatorioDtoIn dto)
        {
            var cita = await _repository.Cita.ObtenerPorIdAsync(dto.CitaId);
            if (cita == null)
                throw new InvalidOperationException("La cita especificada no existe.");

            var nuevo = new Recordatorio();
            nuevo.MapFromDto(dto);
            nuevo.Cita = cita;

            await _repository.Recordatorio.AgregarAsync(nuevo);
        }

        public async Task ActualizarRecordatorioAsync(int id, RecordatorioDtoIn dto)
        {
            var existente = await _repository.Recordatorio.ObtenerPorIdAsync(id);
            if (existente == null) return;

            var cita = await _repository.Cita.ObtenerPorIdAsync(dto.CitaId);
            if (cita == null)
                throw new InvalidOperationException("La cita especificada no existe.");

            existente.MapFromDto(dto);
            existente.Cita = cita;

            await _repository.Recordatorio.ActualizarAsync(existente);
        }

        public async Task EliminarRecordatorioAsync(int id)
        {
            await _repository.Recordatorio.EliminarAsync(id);
        }

        public async Task MarcarComoEnviadoAsync(int id)
        {
            await _repository.Recordatorio.MarcarComoEnviadoAsync(id);
        }
    }
}
