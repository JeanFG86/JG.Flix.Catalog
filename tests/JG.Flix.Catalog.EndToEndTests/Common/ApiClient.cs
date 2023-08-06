﻿using System.Text;
using System.Text.Json;

namespace JG.Flix.Catalog.EndToEndTests.Common;
public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(string route, object payload)
    {
        var response = await _httpClient.PostAsync(route, new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
        var outputString = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<TOutput>(outputString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        return (response, output);
    }
}
