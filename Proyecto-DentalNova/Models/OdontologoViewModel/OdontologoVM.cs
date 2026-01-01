using DentalNova.Core.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto_DentalNova.Models.OdontologoViewModel
{
    public class OdontologoVM
    {
        // --- Para Create / Edit ---
        // Contiene la instancia del Odontólogo que se está creando o editando.
        public OdontologoDtoIn Odontologo { get; set; } = new();

        // Lista para poblar el DropDownList de Usuarios.
        public IEnumerable<SelectListItem> UsuariosDisponibles { get; set; } = new List<SelectListItem>();

        // Propiedad para POBLAR el <select>.
        public IEnumerable<SelectListItem> TodasLasEspecialidades { get; set; } = new List<SelectListItem>();

        // Propiedad para recibir los IDs seleccionados del formulario.
        public int[]? EspecialidadesSeleccionadasIds { get; set; }

        // --- NUEVO: Para la Vista Details (Gestión de Horarios) ---

        // Propiedad para mostrar los datos completos (DTO de Salida) en la vista Details
        public OdontologoDto? OdontologoVisual { get; set; }

        // Lista de horarios asociados al odontólogo.
        public List<HorarioOdontologoDto> Horarios { get; set; } = new();

        // Propiedad auxiliar para el formulario de creación dentro del Modal
        public HorarioOdontologoDtoIn NuevoHorario { get; set; } = new();
    }
    //public class OdontologoVM
    //{
    //    // Contiene la instancia del Odontólogo que se está creando o editando.
    //    public OdontologoDtoIn Odontologo { get; set; } = new();

    //    // Lista para poblar el DropDownList de Usuarios.
    //    // Se llenará solo con usuarios activos que no sean ni pacientes ni otros odontólogos.
    //    public IEnumerable<SelectListItem> UsuariosDisponibles { get; set; } = new List<SelectListItem>();

    //    // Propiedad para POBLAR el <select>.
    //    public IEnumerable<SelectListItem> TodasLasEspecialidades { get; set; } = new List<SelectListItem>();

    //    // Propiedad para recibir los IDs seleccionados del formulario.
    //    public int[]? EspecialidadesSeleccionadasIds { get; set; }

    //    // Lista de horarios asociados al odontólogo.
    //    public List<HorarioOdontologoDto> Horarios { get; set; }

    //    // Propiedad auxiliar para el formulario de creación dentro del Modal
    //    public HorarioOdontologoDtoIn NuevoHorario { get; set; }
    //}
}
