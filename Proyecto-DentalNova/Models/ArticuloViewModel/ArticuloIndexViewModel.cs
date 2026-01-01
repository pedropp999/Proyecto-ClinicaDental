using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;

namespace Proyecto_DentalNova.Models.ArticuloViewModel
{
    public class ArticuloIndexViewModel
    {
        public ArticuloFilterViewModel Filtro { get; set; } = new ArticuloFilterViewModel();
        public PaginatedList<ArticuloDto> Resultados { get; set; }
    }
}
