using Newtonsoft.Json;
using RestSharp;

namespace lexicon.Components.Services;
public class TranslationRequest {
    [JsonProperty("text")]
    public string Text { get; set; }
    
    [JsonProperty("source_lang")]
    public string SourceLang { get; set; }
    
    [JsonProperty("target_lang")]
    public string TargetLang { get; set; }
    
    public TranslationRequest(string text, string source_lang, string target_lang) {
        this.Text = text;
        this.SourceLang = source_lang;
        this.TargetLang = target_lang;
    }
}

public class TranslationResponse {
    [JsonProperty("destination-text")] public string TranslatedText { get; set; }
}

public class TranslationService {
        private readonly RestClient _client;
        private const string ApiBaseUrl = "https://ftapi.pythonanywhere.com/";

        public TranslationService() {
            var options = new RestClientOptions(ApiBaseUrl);
            _client = new RestClient(options);
        }

        public async Task<string> TranslateAsync(string textToTranslate, string sourceLang, string targetLang) {
            try {
                var request = new RestRequest("translate", Method.Get);

                request.AddParameter("sl", sourceLang, ParameterType.QueryString);
                request.AddParameter("dl", targetLang, ParameterType.QueryString);
                request.AddParameter("text", textToTranslate, ParameterType.QueryString);

                var response = await _client.ExecuteAsync(request);

                if (response.IsSuccessful) {
                    string receivedJson = response.Content;

                    TranslationResponse? translationResult =
                        JsonConvert.DeserializeObject<TranslationResponse>(receivedJson);

                    if (translationResult != null && !string.IsNullOrWhiteSpace(translationResult.TranslatedText)) {
                        return translationResult.TranslatedText;
                    }

                    return
                        $"Error: API returned success, but translation was empty. Raw content: {receivedJson.Substring(0, Math.Min(receivedJson.Length, 150))}...";
                }
                else {
                    string errorContent = response.ErrorMessage ?? response.Content;
                    string details = string.IsNullOrWhiteSpace(errorContent)
                        ? "No response content received."
                        : errorContent.Substring(0, Math.Min(errorContent.Length, 150));
                    return $"Error: Translation API failed. Status: {(int)response.StatusCode}. Details: {details}";
                }
            }
            catch (Exception ex) {
                return $"Critical Error during API call: {ex.Message.Substring(0, Math.Min(ex.Message.Length, 150))}";
            }
        }
    
}
