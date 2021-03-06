﻿using System;
using SocialNetworkWithIdentity.Interfaces;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Models.Repositories
{
    // класс, который содержит в себе все репозитории и передает им всем один контекст
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserRepository _applicationUserRepository;
        private EventRepository _eventRepository;
        private FriendshipRepository _friendshipRepository;
        private MessageRepository _messageRepository;
        private OfferFriendshipRepository _offerFriendshipRepository;
        private RoomRepository _roomRepository;
        private PhotoRepository _photoRepository;

        public IRepository<ApplicationUser> Users
        {
            get { return _applicationUserRepository ?? (_applicationUserRepository = new ApplicationUserRepository(db)); }
        }

        public IRepository<Event> Events
        {
            get { return _eventRepository ?? (_eventRepository = new EventRepository(db)); }
        }
        public IRepository<Friendship> Friendships
        {
            get { return _friendshipRepository ?? (_friendshipRepository = new FriendshipRepository(db)); }
        }

        public IRepository<Message> Messages
        {
            get { return _messageRepository ?? (_messageRepository = new MessageRepository(db)); }
        }

        public IRepository<OfferFriendship> OfferFriendships
        {
            get { return _offerFriendshipRepository ?? (_offerFriendshipRepository = new OfferFriendshipRepository(db)); }
        }

        public IRepository<Room> Rooms
        {
            get { return _roomRepository ?? (_roomRepository = new RoomRepository(db)); }
        }

        public IRepository<Photo> Photos
        {
            get { return _photoRepository ?? (_photoRepository = new PhotoRepository(db)); }
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