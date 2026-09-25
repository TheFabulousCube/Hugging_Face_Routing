using Hugging_Face_Routing;
using Hugging_Face_Routing.Services;

Console.WriteLine("Hugging Face Zero-Shot Classifier - Integration Test\n");

string hfToken = "<add your Hugging Face token here; get one here: https://huggingface.co/ ";
var httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };
var classifier = new HuggingFaceZeroShotClassifier(httpClient, hfToken);

var classifications = TestCases.dynamicTargets;
var testMessages = TestCases.testMessages;

Console.WriteLine($"Testing {testMessages.Length} messages against targets: {string.Join(", ", classifications)}\n");
Console.WriteLine("Results:");
Console.WriteLine(new string('-', 80));

var results = new List<(string Message, string Classification)>();

// Classify each message
foreach (var message in testMessages)
{
    try
    {
        string classification = await classifier.PredictRouteAsync(message, classifications);
        results.Add((message, classification));

        Console.WriteLine($"{classification.ToUpper(),-8} | {message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR  | {message}");
        Console.WriteLine($"   └─ {ex.Message}");
    }
}

Console.WriteLine(new string('-', 80));

var summary = results
    .GroupBy(r => r.Classification)
    .Select(g => new { Label = g.Key, Count = g.Count() })
    .OrderByDescending(s => s.Count);

Console.WriteLine("\nSummary:");
foreach (var stat in summary)
{
    Console.WriteLine($"   {stat.Label,-10} : {stat.Count,2} messages");
}

Console.WriteLine($"\nClassification Complete! ({results.Count}/{testMessages.Length} successful)");