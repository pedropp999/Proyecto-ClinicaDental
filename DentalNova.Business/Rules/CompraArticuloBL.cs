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
    public class CompraArticuloBL : ICompraArticuloBL
    {
        private readonly IRepository _repository;

        public CompraArticuloBL(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<CompraArticuloDto>> ObtenerListaPaginadaAsync(CompraArticuloFilterDto filtro)
        {
            var query = _repository.CompraArticulo.ObtenerQueryableParaFiltro();

            if (filtro.Id.HasValue) query = query.Where(c => c.Id == filtro.Id.Value);
            if (filtro.ArticuloId.HasValue) query = query.Where(c => c.Articulo.Id == filtro.ArticuloId.Value);
            if (filtro.FechaDesde.HasValue) query = query.Where(c => c.FechaCompra >= filtro.FechaDesde.Value);
            if (filtro.FechaHasta.HasValue) query = query.Where(c => c.FechaCompra <= filtro.FechaHasta.Value);
            if (filtro.MetodoPago.HasValue) query = query.Where(c => c.MetodoPago == filtro.MetodoPago.Value);
            if (!string.IsNullOrWhiteSpace(filtro.ProveedorLike)) query = query.Where(c => c.Proveedor.Contains(filtro.ProveedorLike));
            if (filtro.MontoMin.HasValue) query = query.Where(c => c.Subtotal >= filtro.MontoMin.Value);
            if (filtro.MontoMax.HasValue) query = query.Where(c => c.Subtotal <= filtro.MontoMax.Value);

            query = query.OrderByDescending(c => c.FechaCompra);

            var pagedEntities = await PaginatedList<CompraArticulo>.CreateAsync(query, filtro.Page, filtro.PageSize);
            var dtos = pagedEntities.Select(c => c.ToDto()).ToList();

            return PaginatedList<CompraArticuloDto>.Create(dtos, pagedEntities.TotalCount, pagedEntities.PageIndex, filtro.PageSize);
        }

        public async Task<CompraArticuloDto> ObtenerPorIdAsync(int id)
        {
            var compra = await _repository.CompraArticulo.ObtenerPorIdAsync(id);
            return compra?.ToDto();
        }

        public async Task CrearCompraArticuloAsync(CompraArticuloDtoIn dto)
        {
            var articulo = await _repository.Articulo.ObtenerPorIdAsync(dto.ArticuloId);
            if (articulo == null)
                throw new InvalidOperationException("El artículo especificado no existe.");

            var nueva = new CompraArticulo();
            nueva.MapFromDto(dto);
            nueva.Articulo = articulo;

            // Actualizar el stock del artículo
            articulo.Stock += dto.Cantidad;
            
            await _repository.CompraArticulo.AgregarAsync(nueva);
            await _repository.Articulo.ActualizarAsync(articulo);
        }

        public async Task ActualizarCompraArticuloAsync(int id, CompraArticuloDtoIn dto)
        {
            var existente = await _repository.CompraArticulo.ObtenerPorIdAsync(id);
            if (existente == null) return;

            var articulo = await _repository.Articulo.ObtenerPorIdAsync(dto.ArticuloId);
            if (articulo == null)
                throw new InvalidOperationException("El artículo especificado no existe.");

            // Revertir el stock anterior
            var articuloAnterior = await _repository.Articulo.ObtenerPorIdAsync(existente.Articulo.Id);
            if (articuloAnterior != null)
            {
                articuloAnterior.Stock -= existente.Cantidad;
                await _repository.Articulo.ActualizarAsync(articuloAnterior);
            }

            // Aplicar el nuevo stock
            articulo.Stock += dto.Cantidad;

            existente.MapFromDto(dto);
            existente.Articulo = articulo;

            await _repository.CompraArticulo.ActualizarAsync(existente);
            await _repository.Articulo.ActualizarAsync(articulo);
        }

        public async Task EliminarCompraArticuloAsync(int id)
        {
            var compra = await _repository.CompraArticulo.ObtenerPorIdAsync(id);
            if (compra != null)
            {
                // Revertir el stock
                var articulo = await _repository.Articulo.ObtenerPorIdAsync(compra.Articulo.Id);
                if (articulo != null)
                {
                    articulo.Stock -= compra.Cantidad;
                    await _repository.Articulo.ActualizarAsync(articulo);
                }

                await _repository.CompraArticulo.EliminarAsync(id);
            }
        }
    }
}
