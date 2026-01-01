using DentalNova.Core.Dtos;
using static DentalNova.Core.Repository.Entities.Enumerables;

namespace Proyecto_DentalNova.Models.ArticuloViewModel
{
    public class ArticuloFilterViewModel : PaginacionDto
    {
        public int? Id { get; set; }
        public Categoria? Categoria { get; set; }
        public string? NombreLike { get; set; }
        public string? CodigoLike { get; set; }
        public bool? Reutilizable { get; set; }
        public int? StockMin { get; set; }
        public int? StockMax { get; set; }
        public bool? Activo { get; set; }
    }
}
