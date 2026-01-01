using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DentalNova.Core.Repository.Entities.Enumerables;

namespace DentalNova.Core.Dtos
{
    public class CompraArticuloDto
    {
        public int Id { get; set; }
        public int ArticuloId { get; set; }
        public string ArticuloNombre { get; set; }
        public string ArticuloCodigo { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime FechaCompra { get; set; }
        public MetodoPago MetodoPago { get; set; }
        public string Proveedor { get; set; }
    }

    public class CompraArticuloDtoIn
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El artículo es obligatorio.")]
        [DisplayName("Artículo")]
        public int ArticuloId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser al menos 1.")]
        [DisplayName("Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Column(TypeName = "decimal(10, 2)")]
        [Range(0.01, 9_999_999.99, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        [DisplayName("Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        [DisplayName("Subtotal")]
        public decimal Subtotal { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria.")]
        [DataType(DataType.Date)]
        [DisplayName("Fecha de Compra")]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "El método de pago es obligatorio.")]
        [DisplayName("Método de Pago")]
        public MetodoPago MetodoPago { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [StringLength(200)]
        [DisplayName("Proveedor")]
        public string Proveedor { get; set; }
    }

    public class CompraArticuloFilterDto : PaginacionDto
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
