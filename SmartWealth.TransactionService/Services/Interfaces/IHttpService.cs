using SmartWealth.TransactionService.ViewModels;

namespace SmartWealth.TransactionService.Services.Interfaces;

public interface IHttpService
{
    public Task<Response> SendAsync(Request request, bool withBearer = true);
}