﻿using System;

namespace SocialNetworkWithIdentity.Models.DomainModels
{
    public class Message
    {
        public long Id { set; get; }
        public ApplicationUser Sender { set; get; }
        public Room Room { set; get; }
        public string Text { set; get; }
        public DateTime Date { set; get; }
        public bool StatusForMessage { set; get; }
    }
}