using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalNova.Core.Dtos
{
    public class RecordatorioDto
    {
        public int Id { get; set; }
        public int CitaId { get; set; }
        public string PacienteNombre { get; set; }
        public DateTime FechaCita { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public string Mensaje { get; set; }
        public bool Enviado { get; set; }
    }

    public class RecordatorioDtoIn
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cita es obligatoria.")]
        [DisplayName("Cita")]
        public int CitaId { get; set; }

        [DisplayName("Fecha de Envío")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaEnvio { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Mensaje")]
        public string Mensaje { get; set; }

        [DisplayName("Enviado")]
        public bool Enviado { get; set; }
    }

    public class RecordatorioFilterDto : PaginacionDto
    {
        public int? Id { get; set; }
        public int? CitaId { get; set; }
        public bool? Enviado { get; set; }
        public DateTime? FechaEnvioDesde { get; set; }
        public DateTime? FechaEnvioHasta { get; set; }
        public string? MensajeLike { get; set; }
    }
}
