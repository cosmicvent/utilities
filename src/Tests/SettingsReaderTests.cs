using System;
using System.Collections.Generic;
using Xunit;
using System.IO;

namespace Cosmicvent.Utilities.Tests {
    public class SettingsReaderTests {

        readonly XmlSettingsReader _reader;

        public SettingsReaderTests() {
            _reader = new XmlSettingsReader();
        }

        [Fact]
        public void Read_should_return_a_dictionary_of_string_key_value_pairs() {
            IDictionary<string, string> settings = _reader.Read();
            Assert.NotNull(settings);
        }

        [Fact]
        public void Read_should_return_more_than_2_setting_pairs() {
            var reader = new XmlSettingsReader("test2_settings.xml");
            var settings = reader.Read();
            Assert.Equal(2, settings.Count);
            Assert.Equal("cosmicvent.com", settings["smtp_server"]);
            Assert.Equal("25", settings["smtp_port"]);
        }

        [Fact]
        public void Save_should_save_the_settings_to_the_xml_file() {
            _reader.Save(new Dictionary<string, string> { { "smtp_username", "khaja minhajuddin" } });

            var expectedText =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<settings>
  <setting name=""smtp_username"" value=""khaja minhajuddin"" />
</settings>";

            string text = File.ReadAllText("settings.xml");
            Assert.Equal(expectedText, text);
        }

        [Fact]
        public void should_trigger_the_data_change_event_whenever_the_file_gets_modified() {
            bool triggered = false;

            _reader.DataChanged += () => triggered = true;

            File.SetLastWriteTime("settings.xml", DateTime.Now.AddHours(1));
            System.Threading.Thread.Sleep(100);//need to do this so that the app doesn't close before the event is raised
            Assert.True(triggered);
        }
    }
}