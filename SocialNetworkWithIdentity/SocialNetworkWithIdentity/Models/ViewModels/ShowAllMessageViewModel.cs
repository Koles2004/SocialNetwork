using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SocialNetworkWithIdentity.Models.ViewModels
{
	public class ShowAllMessageViewModel
	{
		[HiddenInput(DisplayValue = false)]
		public string Id { set; get; }
		[Display(Name = "Имя")]
		public string Name { set; get; }
		[Display(Name = "Фамилия")]
		public string Surname { set; get; }
		[Display(Name = "Аватар")]
		public string Avatar { set; get; }
		[Display(Name = "Сообщение")]
		public string Message { get; set; }
	}
}