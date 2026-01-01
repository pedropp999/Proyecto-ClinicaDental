using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace Proyecto_DentalNova.Services
{
    public class RecordatorioServiceApi : IRecordatorioService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecordatorioServiceApi(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResultDto<RecordatorioDto>> ObtenerRecordatoriosAsync(RecordatorioFilterDto filtro)
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["Page"] = filtro.Page.ToString(),
                ["PageSize"] = filtro.PageSize.ToString()
            };

            if (filtro.Id.HasValue) queryParams.Add("Id", filtro.Id.ToString());
            if (filtro.CitaId.HasValue) queryParams.Add("CitaId", filtro.CitaId.ToString());
            if (filtro.Enviado.HasValue) queryParams.Add("Enviado", filtro.Enviado.ToString());
            if (filtro.FechaEnvioDesde.HasValue) queryParams.Add("FechaEnvioDesde", filtro.FechaEnvioDesde.Value.ToString("yyyy-MM-dd"));
            if (filtro.FechaEnvioHasta.HasValue) queryParams.Add("FechaEnvioHasta", filtro.FechaEnvioHasta.Value.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrWhiteSpace(filtro.MensajeLike)) queryParams.Add("MensajeLike", filtro.MensajeLike);

            var url = QueryHelpers.AddQueryString("api/Recordatorios", queryParams);

            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<PagedResultDto<RecordatorioDto>>(url);
        }

        public async Task<RecordatorioDto> ObtenerRecordatorioPorIdAsync(int id)
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<RecordatorioDto>($"api/Recordatorios/{id}");
        }

        public async Task<IEnumerable<RecordatorioDto>> ObtenerRecordatoriosPendientesAsync()
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<IEnumerable<RecordatorioDto>>("api/Recordatorios/pendientes");
        }

        public async Task CrearRecordatorioAsync(RecordatorioDtoIn dto)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/Recordatorios", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var mensaje = error != null && error.ContainsKey("mensaje") ? error["mensaje"] : "Error al crear recordatorio";
                throw new HttpRequestException(mensaje);
            }
        }

        public async Task ActualizarRecordatorioAsync(int id, RecordatorioDtoIn dto)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/Recordatorios/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task EliminarRecordatorioAsync(int id)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/Recordatorios/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task MarcarComoEnviadoAsync(int id)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PatchAsync($"api/Recordatorios/{id}/marcar-enviado", null);
            response.EnsureSuccessStatusCode();
        }

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
    }
}
