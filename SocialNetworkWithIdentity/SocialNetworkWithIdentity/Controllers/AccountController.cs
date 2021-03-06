﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.ViewModels;
using SocialNetworkWithIdentity.Services;

namespace SocialNetworkWithIdentity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // Класс, в котором происходит основная часть логики. В себе содержит UnitOfWork
        private PeopleService peopleService;
        ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
            peopleService = new PeopleService();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            // проверяем заявки в друзья
            ViewBag.GetCountOffersFriendships = peopleService.GetCountOffersFriendships(User.Identity.GetUserId());
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("UserPage", new {Id = User.Identity.GetUserId(), Welcome = true});
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                    return View(model);
            }
        }

        // моя страница пользователя
        public ActionResult UserPage(string Id, bool Welcome = false)
        {
            //if (Welcome) ViewBag.Welcome = true;
            Random rand = new Random();
            List<PeopleViewModel> friends = new List<PeopleViewModel>();

            if(Id == null)
                friends = peopleService.GetAllFriends(User.Identity.GetUserId());
            else
                friends = peopleService.GetAllFriends(Id);

            ViewBag.CountOfFriends = friends.Count;

            var friendsOnline = friends.Where(f => (f.DateOfActivity - DateTime.Now).Value.TotalMinutes > -3).ToList();
            ViewBag.CountOfFriendsOnline = friendsOnline.Count;

            var onlySixFriends = new List<PeopleViewModel>();

            if (friends.Count <= 6)
                ViewBag.Friends = friends;
            else
            {
                for (var i = 0; i < 6; i++)
                {
                    var index = rand.Next(0, friends.Count);
                    onlySixFriends.Add(friends[index]);

                    friends.RemoveAt(index);
                }

                ViewBag.Friends = onlySixFriends;
            }

            var onlySixFriendsOnline = new List<PeopleViewModel>();

            if (friendsOnline.Count <= 6)
                ViewBag.FriendsOnline = friendsOnline;
            else
            {
                for (var i = 0; i < 6; i++)
                {
                    var index = rand.Next(0, friendsOnline.Count);
                    onlySixFriendsOnline.Add(friendsOnline[index]);

                    friendsOnline.RemoveAt(index);
                }

                ViewBag.FriendsOnline = onlySixFriendsOnline;
            }

            // для отображения общих друзей
            if (Id != User.Identity.GetUserId() && Id != null)
            {
                var commonFriends = peopleService.GetAllCommonFriends(User.Identity.GetUserId(), Id);
                ViewBag.AllCommonFriends = commonFriends; // все общие друзья
                ViewBag.CountOfCommonFriends = commonFriends.Count; // кол-во общих друзей

                var onlySixCommonFriends = new List<PeopleViewModel>();

                if (commonFriends.Count <= 6)
                    ViewBag.SixOrLessCommonFriends = commonFriends; // для отображения 6 или меньше общих друзей
                else
                {
                    for (var i = 0; i < 6; i++)
                    {
                        var index = rand.Next(0, commonFriends.Count);
                        onlySixCommonFriends.Add(commonFriends[index]);

                        commonFriends.RemoveAt(index);
                    }

                    ViewBag.SixOrLessCommonFriends = onlySixCommonFriends; // для отображения 6 или меньше общих друзей
                }

                ViewBag.CurrentUserId = User.Identity.GetUserId();
            }
            //-----------------------------

            // обращаемся к AutoMapper
            UserPageViewModel user = new UserPageViewModel();
            if (Id == null || Id == User.Identity.GetUserId())
            {
                Mapper.Map(peopleService.GetUser(User.Identity.GetUserId()), user);
                return View(user);
            }
            else
            {
                Mapper.Map(peopleService.GetUser(Id), user);
                if (peopleService.CheckIfUserIsMyFriend(User.Identity.GetUserId(), Id))
                {
                    return View("UserPageOfFriend", user);
                }
                else
                {
                    return View("UserPageOfAnotherPerson", user);
                }
            }
        }

        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, HttpPostedFileBase uploadImage)
        {
            Random rnd = new Random();

            if (ModelState.IsValid)
            {
                // тут работа с записью файла с аватаром на сервер
                string directory = Server.MapPath(@"\Content\Images\AccountImages");
                string fileName = null;

                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    string tmp = Path.GetFileName(uploadImage.FileName);
                    fileName = (rnd.Next(1, 100000).ToString() + tmp.GetHashCode() + tmp.Substring(tmp.Length - 4, 4));
                    uploadImage.SaveAs(Path.Combine(directory, fileName));
                }

                model.Avatar = fileName ?? "DefaultAvatar.jpg";

                // обращаемся к AutoMapper
                var user = Mapper.Map<RegisterViewModel, ApplicationUser>(model);
                
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // ---- Не УДАЛЯТЬ -----
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("UserPage"/*, new { Welcome = true }*/);
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public ActionResult GetStatus()
        {
            var status = peopleService.GetStatus(User.Identity.GetUserId());
            return PartialView(status ?? "");
        }

        [HttpPost]
        public ActionResult EditStatus(string status)
        {
            var result = peopleService.EditStatus(User.Identity.GetUserId(), status);

            if(!string.IsNullOrEmpty(result))
                return PartialView("GetStatus", result);

            return PartialView("GetStatus", "Изменить статус");
        }

        public ActionResult EditUser()
        {
            var user = peopleService.GetUser(User.Identity.GetUserId());
            var userView = Mapper.Map<ApplicationUser, UserPageViewModel>(user);

            List<string> listOfGenders = new List<string>();

            if (user != null)
            {
                if (user.Gender == true)
                {
                    listOfGenders.Add("Мужской");
                    listOfGenders.Add("Женский");
                }
                else
                {
                    listOfGenders.Add("Женский");
                    listOfGenders.Add("Мужской");
                }

                SelectList genders = new SelectList(listOfGenders);
                ViewBag.Genders = genders;

                return View(userView);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(string submitButton, UserPageViewModel userView)
        {
            switch (submitButton)
            {
                case "Сохранить":
                    return SaveChanges(userView);
                case "Изменить пароль":
                    return RedirectToChangePassword();
                default:
                    return View();
            }
        }

        public ActionResult RedirectToChangePassword()
        {
            return RedirectToAction("ChangePassword", "Manage");
        }

        public ActionResult SaveChanges(UserPageViewModel userView)
        {
            var currentUser = peopleService.GetUser(userView.Id);
            Mapper.Map(userView, currentUser);
            peopleService.EditUser(currentUser);
            return RedirectToAction("UserPage");
        }

        public ActionResult ChangePhoto()
        {
            var user = peopleService.GetUser(User.Identity.GetUserId());
            var userView = Mapper.Map<ApplicationUser, UserPageViewModel>(user);

            if (user != null)
                return View(userView);

            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePhoto(string submitButton, UserPageViewModel model, HttpPostedFileBase uploadImage)
        {
            switch (submitButton)
            {
                case "Сохранить":
                    return SaveNewPhoto(model, uploadImage);
                case "Отмена":
                    return Cancel();
                default:
                    return View();
            }
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("UserPage");
        }

        public ActionResult SaveNewPhoto(UserPageViewModel model, HttpPostedFileBase uploadImage)
        {
            Random rnd = new Random();

            // тут работа с записью файла с аватаром на сервер
            string directory = Server.MapPath(@"\Content\Images\AccountImages");
            string fileName = null;

            if (uploadImage != null && uploadImage.ContentLength > 0)
            {
                string tmp = Path.GetFileName(uploadImage.FileName);
                fileName = (rnd.Next(1, 100000).ToString() + tmp.GetHashCode() + tmp.Substring(tmp.Length - 4, 4));
                uploadImage.SaveAs(Path.Combine(directory, fileName));
            }

            if (fileName == null) return RedirectToAction("UserPage");
            model.Avatar = fileName;
            var currentUser = peopleService.GetUser(model.Id);
            Mapper.Map(model, currentUser);
            peopleService.EditUser(currentUser);

            return RedirectToAction("UserPage");
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            // здесь появляется ошибка, когда при регистрации пишем почту, которая уже есть
            // перевести ошибку на русский язык можно с обрезанием строки до и после имейла, когда строка начинается на слова Name или Email
            // но так как кроме Name и Email тут могут генерироваться какие-то другие ошибки, то затея тупая
            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}