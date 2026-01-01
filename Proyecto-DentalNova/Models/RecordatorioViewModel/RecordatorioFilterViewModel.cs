using DentalNova.Core.Dtos;

namespace Proyecto_DentalNova.Models.RecordatorioViewModel
{
    public class RecordatorioFilterViewModel : PaginacionDto
    {
        public int? Id { get; set; }
        public int? CitaId { get; set; }
        public bool? Enviado { get; set; }
        public DateTime? FechaEnvioDesde { get; set; }
        public DateTime? FechaEnvioHasta { get; set; }
        public string? MensajeLike { get; set; }
    }
}
