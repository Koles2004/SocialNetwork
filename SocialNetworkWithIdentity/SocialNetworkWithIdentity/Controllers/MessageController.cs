using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.DomainModels;
using SocialNetworkWithIdentity.Models.ViewModels;
using SocialNetworkWithIdentity.Services;
using AutoMapper;

namespace SocialNetworkWithIdentity.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
		private PeopleService peopleService = new PeopleService();

        public ActionResult SendFirstMessage(string receivedUserId)
		{
			var user = peopleService.GetUser(receivedUserId);
			var viewUser = Mapper.Map<ApplicationUser, FirstMessageViewModel>(user);
			return PartialView(viewUser);
		}

		[HttpPost]
		public ActionResult SendFirstMessage(FirstMessageViewModel messageModel)
		{
			var currentUser = peopleService.GetUser(User.Identity.GetUserId());

			var message = new Message
			{
				Date = DateTime.Now,
				Text = messageModel.Message,
				Sender = currentUser
			};

			peopleService.MakeMessage(message, peopleService.GetUser(messageModel.Id));

			return Redirect(HttpContext.Request.UrlReferrer.ToString());
		}

		[HttpPost]
		public ActionResult SendDialogMessage(string resiver, string messageText)
		{
			var currentUser = peopleService.GetUser(User.Identity.GetUserId());

			var message = new Message
			{
				Date = DateTime.Now,
				Text = messageText,
				Sender = currentUser
			};

			peopleService.MakeMessage(message, peopleService.GetUser(resiver));

			var room = peopleService.GetRoom(message, peopleService.GetUser(resiver));
			var Messages = peopleService.GetAllMessagesOfRoom(Convert.ToInt32(room.Id)).OrderBy(s => s.Date);
			var newMessages = Messages.Where(m => m.StatusForMessage == false && m.Sender.Id != User.Identity.GetUserId());
            foreach (var item in newMessages)
            {
				item.StatusForMessage = true;
            }
            peopleService.UnitOfWork.Save();

			return PartialView("EmptyView", Messages);
		}

		public ActionResult ShowAllDialogues()
		{
			var currentUser = peopleService.GetUser(User.Identity.GetUserId());
			var userRooms = peopleService.GetRooms().Where(u => u.Users.Contains(currentUser));
			var lastMessages = new List<Message>();

			foreach(var room in userRooms)
			{
			    var message = room.Messages.OrderBy(d => d.Date).LastOrDefault();

			    if (message.Text.Length > 50)
			        message.Text = message.Text.Substring(0, 50) + "...";

				lastMessages.Add(message);
			}

            return View(lastMessages.OrderByDescending(d => d.Date));
		}

        public ActionResult ShowAllMessagesOfRoom(string roomId)
        {
            var Messages = peopleService.GetAllMessagesOfRoom(Convert.ToInt32(roomId)).OrderBy(s => s.Date);
			var newMessages = Messages.Where(m => m.StatusForMessage == false && m.Sender.Id != User.Identity.GetUserId());
            foreach (var item in newMessages)
            {
				item.StatusForMessage = true;
            }
            peopleService.UnitOfWork.Save();
            return View(Messages);
        }
	}
}