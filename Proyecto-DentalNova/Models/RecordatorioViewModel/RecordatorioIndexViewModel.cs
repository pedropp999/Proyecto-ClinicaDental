using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;

namespace Proyecto_DentalNova.Models.RecordatorioViewModel
{
    public class RecordatorioIndexViewModel
    {
        public RecordatorioFilterViewModel Filtro { get; set; } = new RecordatorioFilterViewModel();
        public PaginatedList<RecordatorioDto> Resultados { get; set; }
    }
}
