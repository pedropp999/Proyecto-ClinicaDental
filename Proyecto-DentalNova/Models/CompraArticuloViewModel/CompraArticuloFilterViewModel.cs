using DentalNova.Core.Dtos;
using static DentalNova.Core.Repository.Entities.Enumerables;

namespace Proyecto_DentalNova.Models.CompraArticuloViewModel
{
    public class CompraArticuloFilterViewModel : PaginacionDto
    {
        public int? Id { get; set; }
        public int? ArticuloId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public MetodoPago? MetodoPago { get; set; }
        public string? ProveedorLike { get; set; }
        public decimal? MontoMin { get; set; }
        public decimal? MontoMax { get; set; }
    }
}
