using System.Collections.Generic;
using Moq;
using Xunit;

namespace Cosmicvent.Utilities.Tests {
    public class SettingsServiceTests {

        /*
         * - Get should return a strongly typed setting ##
         * - Reload should reload the settings from the settings file ##
         * - Set should set the setting in the settings service ##
         * - Persist should save all the settings to the xml file ##
         * - Settings should be reloaded whenever the readers data changes ##
         * - Should be able to read from any settings reader
         * 
         * */

        readonly ISettingsService _service;
        private Mock<ISettingsReader> _mockReader;


        public SettingsServiceTests() {
            _mockReader = new Mock<ISettingsReader>();
            _mockReader.Setup(x => x.Read())
                .Returns(new Dictionary<string, string>
                             {
                                 {"smtp_port", "25"},
                                 {"smtp_server", "cosmicvent.com"}
                             });
            _service = new SettingsService(_mockReader.Object);
        }


        [Fact]
        public void Get_should_return_a_strongly_typed_setting() {
            var setting = _service.Get<string>("smtp_server");
            Assert.IsType<string>(setting);
            Assert.Equal("cosmicvent.com", setting);
        }

        [Fact]
        public void Get_should_return_a_strongly_typed_setting_when_setting_is_int() {
            var setting = _service.Get<int>("smtp_port");
            Assert.IsType<int>(setting);
            Assert.Equal(25, setting);
        }

        [Fact]
        public void Reload_should_reload_the_settings_from_the_settings_file() {
            _mockReader.Setup(x => x.Read()).Returns(new Dictionary<string, string> { { "smtp_username", "khaja minhajuddin" } });
            _service.Reload();

            var setting = _service.Get<string>("smtp_username");
            Assert.Equal("khaja minhajuddin", setting);
        }

        [Fact]
        public void Set_should_set_the_setting_in_the_setting_service() {
            _service.Set("smtp_password", "khaja");

            string setting = _service.Get("smtp_password");
            Assert.Equal("khaja", setting);
        }

        [Fact]
        public void Persist_should_save_the_settings_to_the_settings_file() {
            _service.Set("smtp_password", "khaja");
            _service.Persist();

            _mockReader.Verify(x => x.Save(It.Is<IDictionary<string, string>>(s => s["smtp_password"] == "khaja")));
        }

        [Fact]
        public void Settings_should_be_reloaded_when_the_readers_data_changes() {
            _mockReader.Raise(x => x.DataChanged += null);

            _mockReader.Verify(x => x.Read(), Times.Exactly(2));//once in the constructor once when data is changed
        }

        [Fact]
        public void should_throw_an_exception_when_a_setting_which_does_not_exist_is_fetched() {
            Assert.Throws<KeyNotFoundException>(() => _service.Get("api_key"));
        }

    }
}