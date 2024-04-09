using SmartWealth.TransactionService.ViewModels.DTO;

namespace SmartWealth.TransactionService.Services.Interfaces;

public interface IHttpService
{
    public Task<Response> SendAsync(Request request, bool withBearer = true);
}