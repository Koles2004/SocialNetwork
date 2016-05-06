using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.DomainModels;
using SocialNetworkWithIdentity.Models.Repositories;
using SocialNetworkWithIdentity.Models.ViewModels;

namespace SocialNetworkWithIdentity.Services
{
	public class MessageService
	{
		public IUnitOfWork UnitOfWork { get; set; }

		public MessageService()
        {
            UnitOfWork = new EFUnitOfWork();
        }
		
		public void MakeMessage(Message message, ApplicationUser recievedUser)
		{
			var room = UnitOfWork.Rooms.GetAll().Where(r => r.Users.Count == 2)
				.FirstOrDefault(r => r.Users.Contains(message.Sender) && r.Users.Contains(recievedUser));

			if (room == null)
			{
				room = new Room
				{
					Users = { message.Sender, recievedUser },
				};
				UnitOfWork.Rooms.Create(room);
				UnitOfWork.Save();
			}
			message.Room = room;
			UnitOfWork.Messages.Create(message);
			UnitOfWork.Save();
		}

		public ICollection<Message> GetAllMessagesOfRoom(int roomId)
		{
			return UnitOfWork.Rooms.GetAll().FirstOrDefault(r => r.Id == roomId).Messages;
		}

		public int GetCountNewMessages(string currentUserId)
		{
			//return UnitOfWork.Messages.GetAll().Count(m => m.Status == false && m.Sender.Id != currentUserId);
			return UnitOfWork.Rooms.GetAll().Where(r => r.Users.Any(u => u.Id == currentUserId))
				.Where(r => r.Messages.Any(m => m.Status == false && m.Sender.Id != currentUserId)).Count();
		}

	}
}