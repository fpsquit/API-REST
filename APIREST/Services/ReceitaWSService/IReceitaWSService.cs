using APIREST.Models;

namespace APIREST.Services
{
    public interface IReceitaWSService
    {
        Task<ReceitaWSModel> ObterDadosReceitaWS(string cnpj);
        
    }
}