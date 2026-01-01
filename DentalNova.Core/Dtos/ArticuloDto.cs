using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DentalNova.Core.Repository.Entities.Enumerables;

namespace DentalNova.Core.Dtos
{
    public class ArticuloDto
    {
        public int Id { get; set; }
        public Categoria Categoria { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string Codigo { get; set; }
        public bool Reutilizable { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
    }

    public class ArticuloDtoIn
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [DisplayName("Categoría")]
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100)]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El código es obligatorio.")]
        [StringLength(50)]
        [DisplayName("Código")]
        public string Codigo { get; set; }

        [DisplayName("Reutilizable")]
        public bool Reutilizable { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un valor positivo.")]
        [DisplayName("Stock")]
        public int Stock { get; set; }

        [DisplayName("Activo")]
        public bool Activo { get; set; }
    }

    public class ArticuloFilterDto : PaginacionDto
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
