using SmartWealth.AuthService.ViewModels.DTO;

namespace SmartWealth.AuthService.Services.Interfaces;

public interface IHttpService
{
    public Task<Response> SendAsync(Request request, bool withBearer = true);
}