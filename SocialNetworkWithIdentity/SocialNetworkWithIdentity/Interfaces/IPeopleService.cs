using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetworkWithIdentity.DTO;

namespace SocialNetworkWithIdentity.Interfaces
{
    interface IPeopleService
    {
        void MakeOfferFriendship(string receivedUserId);
        UserDTO GetUser(string id);
        IEnumerable<UserDTO> GetUsers();
        void Dispose();
    }
}
