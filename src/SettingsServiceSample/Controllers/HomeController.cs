using System.Web.Mvc;
using SettingsServiceSample.Infrastructure;
using System;

namespace SettingsServiceSample.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            var homeViewModel = new HomeViewModel()
                                    {
                                        WelcomeMessage = SettingsServiceWrapper.SettingsService.Get<string>("welcome_message"),
                                        Version = SettingsServiceWrapper.SettingsService.Get<string>("version"),
                                        PublishDate = SettingsServiceWrapper.SettingsService.Get<DateTime>("last_publish_date")
                                    };
            return View(homeViewModel);
        }

    }
    public class HomeViewModel {
        public string WelcomeMessage { get; set; }
        public string Version { get; set; }
        public DateTime PublishDate { get; set; }
    }
}