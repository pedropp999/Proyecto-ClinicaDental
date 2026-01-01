using DentalNova.Core.Dtos;
using DentalNova.Core.Interfaces;
using System.Net.Http.Headers;

namespace Proyecto_DentalNova.Services
{
    public class HorarioOdontologoServiceApi : IHorarioOdontologoService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HorarioOdontologoServiceApi(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<HorarioOdontologoDto>> ObtenerPorOdontologoAsync(int odontologoId)
        {
            await AddAuthorizationHeader();
            // GET api/HorariosOdontologos/odontologo/{odontologoId}
            return await _httpClient.GetFromJsonAsync<List<HorarioOdontologoDto>>($"api/HorariosOdontologos/odontologo/{odontologoId}");
        }

        public async Task<HorarioOdontologoDto> ObtenerPorIdAsync(int id)
        {
            await AddAuthorizationHeader();
            // GET api/HorariosOdontologos/{id}
            return await _httpClient.GetFromJsonAsync<HorarioOdontologoDto>($"api/HorariosOdontologos/{id}");
        }

        public async Task CrearAsync(HorarioOdontologoDtoIn dto)
        {
            await AddAuthorizationHeader();
            // POST api/HorariosOdontologos
            var response = await _httpClient.PostAsJsonAsync("api/HorariosOdontologos", dto);

            if (!response.IsSuccessStatusCode)
            {
                await ManejarErrorApi(response);
            }
        }

        public async Task ActualizarAsync(int id, HorarioOdontologoDtoIn dto)
        {
            await AddAuthorizationHeader();
            // PUT api/HorariosOdontologos/{id}
            var response = await _httpClient.PutAsJsonAsync($"api/HorariosOdontologos/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                await ManejarErrorApi(response);
            }
        }

        public async Task EliminarAsync(int id)
        {
            await AddAuthorizationHeader();
            // DELETE api/HorariosOdontologos/{id}
            var response = await _httpClient.DeleteAsync($"api/HorariosOdontologos/{id}");

            if (!response.IsSuccessStatusCode)
            {
                await ManejarErrorApi(response);
            }
        }

        // --- Helpers Privados ---

        private async Task AddAuthorizationHeader()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user != null && user.Identity.IsAuthenticated)
            {
                var token = user.FindFirst("JWT_TOKEN")?.Value;
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
        }

        // Método auxiliar para no repetir la lógica de lectura de errores
        private async Task ManejarErrorApi(HttpResponseMessage response)
        {
            var mensaje = "Error en la operación";
            try
            {
                // Intentamos leer el JSON de error estandarizado: { "Mensaje": "..." }
                // Nota: Usamos CaseInsensitive para leer 'Mensaje' o 'mensaje'
                var errorDict = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                if (errorDict != null && errorDict.TryGetValue("mensaje", out var msg))
                {
                    mensaje = msg;
                }
                else if (errorDict != null && errorDict.TryGetValue("Mensaje", out var msgMayus)) // Por si acaso viene en mayúscula
                {
                    mensaje = msgMayus;
                }
            }
            catch
            {
                // Si no es JSON o falla la lectura, usamos el ReasonPhrase
                mensaje = response.ReasonPhrase ?? "Error desconocido";
            }

            throw new HttpRequestException(mensaje);
        }
    }
}
