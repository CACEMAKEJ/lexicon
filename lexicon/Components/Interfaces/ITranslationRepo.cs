using System.Collections.Generic;
using System.Threading.Tasks;
namespace lexicon.Components.Data;

public interface ITranslationRepo
{
    Task SaveAsync(TranslationRecord record);
    Task<List<TranslationRecord>> GetRecentAsync(int count = 50);
    Task<List<TranslationRecord>> GetByLanguageAsync(string languageCode);
}