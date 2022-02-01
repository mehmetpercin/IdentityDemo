﻿namespace IdentityDemo.Models
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
