using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using SmartWealth.TransactionService.Services.Interfaces;
using SmartWealth.TransactionService.Utilities.Enums;
using SmartWealth.TransactionService.ViewModels.DTO;

namespace SmartWealth.TransactionService.Services;

public class HttpService(IHttpClientFactory httpClientFactory) : IHttpService
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly Response _response = new();

    public async Task<Response> SendAsync(Request request, bool withBearer = true)
    {
        try
        {
            using HttpClient httpClient = _httpClientFactory.CreateClient("TransactionAPI");
            using HttpRequestMessage requestMessage = new();

            PrepareContentType(request, requestMessage);
            PrepareData(request, requestMessage);
            PrepareApiType(request, requestMessage);

            requestMessage.RequestUri = new(request.Url);
            if (withBearer)
                requestMessage.Headers.Add("Authorization", $"Bearer {request.AccessToken}");

            using HttpResponseMessage apiResponse = await httpClient.SendAsync(requestMessage);
            _response.Data = await apiResponse.Content.ReadAsStringAsync();
            return _response;
        }
        catch (Exception exception)
        {
            _response.IsSuccess = false;
            _response.Message = exception.Message;

            return _response;
        }
    }

    private static void PrepareApiType(Request request, HttpRequestMessage message)
    {
        message.Method = request.ApiType switch
        {
            ApiType.POST => HttpMethod.Post,
            ApiType.DELETE => HttpMethod.Delete,
            ApiType.PUT => HttpMethod.Put,
            _ => HttpMethod.Get,
        };
    }

    private static void PrepareContentType(Request request, HttpRequestMessage message)
    {
        switch (request.ContentType)
        {
            case ContentType.Json:
                message.Headers.Add("Accept", "application/json");
                break;
            case ContentType.MultipartFormData:
                message.Headers.Add("Accept", "*/*");
                break;
        }
    }

    private static void PrepareData(Request request, HttpRequestMessage message)
    {
        if (request.ContentType == ContentType.MultipartFormData && request.Data != null)
        {
            MultipartFormDataContent content = [];
            foreach (PropertyInfo propert in request.Data.GetType().GetProperties())
            {
                object? value = propert.GetValue(request.Data);
                if (value is FormFile file)
                {
                    if (file != null)
                    {
                        content.Add(new StreamContent(file.OpenReadStream()), propert.Name, file.FileName);
                    }
                }
                else
                {
                    content.Add(new StringContent(value?.ToString() ?? string.Empty), propert.Name);
                }
            }
            message.Content = content;
        }
        else
        {
            if (request.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
            }
        }
    }
}