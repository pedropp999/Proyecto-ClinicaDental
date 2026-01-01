using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace Proyecto_DentalNova.Services
{
    public class CompraArticuloServiceApi : ICompraArticuloService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompraArticuloServiceApi(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResultDto<CompraArticuloDto>> ObtenerCompraArticulosAsync(CompraArticuloFilterDto filtro)
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["Page"] = filtro.Page.ToString(),
                ["PageSize"] = filtro.PageSize.ToString()
            };

            if (filtro.Id.HasValue) queryParams.Add("Id", filtro.Id.ToString());
            if (filtro.ArticuloId.HasValue) queryParams.Add("ArticuloId", filtro.ArticuloId.ToString());
            if (filtro.FechaDesde.HasValue) queryParams.Add("FechaDesde", filtro.FechaDesde.Value.ToString("yyyy-MM-dd"));
            if (filtro.FechaHasta.HasValue) queryParams.Add("FechaHasta", filtro.FechaHasta.Value.ToString("yyyy-MM-dd"));
            if (filtro.MetodoPago.HasValue) queryParams.Add("MetodoPago", ((int)filtro.MetodoPago.Value).ToString());
            if (!string.IsNullOrWhiteSpace(filtro.ProveedorLike)) queryParams.Add("ProveedorLike", filtro.ProveedorLike);
            if (filtro.MontoMin.HasValue) queryParams.Add("MontoMin", filtro.MontoMin.ToString());
            if (filtro.MontoMax.HasValue) queryParams.Add("MontoMax", filtro.MontoMax.ToString());

            var url = QueryHelpers.AddQueryString("api/CompraArticulos", queryParams);

            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<PagedResultDto<CompraArticuloDto>>(url);
        }

        public async Task<CompraArticuloDto> ObtenerCompraArticuloPorIdAsync(int id)
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<CompraArticuloDto>($"api/CompraArticulos/{id}");
        }

        public async Task<IEnumerable<ArticuloDto>> ObtenerArticulosActivosAsync()
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<IEnumerable<ArticuloDto>>("api/Articulos");
        }

        public async Task CrearCompraArticuloAsync(CompraArticuloDtoIn dto)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/CompraArticulos", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var mensaje = error != null && error.ContainsKey("mensaje") ? error["mensaje"] : "Error al crear compra de artículo";
                throw new HttpRequestException(mensaje);
            }
        }

        public async Task ActualizarCompraArticuloAsync(int id, CompraArticuloDtoIn dto)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/CompraArticulos/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task EliminarCompraArticuloAsync(int id)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/CompraArticulos/{id}");
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
