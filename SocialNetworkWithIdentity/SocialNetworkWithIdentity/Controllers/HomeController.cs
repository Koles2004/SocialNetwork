using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SocialNetworkWithIdentity.Filters;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.ViewModels;
using SocialNetworkWithIdentity.Services;

namespace SocialNetworkWithIdentity.Controllers
{
    //[MyAuth]
    public class HomeController : Controller
    {
        private readonly PeopleService peopleService;

        public HomeController()
        {
            peopleService = new PeopleService();
        }

        public ActionResult Index()
        {
            return RedirectToAction("UserPage", "Account");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}