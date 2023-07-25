using APIREST.Models;
using Newtonsoft.Json;

namespace APIREST.Services
{
    public class ReceitaWSService : IReceitaWSService
    {
        private readonly HttpClient _httpClient;

        public ReceitaWSService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ReceitaWSModel> ObterDadosReceitaWS(string cnpj)
        {
            var response = await _httpClient.GetAsync($"https://receitaws.com.br/v1/cnpj/{cnpj}");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ReceitaWSModel>(jsonString);
            }

            var errorMessage = $"Falha ao acessar dados da ReceitaWS: {response.StatusCode}";
            throw new HttpRequestException(errorMessage);
        }
    }
}