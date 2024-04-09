using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Services.Interfaces;

public interface IHttpService
{
    public Task<Response> SendAsync(Request request, bool withBearer = true);
}