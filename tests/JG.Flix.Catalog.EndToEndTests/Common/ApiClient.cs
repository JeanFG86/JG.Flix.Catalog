﻿using JG.Flix.Catalog.EndToEndTests.Extensions.String;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;


namespace JG.Flix.Catalog.EndToEndTests.Common;

class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name.ToSnakeCase();
}

public class ApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _defaultSerializerOptions;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _defaultSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(string route, object payload) where TOutput : class
    {
        var paylofJson = JsonSerializer.Serialize(payload, _defaultSerializerOptions);

        var response = await _httpClient.PostAsync(route, new StringContent(paylofJson, Encoding.UTF8, "application/json"));
        TOutput? output = await GetOutput<TOutput>(response);

        return (response, output);
    }


    public async Task<(HttpResponseMessage?, TOutput?)> Get<TOutput>(string route, object? queryStringParametersObject = null) where TOutput : class
    {
        var url = PrepareGetRoute(route, queryStringParametersObject);
        var response = await _httpClient.GetAsync(url);
        TOutput? output = await GetOutput<TOutput>(response);

        return (response, output);
    }    

    public async Task<(HttpResponseMessage?, TOutput?)> Delete<TOutput>(string route) where TOutput : class
    {
        var response = await _httpClient.DeleteAsync(route);
        TOutput? output = await GetOutput<TOutput>(response);

        return (response, output);
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Put<TOutput>(string route, object payload) where TOutput : class
    {
        var response = await _httpClient.PutAsync(route, new StringContent(JsonSerializer.Serialize(payload, _defaultSerializerOptions), Encoding.UTF8, "application/json"));
        TOutput? output = await GetOutput<TOutput>(response);

        return (response, output);
    }

    private async Task<TOutput?> GetOutput<TOutput>(HttpResponseMessage response) where TOutput : class
    {
        var outputString = await response.Content.ReadAsStringAsync();
        TOutput? output = null;
        if (!string.IsNullOrEmpty(outputString))        
            output = JsonSerializer.Deserialize<TOutput>(outputString, _defaultSerializerOptions);        

        return output;
    }

    private string PrepareGetRoute(string route, object? queryStringParametersObject)
    {
        if(queryStringParametersObject is null)
            return route;
        var parametersJson = JsonSerializer.Serialize(queryStringParametersObject, _defaultSerializerOptions);
        var parametersDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parametersJson);

        return QueryHelpers.AddQueryString(route, parametersDictionary!);
    }
}
