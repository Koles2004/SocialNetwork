using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.DomainModels;
using SocialNetworkWithIdentity.Models.ViewModels;
using SocialNetworkWithIdentity.Services;

namespace SocialNetworkWithIdentity.Controllers
{
    [Authorize]
    // контроллер, где будут методы для взаимодействия с другими пользователями
    public class PeopleController : Controller
    {
        // как и другие контроллеры, содержит в себе вот такой сервис
        private readonly PeopleService peopleService;

        public PeopleController()
        {
            peopleService = new PeopleService();
        }

        // GET: People
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowUsers(string choice)
        {
            var users = peopleService.GetUsers();

            switch (choice)
            {
                case "Male":
                    users = peopleService.GetUsers().Where(u => u.Gender == true).ToList();
                    break;
                case "Female":
                    users = peopleService.GetUsers().Where(u => u.Gender == false).ToList();
                    break;
                default:
                    users = peopleService.GetUsers().ToList();
                    break;
            }

            // получить себя
            var currentUser = peopleService.GetUser(User.Identity.GetUserId());

            List<PeopleViewModel> ViewUsers = new List<PeopleViewModel>();

            //var users = peopleService.GetUsers();

            // Маппим коллекцию ApplicationUser-ов в коллекцию PeopleViewModel-ов
            ViewUsers = Mapper.Map<IEnumerable<ApplicationUser>, List<PeopleViewModel>>(users);

            foreach (var user in ViewUsers)
            {
                user.IsFriend = peopleService.CheckIfUserIsMyFriend(currentUser.Id, peopleService.GetUser(user.Id).Id);
                user.IsThereNewOfferFriendshipFromUserToMe = peopleService.CheckIfThereIsNewOfferFriendshipFromUserToMe(currentUser.Id, peopleService.GetUser(user.Id).Id);
                user.IsThereNewOfferFriendshipFromMeToUser = peopleService.CheckIfThereIsNewOfferFriendshipFromMeToUser(currentUser.Id, peopleService.GetUser(user.Id).Id);
            }

            return View(ViewUsers);
        }

        public ActionResult ShowFriendsOrOffers()
        {
            var getCountOffersFriendships = peopleService.GetCountOffersFriendships(User.Identity.GetUserId());

            if (getCountOffersFriendships > 0)
                return OfferfriendshipList();
            else
                return FriendshipList(User.Identity.GetUserId());
        }

        // Вью с приглашениями в друзья
        public ActionResult OfferfriendshipList()
        {
            var viewUsers = peopleService.GetAllFriendshipOffers(User.Identity.GetUserId());
            return View("OfferfriendshipList", viewUsers);
        }

        public ActionResult FriendshipList(string id)
        {
            ViewBag.UserId = id;
            var friends = peopleService.GetAllFriends(id);
            return View("FriendshipList", friends);
        }

        public ActionResult FriendsOnlineList(string id)
        {
            ViewBag.UserId = id;
            var friendsOnline = peopleService.GetAllFriendsOnline(id);
            return View("FriendsOnlineList", friendsOnline);
        }

        public ActionResult CommonFriendshipList(string id)
        {
            ViewBag.UserId = id;
            var friends = peopleService.GetAllCommonFriends(User.Identity.GetUserId(), id);
            return View("CommonFriendshipList", friends);
        }

        #region Работа с Дружбой (AJAX)
        // Отправляем заявку в друзья (AJAX)
        [HttpPost]
        public ActionResult SendOfferFriendship(string friendId)
        {
            if (peopleService.MakeOfferFriendship(User.Identity.GetUserId(), friendId))
            {
                var _event = new Event
                {
                    Sender = peopleService.GetUser(User.Identity.GetUserId()),
                    Text = peopleService.GetUser(User.Identity.GetUserId()).Name +
                    (peopleService.GetUser(User.Identity.GetUserId()).Gender == true ? " пригласил " : " пригласила ")
                    + peopleService.GetUser(friendId).Name + " в друзья.",
                    Date = DateTime.Now,
                    Image = null,
                    EventType = EventType.OfferFriendship,
                    Owners = { peopleService.GetUser(User.Identity.GetUserId()), peopleService.GetUser(friendId) }
                };

                peopleService.MakeEvent(_event);
                ViewBag.Result = "Заявка в друзья отправлена.";
            }
            else
            {
                ViewBag.Result = "Что-то пошло не так";
            }

            //return Redirect(HttpContext.Request.UrlReferrer.ToString());
            return PartialView("FriendshipButtonStatus", friendId);
        }

        // Удаляем из друзей (AJAX)
        [HttpPost]
        public ActionResult DeleteFriendship(string friendId)
        {
            if (peopleService.DeleteFriendship(User.Identity.GetUserId(), friendId))
            {
                var _event = new Event
                {
                    Sender = peopleService.GetUser(User.Identity.GetUserId()),
                    Text = peopleService.GetUser(User.Identity.GetUserId()).Name +
                    (peopleService.GetUser(User.Identity.GetUserId()).Gender == true ? " удалил " : " удалила ")
                    + " пользователя " + peopleService.GetUser(friendId).Name + " из списка своих друзей.",

                    Date = DateTime.Now,
                    Image = null,
                    EventType = EventType.DeleteFriendship,
                    Owners = { peopleService.GetUser(User.Identity.GetUserId()), peopleService.GetUser(friendId) }
                };
                peopleService.MakeEvent(_event);
                ViewBag.Result = "Удаление из друзей произведено";
            }
            else
            {
                ViewBag.Result = "Что-то пошло не так";
            }

            return PartialView("FriendshipButtonStatus", friendId);
        }

        // Подтверждаем заявку в друзья (AJAX)
        [HttpPost]
        public ActionResult ConfirmOfferFriendship(string friendId)
        {
            if (peopleService.CreateNewFriendship(User.Identity.GetUserId(), friendId))
            {
                var _event = new Event
                {
                    Sender = peopleService.GetUser(User.Identity.GetUserId()),
                    Text = peopleService.GetUser(User.Identity.GetUserId()).Name +
                    (peopleService.GetUser(User.Identity.GetUserId()).Gender == true ? " подтвердил " : " подтвердила ")
                    + "заявку в друзья от пользователя " + peopleService.GetUser(friendId).Name + ".",

                    Date = DateTime.Now,
                    Image = null,
                    EventType = EventType.ConfirmFriendship,
                    Owners = { peopleService.GetUser(User.Identity.GetUserId()), peopleService.GetUser(friendId) }
                };
                peopleService.MakeEvent(_event);
                ViewBag.Result = "Заявка принята";
            }
            else
            {
                ViewBag.Result = "Что-то пошло не так";
            }

            //return Redirect(HttpContext.Request.UrlReferrer.ToString());
            return PartialView("FriendshipButtonStatus", friendId);
        }

        // Отклоняем заявку в друзья (AJAX)
        [HttpPost]
        public ActionResult DontConfirmOfferFriendship(string friendId)
        {
            if (peopleService.CancelOfferFriendship(friendId, User.Identity.GetUserId()))
            {
                var _event = new Event
                {
                    Sender = peopleService.GetUser(User.Identity.GetUserId()),
                    Text = peopleService.GetUser(User.Identity.GetUserId()).Name +
                    (peopleService.GetUser(User.Identity.GetUserId()).Gender == true ? " отклонил " : " отклонила ")
                    + "заявку в друзья от пользователя " + peopleService.GetUser(friendId).Name + ".",

                    Date = DateTime.Now,
                    Image = null,
                    EventType = EventType.DontConfirmOfferFriendship,
                    Owners = { peopleService.GetUser(User.Identity.GetUserId()), peopleService.GetUser(friendId) }
                };
                peopleService.MakeEvent(_event);
                ViewBag.Result = "Заявка в друзья отклонена.";
            }
            else
            {
                ViewBag.Result = "Что-то пошло не так";
            }

            //return Redirect(HttpContext.Request.UrlReferrer.ToString());
            return PartialView("FriendshipButtonStatus", friendId);
        }

        // Отменяем заявку в друзья (AJAX)
        [HttpPost]
        public ActionResult CancelOfferFriendship(string friendId)
        {
            if (peopleService.CancelOfferFriendship(User.Identity.GetUserId(), friendId))
            {
                var _event = new Event
                {
                    Sender = peopleService.GetUser(User.Identity.GetUserId()),
                    Text = peopleService.GetUser(User.Identity.GetUserId()).Name +
                    (peopleService.GetUser(User.Identity.GetUserId()).Gender == true ? " отменил " : " отменила ")
                    + "заявку в друзья к пользователю " + peopleService.GetUser(friendId).Name + ".",

                    Date = DateTime.Now,
                    Image = null,
                    EventType = EventType.CancelOfferfriendship,
                    Owners = { peopleService.GetUser(User.Identity.GetUserId()), peopleService.GetUser(friendId) }
                };
                peopleService.MakeEvent(_event);
                ViewBag.Result = "Заявка в друзья отменена.";
            }
            else
            {
                ViewBag.Result = "Что-то пошло не так";
            }

            //return Redirect(HttpContext.Request.UrlReferrer.ToString());
            return PartialView("FriendshipButtonStatus", friendId);
        }
        #endregion

        #region Получение списка пользователей (AJAX)

        [HttpPost]
        public ActionResult GetUsersList(string userId, string gender, bool online, string name, string surname, bool commonFriends)
        {
            List<PeopleViewModel> ViewUsers = new List<PeopleViewModel>();

            var Users = new List<ApplicationUser>();

            if (commonFriends)
            {
                Users = Mapper.Map<List<PeopleViewModel>, List<ApplicationUser>>(peopleService.GetAllCommonFriends(User.Identity.GetUserId(), userId));
            }
            else
            {
                if (!string.IsNullOrEmpty(userId))
                    Users = Mapper.Map<List<PeopleViewModel>, List<ApplicationUser>>(peopleService.GetAllFriends(userId));
                else
                    Users = peopleService.GetUsers().ToList();                
            }

            if (online)
                Users = Users.Where(f => (f.DateOfActivity - DateTime.Now).Value.TotalMinutes > -3).ToList();

            switch (gender)
            {
                case "Male":
                    Users = Users.Where(u => u.Gender == true).ToList();
                    break;
                case "Female":
                    Users = Users.Where(u => u.Gender == false).ToList();
                    break;
            }

            if (name != null)
                Users = Users.Where(u => u.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase)).ToList();

            if (surname != null)
                Users = Users.Where(u => u.Surname.StartsWith(surname, StringComparison.OrdinalIgnoreCase)).ToList();

            // Маппим коллекцию ApplicationUser-ов в коллекцию PeopleViewModel-ов
            ViewUsers = Mapper.Map<IEnumerable<ApplicationUser>, List<PeopleViewModel>>(Users);
            return PartialView("UsersList", ViewUsers);
        }
        #endregion
    }
}