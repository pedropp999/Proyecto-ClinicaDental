using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;

namespace Proyecto_DentalNova.Models.CompraArticuloViewModel
{
    public class CompraArticuloIndexViewModel
    {
        public CompraArticuloFilterViewModel Filtro { get; set; } = new CompraArticuloFilterViewModel();
        public PaginatedList<CompraArticuloDto> Resultados { get; set; }
    }
}
