using DentalNova.Core.Dtos;

namespace Proyecto_DentalNova.Models.CompraArticuloViewModel
{
    public class CompraArticuloVM
    {
        public CompraArticuloDtoIn CompraArticulo { get; set; } = new CompraArticuloDtoIn();
        public IEnumerable<ArticuloDto> ArticulosDisponibles { get; set; } = new List<ArticuloDto>();
    }
}
