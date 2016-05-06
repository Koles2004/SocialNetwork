using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace SocialNetworkWithIdentity.Models.DomainModels
{
    public class Photo
    {
        public long Id { set; get; }
        public ApplicationUser Sender { set; get; }
        public string Text { set; get; }
        public DateTime Date { set; get; }
        public string Image { set; get; }
    }
}