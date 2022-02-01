﻿namespace IdentityDemo.Models
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            Moderator,
            User
        }
        public const string DEFAULT_USERNAME = "user";
        public const string DEFAULT_EMAIL = "user@secureapi.com";
        public const string DEFAULT_PASSWORD = "Password12**";
        public const Roles DEFAULT_ROLE = Roles.User;
    }
}
