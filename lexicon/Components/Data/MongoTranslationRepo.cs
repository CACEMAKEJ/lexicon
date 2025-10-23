using MongoDB.Driver;

namespace lexicon.Components.Data
{
    public class MongoTranslationRepo : ITranslationRepo {
        private readonly IMongoCollection<TranslationRecord> _translations;
        public MongoTranslationRepo(string mongoUri) {
            var client = new MongoClient(mongoUri);
            var database = client.GetDatabase("LexiconDB"); // database name
            _translations = database.GetCollection<TranslationRecord>("Translations");
        }

        public async Task SaveAsync(TranslationRecord record) {
            await _translations.InsertOneAsync(record);
        }

        public async Task<List<TranslationRecord>> GetRecentAsync(int count = 50) {
            return await _translations
                .Find(_ => true)
                .SortByDescending(r => r.Timestamp)
                .Limit(count)
                .ToListAsync();
        }

        public async Task<List<TranslationRecord>> GetByLanguageAsync(string languageCode) {
            return await _translations
                .Find(r => r.SourceLanguage == languageCode)
                .SortByDescending(r => r.Timestamp)
                .ToListAsync();
        }
    }
}