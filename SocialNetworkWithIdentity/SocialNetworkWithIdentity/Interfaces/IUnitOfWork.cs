using System;
using SocialNetworkWithIdentity.Models;
using SocialNetworkWithIdentity.Models.DomainModels;

namespace SocialNetworkWithIdentity.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Event> Events { get; }
        IRepository<Friendship> Friendships { get; }
        IRepository<Message> Messages { get; }
        IRepository<OfferFriendship> OfferFriendships { get; }
        IRepository<Room> Rooms { get; }
        void Save();
    }
}