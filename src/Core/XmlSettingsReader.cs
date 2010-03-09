using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cosmicvent.Utilities {
    public class XmlSettingsReader : ISettingsReader {
        private readonly string _filename;
        public event Action DataChanged;

        public XmlSettingsReader()
            : this("settings.xml") {
        }

        public XmlSettingsReader(string filename) {
            if (!File.Exists(filename)) {
                throw new FileNotFoundException(string.Format("Unable to find the xml file {0}", filename));
            }
            _filename = filename;
            SetFileSystemWatcher();
        }

        private void SetFileSystemWatcher() {
            string dir = Path.GetDirectoryName(_filename);
            string file = Path.GetFileName(_filename);
            if (dir == null || dir.Trim().Length == 0) {
                dir = ".";
            }

            var fileSystemWatcher = new FileSystemWatcher(dir, file)
                                        {
                                            NotifyFilter = NotifyFilters.LastWrite,
                                            EnableRaisingEvents = true,
                                        };
            fileSystemWatcher.Changed += delegate { DataChanged(); };
        }


        public IDictionary<string, string> Read() {
            //BUG: This throws an exception when the file is locked by another process
            XDocument document = XDocument.Load(_filename);

            if (document.Root == null) {
                throw new FormatException(string.Format("The {0} file is not in the right format", _filename));
            }

            IEnumerable<XElement> elements = document.Root.Descendants();

            if (elements.Count() == 0) {
                throw new FormatException(string.Format("No seetings found in the {0} file", _filename));
            }

            var settings = new Dictionary<string, string>();

            foreach (var keyValuePair in elements) {
                AddSetting(settings, keyValuePair);
            }

            return settings;
        }

        public void Save(IDictionary<string, string> settings) {
            var root = new XElement("settings");

            foreach (var setting in settings) {
                root.Add(GetSettingElement(setting));
            }

            root.Save(_filename);
        }

        XElement GetSettingElement(KeyValuePair<string, string> setting) {
            return new XElement("setting", new XAttribute("name", setting.Key), new XAttribute("value", setting.Value));
        }

        void AddSetting(Dictionary<string, string> settings, XElement keyValuePair) {
            if (keyValuePair.Attributes().Count() != 2 || keyValuePair.Attribute("name") == null || keyValuePair.Attribute("value") == null) {
                throw new FormatException(string.Format("Invalid format exception for node {0}", keyValuePair));
            }

            settings.Add(keyValuePair.Attribute("name").Value.ToLower(), keyValuePair.Attribute("value").Value);
        }
    }
}