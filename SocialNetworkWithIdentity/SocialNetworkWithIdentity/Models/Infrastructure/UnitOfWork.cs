using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SocialNetworkWithIdentity.Interfaces;

namespace SocialNetworkWithIdentity.Models.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserRepository _applicationUserRepository;
        private EventRepository _eventRepository;
        private EventTypeRepository _eventTypeRepository;
        private FriendshipRepository _friendshipRepository;
        private MessageRepository _messageRepository;
        private OfferFriendshipRepository _offerFriendshipRepository;
        private RoomRepository _roomRepository;

        public ApplicationUserRepository Users
        {
            get { return _applicationUserRepository ?? (_applicationUserRepository = new ApplicationUserRepository(db)); }
        }

        public EventRepository Events
        {
            get { return _eventRepository ?? (_eventRepository = new EventRepository(db)); }
        }

        public EventTypeRepository EventTypes
        {
            get { return _eventTypeRepository ?? (_eventTypeRepository = new EventTypeRepository(db)); }
        }

        public FriendshipRepository Friendships
        {
            get { return _friendshipRepository ?? (_friendshipRepository = new FriendshipRepository(db)); }
        }

        public MessageRepository Messagess
        {
            get { return _messageRepository ?? (_messageRepository = new MessageRepository(db)); }
        }

        public OfferFriendshipRepository OfferFriendships
        {
            get { return _offerFriendshipRepository ?? (_offerFriendshipRepository = new OfferFriendshipRepository(db)); }
        }

        public RoomRepository Rooms
        {
            get { return _roomRepository ?? (_roomRepository = new RoomRepository(db)); }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this._disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}