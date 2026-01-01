using DentalNova.Core.Dtos;
using DentalNova.Core.Helpers;
using DentalNova.Core.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace Proyecto_DentalNova.Services
{
    public class ArticuloServiceApi : IArticuloService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticuloServiceApi(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PagedResultDto<ArticuloDto>> ObtenerArticulosAdminAsync(ArticuloFilterDto filtro)
        {
            var queryParams = new Dictionary<string, string?>
            {
                ["Page"] = filtro.Page.ToString(),
                ["PageSize"] = filtro.PageSize.ToString()
            };

            if (filtro.Id.HasValue) queryParams.Add("Id", filtro.Id.ToString());
            if (filtro.Categoria.HasValue) queryParams.Add("Categoria", ((int)filtro.Categoria.Value).ToString());
            if (!string.IsNullOrWhiteSpace(filtro.NombreLike)) queryParams.Add("NombreLike", filtro.NombreLike);
            if (!string.IsNullOrWhiteSpace(filtro.CodigoLike)) queryParams.Add("CodigoLike", filtro.CodigoLike);
            if (filtro.Reutilizable.HasValue) queryParams.Add("Reutilizable", filtro.Reutilizable.ToString());
            if (filtro.StockMin.HasValue) queryParams.Add("StockMin", filtro.StockMin.ToString());
            if (filtro.StockMax.HasValue) queryParams.Add("StockMax", filtro.StockMax.ToString());
            if (filtro.Activo.HasValue) queryParams.Add("Activo", filtro.Activo.ToString());

            var url = QueryHelpers.AddQueryString("api/Articulos/admin", queryParams);

            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<PagedResultDto<ArticuloDto>>(url);
        }

        public async Task<ArticuloDto> ObtenerArticuloPorIdAsync(int id)
        {
            await AddAuthorizationHeader();
            return await _httpClient.GetFromJsonAsync<ArticuloDto>($"api/Articulos/admin/{id}");
        }

        public async Task CrearArticuloAsync(ArticuloDtoIn dto)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/Articulos", dto);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var mensaje = error != null && error.ContainsKey("mensaje") ? error["mensaje"] : "Error al crear artículo";
                throw new HttpRequestException(mensaje);
            }
        }

        public async Task ActualizarArticuloAsync(int id, ArticuloDtoIn dto)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/Articulos/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task EliminarArticuloAsync(int id)
        {
            await AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/Articulos/{id}");
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
