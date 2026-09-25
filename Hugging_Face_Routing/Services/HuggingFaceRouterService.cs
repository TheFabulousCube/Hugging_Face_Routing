using Hugging_Face_Routing.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Hugging_Face_Routing.Services;

public class HuggingFaceZeroShotClassifier
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://router.huggingface.co/hf-inference/models/MoritzLaurer/deberta-v3-large-zeroshot-v2.0";

    public HuggingFaceZeroShotClassifier(HttpClient httpClient, string hfToken)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", hfToken);
    }

    public async Task<string> PredictRouteAsync(string text, string[] labels, string other = "not classified")
    {
        if (labels == null || labels.Length == 0)
            throw new ArgumentException("Candidate labels cannot be empty.");

        var requestBody = new ZeroShotRequest
        {
            Inputs = text,
            Parameters = new ZeroShotParameters { CandidateLabels = labels }
        };

        // Post the JSON payload seamlessly
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(ApiUrl, requestBody);

        // Ensure successful status code (or bubble up exception handles gracefully)
        response.EnsureSuccessStatusCode();

        string rawContent = await response.Content.ReadAsStringAsync();

        // Deserialize the multi-class array responses
        var result = await response.Content.ReadFromJsonAsync<ZeroShotResponse[]>();

        if (result == null || result.Length == 0)
            return other; // Safer fallback path

        // Hugging Face automatically pre-sorts the "labels" array 
        // by the highest corresponding probability score (Index 0 is always the winner!)
        return (result[0].Score >= 0.5) ? result[0].Label : other;
    }
}
