namespace Cosmicvent.Utilities {
    public interface ISettingsService {
        T Get<T>(string key);
        string Get(string key);
        void Set(string key, object value);
        void Reload();
        void Persist();
    }
}