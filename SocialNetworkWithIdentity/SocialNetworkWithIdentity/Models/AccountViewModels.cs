using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkWithIdentity.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Почта:")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Почта:")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Почта:")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }

        [Display(Name = "Запомнить меня?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Почта: *")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должен состоять как минимум из {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль: *")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля: *")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должно состоять как минимум из {2} символа.", MinimumLength = 1)]
        [Display(Name = "Имя: *")]
        public string Name { set; get; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должна состоять как минимум из {2} символа.", MinimumLength = 1)]
        [Display(Name = "Фамилия: *")]
        public string Surname { set; get; }

        [Required]
        [Display(Name = "Дата рождения: *")]
        [DataType(DataType.Date)]
        // Без следующей строчки тоже работает правильно
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Аватар:")]
        public string Avatar { set; get; }

        [Required]
        [Display(Name = "Пол: *")]
        public string Gender { set; get; }

        [Display(Name = "Город:")]
        public string City { set; get; }

        [RegularExpression("[0-9]*", ErrorMessage = "Поле Телефон может содержать только цифры")]
        [Display(Name = "Телефон:")]
        public string PhoneNumber { set; get; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Почта:")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должен состоять как минимум из {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль:")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля:")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Почта:")]
        public string Email { get; set; }
    }
}