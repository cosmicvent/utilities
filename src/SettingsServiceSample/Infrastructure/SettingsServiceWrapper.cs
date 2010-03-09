using System.IO;
using Cosmicvent.Utilities;
using System;
namespace SettingsServiceSample.Infrastructure {
    public static class SettingsServiceWrapper {

        private static ISettingsService _settingsService;
        private static object _locker = new object();

        public static ISettingsService SettingsService {
            get {
                if (_settingsService != null) {
                    return _settingsService;
                }

                lock (_locker) {
                    var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin\\settings.xml");
                    var reader = new XmlSettingsReader(filename);
                    _settingsService = new SettingsService(reader);
                }
                return _settingsService;
            }
        }
    }
}