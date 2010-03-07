using System;
using System.Collections.Generic;

namespace Cosmicvent.Utilities {
    public interface ISettingsReader {
        IDictionary<string, string> Read();
        void Save(IDictionary<string, string> settings);
        event Action DataChanged;
    }
}