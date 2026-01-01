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
    public class ArticuloBL : IArticuloBL
    {
        private readonly IRepository _repositorio;

        public ArticuloBL(IRepository repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<IEnumerable<ArticuloDto>> ObtenerCatalogoAsync()
        {
            // 1. Llama a la capa de repositorio
            var articulos = await _repositorio.Articulo.ObtenerTodosActivosAsync();

            // 2. Mapea las Entidades a DTOs
            return articulos.Select(a => a.ToDto());
        }

        public async Task<PaginatedList<ArticuloDto>> ObtenerListaPaginadaAsync(ArticuloFilterDto filtro)
        {
            var query = _repositorio.Articulo.ObtenerQueryableParaFiltro();

            if (filtro.Id.HasValue) query = query.Where(a => a.Id == filtro.Id.Value);
            if (filtro.Categoria.HasValue) query = query.Where(a => a.Categoria == filtro.Categoria.Value);
            if (!string.IsNullOrWhiteSpace(filtro.NombreLike)) query = query.Where(a => a.Nombre.Contains(filtro.NombreLike));
            if (!string.IsNullOrWhiteSpace(filtro.CodigoLike)) query = query.Where(a => a.Codigo.Contains(filtro.CodigoLike));
            if (filtro.Reutilizable.HasValue) query = query.Where(a => a.Reutilizable == filtro.Reutilizable.Value);
            if (filtro.StockMin.HasValue) query = query.Where(a => a.Stock >= filtro.StockMin.Value);
            if (filtro.StockMax.HasValue) query = query.Where(a => a.Stock <= filtro.StockMax.Value);
            if (filtro.Activo.HasValue) query = query.Where(a => a.Activo == filtro.Activo.Value);

            query = query.OrderBy(a => a.Nombre);

            var pagedEntities = await PaginatedList<Articulo>.CreateAsync(query, filtro.Page, filtro.PageSize);
            var dtos = pagedEntities.Select(a => a.ToDto()).ToList();

            return PaginatedList<ArticuloDto>.Create(dtos, pagedEntities.TotalCount, pagedEntities.PageIndex, filtro.PageSize);
        }

        public async Task<ArticuloDto> ObtenerPorIdAdminAsync(int id)
        {
            var articulo = await _repositorio.Articulo.ObtenerPorIdAsync(id);
            return articulo?.ToDto();
        }

        public async Task CrearArticuloAdminAsync(ArticuloDtoIn dto)
        {
            if (await _repositorio.Articulo.ExisteCodigoAsync(dto.Codigo))
                throw new InvalidOperationException("Ya existe un artículo con este código.");

            var nuevo = new Articulo();
            nuevo.MapFromDto(dto);
            await _repositorio.Articulo.AgregarAsync(nuevo);
        }

        public async Task ActualizarArticuloAdminAsync(int id, ArticuloDtoIn dto)
        {
            if (await _repositorio.Articulo.ExisteCodigoAsync(dto.Codigo, id))
                throw new InvalidOperationException("Ya existe otro artículo con este código.");

            var existente = await _repositorio.Articulo.ObtenerPorIdAsync(id);
            if (existente == null) return;

            existente.MapFromDto(dto);
            await _repositorio.Articulo.ActualizarAsync(existente);
        }

        public async Task EliminarArticuloAsync(int id)
        {
            await _repositorio.Articulo.EliminarAsync(id);
        }
    }
}
