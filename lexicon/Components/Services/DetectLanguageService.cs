using DetectLanguage;
namespace lexicon.Components.Services;
public class  DetectLanguageService {
    
    private readonly DetectLanguageClient _client;   

    public DetectLanguageService(string apiKey) {
        
        if (string.IsNullOrEmpty(apiKey)) {
            Console.WriteLine("DL_KEY missing. Translation will not work.");
        }
        else {
            _client = new DetectLanguageClient(apiKey);
        }
    }
    public async Task<string> DetectLanguageAsync(string textToDetect) {
        if (string.IsNullOrWhiteSpace(textToDetect)) {
            return null;
        }
        try {
            DetectResult[] results = await _client.DetectAsync(textToDetect);
            var bestResult = results.FirstOrDefault(); 
            return bestResult?.language;
        }
        catch (Exception ex) {
            Console.WriteLine($"Error detecting language: {ex.Message}");
            return null;
        }
    }
    
}

