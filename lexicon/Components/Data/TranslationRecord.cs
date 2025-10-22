namespace lexicon.Components.Data;

public class TranslationRecord {
    
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string OriginalText { get; set; }
    public string TranslatedText { get; set; }
    public string SourceLanguage { get; set; }
    public string TargetLanguage { get; set; } = "en";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    

}