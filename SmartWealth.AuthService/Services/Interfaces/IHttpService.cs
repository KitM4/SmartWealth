using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Services.Interfaces;

public interface IHttpService
{
    public Task<Response> SendAsync(Request request, bool withBearer = true);
}