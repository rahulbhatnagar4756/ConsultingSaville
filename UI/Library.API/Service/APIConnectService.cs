using DocumentFormat.OpenXml.Wordprocessing;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Library.API.Service;

public interface IAPIConnectService
{
    Task<T?> DeleteAsync<T>(string token, string urlEndPoint, string body);
    Task<T?> GetAsync<T>(string token, string urlendpoint);
    Task<T?> PostAsync<T, U>(string token, string urlendpoint, U body);
    Task<T?> PostAsync<T>(string token, string urlendpoint, string body);
    Task<T?> PostAsync<T, U>(string url, U data);
    Task<T?> GetAsync<T>(string url);
    Task<T?> PostMultipartAsync<T>(string token, string url, MultipartFormDataContent content);
    Task<(Stream? Stream, string? ContentType, string? FileName)?>GetFileAsync(string token, string urlEndPoint);

}

public class APIConnectService : IAPIConnectService
{
    //get the configuration from the appsetting.json file 

    /// <summary>
    /// The get command method is used to get data from the API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="token"></param>
    /// <param name="urlEndPoint"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    public async Task<T?> GetAsync<T>(string token, string urlEndPoint)
    {
        if (token == null) return default;

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, urlEndPoint);
        request.Headers.Add("Authorization", $"Bearer {token}");

        try
        {
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var ContentStream = await response.Content.ReadAsStreamAsync();
            var result = await Tools.Conversions.JsonConvert.DeserializeJsonAsync<T>(ContentStream);

            return result;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<T?> PostAsync<T, U>(string token, string urlEndPoint, U body)
    {
        //convert the body object to json then call postasync
        var json = await Tools.Conversions.JsonConvert.SerializeJsonAsync(body);
        if (json == null) return default;
        return await PostAsync<T>(token, urlEndPoint, json);
    }

    public async Task<T?> PostAsync<T>(string token, string urlEndPoint, string body)
    {
        if (token == null || string.IsNullOrEmpty(urlEndPoint)) return default;

        var handler = new HttpClientHandler()
        {
            UseDefaultCredentials = false,
            Credentials = System.Net.CredentialCache.DefaultCredentials,
            AllowAutoRedirect = true
        };

        var client = new HttpClient(handler);

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(body, Encoding.Unicode, "application/json");

        try
        {

            var response = await client.PostAsync(urlEndPoint, content);
            response.EnsureSuccessStatusCode();

            var ContentStream = await response.Content.ReadAsStreamAsync();
            if (ContentStream == null) return default;
            var result = await Tools.Conversions.JsonConvert.DeserializeJsonAsync<T>(ContentStream);
            return result;

        }
        catch (Exception ex)
        {
            return default;
        }
    }

    public async Task<T?> DeleteAsync<T, U>(string token, string urlEndPoint, U body)
    {
        //convert the body object to json then call postasync
        var json = await Tools.Conversions.JsonConvert.SerializeJsonAsync(body);
        if (json == null) return default;
        return await DeleteAsync<T>(token, urlEndPoint, json);
    }


    public async Task<T?> DeleteAsync<T>(string token, string urlEndPoint, string body)
    {
        if (token == null || string.IsNullOrEmpty(urlEndPoint)) return default;
        var handler = new HttpClientHandler()
        {
            UseDefaultCredentials = false,
            Credentials = System.Net.CredentialCache.DefaultCredentials,
            AllowAutoRedirect = true
        };

        var client = new HttpClient(handler);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(body, Encoding.Unicode, "application/json");

        try
        {
            var response = await client.DeleteAsync(urlEndPoint);
            response.EnsureSuccessStatusCode();
            var ContentStream = await response.Content.ReadAsStreamAsync();
            if (ContentStream == null) return default;
            var result = await Tools.Conversions.JsonConvert.DeserializeJsonAsync<T>(ContentStream);
            return result;
        }
        catch (Exception ex)
        {
            return default;
        }
    }

    // Add this to your API Connect helper class
    public async Task<T?> PostMultipartAsync<T>(string token, string url, MultipartFormDataContent content)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await client.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        return default;
    }
    public async Task<T?> PostAsync<T, U>(string urlEndPoint, U body)
    {
        //convert the body object to json then call postasync
        var json = await Tools.Conversions.JsonConvert.SerializeJsonAsync(body);
        if (json == null) return default;
        return await PostAsync<T>(urlEndPoint, json);
    }

    // Add these methods to your existing implementation
    public async Task<T?> PostAsync<T>(string urlEndPoint, string body)
    {
        try
        {
            var handler = new HttpClientHandler()
            {
                UseDefaultCredentials = false,
                Credentials = System.Net.CredentialCache.DefaultCredentials,
                AllowAutoRedirect = true
            };

            var client = new HttpClient(handler);

            
            var content = new StringContent(body, Encoding.Unicode, "application/json");

            try
            {

                var response = await client.PostAsync(urlEndPoint, content);
                response.EnsureSuccessStatusCode();

                var ContentStream = await response.Content.ReadAsStreamAsync();
                if (ContentStream == null) return default;
                var result = await Tools.Conversions.JsonConvert.DeserializeJsonAsync<T>(ContentStream);
                return result;

            }
            catch (Exception ex)
            {
                return default;
            }

            return default(T);
        }
        catch (Exception ex)
        {
            // Log exception
            return default(T);
        }
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            var handler = new HttpClientHandler()
            {
                UseDefaultCredentials = false,
                Credentials = System.Net.CredentialCache.DefaultCredentials,
                AllowAutoRedirect = true
            };

            var client = new HttpClient(handler);

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

            return default(T);
        }
        catch (Exception ex)
        {
            // Log exception
            return default(T);
        }
    }

    public async Task<(Stream? Stream, string? ContentType, string? FileName)?>GetFileAsync(string token, string urlEndPoint)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(urlEndPoint))
            return null;

        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, urlEndPoint);
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return null;

            // Stream
            var stream = await response.Content.ReadAsStreamAsync();

            // Content Type
            string? contentType = response.Content.Headers.ContentType?.MediaType;

            // Filename from Content-Disposition
            string? fileName =
                response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                ?? "download.bin";

            return (stream, contentType, fileName);
        }
        catch
        {
            return null;
        }
    }

}
