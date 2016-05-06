using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.DTO
{
    public class UserDTO
    {
        public string Id { set; get; }
        public string Email { set; get; }
        public string Name { set; get; }
        public string Surname { set; get; }
        public DateTime? DateOfBirth { set; get; }
        public string Avatar { set; get; }
        public string City { set; get; }
        public string PhoneNumber { set; get; }
        public bool? Gender { set; get; }
        public DateTime? DateOfActivity { set; get; }
    }
}