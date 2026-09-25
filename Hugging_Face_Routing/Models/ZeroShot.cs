using System.Text.Json.Serialization;
namespace Hugging_Face_Routing.Models;

public class ZeroShotRequest
{
    [JsonPropertyName("inputs")]
    public string Inputs { get; set; } = string.Empty;

    [JsonPropertyName("parameters")]
    public ZeroShotParameters Parameters { get; set; } = new();
}

public class ZeroShotParameters
{
    [JsonPropertyName("candidate_labels")]
    public string[] CandidateLabels { get; set; } = Array.Empty<string>();
}

public class ZeroShotResponse
{
    [JsonPropertyName("label")]
    public string Label { get; set; } = string.Empty;

    [JsonPropertyName("score")]
    public float Score { get; set; }
}

