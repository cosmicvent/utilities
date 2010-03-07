using System;
using System.Collections.Generic;

namespace Cosmicvent.Utilities {
    public class SettingsService : ISettingsService {
        private readonly ISettingsReader _reader;

        IDictionary<string, string> _cachedSettings;

        public SettingsService(ISettingsReader reader) {
            _reader = reader;
            _reader.DataChanged += Reload;
            Reload();
        }

        public T Get<T>(string key) {
            return (T)Convert.ChangeType(Get(key), typeof(T));
        }

        public string Get(string key) {
            key = key.Trim().ToLower();
            if (_cachedSettings.ContainsKey(key)) {
                return _cachedSettings[key];
            }
            throw new KeyNotFoundException(string.Format("Key with name '{0}' not found", key));
        }

        public void Reload() {
            _cachedSettings = _reader.Read();
        }

        public void Set(string key, object value) {
            _cachedSettings[key] = value.ToString();
        }

        public void Persist() {
            _reader.Save(_cachedSettings);
        }
    }
}