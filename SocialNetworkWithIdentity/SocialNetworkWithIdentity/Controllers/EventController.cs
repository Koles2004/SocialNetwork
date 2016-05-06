using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetworkWithIdentity.Models.DomainModels;
using SocialNetworkWithIdentity.Models.ViewModels;
using SocialNetworkWithIdentity.Services;

namespace SocialNetworkWithIdentity.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        // как и другие контроллеры, содержит в себе вот такой сервис
        private readonly PeopleService peopleService;

        public EventController()
        {
            peopleService = new PeopleService();
        }
        // GET: Event
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllEvents()
        {
            var Events = peopleService.GetAllEventsFromUser(User.Identity.GetUserId()).OrderByDescending(d => d.Date);

            return View(Events);
        }

        public ActionResult RecordsOnTheWall(string UserId)
        {
            var Events =
                peopleService.GetAllEventsFromUser(UserId)
                    .Where(e => e.EventType == EventType.RecordOnTheWall).ToList();

            var Events1 = Events.Where(e => e.Sender.Id == UserId && (e.Owners.Count == 1)).ToList();
            var Events2 = Events.Where(e => e.Sender.Id != UserId).ToList();
            Events1.AddRange(Events2);
            return PartialView("AllEvents", Events1.OrderByDescending(d => d.Date));
        }

        [HttpPost]
        public ActionResult RecordOnTheWall(string senderUserId, string recieverUserId, string TextOnTheWall)
        {
            var keys = Request.Form.Keys;
            var _event = new Event
            {
                Sender = peopleService.GetUser(senderUserId),
                Text = TextOnTheWall,

                Date = DateTime.Now,
                Image = null,
                EventType = EventType.RecordOnTheWall,
                Owners = { peopleService.GetUser(senderUserId), peopleService.GetUser(recieverUserId) }
            };
            peopleService.MakeEvent(_event);
            return PartialView("AllEvents", peopleService.GetAllEventsFromUser(recieverUserId).Where(e => e.EventType == EventType.RecordOnTheWall).OrderByDescending(d => d.Date));
        }
    }
}